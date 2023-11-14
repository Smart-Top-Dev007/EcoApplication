(function () {
	'use strict';

	angular
		.module('eco.giveaway')
		.component('giveawayList', {
			templateUrl: '/template/giveaway/giveaway-list',
			controllerAs: 'vm',
			controller: giveawayList
		});

	giveawayList.$inject = ['giveawayService', 'errorHandler'];
	function giveawayList(giveawayService, errorHandler) {

		var vm = this;
		vm.items = null;
		vm.isLoading = false;
		vm.deleteItem = deleteItem;
		vm.publish = publish;
		vm.unpublish = unpublish;
		
		init();

		function init() {
			vm.isLoading = true;

			giveawayService.getItems()
				.then(function(response) {
					vm.items = response.data;
				})
				.catch(errorHandler)
				.finally(function () {
					vm.isLoading = false;
				});
		};

		function deleteItem(item) {

			if (!confirm("Voulez-vous supprimer '" + item.Type + "'?")) {
				return;
			}

			item.isChangeLoading = true;
			giveawayService.deleteItem(item.Id)
				.then(function() {
					item.isDeleted = true;
				})
				.catch(errorHandler)
				.finally(function () {
					item.isDeleteLoading = false;
				});

		}

		function publish(item) {
			setPublishingStatus(item, true);
		}

		function unpublish(item) {
			setPublishingStatus(item, false);
		}


		function setPublishingStatus(item, isPublished) {
			item.isChangeLoading = true;
			giveawayService.setPublishingStatus(item.Id, isPublished)
				.then(function () {
					item.IsPublished = isPublished;
				})
				.catch(errorHandler)
				.finally(function () {
					item.isChangeLoading = false;
				});

		}


	}
})();
