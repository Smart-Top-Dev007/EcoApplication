
class ViewModel extends kb.ViewModel
	constructor:(model, hubsModel)->
		super(model)
		this.errors = ko.observableArray([])
		this.isNew = ko.observable true
		this.hubs = kb.collectionObservable(hubsModel.hubs)

		isAdminTrueObject=
			read:()->(this.IsAdmin() == true).toString()
			write:(val)->if val=="true" then this.IsAdmin true else this.IsAdmin false
		this.isAdminTrue = ko.computed isAdminTrueObject, this
		model.on 'change', this.updateNew


	updateNew:()=>
		this.IsAdmin(!!this.IsAdmin())
		this.IsAgent(!(!!this.IsAdmin()))
		if this.IsAgent()
			this.IsGlobalAdmin false
		this.isNew this.model().isNew()

	load:(id)=> this.model().fetch { data : {id:id}}
	save:()=>
		this.model().save null, {success : this.onSaved, error:this.onError}

	onSaved:(data)=>
		eco.app.router.navigate 'user/index',{trigger:true}

	onError:(m,errors,data)=>
		errors = $.parseJSON errors.responseText
		this.errors.removeAll()
		for item in errors
			this.errors.push item

exports.createViewModel = ()=>
	model = new (require('user')).NewModel()
	hubsModel = new (require('hub')).ListModel()
	hubsModel.fetch()
	new ViewModel(model, hubsModel)
