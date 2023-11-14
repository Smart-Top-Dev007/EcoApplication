class ViewModel extends Knockback.ViewModel
    constructor:(model)->
        super model
        this.maxYearlyClientVisits = kb.observable model, 'MaxYearlyClientVisits'
        this.maxYearlyClientVisitsWarning = kb.observable model, 'MaxYearlyClientVisitsWarning'
        this.adminNotificationsEmail = kb.observable model, 'AdminNotificationsEmail'
        this.containerFullNotificationsEmail = kb.observable model, 'ContainerFullNotificationsEmail'
        this.gstTaxRate = kb.observable model, 'GstTaxRate'
        this.qstTaxRate = kb.observable model, 'QstTaxRate'
        this.gstTaxLine = kb.observable model, 'GstTaxLine'
        this.qstTaxLine = kb.observable model, 'QstTaxLine'
        this.defaultMaterialUnit = kb.observable model, 'DefaultMaterialUnit'
        this.sessionTimeoutInMinutes = kb.observable model, 'SessionTimeoutInMinutes'
        this.errors = ko.observableArray []
        
    load:()=>this.model().fetch()
    
    save:()=>this.model().save null, {success:this.onSaved,error:this.onError}
    
    onSaved:()=>
        this.errors.removeAll()
        eco.app.notifications.addNotification 'msg-global-settings-saved-successfully'
        return
        
    onError:(m,errors,data)=>
        errors = $.parseJSON errors.responseText
        this.errors.removeAll()
        eco.app.notifications.removeNotification('msg-global-settings-saved-successfully')
        for item in errors
          this.errors.push item
          
exports.createViewModel = ()->
    model = new (require('globalsettings')).Model()
    return new ViewModel(model)