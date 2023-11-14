(function() {
    'use strict';

    angular
        .module('eco.common')
		.directive('loading', loading);

	loading.$inject = [];
	function loading() {
        var directive = {
            link: link,
            restrict: 'EA',
            scope: {
                loading: '='
            }
        };
        return directive;

        function link(scope, element, attrs) {
			scope.$watch('loading', function (newValue) {
                if (newValue) {
                    $(element).loading();
                } else {
                    $(element).stopLoading();
                }
            });
        }
    }

})();