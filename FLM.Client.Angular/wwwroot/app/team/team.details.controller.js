(function () {
	'use strict';

	angular
		.module("FLMClientApp.team")
		.controller("TeamDetailsController", TeamDetailsController);

	function TeamDetailsController($state, teamService, playerService, leagueService, errorLogService, dataItem) {
		var vm = this;

		// - Data -

		vm.item = dataItem;
		vm.teamPlayers = [];
		vm.teamLeagues = [];

		// - Methods -

		vm.deleteItem = deleteItem;
		vm.getDisplayName = getDisplayName;
		vm.refreshTeamPlayers = refreshTeamPlayers;
		vm.refreshTeamLeagues = refreshTeamLeagues;
		vm.getPlayerDisplayName = getPlayerDisplayName;
		vm.getLeagueDisplayName = getLeagueDisplayName;

		vm.refreshTeamPlayers();
		vm.refreshTeamLeagues();

		function refreshTeamPlayers() {
			teamService.getTeamPlayers(vm.item.id).then(function success(data) {
				if (data) {
					vm.teamPlayers = data.model;
				} else {
					vm.teamPlayers = [];
				}
			}).catch(errorLogService.handleApiCallError);
		}

		function refreshTeamLeagues() {
			teamService.getTeamLeagues(vm.item.id).then(function success(data) {
				if (data) {
					vm.teamLeagues = data.model;
				} else {
					vm.teamLeagues = [];
				}
			}).catch(errorLogService.handleApiCallError);
		}

		function getPlayerDisplayName(item) {
			return item ? playerService.getDisplayName(item) : "";
		}

		function getLeagueDisplayName(item) {
			return item ? leagueService.getDisplayName(item) : "";
		}

		function deleteItem() {
			teamService.deleteTeam(vm.item.id).then(function success(data) {
				$state.go("team.list");
			}).catch(errorLogService.handleApiCallError);
		}

		function getDisplayName() {
			return teamService.getDisplayName(vm.item);
		}
	}
})();