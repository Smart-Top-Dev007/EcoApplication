common = require 'common'
client = require 'client'

class ClientRow extends kb.ViewModel
  constructor:(model,options)->
    super(model,options)
    @categoryStr = kb.observable model, {key: 'Category', localizer: common.LocalizedStringLocalizer}
    this.Invoices = kb.collectionObservable model.Invoices
    this.OBNLReinvestments = kb.collectionObservable model.OBNLReinvestments
    this.IncludedOBNLReinvestments = kb.collectionObservable model.IncludedOBNLReinvestments
    this.ExcludedInvoices = kb.collectionObservable model.ExcludedInvoices
    this.IncludedInvoices = kb.collectionObservable model.IncludedInvoices
    this.expanded = ko.observable false
    this.isFromAnotherHub = ko.observable true
  expand:()=>
    return this.expanded true 
  fold:()=>
    return this.expanded false
    

class ViewModel
  constructor:(model, hubs, selectedHub)->
    this.model = model
    this.items = kb.collectionObservable this.model.clients, {view_model: ClientRow}
    this.pageButtons = kb.collectionObservable this.model.pageButtons
    this.firstName = kb.observable(model,'filterFirstName')
    this.lastName = kb.observable(model,'filterLastName')
    this.address = kb.observable(model, 'filterAddress')
    this.civicNumber = kb.observable(model, 'filterCivicNumber')
    this.civicCard = kb.observable(model, 'filterCivicCard')
    this.OBNLNumber = kb.observable(model, 'filterOBNLNumber')
    this.hubId = kb.observable(model, 'filterHubId')
    this.lastNameAutocomplete = new client.ClientLastNameAutocompleteViewModel(this.lastName)
    this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.civicNumber,this.address, null, null, this.hubId)
    this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.civicNumber,this.address)
    this.civicCardAutocomplete = new client.ClientCivicCardAutocompleteViewModel(this.civicCard)
    this.obnlNumberAutocomplete = new (require 'obnlreinvestment').OBNLNumberAutocompleteViewModel(this.OBNLNumber)

    this.lastVisitFrom = kb.observable(model,'filterLastVisitFrom')
    this.lastVisitTo = kb.observable(model,'filterLastVisitTo')
    this.firstName.subscribe this.resetLetter
    this.lastName.subscribe this.resetLetter
    if selectedHub
      model.set('filterHubId', selectedHub.get('Id'))
    this.categoryFilter = kb.observable(model, 'filterCategory')
    this.hubs = kb.collectionObservable(hubs.hubs)
    this.searchFirstLetter = kb.observable(model,'filterFirstLetter')
    this.activeOnly = kb.observable(model,'filterActive')
    this.inactiveOnly = kb.observable(model, 'filterInactive')
    this.sortDir = kb.observable(model,'sortDir')
    this.sortBy = kb.observable(model,'sortBy')
    this.searchType = kb.observable(model,'searchType')
    this.page = kb.observable(model,'page')
    this.pageCount = kb.observable(model,'pageCount')
    this.throttledPage = ko.computed(this.page).extend({ throttle: 400 })
    this.pageSize = kb.observable(model,'pageSize')
    this.filterType = kb.observable(model,'filterType')
    this.term = kb.observable(model,'term')
    this.searchfocus = ko.observable true
    this.mode = ko.observable 'default'
    this.letters = ['Tous','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z']
    this.showName = ko.observable true
    this.showHub = ko.computed(this.computeShowHubs);
    this.showAddress = ko.observable(false);
    this.showFilterSelect = ko.observable(true);
    this.globalSettingsModel = new (require('globalsettings')).Model()
    this.globalSettingsModel.fetch({ async: false })
    this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'))
    @computeShowName()
    @computeShowAddress()
    @searchType.subscribe @computeShowName
    @searchType.subscribe @computeShowAddress
    @filterType.subscribe @updateFilter
    @throttledPage.subscribe @goToPage

  computeShowHubs:()=> return this.hubs().length > 0

  computeShowAddress:()=> return this.showAddress(this.searchType().toLowerCase() == "address")

  goToPage:(val)=> 
    if val and val > 1 and val < this.pageCount() 
      return this.changePage({ number: val })
  
  isOBNL:(item)=>
    if item.toLowerCase() == 'obnl'
       return true
    else
       return false


  updateFilter:()=>
    switch this.filterType().toLowerCase()
      when "active" 
        this.activeOnly true
        this.inactiveOnly false
      when "inactive"
        this.activeOnly false
        this.inactiveOnly true
      else
        this.activeOnly false
        this.inactiveOnly false
      
  computeShowName:()=>
    @showName @searchType().toLowerCase() == "name"

  load:()=>
    this.model.set
        filterOBNLNumber : ''
        filterCategory : 'All'
    this.model.changePage(1)
    this.searchfocus = ko.observable true

  reset:()=>
    @model.clients.reset()
    @model.pageButtons.reset()

  changePage:(vm)=>this.model.changePage(vm.number)

  search:()=>this.model.search()
  resetLetter:()=>
    this.searchFirstLetter ''
  searchLetter:(ltr)=>
    this.firstName ''
    this.lastName ''
    this.searchFirstLetter if ltr == 'Tous' then '' else ltr
    this.search()

  sort:(orderBy)=>
    if this.sortBy() != orderBy
      this.sortBy orderBy
      this.sortDir 'Asc'
    else
      this.sortDir( if this.sortDir() == 'Asc' then 'Desc' else 'Asc')
    this.load()

  onPick:(vm)=>true
  onNew:()=>eco.app.router.navigate 'client/new', {trigger:true}
  onCancel: null
  onShow:(vm,e)=>
    eco.app.router.navigate 'client/show/'+vm.Id(), {trigger:true}
  onEdit:(vm)=>eco.app.router.navigate 'client/edit/'+vm.Id(), {trigger:true}
  onRemove:(vm)=>
    if !confirm(kb.locale_manager.get("Voulez-vous supprimer ce client?"))
      return
    vm.model().set
      'Status' : 1
      'UpdateOnlyStatus' : true
    vm.model().save()
    return vm.model().collection.remove(vm.model())
  onActivate:(vm)=>
    if !confirm(kb.locale_manager.get("Voulez-vous rétablir ce client?"))
      return
    vm.model().set
      'Status' : 0
      'UpdateOnlyStatus' : true
    vm.model().save()
    return vm.model().collection.remove(vm.model())
  onCompletelyRemove:(vm)=>
    if !confirm(kb.locale_manager.get("Voulez-vous supprimer complètement ce client?\nATTENTION! Cette opération est IRRÉVERSIBLE!"))
      return
    return vm.model().destroy()


exports.createViewModel =  (hub)=>
  
  exports.model = new (client).ClientList()
  hubs = new (require('hub')).ListModel()
  hubs.fetch({async: false})
  if !hub
    hub = new (require('hub')).Model()
    hub.fetchCurrent()
  exports.vm = new ViewModel(exports.model, hubs, hub)
  return exports.vm