common = require('common')
class InvoiceList extends Backbone.Model
	invoices : new Backbone.Collection()
	pageButtons : new Backbone.Collection()
	urlRoot : '/invoice/index'
	constructor:()->
		this.invoices.url = '/invoice/index'
		super
			filterTerm : ''
			filterTermFrom : null
			filterTermTo : null
			filterType : 'invoiceNo'
			filterDeleted : false
			CurrentYear : true
			sortBy : 'invoiceDate'
			sortDir : 'desc'
			userId : ''
			page : 1
			pageCount: 1
			pageSize: null
			centerName : 'Tous'
			total: 0
		
	loadForUser:(id)=>
		this.set 'userId',id
		this.search()

	parse:(resp)=>
		this.invoices.reset()
		this.set 'total', resp.Total
		for item in resp.Invoices
			model = new InvoiceModel
			model.set item
			this.invoices.push model

		this.pageButtons.reset()
		pages = common.generatePages(resp.Page,resp.PageCount)
		this.set 'page', resp.Page
		this.set 'pageCount', resp.PageCount
		for page in pages
			this.pageButtons.push page

	search:()=>
		this.changePage(1)

	changePage:(page)=>
		this.set 'page', page
		from = this.get 'filterTermFrom'
		to = this.get 'filterTermTo'
		from = from.toString('yyyy-MM-dd') if from?
		to = to.toString('yyyy-MM-dd') if to?
		$loadingFade = $("#global-loading-fade");
		$loadingFade.modal('show');
		fetchAjax = this.fetch data:
			page : this.get 'page'
			pageSize: this.get 'pageSize'
			userId : this.get 'userId'
			sortDir : this.get 'sortDir'
			sortBy : this.get 'sortBy'
			CurrentYear : this.get 'CurrentYear'
			Deleted : this.get 'filterDeleted'
			Type : this.get 'filterType'
			Term : this.get 'filterTerm'
			TermFrom : from
			TermTo : to,
			CenterName : this.get 'centerName'
		fetchAjax.complete () =>
		  setTimeoutCallback = -> $loadingFade.modal('hide')
		  setTimeout setTimeoutCallback, 500
		  return fetchAjax;


class InvoiceModel extends Backbone.Model
	urlRoot : '/invoice/index'
	idAttribute : 'Id'
	constructor:()->
		super
			InvoiceNo : ''
			Id : ''
			CreatedAt : ''
			Comment : ''
			CreatedBy : null
			Attachments : []
			Materials : []
			GiveawayItems : []
			IsOBNL : false
			AmountIncludingTaxes: ''
			Amount: ''
			Address : new Backbone.Model
				City:'c'
				CItyId:'i'
				Street:'d'
				CivicNumber:'s'
				AptNumber:''
				PostalCode:'a'
			Client:
				FirstName : ''
				LastName : ''
				PhoneNumber : ''
				Address : new Backbone.Model
					City:''
					CItyId:''
					Street:''
					CivicNumber:''
					AptNumber:''
					PostalCode:''
			Center:
				Name: ''
				Url: ''
			Taxes: []
			VisitNumber: 0
			VisitLimit: null
	fetchById:(id)=>
		this.fetch {data:{id:id} }
	url:()=> 
		return "/invoice" if this.isNew()
		"/invoice/index/"+this.id

class PickedMaterial extends Backbone.Model
	constructor:()->
		super
			Id : ''
			Quantity : 0
			Container: ''			
			ProvidedProofOfResidence: false
	

class NewInvoiceModel extends Backbone.Model
	url : 'invoice'
	constructor:()->
		this.Client = ko.observable(null)
		super
			Id : ''
			Comment : ''
			CreatedBy: null
			ClientId : 0
			EmployeeName : 0
			Materials : []
			Attachments:[]
			GiveawayItems : []
			
	selectClient:(client)=>
		this.Client client
		this.set
			ClientId : client.get 'Id'

			
	addMaterial:(material)=>
		this.get('Materials').push material

	clearMaterials:()=>
		this.set
			'Materials': []

	removeMaterial:(material)=>
		newArray = this.get('Materials')

		newArray = if newArray.length <= 1 then [] else  $.grep(newArray, (v)=>v.get('Id') != material.get('Id'))
		this.set 
			'Materials':newArray
	fetchPreview:(success, error)=>
		options = {};
		options.url = '/invoice/preview'
		options.success = success
		options.error = error
		model = new InvoiceModel()
		Backbone.sync.apply(model, ['create', this, options])

class NewInvoiceModel1 extends Backbone.Model
	url : '/invoice/new1'
	constructor:()->
		this.Client = ko.observable(null)
		super
			Id : ''
			Comment : ''
			CreatedBy: null
			ClientId : 0
			EmployeeName : 0
			Materials : []
			Attachments:[]
			GiveawayItems : []
			
	selectClient:(client)=>
		this.Client client
		this.set
			ClientId : client.get 'Id'

			
	addMaterial:(material)=>
		this.get('Materials').push material

	clearMaterials:()=>
		this.set
			'Materials': []

	removeMaterial:(material)=>
		newArray = this.get('Materials')

		newArray = if newArray.length <= 1 then [] else  $.grep(newArray, (v)=>v.get('Id') != material.get('Id'))
		this.set 
			'Materials':newArray
	fetchPreview:(success, error)=>
		options = {};
		options.url = '/invoice/preview'
		options.success = success
		options.error = error
		model = new InvoiceModel()
		Backbone.sync.apply(model, ['create', this, options])
  

exports.InvoiceList = InvoiceList
exports.InvoiceModel = InvoiceModel
exports.NewInvoiceModel = NewInvoiceModel
exports.PickedMaterial = PickedMaterial