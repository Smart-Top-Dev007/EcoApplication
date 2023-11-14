common = require('common')
class Report extends Backbone.Model
	constructor:(name)->
		super
			page : 1,
			pageSize: null,
			from : (1).month().ago()
			material : ''
			to : Date.today()
			city : 0
			orderDir : 'Desc'
			orderBy : 'Date'
			processing : false,
			HubId : null
			pageCount : 1
		this.items = new Backbone.Collection()

		this.pageButtons = new Backbone.Collection()
		this.url = 'reports/'+name
		this.invoiceCount = 0
		this.uniqueAddressCount = 0
		this.totalAmountIncludingTaxes = 0

	parse:(resp)=>
		this.items.reset()
		this.invoiceCount = resp.Summary.InvoiceCount
		this.uniqueAddressCount = resp.Summary.UniqueAddressCount
		this.totalAmountIncludingTaxes = resp.Summary.TotalAmountIncludingTaxes
		resp = resp.Report
		for item in resp.Items
			this.items.push item
		this.pageButtons.reset()
		pages = common.generatePages(resp.Page,resp.PageCount)
		this.set 'pageCount', resp.PageCount
		for page in pages
			this.pageButtons.push page
		@set {processing:false}

	serializeArguments:(xls)=>
		return {
		from : this.get('from').toString('yyyy-MM-dd')
		to : this.get('to').toString('yyyy-MM-dd')
		city: this.get('city')
		page : this.get('page')
		pageSize : this.get('pageSize')
		material : this.get('material')
		xls : xls
		HubId : this.get('HubId')
		OrderBy : this.get('orderBy')
		OrderDir : this.get('orderDir')
		}

	download:()=>
		window.location = this.url + "?" + $.param(this.serializeArguments(true))

	fetch:()=>
		@set {processing:true}
		super data : this.serializeArguments(false)

class ViewModel

	constructor:(model,municipalities,hubs)->
		this.model = model
		this.city = kb.observable model, 'city'
		this.HubId = kb.observable model, 'HubId'
		this.municipalities = kb.collectionObservable(municipalities.municipalities)
		this.from = kb.observable model, 'from'
		this.to = kb.observable model, 'to'
		this.orderBy = kb.observable model, 'orderBy'
		this.orderDir = kb.observable model, 'orderDir'
		this.processing = kb.observable model, 'processing'
		this.material = kb.observable model, 'material'
		this.hubs = kb.collectionObservable(hubs.hubs)
		this.showHub = ko.computed(this.computeShowHubs)
		this.page = kb.observable model, 'page'
		this.pageSize = kb.observable model, 'pageSize'
		this.items = kb.collectionObservable model.items
		this.pageButtons = kb.collectionObservable this.model.pageButtons
		this.invoiceCount = kb.observable model, 'invoiceCount'
		this.uniqueAddressCount = kb.observable model, 'uniqueAddressCount'
		this.totalAmountIncludingTaxes = kb.observable model, 'totalAmountIncludingTaxes'
		this.pageCount = kb.observable(model,'pageCount')
		this.throttledPage = ko.computed(this.page).extend({ throttle: 400 })
		@throttledPage.subscribe @goToPage

	computeShowHubs:()=>return this.hubs().length > 0

	generate:()=>this.load()

	generateXls:()=>this.model.download()

	load:()=>
		this.page 1
		this.model.fetch()

	sort:(orderBy)=>
		if this.orderBy() != orderBy
			this.orderBy orderBy
			this.orderDir 'Asc'
		else
			this.orderDir( if this.orderDir() == 'Asc' then 'Desc' else 'Asc')
		this.load()

	changePage:(vm)=>
		this.page vm.number
		this.model.fetch()

	goToPage:(val)=>
		val = Number(val)
		if val and val >= 1 and val <= this.pageCount() and val != this.page()
			return this.changePage({ number: val })


exports.createViewModel = ()=>
	municipalities = new (require('municipality')).ListModel()
	municipalities.filter = (item)=> item.Enabled
	municipalities.fetch()
	hubs = new (require('hub')).ListModel()
	hubs.fetch()
	model = new Report('journal')
	new ViewModel(model,municipalities,hubs)
