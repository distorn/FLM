(function () {
	'use strict';

	angular
		.module("FLMClientApp.auth")
		.controller("AuthorizeController", AuthorizeController);

	function AuthorizeController(securityService) {
		var vm = this;
		securityService.doAuthorization();
	}
})();