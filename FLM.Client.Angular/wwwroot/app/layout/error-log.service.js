(function () {
	'use strict';

	angular
		.module("FLMClientApp")
		.factory("errorLogService", errorLogService);

	function errorLogService($rootScope) {
		var service = {};

		service.errorsList = [];
		service.lastError = null;

		service.addError = addError;
		service.hideErrorViews = hideErrorViews;
		service.handleApiCallError = handleApiCallError;

		function addError(error) {
			service.errorsList.push(error);
			service.lastError = error;

			$rootScope.$emit("errorRaised", error);
		}

		function hideErrorViews() {
			$rootScope.$emit("hideErrorViews");
		}

		function handleApiCallError(error) {
			console.error("API Call error: ", error);

			var detailsMessage = error.data;
			if (detailsMessage !== null && error.data.errorMessage !== undefined) {
				detailsMessage = error.data.errorMessage;
			}

			var errorMessage = "API Error"
				+ " : " + error.status
				+ " : " + error.statusText
				+ (detailsMessage ? " : " + detailsMessage : "");

			service.addError(errorMessage);
		}

		return service;
	}
})();