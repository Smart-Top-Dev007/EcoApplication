(function () {
	'use strict';

	angular
		.module('eco.giveawayType')
		.factory('giveawayTypeService', giveawayTypeService);

	giveawayTypeService.$inject = ['$http'];
	function giveawayTypeService($http) {
		var service = {
			getItems: getItems,
			addItem: addItem,
			deleteItem: deleteItem
		};

		return service;

		function getItems() {
			return $http.get('/giveawayType/list');
		}
		
		function deleteItem (name) {
			return $http.post('/giveawayType/delete/', {name: name});
		}
		
		function addItem(item) {
			return $http.post('/giveawayType/', item);
		}
	};

})();
