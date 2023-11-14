class ViewModel
	constructor:(model)->
		this.model = model
		this.id = kb.observable(model,'Id')
		this.name = kb.observable(model,'Name')
		this.isNew = ko.observable true
		model.on 'change', this.updateNew

	updateNew:()=>
		this.isNew this.model.isNew()

	load:(id)=> this.model.fetch { data : {id:id}}
	save:()=> 
		this.model.save()
		window.location.hash = 'municipality/index';


exports.createViewModel = ()=> 
	module = require('municipality')
	model = new module.Model() 
	new ViewModel(model)
