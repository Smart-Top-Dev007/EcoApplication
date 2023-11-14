client = require('client')
invoice = require('invoice')
giveaway = require('giveaway')
PickedMaterial  = require('invoice').PickedMaterial 
HubModel  = require('hub').Model 

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
		this.excludedInvoices = ko.computed this.filterExcludedInvoices
		this.includedInvoices = ko.computed this.filterIncludedInvoices

		this.showContactWarning = ko.computed this.computeShowContactWarning
		this.showUnverifiedWarning = ko.computed this.computeShowUnverifiedWarning
		this.showComment = ko.computed this.computeShowComment
		this.showMaxVisitsWarning = ko.computed(this.computeShowMaxVisitsWarning);
		this.showMaxVisitsReachedWarning = ko.computed(this.computeShowMaxVisitsReachedWarning);
		this.isRegisteredInCurrentHub = ko.observable(this.model.get('IsRegisteredInCurrentHub'))
		this.showIsRegisteredInAnotherHubWarning = ko.observable(!this.model.get('IsRegisteredInCurrentHub'))

	filterExcludedInvoices:()=> ko.utils.arrayFilter this.invoices(), (item)=>
		item.IsExcluded()
	filterIncludedInvoices:()=> ko.utils.arrayFilter this.invoices(), (item)=>
		!item.IsExcluded()
	showMaxVisitsReachedAlert:()=>$('#max-visits-reached-msg-modal').modal('show')
	showMaxVisitsWarningAlert:()=>$('#max-visits-warning-msg-modal').modal('show')
	showIsRegisteredInAnotherHubAlert:()=>$('#is-registered-in-another-hub-msg-modal').modal('show')

	computeShowMaxVisitsWarning:()=>
		if this.PersonalVisitsLimit() and this.PersonalVisitsLimit() > 0
			return this.invoices().length >= (this.PersonalVisitsLimit() - this.maxYearlyClientVisitsWarning() - 1) && this.maxYearlyClientVisitsWarning() > 0 && this.invoices().length < this.PersonalVisitsLimit()
		return this.invoices().length >= (this.maxYearlyClientVisits() - this.maxYearlyClientVisitsWarning() - 1) && this.maxYearlyClientVisitsWarning() > 0 && this.invoices().length < this.maxYearlyClientVisits()
	computeShowMaxVisitsReachedWarning:()=>
		if this.PersonalVisitsLimit() and this.PersonalVisitsLimit() > 0
			return this.invoices().length >= this.PersonalVisitsLimit()
		return this.invoices().length >= this.maxYearlyClientVisits() && this.maxYearlyClientVisits() > 0

	computeFullName:()=>this.FirstName() + ' ' + this.LastName()
	computeShowUnverifiedWarning:()=>
		!this.Verified()

	computeShowComment:()=>
		@Comments()!= null && @Comments() != ''

	computeShowContactWarning:()=>
		email = this.Email()
		phone = this.PhoneNumber()
		email == null || email.length < 1 || phone == null || phone.length < 0;

	showFirstNameLastName:()=>
		this.categories.showFirstNameLastName(this.get('Category'))


class PickedMaterialViewModel extends kb.ViewModel
	constructor:(model, material, hub)->
		super model
		this.model = model
		this.width = ko.observable ''
		this.height = ko.observable ''
		this.depth = ko.observable ''
		this.containers = ko.observableArray []
		this.selectedContainer = ko.observable		
		viewModel = this
		this.enteredQuantity = ''
		this.hasFocus = ko.observable true
		this.name = material.get("Name")
		this.unit = material.get("Unit")
		# show calculator only for materials in cubic yards
		this.needsCalculator = this.unit == 'v\u00B3' or this.unit == 'v3'
		
		currentHubId = hub.get("Id")
		hubSettings = _.find(material.get("HubSettings"), (x)=>x.HubId == currentHubId) || {}
		this.needsProofOfResidence = hubSettings.RequireProofOfResidence
		this.hasContainer = hubSettings.HasContainer
		if this.hasContainer
			this.loadContainers()
				
		this.Quantity = kb.observable(model , {
			key:'Quantity'
			read:()=>
				toDecimal = (i)=> (i || "").replace(',', '.').replace(/[^0-9\.]/g, '' );
				w = toDecimal(viewModel.width()) || 0
				h = toDecimal(viewModel.height()) || 0
				d = toDecimal(viewModel.depth()) || 0
				# 3 calculator inputs for dimensions are in cubic feet,
				# and the total amount is in cubic yards.
				# Conversion is ft / 27 = yards.
				quantity = new BigNumber(w).mul(h).mul(d).div(3*3*3).round(3)
				if(quantity > 0)
					result = quantity.toString().replace('.', ',')
					model.set('Quantity', result)
					result
				else
					model.set('Quantity', viewModel.enteredQuantity)
					viewModel.enteredQuantity
			write:(value)=>
				viewModel.enteredQuantity = value
				viewModel.width('')
				viewModel.height('')
				viewModel.depth('')
		}, {}, @)
		
	loadContainers:()=>
		id = this.model.get('Id');
		$.getJSON("/container/list?onlyCurrentHub=true&materialId="+id).done((result)=>this.containers(result.Items || []))



class MaterialViewModel
	constructor:(material)->
		this.model = material
		this.Name = material.get('Name')
		this.Unit = material.get('Unit')
		this.Weight = ''
		this.IsExcluded = material.IsExcluded
		this.Id = material.Id
				

class MaterialsViewModel
	constructor:(model, hubModel)->		
		this.model = model
		this.materialsModel = new (require 'material').ListModel()
		this.materialsModel.filter = (m)=>m.Active

		this.materialsModel.materials.bind 'add', this.updateAvailable
		this.availableMaterials = ko.observableArray []
		this.pickedMaterials = ko.observableArray []
		this.showAvailable = ko.observable false
		this.removedMaterials = {}
		this.hub = hubModel

	reload:(client)=>
		this.availableMaterials([])
		this.pickedMaterials([])
		this.removedMaterials = {}
		cityId = client.get("Address").CityId
		options = {onlyCurrentHub: true, municipality: cityId}
		this.materialsModel.fetch(options)
		
	updateAvailable:(item)=>
		this.availableMaterials.push new MaterialViewModel(item)
	
	showAvailableList:()=>this.showAvailable true

	pickMaterial:(m)=>
		this.availableMaterials.remove m
		this.removedMaterials[m.model.get("Id")] = m
		pickedMaterial = new PickedMaterial()
		pickedMaterial.set("Id", m.model.get("Id"))
		
		this.model.addMaterial(pickedMaterial)
		pickedMaterialVm = new PickedMaterialViewModel(pickedMaterial, m.model, this.hub)
		this.pickedMaterials.push(pickedMaterialVm)	
		this.sortMaterials()
	
	removeMaterial:(m)=>		
		this.model.removeMaterial m.model
		id = m.model.get("Id")
		originalMaterial = this.removedMaterials[id]
		this.availableMaterials.push originalMaterial
		this.pickedMaterials.remove m
		this.sortMaterials()

	sortCallback:(l,r)=>
		return 0 if l.Name == r.Name
		return if l.Name < r.Name then -1 else 1

	sortMaterials:()=>
		this.availableMaterials.sort this.sortCallback
		this.pickedMaterials.sort this.sortCallback


class GiveawayViewModel
	constructor:()->
		this.model = new giveaway.ListModel()
		this.searchTerm = ko.observable ""
		this.selectedItems = ko.observableArray()
		this.showSelectedItems = ko.computed this.computeShowChosenItems
		this.items = kb.collectionObservable(this.model)
		this.model.search()
		
	computeShowChosenItems:()=>
		return this.selectedItems().length

	hideChosenItems:()=>
		this.showChosenItems(false)

	addItem:(item)=>
		if _(this.selectedItems()).find((x)=> x.Id() == item.Id())
			return
		this.selectedItems.push(item)
		this.items.remove(item)

	removeItem:(item)=>
		this.selectedItems.remove(item)
		this.items.push(item)

	getImageUrl:(item)=>
		return '/giveaway/image/' + item.ImageId()

	search:()=>
		this.model.search(this.searchTerm())

class ViewModel
	constructor:(model, clientListViewModel, showClients, hubModel)->
		this.model = model
		this.clientList = clientListViewModel
		this.errors = ko.observableArray()
		this.client = ko.observable(null)
		this.invoicePreview = ko.observable null
		model.Client.subscribe this.onClientSelected
		this.clientId = kb.observable(model,'ClientId')
		this.employeeName = kb.observable(model,'EmployeeName')
		this.comment = kb.observable(model,'Comment')
		this.showList = ko.observable true
		this.showListPoc = ko.observable true
		this.saved = false
		this.attachments = ko.observableArray []
		this.previewAvailable = ko.computed this.computePreviewAvailable
		this.fileUpload =
			dataType: 'json'
			done : this.fileUploaded
		this.clientList.search() if showClients
		this.giveaway = new GiveawayViewModel(model)
		this.materials = new MaterialsViewModel(this.model, hubModel)

	onClientSelected:(client)=>
		this.client new ClientViewModel(client)
		this.materials.reload(client)
		this.model.clearMaterials()
						

	fileUploaded:(e,f)=>
		resp = f.jqXHR.responseText
		if resp == undefined
			resp = f.jqXHR.iframe[0].documentElement.innerText
		result = $.parseJSON(resp)
		att = this.model.get 'Attachments'
		att.push result
		this.attachments.push result

	computePreviewAvailable:()=>this.client() != null
	
	onPreview:(response)=>
		response.CreatedAtFormatted = ko.observable ''
		response.IsOBNL = ko.observable response.IsOBNL
		this.invoicePreview(response)
		this.navigateToPreview

	preview:()=>
		this.errors.removeAll()
		this.model.set('GiveawayItems', this.getGiveawayItems())
		this.model.fetchPreview(this.onPreview, this.onError)

	getGiveawayItems:()=>
		_.map(this.giveaway.selectedItems(), (item)=>
				{id:item.Id()})

	hidePreview:()=>this.invoicePreview null

	navigateToPreview:()=>
		eco.app.router.navigate '/invoice/new/preview'

	save:()=>
		$("#global-loading-fade").modal('show')
		this.model.set('GiveawayItems', this.getGiveawayItems())
		this.model.save(null,{success : this.onSaved, error:this.onError})
	onSaved:()=>
		setTimeoutCallback = -> $("#global-loading-fade").modal('hide')
		setTimeout setTimeoutCallback, 500
		this.saved = true
		eco.app.notifications.addNotification('msg-invoice-saved-successfully');
		eco.app.router.navigate 'invoice/show/'+this.model.get('Id'),{trigger:true}

	changeClient:()=>
		this.showListPoc true
		this.showList true

	editClient:()=> eco.app.router.navigate 'invoice/new/client/edit/'+this.client().Id(),{trigger:true}
	newClient:()=> eco.app.router.navigate 'invoice/new/client/create',{trigger:true}

	onError:(m,errors,data)=>
		setTimeoutCallback = -> $("#global-loading-fade").modal('hide')
		setTimeout setTimeoutCallback, 500
		errors = $.parseJSON errors.responseText
		this.errors.removeAll()
		for item in errors
		  this.errors.push item

	


exports.ViewModel = ViewModel