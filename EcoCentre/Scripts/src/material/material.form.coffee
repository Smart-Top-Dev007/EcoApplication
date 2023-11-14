
class MaterialModel extends Backbone.Model
	constructor:()->
		this.url = 'material'
		this.idAttribute = 'Id'
		super
			Id : null
			Name : ''
			Tag : ''
			Unit : ''
			Price : 0
			MaxYearlyAmount : 100
			IsExcluded : false



class ViewModel
	constructor:(model)->
		this.model = model
		this.id = kb.observable(model,'Id')
		this.tag = kb.observable(model,'Tag')
		this.name = kb.observable(model,'Name')
		this.price = kb.observable(model,'Price')
		this.unit = kb.observable(model,'Unit')
		this.maxYearlyAmount = kb.observable(model,'MaxYearlyAmount')
		this.isExcluded = kb.observable(model, 'IsExcluded')
	isNew:()=>this.id() == null 
	load:(id)=> this.model.fetch { data : {id:id}}
	loadNew:()=>
		this.globalSettingsModel = new (require('globalsettings')).Model()
		this.globalSettingsModel.fetch({ async: false })
		this.unit(this.globalSettingsModel.get('DefaultMaterialUnit'))
	save:()=> 
		this.model.save null, {success : this.onSaved}

	onSaved:()=>
		eco.app.notifications.addNotification('msg-material-saved-successfully');
		window.location.hash = 'material/index';


exports.createViewModel = ()=> new ViewModel(new MaterialModel())
