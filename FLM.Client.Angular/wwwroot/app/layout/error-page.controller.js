(function () {
	'use strict';

	angular
		.module("FLMClientApp")
		.controller("ErrorPageController", ErrorPageController);

	function ErrorPageController($location, $state, $stateParams, EnvConfig) {
		var vm = this;

		vm.errorData = $stateParams.errorData;
		vm.detailsVisible = vm.errorData && EnvConfig.showDebugInfo;

		if ($stateParams.title) {
			// custom title passed
			vm.title = $stateParams.title;
		} else if (vm.errorData && vm.errorData.response) {
			// form title automatically from error response
			var response = vm.errorData.response;
			vm.title = "Error " + response.status + " : " + response.statusText;
		} else {
			// default title
			vm.title = "Error";
		}

		if ($stateParams.message) {
			// custom message passed
			vm.message = $stateParams.message;
		} else if (vm.errorData && vm.errorData.response && vm.errorData.response.data) {
			// form message automatically from error response
			var responseData = vm.errorData.response.data;
			vm.message = responseData.errorMessage;
		} else {
			// default message
			vm.message = "Something went wrong.";
		}

		if (vm.errorData && vm.errorData.toState && vm.errorData.toParams) {
			//$stateChangeError occured, change url manually:
			var url = $state.href(vm.errorData.toState.name, vm.errorData.toParams);
			$location.path(url.replace(/^#!/, ""));
		}
	};
})();