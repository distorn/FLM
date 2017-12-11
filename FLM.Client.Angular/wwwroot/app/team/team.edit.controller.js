(function () {
	'use strict';

	angular
		.module("FLMClientApp.team")
		.controller("TeamEditController", TeamEditController);

	function TeamEditController($state, teamService, playerService, errorLogService, dateUtils, dataItem, mode) {
		const MODE_CREATE = "create";
		const MODE_EDIT = "edit";

		var vm = this;

		vm.mode = mode;

		// - Labels -

		vm.saveBtnText = mode === MODE_CREATE ? "Create" : "Save Changes";
		vm.title = mode === MODE_CREATE ? "Create New Team" : "Edit Team";

		// - Data -

		vm.item = dataItem;
		vm.teamPlayers = [];

		// - UI -

		vm.minYear = 1800;
		vm.maxYear = (new Date()).getFullYear();

		// - Methods -

		vm.refreshTeamPlayers = refreshTeamPlayers;
		vm.getPlayerDisplayName = getPlayerDisplayName;
		vm.save = save;
		vm.deleteItem = deleteItem;

		if (mode === MODE_EDIT) {
			vm.refreshTeamPlayers();
		}

		function refreshTeamPlayers() {
			teamService.getTeamPlayers(vm.item.id).then(function success(data) {
				if (data) {
					vm.teamPlayers = data.model;
				} else {
					vm.teamPlayers = [];
				}
			}).catch(errorLogService.handleApiCallError);
		}

		function getPlayerDisplayName(item) {
			return item ? playerService.getDisplayName(item) : "";
		}

		function save() {
			switch (mode) {
				case MODE_CREATE:

					teamService.createTeam(vm.item).then(function success(data) {
						vm.item = data;
						$state.go("team.list");
					}).catch(errorLogService.handleApiCallError);

					break;

				case MODE_EDIT:

					teamService.editTeam(vm.item).then(function success(data) {
						vm.item = data;
						$state.go("team.list");
					}).catch(errorLogService.handleApiCallError);

					break;

				default:

					console.error("Can't save item. Unknown mode: ", vm.mode);
					break;
			}
		}

		function deleteItem() {
			teamService.deleteTeam(vm.item.id).then(function success(data) {
				$state.go("team.list");
			}).catch(errorLogService.handleApiCallError);
		}
	}
})();