(function () {
	'use strict';

	angular
		.module('eco.common')
		.directive('datepicker', datepicker);

	datepicker.$inject = [];
	function datepicker() {
		var directive = {
			restrict: 'A',
			require: 'ngModel',
			link: link
		};
		return directive;

		function link (scope, element, attrs, ngModelCtrl) {
			element.datepicker({
				dateFormat: 'mm/dd/yy',
				onSelect: function (date) {
					scope.$apply(function () {
						ngModelCtrl.$setViewValue(date);
					});
				}
			});
		}
	};
})();