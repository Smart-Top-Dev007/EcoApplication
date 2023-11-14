(function() {
    'use strict';

    angular
        .module('eco.common')
	    .filter('joinBy', function () {
		    return function (input, delimiter, selector) {
			    return _.chain(input || [])
				    .filter(function(item) { return item !== null && item != ''; })
				    .map(_.iteratee(selector))
				    .value()
				    .join(delimiter || ', ');
		    };
	    });
})();