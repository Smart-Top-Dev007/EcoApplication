(function () {
	'use strict';

	angular
		.module('eco.giveaway')
		.component('giveawayForm', {
			templateUrl: '/template/giveaway/giveaway-form',
			controllerAs: 'vm',
			controller: giveawayForm,
			bindings: {
				id: '='
			}
		});

	giveawayForm.$inject = ['giveawayService', 'errorHandler', '$location', 'hubService', 'giveawayTypeService', '$q'];
	function giveawayForm(giveawayService, errorHandler, $location, hubService, giveawayTypeService, $q) {

		var vm = this;
		vm.save = save;
		vm.isLoading = false;
		vm.onUploaded = onUploaded;
		vm.deleteImage = deleteImage;
		vm.types = null;
		
		vm.$onInit = function () {

			loadItem()
				.then(loadGiveawayTypes)
				.then(loadCurrentHub)
				.catch(errorHandler)
				.finally(function() {
					vm.isLoading = false;
				});
		};

		function loadCurrentHub() {
			return hubService.getCurrent()
				.then(function(response) {
					var hub = response.data || {};
					if (vm.price == null) {
						vm.price = hub.defaultGiveawayPrice;
					}
				});
		}

		function loadItem() {
			if (vm.id) {
				return giveawayService.getItem(vm.id)
					.then(function(response) {
						setItemProperties(response.data);
					});
			}
			return $q.resolve();
		}

		function loadGiveawayTypes() {
			return giveawayTypeService.getItems()
				.then(function(response) {
					vm.types = response.data;

					if (vm.type) {
						var existingTypes = _.where(vm.types, { name: vm.type });
						if (_.isEmpty(existingTypes)) {
							vm.types.push({ name: vm.type });
						}
					}

					vm.types = _.sortBy(vm.types, function(item) {
						return item.name.toLowerCase();
					});
				});
		}

		function onUploaded(response) {
			vm.imageId = response.data.Id;
		}

		function deleteImage() {
			vm.imageId = null;
		}

		function setItemProperties(item) {
			vm.type = item.Type;
			vm.title = item.Title;
			vm.description = item.Description;
			vm.price = item.Price;
			vm.imageId = item.ImageId;
			vm.hubName = item.HubName;
			vm.sequenceNo = item.SequenceNo;
		}
		
		function save() {
			vm.isLoading = true;

			var item = {
				id: vm.id,
				type: vm.type,
				title: vm.title,
				description: vm.description,
				price: vm.price,
				imageId: vm.imageId
			};

			giveawayService.save(item)
				.then(function() {
					$location.path("/giveaway/index");
				})
				.catch(errorHandler)
				.finally(function() {
					vm.isLoading = false;
				});
		}
	}
})();
