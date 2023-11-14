(function () {
	'use strict';

	angular
		.module('eco.common')
		.component('errorList', {
			templateUrl: '/template/common/error-list',
			controllerAs: 'vm',
			controller: errorList
		});

	errorList.$inject = ['errorHandler'];
	function errorList(errorHandler) {

		var vm = this;
		vm.items = errorHandler.getErrors();
		
	}
})();
