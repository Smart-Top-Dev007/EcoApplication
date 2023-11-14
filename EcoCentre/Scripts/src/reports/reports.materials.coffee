common = require('common')
class Report extends Backbone.Model
    constructor:(name)->
        super
            from : (1).month().ago()
            to : Date.today()
            sortBy : 'Name'
            sortDir : 'Asc'
        this.items = new Backbone.Collection()
        this.pageButtons = new Backbone.Collection()
        this.url = 'reports/'+name
        this.InvoiceCount = 0;
        this.UniqueAddressCount = 0;

    parse:(resp)=>
        this.items.reset()
        for item in resp
            this.items.push item
        this.pageButtons.reset()
        pages = common.generatePages(resp.Page,resp.PageCount)
        for page in pages
            this.pageButtons.push page

    serializeArguments:(xls)=>
        return {
            from : this.get('from').toString('yyyy-MM-dd')
            to : this.get('to').toString('yyyy-MM-dd')
            Xls : xls
            sortBy : this.get('sortBy')
            sortDir : this.get('sortDir')
        }
        
    download:()=>
        window.location = this.url + "?" + $.param(this.serializeArguments(true))

    fetch:()=>
        super data : this.serializeArguments(false)

class ViewModel

    constructor:(model)->
        this.model = model
        this.from = kb.observable model, 'from'
        this.to = kb.observable model, 'to'
        this.items = kb.collectionObservable model.items
        this.sortBy = kb.observable model, 'sortBy'
        this.sortDir = kb.observable model, 'sortDir'
        this.pageButtons = kb.collectionObservable this.model.pageButtons

    generate:()=>this.load()

    sort:(sortBy)=>
        if this.sortBy() != sortBy
            this.sortBy sortBy
            this.sortDir 'Asc'
        else
            this.sortDir( if this.sortDir() == 'Asc' then 'Desc' else 'Asc')
        this.load()

    generateXls:()=>this.model.download()

    load:()=> 
        this.model.fetch()


exports.createViewModel = ()=>
    model = new Report('materials')
    new ViewModel(model)