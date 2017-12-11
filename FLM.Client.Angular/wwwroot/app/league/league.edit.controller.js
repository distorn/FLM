(function () {
	'use strict';

	angular
		.module("FLMClientApp.league")
		.controller("LeagueEditController", LeagueEditController);

	function LeagueEditController($state, leagueService, errorLogService, dateUtils, dataItem, mode) {
		const MODE_CREATE = "create";
		const MODE_EDIT = "edit";

		var vm = this;

		vm.mode = mode;

		// - Labels -

		vm.saveBtnText = mode === MODE_CREATE ? "Create" : "Save Changes";
		vm.title = mode === MODE_CREATE ? "Create New League" : "Edit League";

		// - Data -

		vm.item = dataItem;
		vm.leagueTeams = [];

		// - UI -

		vm.startDateOptions = dateUtils.getDefaultDatePickerOptions();
		vm.endDateOptions = dateUtils.getDefaultDatePickerOptions();
		//TODO: validate startDate < endDate

		// - Methods -

		vm.save = save;
		vm.deleteItem = deleteItem;
		vm.refreshLeagueTeams = refreshLeagueTeams;

		if (mode === MODE_EDIT) {
			vm.refreshLeagueTeams();
		}

		function refreshLeagueTeams() {
			leagueService.getLeagueTeams(vm.item.id).then(function success(data) {
				if (data) {
					vm.leagueTeams = data.model;
				} else {
					vm.leagueTeams = [];
				}
			}).catch(errorLogService.handleApiCallError);
		}

		function save() {
			switch (mode) {
				case MODE_CREATE:

					leagueService.createLeague(vm.item).then(function success(data) {
						vm.item = data;
						$state.go("league.list");
					}).catch(errorLogService.handleApiCallError);

					break;

				case MODE_EDIT:

					leagueService.editLeague(vm.item).then(function success(data) {
						vm.item = data;
						$state.go("league.list");
					}).catch(errorLogService.handleApiCallError);

					break;

				default:

					console.error("Can't save item. Unknown mode: ", vm.mode);
					break;
			}
		}

		function deleteItem() {
			leagueService.deleteLeague(vm.item.id).then(function success(data) {
				$state.go("league.list");
			}).catch(errorLogService.handleApiCallError);
		}
	}
})();