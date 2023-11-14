(function () {
	'use strict';

	angular
		.module('eco.container')
		.component('containerForm', {
			templateUrl: '/template/container/container-form',
			controllerAs: 'vm',
			controller: containerForm,
			bindings: {
				id: '='
			}
		});

	containerForm.$inject = ['containerService', 'errorHandler', '$location', 'materialService'];
	function containerForm(containerService, errorHandler, $location, materialService) {

		var vm = this;
		vm.save = save;
		vm.isLoading = true;
		vm.isSaving = false;
		
		vm.$onInit = function () {

			loadMaterials()
				.then(loadContainerIfNeeded)
				.catch(errorHandler)
				.finally(function() { vm.isLoading = false; });
		};

		function loadMaterials() {
			return materialService.getAllWithContainers()
				.then(function(response) {
					vm.availableMaterials = response.data;
				});
		}

		function loadContainerIfNeeded() {
			if (!vm.id) {
				return null;
			}

			return containerService.getItem(vm.id)
				.then(function(response) {
					loadItem(response.data);
				});
		}
		
		function loadItem(item) {
			vm.number = item.number;
			vm.capacity = item.capacity;
			vm.materialId = item.materialId;
			vm.alertAtAmount = item.alertAtAmount;

			vm.materials = _.map(item.materials, _.iteratee("id"));
		}

		function save() {
			vm.isSaving = true;
			
			var item = {
				id: vm.id,
				number: vm.number,
				capacity: vm.capacity,
				materialIds: vm.materials,
				alertAtAmount: vm.alertAtAmount
			};

			containerService.save(item)
				.then(function() {
					$location.path("/container/index");
				})
				.catch(errorHandler)
				.finally(function() {
					vm.isSaving = false;
				});
		}
	}
})();
