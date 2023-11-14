common = require('common')
client  = require('client')

class Report extends Backbone.Model
  constructor:(name)->
    super
      centerName: ''
      OBNLNumber: ''
      page: 1
      pageCount: 1
      pageSize: null
      sortBy: 'Name'
      sortIndex: 0
      sortDir: 'Asc'
      fromDate: Date.today().set({month:0,day:1})
      toDate: Date.today()
      allClients: false
    this.items = new Backbone.Collection()
    this.pageButtons = new Backbone.Collection()
    this.url = 'reports/'+name

  parse:(resp)=>
    this.items.reset()
    _ref = resp
    for item in _ref
      this.items.push item
    this.pageButtons.reset()
    pages = common.generatePages(1,1)
    this.set 'page', 1
    this.set 'pageCount', 1
    for page in pages
      this.pageButtons.push page

  serializeArguments:(xls)=>
    return {
      page: this.get('page')
      pageSize: this.get('pageSize')
      Xls: xls
      sortBy: this.get('sortBy')
      sortIndex: this.get('sortIndex')
      sortDir: this.get('sortDir')
      fromDate: if this.get('fromDate') then this.get('fromDate').toString('yyyy-MM-dd') else null
      toDate: if this.get('toDate') then this.get('toDate').toString('yyyy-MM-dd') else null
      centerName: this.get('centerName')
      OBNLNumber: this.get('OBNLNumber')
      allClients: this.get('allClients')
    }


  download:()=>
    return window.location = this.url + "?" + $.param(this.serializeArguments(true))

  fetch:()=>
    serializedData = this.serializeArguments(false);
    $loadingFade = $("#global-loading-fade");
    $loadingFade.modal('show');
    fetchAjax = Report.__super__.fetch.call(this, {
      data: serializedData
    });
    fetchAjax.complete () =>
      setTimeoutCallback = -> $loadingFade.modal('hide')
      setTimeout setTimeoutCallback, 500
      return fetchAjax;

class ItemViewModel extends Knockback.ViewModel
    constructor:(model)->
        super model
        this.expanded = ko.observable false
    
    expand:()=>
        this.expanded true
    
    fold:()=>
        this.expanded false

class ViewModel

  constructor:(model)->
    this.model = model
    this.page = kb.observable(model,'page')
    this.pageCount = kb.observable(model,'pageCount')
    this.throttledPage = ko.computed(this.page).extend({ throttle: 400 })
    this.pageSize = kb.observable(model,'pageSize')
    this.items = kb.collectionObservable(model.items, {view_model: ItemViewModel})
    this.sortBy = kb.observable(model, 'sortBy')
    this.sortIndex = kb.observable(model, 'sortIndex')
    this.sortDir = kb.observable(model, 'sortDir')
    this.centerName = kb.observable(model, 'centerName')
    this.OBNLNumber = kb.observable(model, 'OBNLNumber')
    this.obnlNumberAutocomplete = new (require 'obnlreinvestment').OBNLNumberAutocompleteViewModel(this.OBNLNumber)
    this.allClients = kb.observable(model, 'allClients')

    this.fromDate = kb.observable(model, 'fromDate')
    this.toDate = kb.observable(model, 'toDate')
    this.searchfocus = ko.observable(true)
    this.pageButtons = kb.collectionObservable(this.model.pageButtons)
    this.pageButtons = kb.collectionObservable(this.model.pageButtons)
    this.showDateFields = ko.observable(false)
    @throttledPage.subscribe @goToPage
    this.globalSettingsModel = new (require('globalsettings')).Model()
    this.globalSettingsModel.fetch({ async: false })

  goToPage:(val)=> 
    if val and val > 1 and val < this.pageCount() 
      return this.changePage({ number: val })
      
  load:()=>
    this.model.fetch()
  
  search:()=>
    this.page(1);
    return this.model.fetch()
    
  sort:(sortBy, sortIndex)=>
    sortIndex = (if sortIndex? then sortIndex else 0)
    if this.sortBy() != sortBy || this.sortIndex() != sortIndex
      this.sortBy(sortBy)
      this.sortIndex(sortIndex)
      this.sortDir('Asc')
    else
      this.sortDir(if this.sortDir() == 'Asc' then 'Desc' else 'Asc')
    return this.load()
    
  generateXls:()=>this.model.download()
  
  changePage:(vm)=>
    this.page(vm.number);
    return this.model.fetch()


exports.createViewModel = ()=>
    model = new Report("obnlglobal");
    return new ViewModel(model);