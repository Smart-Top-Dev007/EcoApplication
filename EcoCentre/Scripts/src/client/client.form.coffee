client = require 'client'

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
		this.AptNumber = kb.observable model, 'Address.AptNumber'
		this.CivicNumber = kb.observable model, 'Address.CivicNumber'
		this.AptNumber = kb.observable model, 'Address.AptNumber'
		this.CityId = kb.observable model, 'Address.CityId'
		this.NewCityName = kb.observable model, 'Address.NewCityName'
		this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode, this.CityId)
		this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode)
		this.postalCodeAutocomplete = new client.ClientPostalCodeAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode)
		
		model.on 'change:Address.PostalCode', this.updatePostalCode
		model.on 'change:Address.Street', this.updateStreet
		model.on 'change:Address.City', this.updateCity
		model.on 'change:Address.CityId', this.updateCityId
		model.on 'change:Address.CivicNumber', this.updateCivicNumber
		model.on 'change:Address.AptNumber', this.updateAptNumber
		
	updatePostalCode:(m,v)=> 
		this.PostalCode v
	updateStreet:(m,v)=> this.Street v
	updateCity:(m,v)=> this.City v
	updateAptNumber:(m,v)=> this.AptNumber v
	updateCivicNumber:(m,v)=> 
		this.CivicNumber v
	updateCityId:(m,v)=> this.CityId v



class ViewModel extends kb.ViewModel
	constructor:(model, municipalities,categoriesModel,hubs, currentUserModel)->
		super(model)
		this.municipalitiesModel = municipalities
		this.address = ko.observable new AddressViewModel(model)
		this.hub = ko.observable new HubViewModel(model)
		this.errors = ko.observableArray([])
		this.OBNLNumbersInputed = ko.observableArray([])
		this.categoriesModel = categoriesModel
		this.currentUserModel = currentUserModel

		this.Municipalities = ko.observableArray([])
		municipalities.on('reloaded', this.onMunicipalitiesReloaded)
		this.onMunicipalitiesReloaded()
		this.Categories = kb.collectionObservable(categoriesModel.list)
		this.showFirstNameLastName = ko.computed this.computeShowFirstNameLastName
		this.hubs = kb.collectionObservable(hubs.hubs)
		this.showHub = ko.computed this.computeShowHubs
		this.showOrganizationName = ko.computed this.computeShowOrganizationName
		this.showCustomCategoryName = ko.computed this.computeShowCustomCategoryName
		this.showOBNLNumber = ko.computed this.computeShowOBNLNumber
		this.allowAddressCreation = kb.observable model, 'AllowAddressCreation'
		this.NewCityName = ko.observable ''

	computeShowCustomCategoryName:()=> this.Category() == "Other"
	computeShowHubs :()=>this.hubs().length > 0

	onMunicipalitiesReloaded:()=>
		this.Municipalities.removeAll()
		for m in (this.municipalitiesModel.municipalities.models)
			this.Municipalities.push(m.attributes)
		this.Municipalities.push({Id:"", Name:"Autre"})

	computeShowFirstNameLastName:()=>
		for item in this.Categories()
			return item.ShowFirstNameLastName() if item.CategoryName() == this.Category()
		true

	computeShowOrganizationName:()=>
		for item in this.Categories()
			return item.ShowOrganizationName() if item.CategoryName() == this.Category()
		false

	computeShowOBNLNumber:()=> this.Category() == 'OBNL'

	addNewOBNL:()=> 
		if this.OBNLNumber()
				if this.OBNLNumbers() == null
						this.OBNLNumbers([])
				this.OBNLNumbers().push this.OBNLNumber()
				this.OBNLNumbersInputed.push this.OBNLNumber()
				this.OBNLNumber('')

	isNew:()=>
		this.model().isNew()
	load:(id)=>
		this.model().fetch { data : {id:id}}
	loadNew:(id)=>
		this.currentUserModel.fetch({success: this.onCurrentUserLoaded})
	onCurrentUserLoaded:()=>
		this.model().set('Hub.Id', this.currentUserModel.get("HubId"))
		this.model().set('Address.CityId', this.currentUserModel.get("DefaultCityId"))
	save:()=>
		this.model().save null, {success : this.onSaved, error:this.onError}

	onSaved:(data)=>
		eco.app.notifications.addNotification('msg-client-saved-successfully');
		eco.app.router.navigate 'client/index',{trigger:true}

	onError:(m,errors,data)=>
		errors = $.parseJSON errors.responseText
		if errors.length == 1 and errors[0].PropertyName == 'AllowAddressCreation'
			confirmationText = kb.locale_manager.get("This address already exists in the database. Are you sure you want to create a customer at this address?")
			if confirm(confirmationText)
				this.model().set('AllowAddressCreation', true)
				this.save()
		else
			this.errors.removeAll()
			for item in errors
				this.errors.push item

exports.createViewModel = ()=>
	clientModule = require('client')
	model = new clientModule.Model()
	municipalities = new (require('municipality')).ListModel()
	municipalities.filter = (item)=> item.Enabled
	municipalities.fetch({async:false})
	hubs = new (require('hub')).ListModel()
	hubs.fetch()
	categories = new clientModule.CategoriesModel()
	categories.fetch()
	currentUserModel = new (require('user')).CurrentUserModel()
	new ViewModel(model,municipalities,categories,hubs, currentUserModel)
