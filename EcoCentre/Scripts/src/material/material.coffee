class MaterialModel extends Backbone.Model
	url : 'material'
	idAttribute : 'Id'
	constructor:()->
		super
			Id : null
			Name : ''
			Tag : ''
			Unit : ''
			Price : 0
			Active : false
			MaxYearlyAmount : 100
			IsExcluded : false
	enable:(vm)=>
		this.set 
			Active : true
		this.save()
	disable:(vm)=>
		this.set 
			Active : false
		this.save()
	exclude:(vm)=>
	    this.set
	        IsExcluded : true
	    this.save()
	include:(vm)=>
	    this.set
	        IsExcluded : false
	    this.save()
class ListModel extends Backbone.Model

	url : 'material'
	
	filter : null
	constructor:()->
		super
			term : ''
		this.materials = new Backbone.Collection()

	fetch:(options)=>
		options = options || {}
		super data : 
			term : this.get 'term'
			onlyCurrentHub: options.onlyCurrentHub
			municipality: options.municipality

	parse:(resp)=>
		this.materials.reset()
		for item in resp
			continue if this.filter != null and !this.filter(item)
			model = new MaterialModel()
			model.set item
			this.materials.push model

exports.ListModel = ListModel