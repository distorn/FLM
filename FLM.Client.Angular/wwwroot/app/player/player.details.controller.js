(function () {
	'use strict';

	angular
		.module("FLMClientApp.player")
		.controller("PlayerDetailsController", PlayerDetailsController);

	function PlayerDetailsController($state, playerService, errorLogService, dataItem) {
		var vm = this;

		// - Data -

		vm.item = dataItem;

		// - Methods -

		vm.deleteItem = deleteItem;
		vm.getDisplayName = getDisplayName;

		function deleteItem() {
			playerService.deletePlayer(vm.item.id).then(function success(data) {
				$state.go("player.list");
			}).catch(errorLogService.handleApiCallError);
		}

		function getDisplayName() {
			return playerService.getDisplayName(vm.item);
		}
	}
})();