(function () {
	"use strict";
	angular
		.module("FLMClientApp")
		.component("errorView", {
			templateUrl: "app/layout/error-view.html",
			controller: ErrorViewController
		});

	function ErrorViewController($rootScope) {
		var vm = this;

		vm.visible = false;
		vm.errorMessage = "";

		vm.showErrorMessage = showErrorMessage;
		vm.hide = hide;

		$rootScope.$on("errorRaised", errorRaisedHandler);
		$rootScope.$on("hideErrorViews", hideErrorViewsHandler);

		function errorRaisedHandler(event, message) {
			showErrorMessage(message);
		}

		function hideErrorViewsHandler(event) {
			hide();
		}

		function showErrorMessage(errorMessage) {
			vm.errorMessage = errorMessage;
			vm.visible = true;
		}

		function hide() {
			vm.visible = false;
		}
	}
})();