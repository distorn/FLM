(function () {
	'use strict';

	angular
		.module("FLMClientApp.auth")
		.factory("appStateService", appStateService);

	function appStateService($rootScope, $location, localStorageService, jwtUtils, AUTH) {
		const FIELD_EMAIL = "email";
		const FIELD_USER_ID = "sub";
		const FIELD_ROLE = "role";

		const ROLE_ADMIN = "Administrator";

		var service = {};

		service.resetAuthorizationData = resetAuthorizationData;
		service.setAuthorizationData = setAuthorizationData;
		service.updateUserInfo = updateUserInfo;
		service.getAuthToken = getAuthToken;
		service.getAuthIdToken = getAuthIdToken;
		service.getAppURL = getAppURL;

		function setAuthorizationData(token, id_token) {
			localStorageService.set(AUTH.KEY_AUTH_TOKEN, token);
			localStorageService.set(AUTH.KEY_AUTH_ID_TOKEN, id_token);
			updateUserInfo();
		}

		function resetAuthorizationData() {
			setAuthorizationData("", "");
		}

		function updateUserInfo() {
			var token = localStorageService.get(AUTH.KEY_AUTH_TOKEN);

			if (token) {
				// user is logged on
				var data = jwtUtils.getDataFromToken(token);
				$rootScope.user = {
					isAuthorized: true,
					isAdmin: data[FIELD_ROLE] === ROLE_ADMIN,
					id: data[FIELD_USER_ID],
					email: data[FIELD_EMAIL]
				};
			} else {
				// user is not logged on
				$rootScope.user = {
					isAuthorized: false,
					isAdmin: false,
					id: null,
					email: null
				};
			}
		}

		function getAuthToken() {
			return localStorageService.get(AUTH.KEY_AUTH_TOKEN);
		}

		function getAuthIdToken() {
			return localStorageService.get(AUTH.KEY_AUTH_ID_TOKEN);
		}

		function getAppURL() {
			var url = $location.protocol() + "://" + $location.host() + ":" + $location.port();
			return url;
		}

		return service;
	}
})();