(function () {
	'use strict';

	angular
		.module('eco.material')
		.component('materialForm', {
			templateUrl: '/template/material/material-form',
			controllerAs: 'vm',
			controller: materialForm,
			bindings: {
				id: '='
			}
		});

	materialForm.$inject = ['materialService', 'errorHandler', '$location', 'hubService', 'municipalityService', '$q'];
	function materialForm(materialService, errorHandler, $location, hubService, municipalityService, $q) {

		var vm = this;
		var hubs = null;
		var municipalities = null;

		vm.save = save;
		vm.isSaving = false;
		vm.isLoading = false;
		vm.isNew = true;
		vm.hubs = null;
		vm.toggleChildSettings = toggleChildSettings;
		vm.addException = addException;
		vm.showAddExceptionPopup = showAddExceptionPopup;
		vm.deleteException = deleteException;

		vm.$onInit = function () {

			vm.isLoading = true;


			loadHubs()
				.then(loadMunicipalities)
				.then(loadMaterialIfNeeded)
				.catch(errorHandler)
				.finally(function () {
					vm.isLoading = false;
				});
		};

		function toggleChildSettings(setting) {
			setting.isSettingDisabled = !setting.isSettingDisabled;
			_.map(setting.childSettings,
				function(s) {
					s.isSettingDisabled = !setting.isSettingDisabled;
				});
		}

		function loadHubs() {
			return hubService.getAll()
				.then(function (response) {
					hubs = response.data || [];
				});
		}

		function loadMaterialIfNeeded() {
			if (vm.id) {
				vm.isNew = false;
				return materialService.getItem(vm.id)
					.then(function (response) {
						loadItem(response.data);
					});
			} else {
				loadItem({});
				return $q.resolve();
			}
		}

		function loadMunicipalities() {
			return municipalityService.getAll()
				.then(function (response) {
					municipalities = _.filter(response.data, function (i) {return i.enabled});
				});
		}

		function loadItem(item) {
			vm.tag = item.tag;
			vm.name = item.name;
			vm.price = item.price;
			vm.unit = item.unit;
			vm.isExcluded = item.isExcluded;
			vm.maxYearlyAmount = item.maxYearlyAmount;

			var settings = toLookup(item.hubSettings, 'hubId');
			var municipalitiesById = toDict(municipalities, 'id');
			
			
			vm.hubSettings = _.map(hubs, function (hub) {

				var hubSettingsWithExceptions = settings[hub.id] || [];

				var existingHubSetting = _.find(hubSettingsWithExceptions,
					function(s) {
						return !s.municipalityId;
					});

				existingHubSetting = existingHubSetting || {};

				var hubSettings = {
					hubId: hub.id,
					name: hub.name,
					maxAmountPerVisit: existingHubSetting.maxAmountPerVisit,
					maxVisits: existingHubSetting.maxVisits,
					requireProofOfResidence: existingHubSetting.requireProofOfResidence,
					hasContainer: existingHubSetting.hasContainer,
					isActive: existingHubSetting.isActive,
					freeAmount: existingHubSetting.freeAmount
				}

				var exceptions = _.chain(hubSettingsWithExceptions)
					.filter(function(s) {
						return !!s.municipalityId;
					})
					.map(function (s) {
						var m = municipalitiesById[s.municipalityId];

						return {
							hubId: hub.id,
							municipalityId: m.id,
							name: m.name,
							maxAmountPerVisit: s.maxAmountPerVisit,
							maxVisits: s.maxVisits,
							requireProofOfResidence: s.requireProofOfResidence,
							hasContainer: s.hasContainer,
							freeAmount: s.freeAmount,
							isActive: s.isActive
						};
					})
					.value();


				hubSettings.exceptions = exceptions;

				return hubSettings;

			});
			
		}

		function toLookup(source, keySelector) {
			return _.reduce(source || [],
				function (memo, s) {
					var key = s[keySelector];
					if (!memo[key]) {
						memo[key] = [];
					}
					memo[key].push(s);
					return memo;
				},
				{});
		}

		function toDict(source, keySelector) {
			return _.reduce(source || [],
				function (memo, s) {
					var key = s[keySelector];
					memo[key] = s;
					return memo;
				},
				{});
		}

		function save() {
			vm.isLoading = true;

			var hubSettings = _.chain(vm.hubSettings)
				.map(function(s) {
					return {
						hubId: s.hubId,
						maxAmountPerVisit: s.maxAmountPerVisit,
						maxVisits: s.maxVisits,
						requireProofOfResidence: s.requireProofOfResidence,
						freeAmount: s.freeAmount,
						hasContainer: s.hasContainer,
						isActive: s.isActive
					}
				})
				.value();

			var exceptions = _.chain(vm.hubSettings)
				.map(function(s) {
					return _.map(s.exceptions, function(e) {
						return {
							hubId: e.hubId,
							municipalityId: e.municipalityId,
							maxAmountPerVisit: e.maxAmountPerVisit,
							maxVisits: e.maxVisits,
							requireProofOfResidence: e.requireProofOfResidence,
							freeAmount: e.freeAmount,
							hasContainer: e.hasContainer,
							isActive: e.isActive
						}
					});
				})
				.flatten()
				.value();

			var hubSettingsAndExceptions = hubSettings.concat(exceptions);

			var item = {
				id: vm.id,
				tag: vm.tag,
				name: vm.name,
				price: vm.price,
				unit: vm.unit,
				isExcluded: vm.isExcluded,
				maxYearlyAmount: vm.maxYearlyAmount,
				hubSettings: hubSettingsAndExceptions,
				active: true
			};

			materialService.save(item)
				.then(function () {
					$location.path("/material/index");
				})
				.catch(errorHandler)
				.finally(function () {
					vm.isLoading = false;
				});
		}

		function showAddExceptionPopup(setting) {
			vm.selectedSetting = setting;
			
			vm.municipalitiesForException = _.filter(municipalities, function(m) {
				return !_.some(setting.exceptions,
					function(e) {
						return e.municipalityId == m.id;
					});
			});
			vm.municipalityForException = vm.municipalitiesForException[0];

			$('#add-exception-modal').modal();
		}

		function addException() {
			var mun = vm.municipalityForException;
			vm.selectedSetting.exceptions.push({
				hubId: vm.selectedSetting.hubId,
				municipalityId: mun.id,
				name: mun.name
			});
			vm.selectedSetting = null;
			$('#add-exception-modal').modal('hide');
		}

		function deleteException(parent, exception) {
			parent.exceptions = _.without(parent.exceptions, exception);
		}
	}
})();
