(function () {
	"use strict";
	angular
		.module("FLMClientApp")
		.component("noContentView", {
			templateUrl: "app/layout/no-content-view.html",
			controller: NoContentViewController
		});

	function NoContentViewController() {
		var vm = this;
	}
})();