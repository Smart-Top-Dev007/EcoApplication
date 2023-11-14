(function () {
	'use strict';

	angular
		.module('eco.material-report')
		.component('materialReport', {
			templateUrl: '/template/material-report/material-report',
			controllerAs: 'vm',
			controller: materialReport
		});

	materialReport.$inject = ['materialReportService', 'municipalityService', 'errorHandler', '$location', 'hubService', '$q'];
	function materialReport(materialReportService, municipalityService, errorHandler, $location, hubService, $q) {

		var vm = this;

		vm.hubs = null;
		vm.municipalities = null;
		vm.items = null;
		vm.hubId = "";
		vm.municipalityId = "";
		vm.isLoading = false;
		vm.sort = sort;
		vm.generate = generate;
		vm.generateXls = generateXls;
		vm.sortBy = "name";
		vm.sortDir = "asc";

		vm.from = moment().add(-1, "month").format("MM/DD/YYYY");;
		vm.to = moment().format("MM/DD/YYYY");;

		init();

		function init() {
			vm.isLoading = true;
			loadHubs()
				.then(loadMunicipalities)
				.then(loadReport)
				.catch(errorHandler)
				.finally(function () {
					vm.isLoading = false;
				});
		}
		
		function loadHubs() {
			return hubService.getAll()
				.then(function (response) {
					vm.hubs = response.data || [];
				});
		}

		function loadMunicipalities() {
			return municipalityService.getAll()
				.then(function (response) {
					vm.municipalities = _.filter(response.data, function (i) { return i.enabled });
				});
		}

		function generateXls() {

			vm.isDownloading = true;

			var params = getParams();

			materialReportService.load(params, true)
				.catch(errorHandler)
				.finally(function () {
					vm.isDownloading = false;
				});
		}

		function generate() {
			vm.isLoading = true;

			loadReport(false)
				.catch(errorHandler)
				.finally(function () {
					vm.isLoading = false;
				});
		}

		function sort(sortBy) {
			if (vm.sortBy == sortBy) {
				vm.sortDir = vm.sortDir == "asc" ? "desc" : "asc";
			} else {
				vm.sortBy = sortBy;
				vm.sortDir = "asc";
			}
			loadReport();
		}

		function loadReport() {
			var params = getParams();

			return materialReportService.load(params, false)
				.then(function (response) {
					vm.items = response.data || [];
				});
		}

		function getParams() {
			return {
				from: moment(vm.from, "MM/DD/YYYY").format("YYYY-MM-DD"),
				to: moment(vm.to, "MM/DD/YYYY").format("YYYY-MM-DD"),
				sortBy: vm.sortBy,
				hubId: vm.hubId,
				sortDir: vm.sortDir,
				municipalityId: vm.municipalityId
			};
		}
		
	}
})();
