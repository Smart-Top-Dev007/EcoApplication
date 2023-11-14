common = require('common')
class Report extends Backbone.Model
    constructor:(name)->
        super
            page : 1
            page: 1
            pageCount: 1
            pageSize: null
        this.items = new Backbone.Collection()
        this.pageButtons = new Backbone.Collection()
        this.url = 'reports/limits'

    parse:(resp)=>
        this.items.reset()
        for item in resp.Items
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
    expand:()=>this.expanded true
    fold:()=>this.expanded false

class ViewModel

    constructor:(model)->
        this.model = model
        this.page = kb.observable(model,'page')
        this.pageCount = kb.observable(model,'pageCount')
        this.throttledPage = ko.computed(this.page).extend({ throttle: 400 })
        this.pageSize = kb.observable(model,'pageSize')
        this.items = kb.collectionObservable model.items, {view_model:ItemViewModel}
        this.pageButtons = kb.collectionObservable this.model.pageButtons
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
