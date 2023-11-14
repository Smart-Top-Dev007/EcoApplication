(function () {
	'use strict';

	angular.module('eco.common', ['ngToast', 'ngFileSaver'])
		.config([
			"$httpProvider", '$locationProvider',
			function ($httpProvider, $locationProvider) {
				$httpProvider.interceptors.push("httpUnauthorizedInterceptor");
				$httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
				$httpProvider.defaults.headers.common["X-Camel-Case-Json"] = 'true';
				$locationProvider.html5Mode(false).hashPrefix('');
			}
		]);


})();