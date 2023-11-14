(function () {
	'use strict';

	angular
		.module('eco.giveaway')
		.factory('giveawayService', giveawayService);

	giveawayService.$inject = ['$http'];
	function giveawayService($http) {
		var service = {
			getItems: getItems,
			save: save,
			getItem: getItem,
			deleteItem: deleteItem,
			setPublishingStatus: setPublishingStatus
		};

		return service;

		function getItems() {
			return $http.get('/giveaway/list');
		}

		function getItem(id) {
			return $http.get('/giveaway/index/'+id);
		}

		function deleteItem (id) {
			return $http.post('/giveaway/delete/'+id);
		}

		function setPublishingStatus(id, isPublished) {
			return $http.post('/giveaway/setPublishingStatus/' + id, { isPublished: isPublished});
		}

		function save(item) {
			if (item.id) {
				return $http.put('/giveaway', item);
			} else {
				return $http.post('/giveaway/', item);
			}
		}
	};

})();
