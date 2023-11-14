(function () {
	'use strict';

	angular
		.module('eco.material')
		.factory('materialService', materialService);

	materialService.$inject = ['$http'];
	function materialService($http) {
		var service = {
			save: save,
			getItem: getItem,
			getAllWithContainers: getAllWithContainers,
			getAll: getAll
		};

		return service;
		
		function getItem(id) {
			return $http.get('/material/index/'+id);
		}
		
		function getAll() {
			return $http.get('/material/index/');
		}
		
		function getAllWithContainers() {
			return $http.get('/material/index/?hasContainer=true&active=true');
		}
		
		function save(item) {
			if (item.id) {
				return $http.put('/material', item);
			} else {
				return $http.post('/material/', item);
			}
		}
	};

})();
