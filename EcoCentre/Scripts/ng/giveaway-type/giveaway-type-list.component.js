(function () {
	'use strict';

	angular
		.module('eco.giveawayType')
		.component('giveawayTypeList', {
			templateUrl: '/template/giveaway-type/giveaway-type-list',
			controllerAs: 'vm',
			controller: giveawayTypeList
		});

	giveawayTypeList.$inject = ['giveawayTypeService', 'errorHandler'];
	function giveawayTypeList(giveawayTypeService, errorHandler) {

		var vm = this;
		vm.items = null;
		vm.isLoading = false;
		vm.isAddLoading = false;
		vm.newName = "";
		vm.deleteItem = deleteItem;
		vm.add = add;
		
		init();

		function init() {
			vm.isLoading = true;

			giveawayTypeService.getItems()
				.then(function(response) {
					vm.items = _.sortBy(response.data, function (item) {
							return item.name.toLowerCase();
						});
				})
				.catch(errorHandler)
				.finally(function () {
					vm.isLoading = false;
				});
		};


		function add() {
			if (!vm.newName) {
				return;
			}

			if (vm.isAddLoading) {
				return;
			}


			vm.isAddLoading = true;
			var item = { name: vm.newName};
			giveawayTypeService.addItem(item)
				.then(function (response) {
					vm.items.unshift(item);
					vm.newName = "";
				})
				.catch(errorHandler)
				.finally(function () {
					vm.isAddLoading = false;
				});
		};

		function deleteItem(item) {
			if (item.isDeleteLoading) {
				return;
			}
			if (!confirm("Voulez-vous supprimer '" + item.name + "'?")) {
				return;
			}

			item.isChangeLoading = true;
			giveawayTypeService.deleteItem(item.name)
				.then(function() {
					item.isDeleted = true;
				})
				.catch(errorHandler)
				.finally(function () {
					item.isDeleteLoading = false;
				});

		}
		
	}
})();
