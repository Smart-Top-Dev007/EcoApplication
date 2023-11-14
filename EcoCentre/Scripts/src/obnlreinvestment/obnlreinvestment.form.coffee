client = require('client')

class ClientViewModel extends kb.ViewModel
  constructor:(model)->
    super model
    this.model = model

    this.categories = new (require('client')).CategoriesModel()
    this.categories.fetch()
    this.fullName = ko.computed this.computeFullName

    this.limitsModel = new (require('limits')).Model()
    this.limitsModel.fetchByClientId(this.model.get 'Id')
    this.limits = kb.collectionObservable this.limitsModel.CurrentLimits
    
    this.obnlMaterialsModel = new (require('obnl.materials')).Model()
    this.obnlMaterialsModel.fetchByClientId(this.model.get 'Id')
    this.obnlMaterials = kb.collectionObservable this.obnlMaterialsModel.CurrentMaterials

    this.globalSettingsModel = new (require('globalsettings')).Model()
    this.globalSettingsModel.fetch({ async: false })
    this.maxYearlyClientVisitsWarning = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisitsWarning'))
    this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'))

    this.invoices = kb.collectionObservable(this.model.Invoices)
    this.includedInvoices = ko.computed this.filterIncludedInvoices
    
    this.obnlReinvestments = kb.collectionObservable(this.model.OBNLReinvestments)
    this.excludedOBNLReinvestments = ko.computed this.filterExcludedOBNLReinvestments
    this.includedOBNLReinvestments = ko.computed this.filterIncludedOBNLReinvestments

    this.showContactWarning = ko.computed this.computeShowContactWarning
    this.showUnverifiedWarning = ko.computed this.computeShowUnverifiedWarning
    this.showComment = ko.computed this.computeShowComment
    this.showMaxVisitsWarning = ko.computed(this.computeShowMaxVisitsWarning);
    this.showMaxVisitsReachedWarning = ko.computed(this.computeShowMaxVisitsReachedWarning)

  filterExcludedOBNLReinvestments:()=> ko.utils.arrayFilter this.obnlReinvestments(), (item)=>
    item.IsExcluded()
  filterIncludedOBNLReinvestments:()=> ko.utils.arrayFilter this.obnlReinvestments(), (item)=>
    !item.IsExcluded()
  filterIncludedInvoices:()=> ko.utils.arrayFilter this.invoices(), (item)=>
    !item.IsExcluded()
  showMaxVisitsReachedAlert:()=>$('#max-visits-reached-msg-modal').modal('show')
  showMaxVisitsWarningAlert:()=>$('#max-visits-warning-msg-modal').modal('show')

  computeShowMaxVisitsWarning:()=>
    if this.PersonalVisitsLimit() and this.PersonalVisitsLimit() > 0
      return this.obnlReinvestments().length >= (this.PersonalVisitsLimit() - this.maxYearlyClientVisitsWarning() - 1) && this.maxYearlyClientVisitsWarning() > 0 && this.obnlReinvestments().length < this.PersonalVisitsLimit()
    return this.obnlReinvestments().length >= (this.maxYearlyClientVisits() - this.maxYearlyClientVisitsWarning() - 1) && this.maxYearlyClientVisitsWarning() > 0 && this.obnlReinvestments().length < this.maxYearlyClientVisits()
  computeShowMaxVisitsReachedWarning:()=>
    if this.PersonalVisitsLimit() and this.PersonalVisitsLimit() > 0
      return this.obnlReinvestments().length >= this.PersonalVisitsLimit()
    return this.obnlReinvestments().length >= this.maxYearlyClientVisits() && this.maxYearlyClientVisits() > 0

  computeFullName:()=>this.FirstName() + ' ' + this.LastName()
  computeShowUnverifiedWarning:()=>
    !this.Verified()

  computeShowComment:()=>
    @Comments()!= null && @Comments() != ''

  computeShowContactWarning:()=>
    email = this.Email()
    phone = this.PhoneNumber()
    email == null || email.length < 1 || phone == null || phone.length < 0

  showFirstNameLastName:()=>
    this.categories.showFirstNameLastName(this.get('Category'))

class MaterialViewModel
  constructor:(material)->
    this.Name = material.Name
    this.Unit = material.Unit
    this.Weight = ''
    this.IsExcluded = material.IsExcluded
    this.hasFocus = ko.observable true
    this.Id = material.Id

class MaterialsViewModel
  constructor:(model, materialModel)->
    this.model = model
    this.materialsModel = materialModel
    this.materialsModel.materials.bind 'add', this.updateAvailable
    this.availableMaterials = ko.observableArray []
    this.pickedMaterials = ko.observableArray []
    this.showAvailable = ko.observable false

  updateAvailable:(item)=>
    this.availableMaterials.push new MaterialViewModel(item.attributes)

  showAvailableList:()=>this.showAvailable true

  pickMaterial:(m)=>
    this.availableMaterials.remove m
    this.pickedMaterials.push m
    this.model.addMaterial m
    this.sortMaterials()

  removeMaterial:(m)=>
    this.model.removeMaterial m
    this.availableMaterials.push m
    this.pickedMaterials.remove m
    this.sortMaterials()

  sortCallback:(l,r)=>
    return 0 if l.Name == r.Name
    return if l.Name < r.Name then -1 else 1

  sortMaterials:()=>
    this.availableMaterials.sort this.sortCallback
    this.pickedMaterials.sort this.sortCallback

class ViewModel
  constructor:(model,materialModel, clientListViewModel, showClients)->
    this.model = model
    this.clientList = clientListViewModel
    this.errors = ko.observableArray()
    this.client = ko.observable(null)
    this.obnlReinvestmentPreview = ko.observable null
    model.Client.subscribe (c)=>this.client new ClientViewModel(c)
    this.clientId = kb.observable(model,'ClientId')
    this.employeeName = kb.observable(model,'EmployeeName')
    this.comment = kb.observable(model,'Comment')
    this.materials = new MaterialsViewModel(model,materialModel)
    this.showList = ko.observable true
    this.saved = false
    this.attachments = ko.observableArray []

    this.previewAvailable = ko.computed this.computePreviewAvailable

    materialModel.fetch()
    this.fileUpload =
      dataType: 'json'
      done : this.fileUploaded

  fileUploaded:(e,f)=>
    resp = f.jqXHR.responseText
    if resp == undefined
      resp = f.jqXHR.iframe[0].documentElement.innerText
    result = $.parseJSON(resp)
    att = this.model.get 'Attachments'
    att.push result
    this.attachments.push result

  computePreviewAvailable:()=>this.client() != null
  updatePreview:()=>
    newPreview =
      OBNLReinvestmentNo : '2013-xxxxxx'
      CreatedAtFormatted : (new Date()).toString('yyyy-MM-dd')
      Comment : this.comment()
      CreatedBy : null
      Client :
        FirstName : this.client().FirstName()
        LastName : this.client().LastName()
        PhoneNumber : this.client().PhoneNumber()
      Address : this.client().Address()
      Materials : []
      Attachments : []
    for material in this.model.attributes.Materials
      newPreview.Materials.push(material)
    for at in this.attachments
      newPreview.Attachments.push at

    this.obnlReinvestmentPreview newPreview

  preview:()=>
    this.updatePreview()
    this.errors.removeAll()
    eco.app.router.navigate '/obnlreinvestment/new/preview'
  hidePreview:()=>this.obnlReinvestmentPreview null

  save:()=>
    $("#global-loading-fade").modal('show')
    this.model.save(null,{success : this.onSaved, error:this.onError})
  onSaved:()=>
    setTimeoutCallback = -> $("#global-loading-fade").modal('hide')
    setTimeout setTimeoutCallback, 500
    this.saved = true
    eco.app.notifications.addNotification('msg-invoice-saved-successfully');
    eco.app.router.navigate 'obnlreinvestment/show/'+this.model.get('Id'),{trigger:true}

  changeClient:()=> this.showList true

  editClient:()=> eco.app.router.navigate 'obnlreinvestment/new/client/edit/'+this.client().Id(),{trigger:true}
  newClient:()=> eco.app.router.navigate 'obnlreinvestment/new/client/create',{trigger:true}

  onError:(m,errors,data)=>
    setTimeoutCallback = -> $("#global-loading-fade").modal('hide')
    setTimeout setTimeoutCallback, 500
    errors = $.parseJSON errors.responseText
    this.errors.removeAll()
    for item in errors
      this.errors.push item

exports.ViewModel = ViewModel