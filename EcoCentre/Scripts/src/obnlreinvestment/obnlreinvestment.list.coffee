class OBNLReinvestmentRowViewModel
	constructor:(data)->
		this.model = data;
		this.OBNLReinvestmentNo = data.get 'OBNLReinvestmentNo'
		this.Id = data.get 'Id'
		client = data.get 'Client'
		center = data.get 'Center'
		this.CenterUrl = center.Url
		if client == undefined or client == null
			this.Client = 
				FirstName : ''
				Category : ''
				LastName : ''
				Address : 
				  City : ''
				  CivicNumber : ''
				  Street : ''
		else
			this.Client = client
		this.onUndeleted = null
			
		ca = data.get 'CreatedAt'
		res = ca.match(/\d+/g)[0]
		ca = new Date(res * 1)
		this.CreatedAt = ca.toString('yyyy-MM-dd hh:mm:ss')
	destroy:()=>
		return unless confirm 'êtes-vous sûr de vouloir supprimer la facture'
		this.model.destroy()
	showUrl:()=>'#obnlreinvestment/show/'+this.Id
	
	undelete:()=>
		data = 
			id: this.Id
		$.post '/obnlreinvestment/undelete', data, this.undeleted

	undeleted:(data)=>
		return unless this.onUndeleted?
		this.onUndeleted(data)


class ViewModel
	
	constructor:(model)->
		this.model = model
		vmItemFactory = (data)=> 
				item = new OBNLReinvestmentRowViewModel(data)
				item.onUndeleted = this.load
				item
			
		this.items = kb.collectionObservable this.model.obnlReinvestment, {create :vmItemFactory}
		this.pageButtons = kb.collectionObservable this.model.pageButtons
		this.page = kb.observable(model,'page')
		this.pageCount = kb.observable(model,'pageCount')
		this.throttledPage = ko.computed(this.page).extend({ throttle: 400 })
		this.pageSize = kb.observable(model, 'pageSize')
		this.term= kb.observable(model,'filterTerm')
		this.termFrom = kb.observable(model,'filterTermFrom')
		this.termTo = kb.observable(model,'filterTermTo')
		this.searchType= kb.observable(model,'filterType')
		this.sortBy = kb.observable(model,'sortBy')
		this.sortDir = kb.observable(model,'sortDir')
		this.searchfocus = ko.observable true
		this.deleted = kb.observable(model,'filterDeleted')
		this.isCurrentYear = kb.observable(model,'CurrentYear')
		this.centerName = kb.observable(model, 'centerName')

		this.showDateFields = ko.observable false
		this.searchType.subscribe this.computeShowDateFields
		@throttledPage.subscribe @goToPage

	computeShowDateFields:()=> 
		res = this.searchType() == "OBNLReinvestmentDate"
		this.showDateFields res
	goToPage:(val)=> 
        if val and val > 1 and val < this.pageCount() 
            return this.changePage({ number: val })
      
	load:()=> this.model.changePage(1)
        
	changePage:(vm)=>
		page = if vm? then vm.number else 1
		this.model.changePage(page)		   

	search:()=>this.model.search()

	toggleCurrentYear:()=>
		this.isCurrentYear(true)
		this.load()

	togglePreviousYears:()=>
		this.isCurrentYear(false)
		this.load()

	sort:(sortBy)=>
		if sortBy != this.sortBy()
			this.sortBy sortBy
			this.sortDir 'desc'
		else
			this.sortDir( if this.sortDir() == 'desc' then 'asc' else 'desc')
		this.model.changePage(1)
		

exports.createViewModel = ()=>
	model = new (require('obnlreinvestement')).OBNLReinvestmentsList()
	new ViewModel(model)