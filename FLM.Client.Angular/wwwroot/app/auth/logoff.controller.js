(function () {
	'use strict';

	angular
		.module("FLMClientApp.auth")
		.controller("LogoffController", LogoffController);

	function LogoffController(securityService) {
		var vm = this;
		console.log("LogoffController created");
		securityService.logoff();
	}
})();