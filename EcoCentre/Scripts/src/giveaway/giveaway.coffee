class GiveawayModel extends Backbone.Model
	url : 'giveaway/index'
	defaults :
		Id : null
		Title : ''
		Description : ''
		Price: 0
	constructor:(args)->
		this.url = 'giveaway/index'
		this.idAttribute = 'Id'
		super(args)

class GiveawayListModel extends Backbone.Collection
	constructor:(models,options)->
		this.url = 'giveaway/list'
		this.model = GiveawayModel
		super models,options
	
	search:(term)=>
		this.fetch({ data: $.param({ q: term, isGivenAway: false, onlyCurrentHub: true}) });

exports.Model = GiveawayModel
exports.ListModel = GiveawayListModel