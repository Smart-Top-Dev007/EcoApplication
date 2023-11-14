common = require('common')

class ClientLastNameAutocompleteViewModel
	constructor:(value)->
		this.value = value

	select:(e,i)=>
		this.value i.item.label
		return false

	change:(e)=> return false

	source:( request, response )=>
		url = '/client/suggest'
		data = {term: request.term}
		$.get url,data,(cat)=>response(cat)

class ClientStreetNameAutocompleteViewModel
	constructor:(number, streetName, postalCode, cityId, hubId)->
		this.number = number
		this.streetName = streetName
		this.number = number
		this.postalCode = postalCode
		this.cityId = cityId
		this.hubId = hubId

	select:(e,i)=>
		this.streetName i.item.label
		return false

	change:(e)=> return false

	source:( request, response )=>
		url = '/client/suggeststreet'
		hubId = this.hubId
		hubId = hubId() if typeof hubId == "function"
		hubId = "" if !hubId
		data = {streetName: request.term || '', cityId: this.cityId|| '', hubId: hubId|| ''}
		$.get url,data,(cat)=>response(cat)

class ClientCivicNumberAutocompleteViewModel
	constructor:(number, streetName, postalCode)->
		this.number = number
		this.streetName = streetName
		this.number = number
		this.postalCode = postalCode

	select:(e,i)=>
		this.number i.item.label
		return false

	change:(e)=> return false

	source:( request, response )=>
		url = '/client/suggestcivicnumber'
		data = {number: request.term || '', streetName: this.streetName || '', postalCode: this.postalCode || ''}
		$.get url,data,(cat)=>response(cat)

class ClientCivicCardAutocompleteViewModel
	constructor:(card)->
		this.card = card

	select:(e,i)=>
		this.card i.item.label
		return false

	change:(e)=> return false

	source:( request, response )=>
		url = '/client/suggestciviccard'
		data = {number: request.term || ''}
		$.get url,data,(cat)=>response(cat)

class ClientPostalCodeAutocompleteViewModel
	constructor:(number, streetName, postalCode)->
		this.number = number
		this.streetName = streetName
		this.number = number
		this.postalCode = postalCode

	select:(e,i)=>
		this.postalCode i.item.label
		return false

	change:(e)=> return false

	source:( request, response )=>
		url = '/client/suggestpostalcode'
		data = {number: this.number || '', streetName: this.streetName || '', postalCode: request.term || ''}
		$.get url,data,(cat)=>response(cat)


class CategoriesModel extends Backbone.Model
	url:'ClientCategories'
	constructor:()->
		super()
		this.list = new Backbone.Collection()
	parse:(resp)=>
		for item in resp
			this.list.push item
	showFirstNameLastName:(category)=>
		for item in this.items
			return item.ShowFirstNameLastName() if item.CategoryName() == category
		true

	showOrganizationName:(category)=>
		for item in this.items
			return item.ShowOrganizationName() if item.CategoryName() == category
		true

	showOBNLNumber:(category)=>
		for item in this.items
			return item.ShowOBNLNumber() if item.CategoryName() == category
		true

class ClientModel extends Backbone.DeepModel
	urlRoot : 'client/index'
	idAttribute : 'Id'

	constructor:(data = null)->
		this.Invoices = new Backbone.Collection()
		this.ExcludedInvoices = new Backbone.Collection()
		this.IncludedInvoices = new Backbone.Collection()
		this.OBNLReinvestments = new Backbone.Collection()
		this.ExcludedOBNLReinvestments = new Backbone.Collection()
		this.IncludedOBNLReinvestments = new Backbone.Collection()
		super
			AllowCredit : false
			CreditAcountNumber : ''
			AllowAddressCreation : false
			Id : null
			RefId : null
			FirstName : ''
			LastName : ''
			OBNLNumber : ''
			OBNLNumbers : []
			Category : 'Resident'
			CategoryCustom : ''
			Email: ''
			PhoneNumber : ''
			MobilePhoneNumber : ''
			Hub :
				Id : null
				Name: ''
			Comments : ''
			PersonalVisitsLimit : 0
			Status : ''
			Verified : ''
			LastChange:''
			Address:
				Street : ''
				City : ''
				CityId : ''
				CivicNumber : ''
				AptNumber : ''
				PostalCode : ''
				ExternalId : ''
				NewCityName : ''
			UpdateOnlyStatus: false
			IsRegisteredInCurrentHub : true
		this.parse(data) if data?

	parse:(resp)=>
		if resp.Invoices
			for inv in resp.Invoices
				inv.CenterUrl = inv.Center.Url
				this.Invoices.push inv
				if inv.IsExcluded then this.ExcludedInvoices.push inv else this.IncludedInvoices.push inv
		
		if resp.OBNLReinvestments
			for reinv in resp.OBNLReinvestments
				reinv.CenterUrl = reinv.Center.Url
				this.OBNLReinvestments.push reinv
				if reinv.IsExcluded then this.ExcludedOBNLReinvestments.push reinv else this.IncludedOBNLReinvestments.push reinv
		this.set
			AllowCredit : resp.AllowCredit
			CreditAcountNumber : resp.CreditAcountNumber
			AllowAddressCreation : resp.AllowAddressCreation
			Id : resp.Id
			RefId : resp.RefId
			FirstName : resp.FirstName
			LastName : resp.LastName
			OBNLNumber : resp.OBNLNumber
			OBNLNumbers : resp.OBNLNumbers,
			LastOBNLVisit: @parseDate(resp.LastOBNLVisit),
			Category : resp.Category
			Email: resp.Email
			PhoneNumber : resp.PhoneNumber
			MobilePhoneNumber : resp.MobilePhoneNumber
			Comments : resp.Comments
			CategoryCustom : resp.CategoryCustom
			Status : resp.Status
			LastChange: resp.LastChange
			Verified : resp.Verified
			PersonalVisitsLimit : resp.PersonalVisitsLimit
			IsRegisteredInCurrentHub : resp.IsRegisteredInCurrentHub
		this.set
			'Address.Street' : resp.Address.Street
			'Address.City' : resp.Address.City
			'Address.CityId' : resp.Address.CityId
			'Address.CivicNumber' : resp.Address.CivicNumber
			'Address.AptNumber' : resp.Address.AptNumber
			'Address.PostalCode' : resp.Address.PostalCode
			'Address.AllowAddressCreation' : resp.Address.AllowAddressCreation
			'Address.ExternalId' : resp.Address.ExternalId
		if resp.Hub
			this.set
				'Hub.Id' : resp.Hub.Id
				'Hub.Name' : resp.Hub.Name
		this.change()
		this.attributes
	parseDate:(item)=>
		if not item
			return
		res = item.match(/\d+/g)[0]
		ca = new Date(res * 1)
		ca.toString('yyyy-MM-dd')

class ClientList extends Backbone.Model
	clients : new Backbone.Collection()
	pageButtons : new Backbone.Collection()
	url : 'client'
	constructor:()->
		super
			filterFirstName : ''
			filterLastName : ''
			filterAddress: ''
			filterCivicNumber: ''
			filterLastVisitFrom : ''
			filterLastVisitTo : ''
			filterFirstLetter : ''
			filterHubId : ''
			filterOBNLNumber : ''
			filterCategory : 'All'
			filterActive : true
			filterInactive : false
			noCommercial : false
			sortBy : 'LastName'
			sortDir : 'Asc'
			searchType : 'Address'
			filterType : 'active'
			term : ''
			page: 1
			pageCount: 1
			pageSize: null

	parse:(resp)=>
		this.clients.reset()
		for item in resp.Clients
				this.clients.push new ClientModel(item)
		this.pageButtons.reset()

		pages = common.generatePages(resp.Page,resp.PageCount)
		this.set 'page', resp.Page
		this.set 'pageCount', resp.PageCount
		for page in pages
			this.pageButtons.push page

	search:()=>
		this.changePage(1)

	changePage:(page)=>
		data =
			page : page
			pageSize : this.attributes.pageSize
			firstName : this.attributes.filterFirstName
			lastName : this.attributes.filterLastName
			address : this.attributes.filterAddress
			civicNumber : this.attributes.filterCivicNumber
			OBNLNumber : this.attributes.filterOBNLNumber
			firstLetter : this.attributes.filterFirstLetter
			hubId: this.attributes.filterHubId
			categoryFilter: this.attributes.filterCategory
			active : this.attributes.filterActive
			inactive: this.attributes.filterInactive
			noCommercial : this.attributes.noCommercial
			sortDir : this.attributes.sortDir
			sortBy : this.attributes.sortBy
			lastVisitFrom : this.attributes.filterLastVisitFrom.toString('yyyy-MM-dd')
			lastVisitTo : this.attributes.filterLastVisitTo.toString('yyyy-MM-dd')
			term : this.attributes.term
			searchType : this.attributes.searchType

		$loadingFade = $("#global-loading-fade");
		$loadingFade.modal('show');
		fetchAjax = this.fetch {data:data}
		fetchAjax.complete () =>
				setTimeoutCallback = -> $loadingFade.modal('hide')
				setTimeout setTimeoutCallback, 500
				return fetchAjax;


exports.ClientList = ClientList
exports.Model = ClientModel
exports.CategoriesModel = CategoriesModel
exports.ClientLastNameAutocompleteViewModel = ClientLastNameAutocompleteViewModel
exports.ClientStreetNameAutocompleteViewModel = ClientStreetNameAutocompleteViewModel
exports.ClientCivicNumberAutocompleteViewModel = ClientCivicNumberAutocompleteViewModel
exports.ClientCivicCardAutocompleteViewModel = ClientCivicCardAutocompleteViewModel
exports.ClientPostalCodeAutocompleteViewModel = ClientPostalCodeAutocompleteViewModel