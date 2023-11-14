(function () {
	'use strict';

	angular
		.module('eco.hub')
		.factory('hubService', hubService);

	hubService.$inject = ['$http'];
	function hubService($http) {
		var service = {
			getCurrent: getCurrent,
			getAll: getAll,
			getHub: getHub,
			save: save,
		};

		return service;

		function getCurrent() {
			return $http.get('/hub/current');
		}

		function getAll() {
			return $http.get('/hub/index');
		}

		function getHub(id) {
			return $http.get('/hub/index/'+id);
		}
		

		function save(item) {
			if (item.id) {
				return $http.put('/hub', item);
			} else {
				return $http.post('/hub/', item);
			}
		}

	};
})();
