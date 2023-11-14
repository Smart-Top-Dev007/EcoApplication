class OBNLMaterialRow
	constructor:(data)->
		@Name = data.Name
		@Weight = data.Weight

class Model extends Backbone.Model
	url : 'obnlmaterials'

	constructor:()->
		@CurrentMaterials = new Backbone.Collection()
		super {}
	parse:(resp)=>
		@CurrentMaterials.reset()
		for material in resp
			@CurrentMaterials.push new OBNLMaterialRow(material)

	fetchByClientId:(id)=>
		@fetch {data:{clientId:id}}

exports.Model = Model