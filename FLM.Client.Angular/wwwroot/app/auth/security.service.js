(function () {
	'use strict';

	angular
		.module("FLMClientApp.auth")
		.factory("securityService", securityService);

	function securityService($window, $location, EnvConfig, localStorageService, appStateService, jwtUtils, AUTH) {
		var service = {};

		service.doAuthorization = doAuthorization;
		service.logoff = logoff;
		service.checkToken = checkToken;

		function doAuthorization() {
			appStateService.resetAuthorizationData();

			if ($window.location.hash) {
				authorizeCallback();
			}
			else {
				authorize();
			}
		}

		function logoff() {
			var authorizationUrl = EnvConfig.auth.server + "connect/endsession";
			var id_token_hint = appStateService.getAuthIdToken();
			var post_logout_redirect_uri = appStateService.getAppURL();

			var url = authorizationUrl + "?" +
				"id_token_hint=" + id_token_hint + "&" +
				"post_logout_redirect_uri=" + encodeURI(post_logout_redirect_uri);

			appStateService.resetAuthorizationData();
			$window.location = url;
		}

		function checkToken() {
			var token = appStateService.getAuthToken();
			if (token) {
				var data = jwtUtils.getDataFromToken(token);
				var expirationDate = new Date(data.exp * 1000);
				//console.log("EXP: ", expirationDate);

				if (expirationDate > new Date()) {
					// valid token
					appStateService.updateUserInfo();
				} else {
					// token expired
					console.log("Auth token expired: ", expirationDate);
					appStateService.resetAuthorizationData();
				}
			}
		}

		var authorize = function () {
			var authorizationUrl = EnvConfig.auth.server + "connect/authorize";
			var client_id = EnvConfig.auth.clientID;
			var redirect_uri = appStateService.getAppURL() + "/authorize";
			var response_type = "id_token token";
			var scope = EnvConfig.auth.requestedScopes.join(" ");
			var nonce = "N" + Math.random() + "" + Date.now();
			var state = Date.now() + "" + Math.random();

			localStorageService.set(AUTH.KEY_AUTH_NONCE, nonce);
			localStorageService.set(AUTH.KEY_AUTH_STATE_CONTROL, state);

			var url =
				authorizationUrl + "?" +
				"response_type=" + encodeURI(response_type) + "&" +
				"client_id=" + encodeURI(client_id) + "&" +
				"redirect_uri=" + encodeURI(redirect_uri) + "&" +
				"scope=" + encodeURI(scope) + "&" +
				"nonce=" + encodeURI(nonce) + "&" +
				"state=" + encodeURI(state);

			$window.location = url;
		};

		var authorizeCallback = function () {
			// callback from auth server, parse hash
			var hash = $window.location.hash.substr(1);

			var result = hash.split('&').reduce(function (result, item) {
				var parts = item.split('=');
				result[parts[0]] = parts[1];
				return result;
			}, {});

			var token = "";
			var id_token = "";
			var authResponseIsValid = false;

			if (!result.error) {
				if (result.state !== localStorageService.get(AUTH.KEY_AUTH_STATE_CONTROL)) {
					console.log("Auth Callback: incorrect state");
				} else {
					token = result.access_token;
					id_token = result.id_token;

					var dataIdToken = jwtUtils.getDataFromToken(id_token);

					// validate nonce
					if (dataIdToken.nonce !== localStorageService.get(AUTH.KEY_AUTH_NONCE)) {
						console.log("Auth Callback: incorrect nonce");
					} else {
						authResponseIsValid = true;
					}
				}
			}

			if (authResponseIsValid) {
				appStateService.setAuthorizationData(token, id_token);
				// successfully authorized, go to main page
				$location.path("/");
			}
			else {
				appStateService.resetAuthorizationData();
				//$state.go("unauthorized");
				$location.path(getAppURL() + "/unauthorized")
			}
		};

		return service;
    }
})();