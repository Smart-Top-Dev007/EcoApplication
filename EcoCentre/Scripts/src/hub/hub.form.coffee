class ViewModel
	constructor:(model)->
		this.model = model
		this.id = kb.observable(model,'Id')
		this.name = kb.observable(model,'Name')
		this.invoiceIdentifier = kb.observable(model,'InvoiceIdentifier')
		this.defaultGiveawayPrice = kb.observable(model,'DefaultGiveawayPrice')
		this.address = kb.observable(model,'Address')
		this.isNew = ko.observable true
		model.on 'change', this.updateNew

	updateNew:()=>
		this.isNew this.model.isNew()

	load:(id)=> this.model.fetch { data : {id:id}}
	save:()=> 
		this.model.save()
		window.location.hash = 'hub/index';


exports.createViewModel = ()=> 
	module = require('hub')
	model = new module.Model() 
	new ViewModel(model)
