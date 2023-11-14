
class UserModel extends Backbone.Model
	
	idAttribute : 'Id'
	constructor:(data)->
		super data

	url:()=> 
		return "/user" if this.isNew()
		"/user/index/"+this.id

class CurrentUserModel extends Backbone.Model
	
	idAttribute : 'Id'
	constructor:(data)->
		super data

	url:()=> 
		return "/user/current"

class NewUserModel extends UserModel
	constructor:()->
		super
			Id : null
			HubId : ''
			Login : ''
			Email : ''
			Password : ''
			PasswordConfirmation : ''
			IsReadOnly : false
			IsAdmin : false
			IsGlobalAdmin: false
			IsAgent : true
			Name : ""
			IsLoginAlertEnabled : false


class UserList extends Backbone.Collection
	constructor:(models,options)->
		this.url = '/user'
		this.model = UserModel
		super models,options


exports.ListModel = UserList
exports.Model = UserModel
exports.NewModel = NewUserModel
exports.CurrentUserModel = CurrentUserModel