(function () {
	'use strict';

	angular
		.module('eco.municipality')
		.component('municipalityForm', {
			templateUrl: '/template/municipality/municipality-form',
			controllerAs: 'vm',
			controller: municipalityForm,
			bindings: {
				id: '='
			}
		});

	municipalityForm.$inject = ['municipalityService', 'errorHandler', '$location', 'hubService', '$q'];
	function municipalityForm(municipalityService, errorHandler, $location, hubService, $q) {

		var vm = this;
		var hubs = null;

		vm.save = save;
		vm.isLoading = false;
		vm.isSaving = false;
		vm.isNew = true;
		vm.hubs = null;
		
		vm.$onInit = function () {

			vm.isLoading = true;

			loadHubs()
				.then(loadmunicipalityIfNeeded)
				.catch(errorHandler)
				.finally(function () {
					vm.isLoading = false;
				});
			
		};

		function loadHubs() {
			return hubService.getAll().then(function (response) {
				vm.hubs = response.data || [];
			});
		}

		function loadmunicipalityIfNeeded() {
			if (vm.id) {
				return municipalityService.getItem(vm.id)
					.then(function(response) {
						loadItem(response.data);
					});
			} else {
				loadItem({});
				return $q.resolve();
			}
		}

		function loadItem(item) {
			vm.name = item.name;
			vm.hubId = item.hubId;
			
			vm.isNew = !item.name;
		}
		
		function save() {
			vm.isSaving = true;
			
			var item = {
				id: vm.id,
				name: vm.name,
				hubId : vm.hubId,
				enabled: true
			};

			municipalityService.save(item)
				.then(function() {
					$location.path("/municipality/index");
				})
				.catch(errorHandler)
				.finally(function() {
					vm.isSaving = false;
				});
		}
	}
})();
