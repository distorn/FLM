(function () {
	"use strict";
	angular
		.module("FLMClientApp")
		.component("itemAuditView", {
			templateUrl: "app/layout/item-audit-view.html",
			bindings: { item: "=" },
			controller: ItemAuditViewController,
			controllerAs: "ctrl",
		});

	function ItemAuditViewController() {
		var vm = this;
	}
})();