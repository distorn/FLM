(function () {
	'use strict';

	angular
		.module("FLMClientApp.auth")
		.constant("AUTH", {
			KEY_AUTH_TOKEN: "authToken",
			KEY_AUTH_ID_TOKEN: "authIdToken",
			KEY_AUTH_NONCE: "authNonce",
			KEY_AUTH_STATE_CONTROL: "authStateControl"
		});
})();