(function () {
	'use strict';

	angular
		.module('eco.common')
		.factory('errorHandler', errorHandler);

	errorHandler.$inject = ['ngToast'];
	function errorHandler(ngToast) {

		var currentErrors = [];

		var service = function (error) {
			currentErrors.splice(0, currentErrors.length);
			var messages = formatErrorMessages(error);
			_.map(messages, addError);
		};

		function toast(error) {
			ngToast.dismiss();
			var messages = formatErrorMessages(error);
			ngToast.create({
				className: 'danger',
				content: messages.join("<br/>"),
				dismissOnTimeout: false,
				dismissButton: true
			});
		}

		function formatErrorMessages(error) {

			var isDebug = window.location.href.startsWith("http://localhost");
			if (!isDebug && error && error.headers) {
				var header = error.headers("Content-Type");
				if (header && header.startsWith("text/html")) {
					return ["Erreur inconnue"];
				}
			}

			if (error.data) {
				error = trimMessage(error.data);
			}

			if (_.isArray(error)) {
				return _.map(error,
					function(item) {
						return trimMessage(item.ErrorMessage);
					}
				);
			}

			if (_.isString(error)) {
				return [trimMessage(error)];
			}
			console.error(error);
			return ["Erreur inconnue"];
		}

		function trimMessage(message) {
			if (message && message.length > 1000) {
				return message.substring(0, 1000) + "...";
			}
			return message;
		}

		function addError(error) {
			
			currentErrors.push(error);
		}

		service.getErrors = function() {
			return currentErrors;
		}

		service.toast = toast;

		return service;
	};

})();
