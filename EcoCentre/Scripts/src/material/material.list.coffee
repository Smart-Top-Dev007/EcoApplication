material = require('material')

class ViewModel
	
	constructor:(model)->
		this.model = model
		this.materials = kb.collectionObservable this.model.materials
		this.materialsActive = ko.computed this.filterActive
		this.materialsInactive= ko.computed this.filterInactive
		this.materialIsExcluded = ko.computed this.filterNotExcluded
		this.materialIsNotExcluded = ko.computed this.filterExcluded
		this.term = kb.observable model, 'term'
		this.searchfocus = ko.observable true

	search:()=>
		this.model.fetch()

	filterActive:()=> ko.utils.arrayFilter this.materials(), (item)=>item.Active()
	filterInactive:()=> ko.utils.arrayFilter this.materials(), (item)=>!item.Active()
	filterExcluded:()=> ko.utils.arrayFilter this.materials(), (item)=>item.IsExcluded()
	filterNotExcluded:()=> ko.utils.arrayFilter this.materials(), (item)=>!item.IsExcluded()

	load:()=>
		this.model.fetch()
	editUrl:(m)=>
		'#material/edit/'+m.Id()

	
	enable:(vm)->vm.model().enable()
	disable:(vm)->vm.model().disable()
	exclude:(vm)->vm.model().exclude()
	include:(vm)->vm.model().include()


exports.createViewModel = ()=> 
	model = new material.ListModel()
	new ViewModel(model)