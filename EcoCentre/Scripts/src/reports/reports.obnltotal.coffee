common = require('common')
client  = require('client')

class Report extends Backbone.Model
  constructor:(name)->
    super
      centerName: ''
      OBNLNumber: ''
      filterFirstName: ''
      filterLastName: ''
      filterStreet: ''
      filterCivicNumber: ''
      searchType: 'Address'
      searchTerm: ''
      page: 1
      pageCount: 1
      pageSize: null
      sortBy: 'Name'
      sortIndex: 0
      sortDir: 'Asc'
      fromDate: Date.today().set({month:0,day:1})
      toDate: Date.today()
      personalVisitsLimitHigherThenGlobalOnly: false
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
      filterFirstName: this.get('filterFirstName')
      filterLastName: this.get('filterLastName')
      filterStreet: this.get('filterStreet')
      filterCivicNumber: this.get('filterCivicNumber')
      filterCivicCard: this.get('filterCivicCard')
      searchTerm: this.get('searchTerm')
      searchType: this.get('searchType')
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
      personalVisitsLimitHigherThenGlobalOnly: this.get('personalVisitsLimitHigherThenGlobalOnly')
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
    this.searchType = kb.observable(model, 'searchType')
    this.firstName = kb.observable(model, 'filterFirstName')
    this.lastName = kb.observable(model, 'filterLastName')
    this.street = kb.observable(model, 'filterStreet')
    this.civicNumber = kb.observable(model, 'filterCivicNumber')
    this.civicCard = kb.observable(model, 'filterCivicCard')
    this.centerName = kb.observable(model, 'centerName')
    this.OBNLNumber = kb.observable(model, 'OBNLNumber')
    this.obnlNumberAutocomplete = new (require 'obnlreinvestment').OBNLNumberAutocompleteViewModel(this.OBNLNumber)
    this.personalVisitsLimitHigherThenGlobalOnly = kb.observable(model, 'personalVisitsLimitHigherThenGlobalOnly')
    this.allClients = kb.observable(model, 'allClients')
    this.lastNameAutocomplete = new client.ClientLastNameAutocompleteViewModel(this.lastName)
    this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.civicNumber,this.street)
    this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.civicNumber,this.street)
    this.civicCardAutocomplete = new client.ClientCivicCardAutocompleteViewModel(this.civicCard)
    
    this.totalReinvestments = ko.computed () => 
      @items().reduce ((x, s) ->
        x + s.Invoices().length
      ), 0
    this.totalWeight = ko.computed () => 
      @items().reduce ((x, s) ->
        x + s.TotalWeight()
      ), 0
    this.totalVisits = ko.computed () => 
      @items().reduce ((x, s) ->
        x + s.TotalVisits()
      ), 0
    this.searchTerm = kb.observable(model, 'searchTerm')
    this.fromDate = kb.observable(model, 'fromDate')
    this.toDate = kb.observable(model, 'toDate')
    this.searchfocus = ko.observable(true)
    this.pageButtons = kb.collectionObservable(this.model.pageButtons)
    this.showName = ko.observable(true);
    this.showAddress = ko.observable(false);
    this.computeShowName()
    this.computeShowAddress()
    this.searchType.subscribe(this.computeShowName)
    this.searchType.subscribe(this.computeShowAddress)
    this.pageButtons = kb.collectionObservable(this.model.pageButtons)
    this.showDateFields = ko.observable(false)
    @throttledPage.subscribe @goToPage
    this.globalSettingsModel = new (require('globalsettings')).Model()
    this.globalSettingsModel.fetch({ async: false })
    this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'))

  computeShowName:()=>return this.showName this.searchType().toLowerCase() == "name"
          
  computeShowAddress:()=>return this.showAddress this.searchType().toLowerCase() == "address"

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
    model = new Report("obnltotal");
    return new ViewModel(model);