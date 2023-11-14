class MunicipalityModel extends Backbone.Model
	url : 'municipality'
	constructor:()->
		this.url = 'municipality'
		this.idAttribute = 'Id'
		super
			Id : null
			Name : '',
			Enabled : false
	enable:(vm)=>
		this.set 
			Enabled : true
		this.save()
	disable:(vm)=>
		this.set 
			Enabled : false
		this.save()

class MunicipalityListModel extends Backbone.Model
	url : 'municipality'
	municipalities : new Backbone.Collection()
	filter : null
	parse:(resp)=>
		this.municipalities.reset()
		for item in resp
			continue if this.filter != null and !this.filter(item)
			row = new MunicipalityModel();
			row.set item
			this.municipalities.push row
		this.trigger 'reloaded'

exports.Model = MunicipalityModel
exports.ListModel = MunicipalityListModel