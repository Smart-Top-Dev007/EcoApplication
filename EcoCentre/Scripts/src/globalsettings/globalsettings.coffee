
class GlobalSettingsModel extends Backbone.Model
	constructor:()->
		super
			MaxYearlyClientVisits:0
			MaxYearlyClientVisitsWarning:0
			QstTaxRate:0
			GstTaxRate:0
			GstTaxLine:""
			QstTaxLine:""
			DefaultMaterialUnit: ""
			SessionTimeoutInMinutes: ""
		this.url = 'globalsettings'
		
exports.Model = GlobalSettingsModel
