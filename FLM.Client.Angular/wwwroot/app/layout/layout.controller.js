(function () {
	'use strict';

	angular
		.module("FLMClientApp")
		.controller("LayoutController", LayoutController);

	function LayoutController($state) {
		var vm = this;

		vm.menuItems = [
			{ title: "Leagues", link: "league" },
			{ title: "Teams", link: "team" },
			{ title: "Players", link: "player" },
			{ title: "Matches", link: "match" },
		];

		vm.isSelected = isSelected;

		function isSelected(menuItem) {
			var currentState = $state.current.name;
			return currentState && currentState.startsWith(menuItem.link);
		}
	};
})();