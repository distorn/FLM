(function () {
	'use strict';

	angular
		.module("FLMClientApp.auth")
		.service("authorizationInterceptor", authorizationInterceptor)
		.config(configure);

	function authorizationInterceptor($q, appStateService) {
		var service = {};
		//TODO: replace hardcode with constants
		//TODO: check 403, 401 behaviour

		service.request = function (requestSuccess) {
			requestSuccess.headers = requestSuccess.headers || {};

			var authData = appStateService.getAuthToken();

			if (authData !== "" && authData != null) {
				requestSuccess.headers.Authorization = 'Bearer ' + authData;
			}

			return requestSuccess || $q.when(requestSuccess);
		};

		service.responseError = function (responseFailure) {
			if (responseFailure.status === 403) {
				// TODO: could use some redirection here
				window.location = appStateService.getAppUrl() + "/forbidden";
			} else if (responseFailure.status === 401) {
				appStateService.resetAuthorizationData();
			}

			return $q.reject(responseFailure);
		};

		return service;
	}

	function configure($httpProvider) {
		$httpProvider.interceptors.push("authorizationInterceptor");
	}
})();