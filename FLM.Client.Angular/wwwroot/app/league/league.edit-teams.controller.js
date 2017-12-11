(function () {
	'use strict';

	angular
		.module("FLMClientApp.league")
		.controller("LeagueEditTeamsController", LeagueEditTeamsController);

	function LeagueEditTeamsController($state, leagueService, teamService, errorLogService, dataItem) {
		var vm = this;

		// - Data -

		vm.item = dataItem;

		// - UI -

		vm.teamSearchItem = null;
		vm.teamSearchResults = [];

		vm.noContentFound = false;
		vm.leagueTeams = [];

		// - Methods -

		vm.refreshTeamSearch = refreshTeamSearch;
		vm.refreshLeagueTeams = refreshLeagueTeams;
		vm.assignTeam = assignTeam;
		vm.removeTeam = removeTeam;
		vm.getDisplayName = getDisplayName;

		vm.refreshLeagueTeams();

		function refreshLeagueTeams() {
			vm.noContentFound = false;
			errorLogService.hideErrorViews()

			leagueService.getLeagueTeams(vm.item.id).then(function success(data) {
				if (data) {
					vm.leagueTeams = data.model;
					if (vm.leagueTeams && vm.leagueTeams.length === 0) {
						vm.noContentFound = true;
					}
				} else {
					vm.leagueTeams = [];
					vm.noContentFound = true;
				}
			}).catch(errorLogService.handleApiCallError);
		}

		function refreshTeamSearch(criteria) {
			if (criteria && criteria.length > 0) {
				teamService.lookupTeam(criteria).then(function success(data) {
					vm.teamSearchResults = data ? data.model : [];
				}).catch(errorLogService.handleApiCallError);
			}
		}

		function assignTeam() {
			leagueService.assignTeam(vm.item.id, vm.teamSearchItem.id, vm.teamNumber).then(function success(data) {
				vm.teamSearchItem = null;
				vm.refreshLeagueTeams();
			}).catch(errorLogService.handleApiCallError);
		}

		function removeTeam(team) {
			leagueService.removeTeam(vm.item.id, team.teamId).then(function success(data) {
				vm.refreshLeagueTeams();
			}).catch(errorLogService.handleApiCallError);
		}

		function getDisplayName() {
			return leagueService.getDisplayName(vm.item);
		}
	}
})();