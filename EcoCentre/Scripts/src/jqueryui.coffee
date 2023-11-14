class Dialog
	constructor:()->
		this.isVisible = ko.observable false
		this.settings = 
			autoOpen : false
			close : this.close

	open:()=>this.isVisible true
	close:()=>this.isVisible false
exports.Dialog = Dialog