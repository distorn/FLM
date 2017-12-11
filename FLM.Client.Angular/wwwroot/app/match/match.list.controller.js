(function () {
	'use strict';

	angular
		.module("FLMClientApp.match")
		.controller("MatchListController", MatchListController);

	function MatchListController($stateParams, leagues, team, matchService, leagueService, teamService, errorLogService, pagingUtils, dateUtils) {
		var vm = this;

		// - Data & UI -

		vm.items = [];
		vm.byTeamMode = false;

		if (leagues) {
			// By League mode
			vm.tournamentLocked = false;
			var leagueId = parseInt($stateParams.leagueID);

			vm.tournaments = [{ label: "All Tournaments" }];

			for (var i = 0; i < leagues.length; i++) {
				var league = leagues[i];

				var item = {
					label: leagueService.getDisplayName(league),
					data: league
				};

				vm.tournaments.push(item);

				if (league.id === leagueId) {
					vm.selectedLeague = item;
					vm.tournamentLocked = true;
				}
			}

			if (!vm.selectedLeague) {
				// Select "All" item by default
				vm.selectedLeague = vm.tournaments[0];
			}
		} else {
			// By Team mode
			vm.byTeamMode = true;
			vm.teamSearchResults = [];
			vm.selectedTeam = null;
			vm.teamLocked = false;

			if (team) {
				vm.teamLocked = true;
				vm.selectedTeam = {
					id: team.id,
					fullName: teamService.getDisplayName(team)
				};
			}
		}

		vm.minDate = null;
		vm.maxDate = null;

		vm.dateOptions = dateUtils.getDefaultDatePickerOptions();
		vm.pagingModel = pagingUtils.newPagingModel();
		vm.noContentFound = false;

		// - Methods -

		vm.getFilter = getFilter;
		vm.updateList = updateList;
		vm.deleteItem = deleteItem;
		vm.refreshTeamSearch = refreshTeamSearch;
		vm.pageChangeHandler = pageChangeHandler;

		vm.updateList();

		function pageChangeHandler() {
			vm.updateList();
		}

		function getFilter() {
			var filter = matchService.newFilter();

			if (vm.selectedTeam) {
				filter.teamId = vm.selectedTeam.id;
			}

			if (vm.selectedLeague && vm.selectedLeague.data) {
				filter.leagueId = vm.selectedLeague.data.id;
			}

			if (vm.minDate) {
				vm.minDate.setHours(0, 0, 0, 0);
				filter.minDate = vm.minDate;
			}

			if (vm.maxDate) {
				vm.maxDate.setHours(23, 59, 59, 999);
				filter.maxDate = vm.maxDate;
			}

			return filter;
		}

		function updateList() {
			errorLogService.hideErrorViews();
			vm.noContentFound = false;

			matchService.getMatches(vm.pagingModel, vm.getFilter()).then(function success(data) {
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

		function refreshTeamSearch(criteria) {
			if (criteria && criteria.length > 0) {
				teamService.lookupTeam(criteria).then(function success(data) {
					vm.teamSearchResults = data ? data.model : [];
				}).catch(errorLogService.handleApiCallError);
			}
		}

		function deleteItem(item) {
			matchService.deleteMatch(item.id).then(function success(data) {
				updateList();
			}).catch(errorLogService.handleApiCallError);
		}
	}
})();