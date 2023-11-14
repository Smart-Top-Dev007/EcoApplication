class OBNLReinvestmentWorkflow
    constructor:(navigate,remoteTemplate)->
        this.remoteTemplate = remoteTemplate
        this.navigate = navigate
        this.formViewModel = null
        this.obnlReinvestment = null
    
    redirectIfNewOBNLReinvestment:(obnl)=>
        if this.formViewModel == null
            this.navigate 'obnlreinvestment/new'
            true
        false

    newOBNLReinvestment:()=>
        this.obnlReinvestment = new (require 'obnlreinvestment').NewOBNLReinvestmentModel()
        materials = new (require 'material').ListModel()
        materials.filter = (m)=>m.Active
        
        clientListVm = this.showClientList();
        clientListVm.reset()

        this.formViewModel = new (require 'obnlreinvestment.form').ViewModel(this.obnlReinvestment,materials,clientListVm,false)
        this.navigate 'obnlreinvestment/new/form'

    showClientList:(setup=null)=>
        clientListVm = new (require 'client.list').createViewModel();
        clientListVm.mode 'pick'
        clientListVm.model.set
            noCommercial : true
            filterOBNLNumber : ''
            searchType : 'OBNLNumber'
        clientListVm.model.set
            filterCategory : 'OBNL'
        clientListVm.onNew = ()=> this.navigate 'obnlreinvestment/new/client/create'
        clientListVm.onEdit = (vm)=> this.navigate 'obnlreinvestment/new/client/edit/'+vm.Id()
        clientListVm.onPick = (client)=>
            this.obnlReinvestment.selectClient client.model()
            this.formViewModel.showList false
            
        setup(clientListVm) if setup?
        clientListVm

    showForm:()=>
        this.navigate 'obnlreinvestment/new' if this.formViewModel?.saved
        return if this.redirectIfNewOBNLReinvestment()
        this.formViewModel.obnlReinvestmentPreview null
        this.remoteTemplate 'obnlreinvestment_newTemplate', this.formViewModel

    clientChange:()=>
        return if this.redirectIfNewOBNLReinvestment()
        this.showClientList (cvm)=>
            cvm.onCancel = ()=>this.navigate 'obnlreinvestment/new/form'

    clientCreate:()=>
        return if this.redirectIfNewOBNLReinvestment()
        clientFormVM = new (require 'client.form').createViewModel();

        clientFormVM.onCancel = ()=>this.navigate 'obnlreinvestment/new/client/change'
        this.remoteTemplate 'client_newTemplate', clientFormVM
        clientFormVM.onSaved = (client)=> 
            this.obnlReinvestment.selectClient client
            this.formViewModel.showList false
            this.navigate 'obnlreinvestment/new/form'
            false

    clientEdit:(id)=>
        return if this.redirectIfNewOBNLReinvestment()
        clientFormVM = new (require 'client.form').createViewModel();

        clientFormVM.onCancel = ()=>this.navigate 'obnlreinvestment/new/client/change'
        this.remoteTemplate 'client_newTemplate', clientFormVM
        clientFormVM.load(id)
        clientFormVM.onSaved = (client)=> 
            this.obnlReinvestment.selectClient client
            this.formViewModel.showList false
            this.navigate 'obnlreinvestment/new/form'
            false

exports.Workflow = OBNLReinvestmentWorkflow