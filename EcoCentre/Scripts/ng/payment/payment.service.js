(function () {
	'use strict';

	angular
		.module('eco.payment')
		.factory('paymentService', paymentService);

	paymentService.$inject = ['$http'];
	function paymentService($http) {
		var service = {
			getSettings: getSettings,
			saveSettings: saveSettings,
			payWithCash: payWithCash ,
			payWithCredit: payWithCredit
		};

		return service;
		
		function getSettings () {
			return $http.get('/payment/settings');
		}
		
		function saveSettings (params) {
			return $http.post('/payment/settings', params);
		}
		
		function payWithCash  (params) {
			return $http.post('/payment/payWithCash', params);
		}

		function payWithCredit  (params) {
			return $http.post('/payment/payWithCredit', params);
		}
		
	};

})();
