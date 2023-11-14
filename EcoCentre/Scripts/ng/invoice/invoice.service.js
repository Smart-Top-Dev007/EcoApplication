(function () {
	'use strict';

	angular
		.module('eco.invoice')
		.factory('invoiceService', invoiceService);

	invoiceService.$inject = ['$http'];
	function invoiceService($http) {
		var service = {
			getInvoice: getInvoice,
			getPaymentTypes: getPaymentTypes,
			deleteInvoice: deleteInvoice
		};

		return service;
		
		function getInvoice(id) {
			return $http.get('/invoice/index/'+id);
		}
		
		function deleteInvoice(id) {
			return $http.delete('/invoice/index/'+id);
		}
		
		function getPaymentTypes(id) {
			return $http.get('/invoice/paymentTypes/');
		}
		
	};

})();
