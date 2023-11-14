(function () {
	'use strict';

	angular
		.module('eco.container')
		.component('containerList', {
			templateUrl: '/template/container/container-list',
			controllerAs: 'vm',
			controller: containerList
		});

	containerList.$inject = ['containerService', 'errorHandler'];
	function containerList(containerService, errorHandler) {

		var vm = this;
		vm.items = null;
		vm.isLoading = false;
		vm.isDownloading = false;
		vm.sendAlert = sendAlert;
		vm.deleteItem = deleteItem;
		vm.download = download;
		vm.showDeleted = false;
		vm.selectDeletedTab = selectDeletedTab;
		vm.undeleteItem = undeleteItem;
		vm.onPageChange = onPageChange;
		vm.paging = null;
		vm.currentPage = 1;
		vm.pageSize = 20;

		vm.$onInit = init;

		function init() {
			loadItems();
		}

		function loadItems() {
			vm.isLoading = true;

			var filter = {
				onlyCurrentHub: false,
				deleted: vm.showDeleted,
				pageSize: vm.pageSize,
				page: vm.currentPage
			};

			containerService.getItems(filter)
				.then(function(response) {
					vm.items = response.data.items;
					vm.paging = response.data.paging;
				})
				.catch(errorHandler)
				.finally(function () {
					vm.isLoading = false;
				});
		};

		function sendAlert(item) {

			if (!confirm("Etes-vous sûr de vouloir envoyer une alerte pour '" + item.number + "'? ")) {
				return;
			}

			item.isSendingAlert = true;
			containerService.sendAlert(item.id)
				.then(function (response) {
					var result = response.data;
					item.dateOfLastAlert = result.dateOfLastAlert;
					alert("Courriel d'alerte envoyé");
				})
				.catch(errorHandler)
				.finally(function () {
					item.isSendingAlert = false;
				});
		}


		function deleteItem(item) {
			if (!confirm("Voulez-vous supprimer '"+item.number+"'?")) {
				return;
			}

			item.isDeleteLoading = true;
			containerService.deleteItem(item.id)
				.then(function () {
					item.isDeleted = true;
				})
				.catch(errorHandler)
				.finally(function () {
					item.isDeleteLoading = false;
				});
		}

		function undeleteItem(item) {
			if (!confirm("Êtes-vous sûr de vouloir restaurer le conteneur '" + item.number +"'?")) {
				return;
			}

			item.isUndeleteLoading = true;
			containerService.undeleteItem(item.id)
				.then(function () {
					item.isDeleted = false;
				})
				.catch(errorHandler)
				.finally(function () {
					item.isUndeleteLoading = false;
				});
		}

		function selectDeletedTab(deleted) {
			if (vm.showDeleted === deleted) {
				return;
			}
			vm.showDeleted = deleted;
			loadItems();
		}

		function onPageChange() {
			loadItems();
		}

		function download() {

			vm.isDownloading = true;

			var filter = {
				onlyCurrentHub: false,
				deleted: vm.showDeleted
			};

			containerService.downloadItems(filter)
				.catch(errorHandler)
				.finally(function () {
					vm.isDownloading = false;
				});
		}
	}
})();
