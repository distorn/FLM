(function () {
	'use strict';

	angular
		.module("FLMClientApp.player")
		.controller("PlayerListController", PlayerListController);

	function PlayerListController($stateParams, playerService, errorLogService, pagingUtils) {
		var vm = this;

		vm.items = [];
		vm.pagingModel = pagingUtils.newPagingModel();
		vm.noContentFound = false;

		vm.updateList = updateList;
		vm.deleteItem = deleteItem;
		vm.pageChangeHandler = pageChangeHandler;

		function pageChangeHandler() {
			vm.updateList();
		}

		function updateList() {
			errorLogService.hideErrorViews();
			vm.noContentFound = false;

			playerService.getPlayers(vm.pagingModel).then(function success(data) {
				if (data) {
					vm.items = data.model;
					pagingUtils.updatePagingModel(vm.pagingModel, data);
					if (vm.items && vm.items.length === 0) {
						vm.noContentFound = true;
					}
				} else {
					vm.items = [];
					vm.noContentFound = true;
				}
			}).catch(errorLogService.handleApiCallError);
		}

		function deleteItem(item) {
			playerService.deletePlayer(item.id).then(function success(data) {
				updateList();
			}).catch(errorLogService.handleApiCallError);
		}

		vm.updateList();
	}
})();