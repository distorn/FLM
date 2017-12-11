(function () {
	'use strict';

	angular
		.module("FLMClientApp.auth")
		.config(configure)
		.run(initializer);

	function configure() {
		//$httpProvider.interceptors.push("authorizationInterceptor");
	}

	function initializer(securityService) {
		// - Restore user info from previously saved token if so exists
		securityService.checkToken();
	}
})();