(function () {
	'use strict';

	angular
		.module('eco.hub')
		.component('hubForm', {
			templateUrl: '/template/hub/hub-form',
			controllerAs: 'vm',
			controller: hubForm,
			bindings: {
				id: '='
			}
		});

	hubForm.$inject = ['hubService', 'errorHandler', '$location', '$q'];
	function hubForm(hubService, errorHandler, $location, $q) {

		var vm = this;
		vm.save = save;
		vm.isLoading = false;
		vm.isSaving = false;
		vm.emailsForLoginAlerts = [];
		
		vm.$onInit = function () {

			vm.isLoading = true;

			loadHub()
				.catch(errorHandler)
				.finally(function() {
					vm.isLoading = false;
				});
		};

		function loadHub() {
			if (vm.id) {
				return hubService.getHub(vm.id)
					.then(function(response) {
						setHubProperties(response.data);
					});
			}
			return $q.resolve();
		}

		function setHubProperties(hub) {
			vm.name = hub.name;
			vm.invoiceIdentifier = hub.invoiceIdentifier;
			vm.defaultGiveawayPrice = hub.defaultGiveawayPrice;
			vm.address = hub.address;
			vm.emailsForLoginAlerts = _.filter((hub.emailForLoginAlerts || "").split(";"), _.identity);
		}
		
		function save() {
			vm.isSaving = true;

			var item = {
				id: vm.id,
				name: vm.name,
				invoiceIdentifier: vm.invoiceIdentifier,
				defaultGiveawayPrice: vm.defaultGiveawayPrice,
				address: vm.address,
				emailForLoginAlerts: vm.emailsForLoginAlerts.join(";")
		};

			hubService.save(item)
				.then(function() {
					$location.path("/hub/index");
				})
				.catch(errorHandler)
				.finally(function() {
					vm.isSaving = false;
				});
		}
	}
})();
