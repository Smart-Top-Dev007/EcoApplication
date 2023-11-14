(function () {
	'use strict';

	angular
		.module('eco.common')
		.factory('getFile', getFile);

	getFile.$inject = ['Blob', '$http', 'FileSaver'];
	function getFile(Blob, $http, fileSaver) {

		var service = function (url, params) {
			return $http.get(url, { params: params, responseType: "arraybuffer" })
				.then(function (response) {
					var data = new Blob([response.data], { type: response.headers("content-type") });
					var dispositionHeader = response.headers("content-disposition");
					var filename = 'file.txt';
					if (dispositionHeader) {
						var index = dispositionHeader.indexOf("filename=");
						if (index > -1) {
							filename = dispositionHeader.substring(index + "filename=".length);
						} else {
							index = dispositionHeader.indexOf("filename*=UTF-8''");
							if (index > -1) {
								filename = decodeURIComponent(dispositionHeader.substring(index + "filename*=UTF-8''".length));
							}	
						}
					}
					fileSaver.saveAs(data, filename);
				});
		};

		return service;
	};

})();
