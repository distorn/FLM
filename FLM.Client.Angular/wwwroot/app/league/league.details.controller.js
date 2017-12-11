(function () {
	'use strict';

	angular
		.module("FLMClientApp.league")
		.controller("LeagueDetailsController", LeagueDetailsController);

	function LeagueDetailsController($state, leagueService, errorLogService, dataItem) {
		var vm = this;

		// - Data -

		vm.item = dataItem;
		vm.leagueTable = [];
		vm.matches = [];
		vm.roundNum = 1;
		vm.matchesPerRound = 0;

		// - Methods -

		vm.refreshLeagueTable = refreshLeagueTable;
		vm.refreshMatches = refreshMatches;
		vm.getMissingMatches = getMissingMatches;
		vm.deleteItem = deleteItem;
		vm.onRoundChanged = onRoundChanged;
		vm.getDisplayName = getDisplayName;

		vm.refreshLeagueTable();

		function refreshLeagueTable() {
			leagueService.getStandings(vm.item.id).then(function success(data) {
				if (data) {
					vm.leagueTable = data.model;
				} else {
					vm.leagueTable = [];
				}
				vm.matchesPerRound = Math.floor(vm.leagueTable.length / 2);
				vm.refreshMatches();
			}).catch(errorLogService.handleApiCallError);
		}

		function getMissingMatches() {
			var num = vm.matchesPerRound - (vm.matches ? vm.matches.length : 0);
			return new Array(num);
		}

		function refreshMatches() {
			leagueService.getRoundSchedule(vm.item.id, vm.roundNum).then(function success(data) {
				if (data) {
					vm.matches = data.model;
				}
			}).catch(errorLogService.handleApiCallError);
		}

		function deleteItem() {
			leagueService.deleteLeague(vm.item.id).then(function success(data) {
				$state.go("league.list");
			}).catch(errorLogService.handleApiCallError);
		}

		function onRoundChanged() {
			if (vm.roundNum != undefined) {
				refreshMatches();
			}
		}

		function getDisplayName() {
			return leagueService.getDisplayName(vm.item);
		}
	}
})();