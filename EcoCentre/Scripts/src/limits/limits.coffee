class LimitRow
	constructor:(data)->
		this.Name = data.MaterialName
		this.Quantity = data.QuantitySoFar
		this.MaxYearlyAmount = data.MaxQuantity
		this.Unit = data.Unit
		this.IsExcluded = data.IsExcluded
		this.BaseProgress = (this.Quantity/this.MaxYearlyAmount)*100
		if this.BaseProgress > 100
			this.BaseProgress = 100


class Model extends Backbone.Model
	url : 'limits'
	idAttribute : 'Id'

	constructor:()->
		this.CurrentLimits = new Backbone.Collection()
		super {}
	parse:(resp)=>
		this.CurrentLimits.reset()
		for limit in resp.CurrentLimits
			this.CurrentLimits.push new LimitRow(limit)

	fetchByClientId:(id)=>
		this.fetch {data:{clientId:id}}
	fetchById:(id)=>
		this.fetch {data:{id:id} }

exports.Model = Model