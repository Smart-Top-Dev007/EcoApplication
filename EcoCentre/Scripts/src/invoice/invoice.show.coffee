invoice = require('invoice')

class ViewModel extends kb.ViewModel
	constructor:(model)->
		super model
		this.model = model
		this.CreatedAtFormatted = ko.computed this.computeCreatedAt
	
	load:(id)=> this.model.fetchById(id)
	
	computeCreatedAt:()=>
		ca = this.CreatedAt()
		
		res = ca.match(/\d+/g)
		return '' if res == null or res.length < 1
		res = res[0]
		ca = new Date(res * 1)
		ca.toString('yyyy-MM-dd hh:mm:ss')

	payWithCash:()=>
		$.post("/payment/payWithCash?invoiceId="+this.model.get("Id")).done(this.onPaymentComplete).fail(this.onPaymentError)

	onPaymentComplete:(result)=>
		this.Payment = result.data
		alert("done");

	onPaymentError:()=>
		alert("error");
		

exports.createViewModel = ()=> new ViewModel( new invoice.InvoiceModel())