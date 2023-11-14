(function () {
	'use strict';

	angular
		.module('eco.payment')
		.component('paymentSettings', {
			templateUrl: '/template/payment/payment-settings',
			controllerAs: 'vm',
			controller: paymentSettings,
			bindings: {
				invoiceId: '='
			}
		});

	paymentSettings.$inject = ['paymentService', 'errorHandler', 'ngToast'];
	function paymentSettings(paymentService, errorHandler, ngToast) {

		var vm = this;
		vm.isLoading = false;
		vm.isSaving = false;
		vm.save = save;
		vm.settings = null;
		
		this.$onInit = init;

		function init() {
			vm.isLoading = true;
			paymentService.getSettings()
				.then(function(response) {
					vm.settings = response.data;
				})
				.catch(errorHandler.toast)
				.finally(function() {
					vm.isLoading = false;
				});
			
		};

		function save() {
			if (vm.isSaving) {
				return;
			}

			if (!confirm(kb.locale_manager.get("Voulez-vous enregistrer les modifications?"))) {
				return;
			}

			ngToast.dismiss();
			vm.isSaving = true;
			paymentService.saveSettings(vm.settings)
				.then(function () {
					ngToast.create("Les modifications ont été enregistrées");
				})
				.catch(errorHandler.toast)
				.finally(function () {
					vm.isSaving = false;
				});
		}
	}
})();
