class HubModel extends Backbone.Model
	url : 'hub'
	defaults :
		Id : null
		Name : ''
		InvoiceIdentifier : ''
		Address : ''
		DefaultGiveawayPrice: 0
	constructor:(args)->
		this.url = 'hub'
		this.idAttribute = 'Id'
		super(args)
	fetchCurrent:()=>
		this.fetch({url: "hub/current", async:false})


class HubListModel extends Backbone.Model
	url : 'hub'
	hubs : new Backbone.Collection()
	parse:(resp)=>
		this.hubs.reset()
		this.hubList = this.hubs
		for item in resp
			row = new HubModel(item);
			this.hubs.push row

exports.Model = HubModel
exports.ListModel = HubListModel