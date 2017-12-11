(function () {
	'use strict';

	angular
		.module("FLMClientApp.auth")
		.config(configRoutes);

	function configRoutes($stateProvider, $urlRouterProvider) {
		$stateProvider
			.state("authorize", {
				url: "/authorize",
				templateUrl: "app/auth/authorize.html",
				controller: "AuthorizeController as ctrl"
			})
			.state("logoff", {
				url: "/logoff",
				templateUrl: "app/auth/logoff.html",
				controller: "LogoffController as ctrl"
			})
			.state("forbidden", {
				url: "/forbidden",
				templateUrl: "app/auth/forbidden.html"
			})
			.state("unauthorized", {
				url: "/unauthorized",
				templateUrl: "app/auth/unauthorized.html"
			})
			;
	}
})();