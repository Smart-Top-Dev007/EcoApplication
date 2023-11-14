class LoginViewModel
	constructor:()->
		this.login = ko.observable()
		this.password = ko.observable()
		this.processing = ko.observable false
		this.errors = ko.observableArray([])
	submit:()=>
		this.processing true
		this.errors.removeAll()
		data = 
			login: this.login()
			password : this.password()
		$.ajax
			type : "POST" 
			url: '/user/login'
			data: data
			success: ()=> 
				this.processing false
				window.location.pathname = ''
			error: this.onError

	onError:(errors,data)=>
		this.processing false
		errors = $.parseJSON errors.responseText
		this.errors.removeAll()
		for item in errors
			this.errors.push item
exports.setup = ()=>
	vm = new LoginViewModel()
	ko.applyBindings vm, $('#login-form')[0]
