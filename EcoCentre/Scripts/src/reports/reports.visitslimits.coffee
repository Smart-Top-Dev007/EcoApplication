common = require('common')
client = require('client')

class Report extends Backbone.Model
    constructor:(name)->
        super
            page: 1
            pageCount: 1
            pageSize: null
        this.items = new Backbone.Collection()
        this.pageButtons = new Backbone.Collection()
        this.url = 'reports/visitslimits'

    parse:(resp)=>
        this.items.reset()
        for item in resp.Items
            item.$parent = this
            
            item.includedInvoicesCount = 0
            for invoice in item.Invoices
                if !invoice.IsExcluded then item.includedInvoicesCount++
                
            this.items.push item
        this.pageButtons.reset()
        pages = common.generatePages(resp.Page,resp.PageCount)
        this.set 'page', resp.Page
        this.set 'pageCount', resp.PageCount
        for page in pages
            this.pageButtons.push page

    fetch:()=>
        super data : 
            page : this.get 'page'

class ItemViewModel extends Knockback.ViewModel
    constructor:(model)->
        super(model)
        this.expanded = ko.observable false
        this.edited = ko.observable false
        this.newPersVisLimit = ko.observable()
        this.newPersVisLimit this.model().get('Client').PersonalVisitsLimit || 0
    expand:()=>this.expanded true
    fold:()=>this.expanded false
    showEditInput:()=>this.edited true
    hideEditInput:()=>
        this.edited false
        this.newPersVisLimit this.model().get('Client').PersonalVisitsLimit || 0
        
    saveEditChanges:()=>
        if !this.newPersVisLimit() and this.newPersVisLimit() != 0 then return
        curClient = new client.Model(this.model().get('Client'))
        curClient.set
            'PersonalVisitsLimit': this.newPersVisLimit()
            'UpdateOnlyPersonalVisitsLimit': true
            
        $loadingFade = $("#global-loading-fade");
        $loadingFade.modal('show');
        curClient.save().complete ()=>
            this.$parent().model().set
                page: 1
            this.$parent().model().fetch().complete ()=>
                setTimeoutCallback = -> $loadingFade.modal('hide')
                setTimeout setTimeoutCallback, 500
        

class ViewModel

    constructor:(model)->
        this.model = model
        this.page = kb.observable(model,'page')
        this.pageCount = kb.observable(model,'pageCount')
        this.throttledPage = ko.computed(this.page).extend({ throttle: 400 })
        this.pageSize = kb.observable(model,'pageSize')
        this.items = kb.collectionObservable model.items, {view_model:ItemViewModel}
        this.pageButtons = kb.collectionObservable this.model.pageButtons
        this.globalSettingsModel = new (require('globalsettings')).Model()
        this.globalSettingsModel.fetch({ async: false })
        this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'))
        @throttledPage.subscribe @goToPage

    goToPage:(val)=> 
        if val and val > 1 and val < this.pageCount() 
            return this.changePage({ number: val })
            
    load:()=> 
        this.page 1
        this.model.fetch()
    
    changePage:(vm)=>
        if !vm.number then vm.number = 1
        this.page vm.number
        this.model.fetch()

exports.createViewModel = ()=>
    model = new Report()
    new ViewModel(model)
