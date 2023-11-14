class HubViewModel
  constructor:(model)->
    this.Name = kb.observable model, 'Hub.Name'
    this.Id = kb.observable model, 'Hub.Id'
  updateName:(m,v)=> this.Name v
  updateId:(m,v)=> this.Id v

class AddressViewModel
  constructor:(model)->

    this.City = kb.observable model, 'Address.City'
    this.Street = kb.observable model, 'Address.Street'
    this.PostalCode = kb.observable model, 'Address.PostalCode'
    this.CivicNumber = kb.observable model, 'Address.CivicNumber'
    this.AptNumber = kb.observable model, 'Address.AptNumber'
    this.CityId = kb.observable model, 'Address.CityId'

    model.on 'change:Address.PostalCode', this.updatePostalCode
    model.on 'change:Address.Street', this.updateStreet
    model.on 'change:Address.City', this.updateCity
    model.on 'change:Address.CityId', this.updateCityId
    model.on 'change:Address.CivicNumber', this.updateCivicNumber
    model.on 'change:Address.AptNumber', this.updateAptNumber
  updatePostalCode:(m,v)=> this.PostalCode v
  updateStreet:(m,v)=> this.Street v
  updateCity:(m,v)=> this.City v
  updateCivicNumber:(m,v)=> this.CivicNumber v
  updateCityId:(m,v)=> this.CityId v
  updateAptNumber:(m,v)=> this.AptNumber v


class ViewModel extends kb.ViewModel
  constructor:(model,invoices,obnlReinvestments,obnlMaterials,limits,hubs,globalSettings)->
    super(model)
    this.address = new AddressViewModel(model)
    this.invoicesModel = invoices
    this.obnlReinvestmentsModel = obnlReinvestments
    this.hub = new HubViewModel(model)
    this.limitsModel = limits
    this.obnlMaterialsModel = obnlMaterials;
    this.hubs = kb.collectionObservable(hubs.hubs)
    this.invoices = kb.collectionObservable this.invoicesModel.invoices
    this.totalInvoices = kb.observable this.invoicesModel, 'total'
    this.obnlReinvestments = kb.collectionObservable this.obnlReinvestmentsModel.obnlReinvestments
    this.totalOBNLReinvestments = kb.observable this.obnlReinvestmentsModel, 'total'
    this.invoicesPageButtons = kb.collectionObservable this.invoicesModel.pageButtons
    this.obnlReinvestmentsPageButtons = kb.collectionObservable this.obnlReinvestmentsModel.pageButtons
    this.limits = kb.collectionObservable this.limitsModel.CurrentLimits
    this.obnlMaterials = kb.collectionObservable this.obnlMaterialsModel.CurrentMaterials
    this.errors = ko.observableArray([])
    this.globalSettings = globalSettings
    this.isMunicipality = ko.computed this.computeIsMunicipality

  changeInvoicesPage:(vm)=>
    page = if vm? then vm.number else 1
    this.invoicesModel.changePage(page)
  
  changeOBNLReinvestmentsPage:(vm)=>
    page = if vm? then vm.number else 1
    this.obnlReinvestmentsModel.changePage(page)
    
  computeIsMunicipality:()=>
    this.Category() == null || this.Category() == 'Municipality'
  isNew:()=>
    this.model().isNew()
  load:(id)=>
    this.model().fetch { data : {id:id}}
    this.invoicesModel.loadForUser id
    this.obnlReinvestmentsModel.loadForUser id
    this.limitsModel.fetchByClientId id
    this.obnlMaterialsModel.fetchByClientId id

  removeInvoice:(inv)=>
    return unless confirm(kb.locale_manager.get("Do you want to remove the invoice?"))
    inv.model().destroy()
    
  removeOBNLReinvestment:(reinv)=>
    return unless confirm(kb.locale_manager.get("Do you want to remove the réemploi?"))
    reinv.model().destroy()

  save:()=>
    this.model().save null, {success : this.onSaved, error:this.onError}
  onSaved:()=>
    window.location.hash = 'client/index';
  onError:(m,errors,data)=>
    errors = $.parseJSON errors.responseText
    this.errors.removeAll()
    for item in errors
      this.errors.push item
  parseDate:(item)=>
    createdAt = item.CreatedAt()
    return moment(createdAt).format('YYYY-MM-DD HH:mm')

exports.createViewModel = ()=>
  model = new (require('client')).Model()
  invoices = new (require('invoice')).InvoiceList()
  obnlReinvestments = new (require('obnlreinvestment')).OBNLReinvestmentsList()
  obnlMaterials = new (require('obnl.materials')).Model()
  limits = new (require 'limits').Model()
  hubs = new (require('hub')).ListModel()
  hubs.fetch()
  globalSettings = new (require 'globalsettings').Model()
  globalSettings.fetch()
  return new ViewModel(model,invoices,obnlReinvestments,obnlMaterials,limits,hubs,globalSettings)