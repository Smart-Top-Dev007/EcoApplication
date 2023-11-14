(function () {
	'use strict';

	angular
		.module('eco.municipality')
		.factory('municipalityService', municipalityService);

	municipalityService.$inject = ['$http'];
	function municipalityService($http) {
		var service = {
			save: save,
			getItem: getItem,
			getAll : getAll
		};

		return service;
		
		function getItem(id) {
			return $http.get('/municipality/index/'+id);
		}
		
		function getAll() {
			return $http.get('/municipality/index/');
		}
		
		function save(item) {
			if (item.id) {
				return $http.put('/municipality', item);
			} else {
				return $http.post('/municipality/', item);
			}
		}
	};

})();
