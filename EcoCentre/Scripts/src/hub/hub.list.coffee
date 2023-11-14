
class ViewModel
	
	constructor:(model)->
		this.model = model
		this.hubs = kb.collectionObservable this.model.hubs

	load:()=>
		this.model.fetch()


exports.createViewModel = ()=>
	model = new (require('hub')).ListModel()
	new ViewModel(model)