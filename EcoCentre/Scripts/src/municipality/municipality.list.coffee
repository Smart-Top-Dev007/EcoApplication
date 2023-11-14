
class ViewModel
	
	constructor:(model)->
		this.model = model
		this.municipalities = kb.collectionObservable this.model.municipalities
		this.municipalitiesActive = ko.computed this.filterActive
		this.municipalitiesInactive = ko.computed this.filterInactive

	filterActive:()=> ko.utils.arrayFilter this.municipalities(), (item)=>item.Enabled()
	filterInactive:()=> ko.utils.arrayFilter this.municipalities(), (item)=>!item.Enabled()

	load:()=>
		this.model.fetch()
	editUrl:(m)=>
		'#municipality/edit/'+m.Id()

	enable:(vm)->vm.model().enable()
	disable:(vm)->vm.model().disable()


exports.createViewModel = ()=>
	model = new (require('municipality')).ListModel()
	new ViewModel(model)