(function () {
	'use strict';

	angular
		.module("FLMClientApp")
		.config(configure)
		.run(initializer);

	function configure($stateProvider, $urlRouterProvider, $locationProvider) {
		// - Default URL
		const DEFAULT_PATH = "/league/list";

		$urlRouterProvider.when("", DEFAULT_PATH);
		$urlRouterProvider.when("/", DEFAULT_PATH);

		// - 404 Handling
		$stateProvider.state("error", {
			templateUrl: "app/layout/error-page.html",
			params: {
				title: null,
				message: null,
				errorData: null
			}
		});

		$urlRouterProvider.otherwise(function ($injector, $location) {
			var state = $injector.get("$state");
			state.go("error", {
				title: "404 : Not Found",
				message: "Requested page not found. Please check the URL."
			});
			return $location.path;
		});

		$locationProvider.html5Mode(true);
	}

	function initializer($rootScope, $state) {
		// - Handle state change errors
		$rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, response) {
			//event.preventDefault();
			var errorData = {
				toState: toState,
				toParams: toParams,
				fromState: fromState,
				fromParams: fromParams,
				response: response
			};

			$state.go("error", { errorData: errorData });
		});
	}
})();