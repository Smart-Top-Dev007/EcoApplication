koExt = require('ko.ext')
jqueryExt = require('jquery.ext')
class Notifications
	constructor:()->
		this.items = ko.observableArray []
		this.intervals = new Array()
		this.node = $('notifications')
		ko.applyBindings this.items, this.node[0]

	addNotification:(n)=>
		n = $('#'+n).html()
		this.items.push(n)
		callback = ()=>this.removeNotification(n)
		interval = setInterval callback, 4000
		this.intervals.push interval

	removeNotification:(n)=>
		this.items.remove(n)
		interval = this.intervals.shift()
		window.clearInterval(interval)

class AppManager
	startApp:(appContainerId, appName, params)=>
		this.appContainer = document.getElementById(appContainerId);
		if (this.appContainer)
			this.currentAppName = appName;
			this.currentApp = angular.bootstrap(this.appContainer, [appName]);
			$rootScope = this.currentApp.get('$rootScope');
			_.extend($rootScope, params)
			$rootScope.$apply()
	destroyCurrentApp:()=>
		if this.currentApp == null or this.currentApp == undefined
			return
		$rootScope = this.currentApp.get('$rootScope');
		$rootScope.$destroy();
		$(this.appContainer).empty()

class Site

	init:()=>
		koExt.setup()
		jqueryExt.setup()
		this.notifications = new Notifications()

		this.appManager = new AppManager();
		
		$.ajaxSetup
			cache: false

		node = $('#app')[0]
		kb.active_transitions=''
		this.page_navigator = new kb.PageNavigatorPanes(node, {no_remove: false})
		this.router = new Backbone.Router()

		this.router.route '', null, ()=> this.loadModule('dashboard','default_dashboardTemplate', ((vm)=>vm.load()), ()=>this.appManager.startApp('ng-app', 'eco.container'))
		
		this.router.route 'giveaway/index', null, ()=> this.loadModule 'empty.module','giveaway_indexTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.giveaway')
		this.router.route 'giveaway/new', null, ()=> this.loadModule 'empty.module','giveaway_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.giveaway')
		this.router.route 'giveaway/edit/:id', null, (id)=> this.loadModule 'empty.module','giveaway_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.giveaway', {id:id})

		this.router.route 'giveaway-type/index', null, (id)=> this.loadModule 'empty.module','giveawayType_indexTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.giveawayType')

		this.router.route 'container/index', null, ()=> this.loadModule 'empty.module','container_indexTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.container')
		this.router.route 'container/new', null, ()=> this.loadModule 'empty.module','container_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.container')
		this.router.route 'container/edit/:id', null, (id)=> this.loadModule 'empty.module','container_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.container', {id:id})

		this.router.route 'invoice/payment/:id', null, (id)=> this.loadModule 'empty.module','payment_indexTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.payment', {id:id})
		this.router.route 'invoice/show/:id', null, (id)=> this.loadModule 'empty.module','invoice_showTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.invoice', {id:id})

		this.router.route 'payment/settings', null, (id)=> this.loadModule 'empty.module','payment_settingsTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.payment', {id:id})

		#municipality
		this.router.route 'municipality/index', null, ()=> this.loadModule 'municipality.list','municipality_indexTemplate', (vm)=>vm.load()
		this.router.route 'municipality/new', null, (id)=> this.loadModule 'empty.module','municipality_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.municipality', {id:null})
		this.router.route 'municipality/edit/:id', null, (id)=> this.loadModule 'empty.module','municipality_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.municipality', {id:id})


		#hubs
		this.router.route 'hub/index', null, ()=> this.loadModule 'hub.list','hub_indexTemplate', (vm)=>vm.load()
		this.router.route 'hub/new', null, ()=> this.loadModule 'hub.form','hub_newTemplate'
		this.router.route 'hub/edit/:id', null, (id)=> this.loadModule 'empty.module','hub_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.hub', {id:id})

		#client
		this.router.route 'client/index', null, ()=> this.loadModule 'client.list','client_indexTemplate', (vm)=>vm.load()
		this.router.route 'client/index/:filter', null, (filter)=> this.loadModule 'client.list','client_indexTemplate', (vm)=>
			if filter=='currentMonth'
				vm.lastVisitFrom(Date.today().set({day:1}))
			else if filter == 'currentYear'
				vm.lastVisitFrom(Date.today().set({day: 1, month: 0}))
			else if filter == 'today'
				vm.lastVisitFrom(Date.today())
			vm.load()
		this.router.route 'client/inactive', null, () =>
			return this.loadModule 'client.inactive', 'client_indexTemplate', (vm) => vm.load();

		this.router.route 'client/new', null, ()=> this.loadModule 'client.form','client_newTemplate', (vm)=>vm.loadNew()
		this.router.route 'client/edit/:id', null, (id)=> this.loadModule 'client.form','client_newTemplate', (vm)=>vm.load(id)
		this.router.route 'client/show/:id', null,	(id)=> this.loadModule 'client.show','client_showTemplate', (vm)=>vm.load(id)
		this.router.route 'client/merge',null, ()=> this.loadModule 'client.merger','client_mergerTemplate'

		#invoice
		this.router.route 'invoice/index', null, ()=>this.loadModule 'invoice.list','invoice_indexTemplate',(vm)=>vm.load()
		this.router.route 'invoice/trash', null, ()=>this.loadModule 'invoice.list','invoice_trashTemplate',(vm)=>
			vm.deleted true
			vm.load()

		this.setupNewInvoiceWorkflow()

		this.setupNewPocInvoiceWorkflow()
		this.setupNewOBNLReinvestmentWorkflow()
		this.router.route 'obnlreinvestment/show/:id', null, (id)=>this.loadModule 'obnlreinvestment.show','obnlreinvestment_showTemplate', (vm)=>vm.load id

		#material
		this.router.route 'material/index', null,	()=>this.loadModule 'material.list','material_indexTemplate', (vm)=>vm.load()
		this.router.route 'material/new', null, ()=> this.loadModule 'empty.module','material_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.material')
		this.router.route 'material/edit/:id', null, (id)=> this.loadModule 'empty.module','material_newTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.material', {id:id})
		this.router.route 'material/merge',null, ()=> this.loadModule 'material.merger','material_mergerTemplate'

		#user
		this.router.route 'user/index', null,	()=>this.loadModule 'user.list','user_indexTemplate', (vm)=>vm.load()
		this.router.route 'user/new', null,	()=>this.loadModule 'user.form','user_newTemplate'
		this.router.route 'user/edit/:id', null,	(id)=>this.loadModule 'user.form','user_newTemplate',(vm)=>vm.load(id)


		#reports
		this.router.route 'report/journal',null,()=>this.loadModule 'reports.journal','reports_journalTemplate',(vm)=>vm.load()
		this.router.route 'report/journal/:filter',null,(filter)=>this.loadModule 'reports.journal','reports_journalTemplate',(vm)=>
			if filter=='currentMonth'
				vm.from(Date.today().set({day:1}))
			else if filter == 'currentYear'
				vm.from(Date.today().set({day: 1, month: 0}))
			else if filter == 'today'
				vm.from(Date.today())
			vm.load()

		this.router.route 'report/limits', null, ()=>this.loadModule 'reports.limits','reports_limitsTemplate',(vm)=>vm.load()
		this.router.route 'report/visitslimits', null, ()=>this.loadModule 'reports.visitslimits','reports_visitsLimitsTemplate',(vm)=>vm.load()

		this.router.route 'report/materials', null, (id)=> this.loadModule 'empty.module','reports_materialsTemplate', null, ()=>this.appManager.startApp('ng-app', 'eco.material-report')

		this.router.route 'report/materialsbyaddress', null, ()=>this.loadModule 'reports.materialsbyaddress', 'reports_materialsByAddressTemplate', (vm)=>{}
		this.router.route 'report/obnltotal', null, ()=>this.loadModule 'reports.obnltotal','reports_OBNLTotalTemplate',(vm)=>vm.load()
		this.router.route 'report/obnlglobal', null, ()=>this.loadModule 'reports.obnlglobal','reports_OBNLGlobalTemplate',(vm)=>vm.load()

		#global settings
		this.router.route 'globalsettings/index', null, ()=>this.loadModule 'globalsettings.form','globalsettings_mainForm',(vm)=>vm.load()

		this.router.route 'globalsettings/printerTest', null, ()=>this.loadModule 'empty.module','globalsettings_printerTest',(vm)=>{}

		$(document).ajaxError (event, jqXHR, ajaxSettings, thrownError)=>
			return unless jqXHR.status == 401
			window.location = '/user/login'
		window.onerror = this.sendErrorMessage
		Backbone.history.start({hashChange: true})


	sendErrorMessage:(errorMsg, url, lineNumber)=>
		$.post('default/logError',{error:errorMsg,url:url,lineNumber:lineNumber});

	setupNewInvoiceWorkflow:()=>

		navigate = (route)=>
			this.router.navigate route,{trigger:true, replace:true}
			false

		workflow = new (require 'invoice.workflow').Workflow(navigate,this.remoteTemplate)

		this.router.route 'invoice/new', null, workflow.newInvoice

		this.router.route 'invoice/new/form', null, workflow.showForm

		this.router.route 'invoice/new/client/change', null, workflow.clientChange

		this.router.route 'invoice/new/client/create', null, workflow.clientCreate

		this.router.route 'invoice/new/client/edit/:id', null, workflow.clientEdit

	setupNewInvoiceWorkflow2023:()=>

		navigate = (route)=>
			this.router.navigate route,{trigger:true, replace:true}
			false

		workflow = new (require 'invoice.workflow').Workflow(navigate,this.remoteTemplate)

		this.router.route 'invoice/new2023', null, workflow.newInvoice2023

		this.router.route 'invoice/new/form2023', null, workflow.showForm2023

		this.router.route 'invoice/new/client/change', null, workflow.clientChange

		this.router.route 'invoice/new/client/create', null, workflow.clientCreate

		this.router.route 'invoice/new/client/edit/:id', null, workflow.clientEdit

	setupNewPocInvoiceWorkflow:()=>

		navigate = (route)=>
			this.router.navigate route,{trigger:true, replace:true}
			false

		workflow = new (require 'invoice.workflow').Workflow(navigate,this.remoteTemplate)

		this.router.route 'invoice/newPocPage', null, workflow.newPocInvoice

		this.router.route 'invoice/newPocPage/form', null, workflow.showPocForm

		this.router.route 'invoice/new/client/change', null, workflow.clientChange

		this.router.route 'invoice/new/client/create', null, workflow.clientCreate

		this.router.route 'invoice/new/client/edit/:id', null, workflow.clientEdit


	setupNewOBNLReinvestmentWorkflow:()=>
		navigate = (route)=>
			this.router.navigate route,{trigger:true, replace:true}
			false

		workflow = new (require 'obnlreinvestment.workflow').Workflow(navigate,this.remoteTemplate)
	 
		this.router.route 'obnlreinvestment/new', null, workflow.newOBNLReinvestment

		this.router.route 'obnlreinvestment/new/form', null, workflow.showForm

		this.router.route 'obnlreinvestment/new/client/change', null, workflow.clientChange

		this.router.route 'obnlreinvestment/new/client/create', null, workflow.clientCreate

		this.router.route 'obnlreinvestment/new/client/edit/:id', null, workflow.clientEdit

	loadModule:(module, template, callback = null, templateLoadedCallback = null)=>
		this.appManager.destroyCurrentApp();
		module = require(module)
		vm = module.createViewModel()
		this.remoteTemplate template, vm, templateLoadedCallback
		callback vm if callback != null

	remoteTemplate:(name,vm, callback)=>
		$('#app').hide()
		$('#loading').show();
		if $('#'+name).length < 1
			url = name.replace('_','/')
			$.get url, {}, (t)=>
				this.onTemplateLoaded(t,name)
				this.render(name,vm)
				callback() if callback
		else
			this.render(name,vm)
			callback() if callback

	onTemplateLoaded:(template,name)=>
		template = '<script type="text/html" id="'+name+'">'+template+'</script>'
		$('body').append(template)


	render:(name,viewModel)=>
		$('#app').show()
		$('#loading').hide();
		this.page_navigator.loadPage kb.renderTemplate(name, viewModel)
		input = $('input')
		$('.postalcode-mask').mask 'S0S-0S0';
		$('.visits-limit-mask').mask '000';
		$('.civic-number-autocomplete').on 'focus', ()->
				$(this).autocomplete("search", '');
				return
		$('.street-name-autocomplete').on 'focus', ()->
				$(this).autocomplete("search", '');
				return
		$('.postal-code-autocomplete').on 'focus', ()->
				$(this).autocomplete("search", '');
				return




		input.simplePlaceholder()

$(document).ready ()=>
	site = new Site();
	site.init()
	eco.app = site