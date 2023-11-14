(function () {
	'use strict';

	angular
		.module('eco.container')
		.factory('containerService', containerService);

	containerService.$inject = ['$http', 'getFile'];
	function containerService($http, getFile) {
		var service = {
			getItems: getItems,
			save: save,
			getItem: getItem,
			deleteItem: deleteItem,
			undeleteItem: undeleteItem,
			sendAlert: sendAlert,
			downloadItems: downloadItems
		};

		return service;

		function getItems(params) {
			return $http.get('/container/list', { params: params });
		}

		function downloadItems(params) {
			params = _.clone(params);
			params.pageSize = 1000;
			params.page = 1;
			params.xls = true;
			return getFile('/container/list', params);
		}

		function getItem(id) {
			return $http.get('/container/index/'+id);
		}

		function deleteItem (id) {
			return $http.post('/container/delete/'+id);
		}

		function undeleteItem (id) {
			return $http.post('/container/undelete/'+id);
		}

		function sendAlert (id) {
			return $http.post('/container/sendAlert/'+id);
		}
		
		function save(item) {
			if (item.id) {
				return $http.put('/container', item);
			} else {
				return $http.post('/container/', item);
			}
		}
	};

})();
