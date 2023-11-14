(function () {
	'use strict';

	angular
		.module('eco.invoice')
		.component('invoiceView', {
			templateUrl: '/template/invoice/invoice-view',
			controllerAs: 'vm',
			controller: invoiceView,
			bindings: {
				invoice: '='
			}
		});

	invoiceView.$inject = ['invoiceService', 'errorHandler'];
	function invoiceView(invoiceService, errorHandler) {
		
	}
})();
