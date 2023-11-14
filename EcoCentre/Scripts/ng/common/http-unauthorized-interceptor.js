(function() {
    'use strict';

	angular
		.module('eco.common')
		.factory("httpUnauthorizedInterceptor",
			[
				"$q", "$rootScope", "$location",
				function($q, $rootScope, $location) {

					function redirectOnUnuathorized(response) {
						if (response.status === 401) {
							$location.path('/');
						}
						return $q.reject(response);
					};

					return { responseError: redirectOnUnuathorized }
				}
			]);
})();