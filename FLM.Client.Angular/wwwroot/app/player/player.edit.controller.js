(function () {
	'use strict';

	angular
		.module("FLMClientApp.player")
		.controller("PlayerEditController", PlayerEditController);

	function PlayerEditController($state, playerService, errorLogService, dateUtils, dataItem, mode) {
		const MODE_CREATE = "create";
		const MODE_EDIT = "edit";

		var vm = this;

		vm.mode = mode;

		// - Labels -

		vm.saveBtnText = mode === MODE_CREATE ? "Create" : "Save Changes";
		vm.title = mode === MODE_CREATE ? "Create New Player" : "Edit Player";

		// - Data -

		vm.item = dataItem;

		// - UI -

		vm.dobDateOptions = dateUtils.getDefaultDatePickerOptions();
		vm.dobDateOptions.minDate = new Date(1900, 0, 1);
		vm.dobDateOptions.maxDate = new Date();

		// - Methods -

		vm.save = save;
		vm.deleteItem = deleteItem;

		function save() {
			switch (mode) {
				case MODE_CREATE:

					playerService.createPlayer(vm.item).then(function success(data) {
						vm.item = data;
						$state.go("player.list");
					}).catch(errorLogService.handleApiCallError);

					break;

				case MODE_EDIT:

					playerService.editPlayer(vm.item).then(function success(data) {
						vm.item = data;
						$state.go("player.list");
					}).catch(errorLogService.handleApiCallError);

					break;

				default:

					console.error("Can't save item. Unknown mode: ", vm.mode);
					break;
			}
		}

		function deleteItem() {
			playerService.deletePlayer(vm.item.id).then(function success(data) {
				$state.go("player.list");
			}).catch(errorLogService.handleApiCallError);
		}
	}
})();