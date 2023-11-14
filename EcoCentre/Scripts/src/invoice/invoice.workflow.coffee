HubModel  = require('hub').Model 

class InvoiceWorkflow
	constructor:(navigate,remoteTemplate)->
		this.remoteTemplate = remoteTemplate
		this.navigate = navigate
		this.formViewModel = null
		this.invoice = null
	
	redirectIfNewInvoice:(obnl, poc)=>
		if this.formViewModel == null
				this.navigate 'invoice/new2023' if poc
				this.navigate 'invoice/new' if !obnl
				this.navigate 'invoice/newOBNL' if obnl
			true
		false

	newInvoice:()=>
		this.invoice = new (require 'invoice').NewInvoiceModel()
		
		hub = new HubModel()
		request = hub.fetchCurrent().done(()=>
			clientListVm = this.showClientList(true, hub);
			clientListVm.reset()
			
			this.formViewModel = new (require 'invoice.form').ViewModel(this.invoice, clientListVm, false, hub)
			this.navigate 'invoice/new/form')

		return request

	newInvoice2023:()=>
		this.invoice = new (require 'invoice').NewInvoiceModel1()
		
		hub = new HubModel()
		request = hub.fetchCurrent().done(()=>
			clientListVm = this.showClientList(true, hub);
			clientListVm.reset()
			
			this.formViewModel = new (require 'invoice.form').ViewModel(this.invoice, clientListVm, false, hub)
			this.navigate 'invoice/new/form2023')

		return request

	newPocPageInvoice:()=>
		this.invoice = new (require 'invoice').NewInvoiceModel()

		hub = new HubModel()
		request = hub.fetchCurrent().done(()=>
			clientListVm = this.showClientList(true, hub);
			clientListVm.reset()
			
			this.formViewModel = new (require 'invoice.form').ViewModel(this.invoice, clientListVm, false, hub)
			this.navigate 'invoice/newPocPage/form')

		return request

	newOBNLInvoice:()=>
		this.invoice = new (require 'invoice').NewInvoiceModel()
		materials = new (require 'material').ListModel()
		materials.filter = (m)=>m.Active

		hub = new HubModel()
		hub.fetchCurrent()
		
		clientListVm = this.showClientList(false);
		clientListVm.reset()

		this.formViewModel = new (require 'invoice.form').ViewModel(this.invoice,materials,clientListVm,true, hub)
		this.navigate 'invoice/new/OBNLForm'

	showClientList:(notOBNL)=>
		clientListVm = new (require 'client.list').createViewModel();
		clientListVm.mode 'pick'
		clientListVm.model.set
			noCommercial : true
			filterOBNLNumber : ''
			searchType : 'OBNLNumber'
		if !notOBNL
			clientListVm.model.set
				filterCategory : 'OBNL'
		else
			clientListVm.model.set
				filterCategory : 'notOBNL'
		clientListVm.onNew = ()=> this.navigate 'invoice/new/client/create'
		clientListVm.onEdit = (vm)=> this.navigate 'invoice/new/client/edit/'+vm.Id()
		clientListVm.onPick = (client)=>
			this.invoice.selectClient client.model()
			this.formViewModel.showListPoc false
			this.formViewModel.showList false
		clientListVm

	showForm:()=>
		this.navigate 'invoice/new' if this.formViewModel?.saved
		return if this.redirectIfNewInvoice(false, false)
		this.formViewModel.invoicePreview null
		this.remoteTemplate 'invoice_newTemplate', this.formViewModel

	showForm2023:()=>
		this.navigate 'invoice/new2023' if this.formViewModel?.saved
		return if this.redirectIfNewInvoice(false, true)
		this.formViewModel.invoicePreview null
		this.remoteTemplate 'invoice_newTemplate2023', this.formViewModel

	showPocForm:()=>
		this.navigate 'invoice/newPocPage' if this.formViewModel?.saved
		return if this.redirectIfNewInvoice(false, true)
		this.formViewModel.invoicePreview null
		this.remoteTemplate 'invoice_newPocPageTemplate', this.formViewModel

	showOBNLForm: ()=>
		this.navigate 'invoice/newOBNL' if this.formViewModel?.saved
		return if this.redirectIfNewInvoice(true, false)
		this.formViewModel.invoicePreview null
		this.remoteTemplate 'invoice_newOBNLTemplate', this.formViewModel

	clientChange:()=>
		return if this.redirectIfNewInvoice()
		this.showClientList (cvm)=>
			cvm.onCancel = ()=>this.navigate 'invoice/new/form'

	clientCreate:()=>
		return if this.redirectIfNewInvoice()
		clientFormVM = new (require 'client.form').createViewModel();

		clientFormVM.onCancel = ()=>this.navigate 'invoice/new/client/change'
		clientFormVM.loadNew()
		this.remoteTemplate 'client_newTemplate', clientFormVM
		clientFormVM.onSaved = (client)=> 
			client.fetch({reset: true, success: this.onReloaded})

	clientEdit:(id)=>
		return if this.redirectIfNewInvoice()
		clientFormVM = new (require 'client.form').createViewModel();

		clientFormVM.onCancel = ()=>this.navigate 'invoice/new/client/change'
		this.remoteTemplate 'client_newTemplate', clientFormVM
		clientFormVM.load(id)
		clientFormVM.onSaved = (client)=>
			client.fetch({reset: true, success: this.onReloaded})

	onReloaded:(client)=>
		debugger
		this.invoice.selectClient client
		this.formViewModel.showList false
		this.formViewModel.showListPoc false
		this.navigate 'invoice/new/form'
		false


exports.Workflow = InvoiceWorkflow