(function () {
	'use strict';

	angular
		.module('eco.material-report')
		.factory('materialReportService', materialReportService);

	materialReportService.$inject = ['$http', 'getFile'];
	function materialReportService($http, getFile) {
		var service = {
			load: load
		};

		return service;

		function load(params, xls) {
			if (xls) {
				return getFile('reports/materials?xls=true', params);
			} else {
				return $http.get('reports/materials?xls=false', { params: params });
			}
		}

	};

})();
