(function () {
	'use strict';

	angular
		.module("FLMClientApp.team")
		.controller("TeamEditPlayersController", TeamEditPlayersController);

	function TeamEditPlayersController($state, teamService, playerService, errorLogService, dataItem) {
		var vm = this;

		// - Data -

		vm.item = dataItem;

		// - UI -

		vm.playerSearchItem = null;
		vm.playerSearchResults = [];
		vm.playerNumber = undefined;

		vm.noContentFound = false;
		vm.teamPlayers = [];

		// - Methods -

		vm.refreshPlayerSearch = refreshPlayerSearch;
		vm.refreshTeamPlayers = refreshTeamPlayers;
		vm.assignPlayer = assignPlayer;
		vm.removePlayer = removePlayer;
		vm.getDisplayName = getDisplayName;
		vm.getPlayerDisplayName = getPlayerDisplayName;

		vm.refreshTeamPlayers();

		function refreshTeamPlayers() {
			vm.noContentFound = false;
			errorLogService.hideErrorViews()

			teamService.getTeamPlayers(vm.item.id).then(function success(data) {
				if (data) {
					vm.teamPlayers = data.model;
					if (vm.teamPlayers && vm.teamPlayers.length === 0) {
						vm.noContentFound = true;
					}
				} else {
					vm.teamPlayers = [];
					vm.noContentFound = true;
				}
			}).catch(errorLogService.handleApiCallError);
		}

		function refreshPlayerSearch(criteria) {
			if (criteria && criteria.length > 0) {
				playerService.lookupPlayer(criteria).then(function success(data) {
					vm.playerSearchResults = data ? data.model : [];
				}).catch(errorLogService.handleApiCallError);
			}
		}

		function assignPlayer() {
			teamService.assignPlayer(vm.item.id, vm.playerSearchItem.id, vm.playerNumber).then(function success(data) {
				vm.playerSearchItem = null;
				vm.playerNumber = undefined;
				vm.refreshTeamPlayers();
			}).catch(errorLogService.handleApiCallError);
		}

		function removePlayer(player) {
			teamService.removePlayer(vm.item.id, player.playerId).then(function success(data) {
				vm.refreshTeamPlayers();
			}).catch(errorLogService.handleApiCallError);
		}

		function getDisplayName() {
			return teamService.getDisplayName(vm.item);
		}

		function getPlayerDisplayName(item) {
			return item ? playerService.getDisplayName(item) : "";
		}
	}
})();