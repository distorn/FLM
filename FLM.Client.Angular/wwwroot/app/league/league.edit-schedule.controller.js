(function () {
	'use strict';

	angular
		.module("FLMClientApp.league")
		.controller("LeagueEditScheduleController", LeagueEditScheduleController);

	function LeagueEditScheduleController($state, $stateParams, leagueService, matchService, errorLogService, dataItem, dateUtils, lodash) {
		var vm = this;

		// - Data -

		vm.item = dataItem;
		vm.roundNum = $stateParams.roundNum || 1; // Show first round by default

		// - UI -

		vm.noContentFound = false;
		vm.formDisabled = false;
		vm.hasSavedMatches = false;
		vm.leagueTeams = [];
		vm.availableTeams = [];
		vm.matches = [];

		vm.matchDateOptions = dateUtils.getDefaultDatePickerOptions();
		vm.matchDateOptions.minDate = vm.item.startDate;
		vm.matchDateOptions.maxDate = vm.item.endDate;
		vm.datePickersOpened = [];

		// - Methods -

		vm.refreshLeagueTeams = refreshLeagueTeams;
		vm.refreshMatches = refreshMatches;
		vm.onTeamAssigned = onTeamAssigned;
		vm.onTeamRemoved = onTeamRemoved;
		vm.isTeamAssigned = isTeamAssigned;
		vm.getTeamById = getTeamById;
		vm.getDisplayName = getDisplayName;
		vm.onEditClick = onEditClick;
		vm.onRemoveAllClick = onRemoveAllClick;
		vm.onSaveClick = onSaveClick;
		vm.isSavePossible = isSavePossible;

		// As ui-select doesn't call on-remove callback for single select mode
		// a little bit ugly but working fix provided
		// See problem described here: https://github.com/angular-ui/ui-select/issues/1225
		vm.fixClearFunction = function ($select) {
			var originalClear = $select.clear;
			$select.clear = function (event) {
				vm.onTeamRemoved($select.selected);
				originalClear.call($select, event);
			};
		};

		vm.refreshLeagueTeams();

		function isSavePossible(matchForms) {
			for (var i = 0; i < vm.matchForms.length; i++) {
				var form = vm.matchForms[i];
				if (form.$invalid) {
					return false;
				}
			}
			return true;
		}

		function isTeamAssigned(item) {
			return !lodash.includes(vm.availableTeams, item);
		}

		function onTeamAssigned(item) {
			if (item && lodash.includes(vm.availableTeams, item)) {
				lodash.pull(vm.availableTeams, item);
			}
		}

		function onTeamRemoved(item) {
			if (!lodash.includes(vm.availableTeams, item)) {
				vm.availableTeams.push(item);
			}
		}

		function getTeamById(teamId) {
			var team = lodash.find(vm.leagueTeams, { teamId: teamId });
			return team;
		}

		function refreshMatches() {
			vm.availableTeams = vm.leagueTeams;
			vm.formDisabled = true;
			vm.hasSavedMatches = false;

			if (vm.leagueTeams.length > 0) {
				// load matches for tour
				leagueService.getRoundSchedule(vm.item.id, vm.roundNum).then(function success(data) {
					if (data) {
						vm.matches = data.model;

						var assignedTeams = [];

						vm.formDisabled = vm.hasSavedMatches = vm.matches.length > 0;

						for (var i = 0; i < vm.matches.length; i++) {
							var match = vm.matches[i];
							// mark home team as assigned
							match.team1 = vm.getTeamById(match.team1Id);
							assignedTeams.push(match.team1);
							// mark away team as assigned
							match.team2 = vm.getTeamById(match.team2Id);
							assignedTeams.push(match.team2);
						}

						var matchesCount = Math.floor(vm.leagueTeams.length / 2);
						var startIndex = vm.matches.length;

						// create models for not setted matches
						for (i = startIndex; i < matchesCount; i++) {
							var newMatch = matchService.newItem();
							newMatch.leagueId = vm.item.id;
							newMatch.round = vm.roundNum;
							vm.matches[i] = newMatch;
						}

						vm.availableTeams = lodash.difference(vm.leagueTeams, assignedTeams);
					}
				}).catch(errorLogService.handleApiCallError);
			}
		}

		function refreshLeagueTeams() {
			vm.noContentFound = false;
			errorLogService.hideErrorViews();

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
				refreshMatches();
			}).catch(errorLogService.handleApiCallError);
		}

		function onEditClick() {
			vm.formDisabled = false;
		}

		function onRemoveAllClick() {
			leagueService.clearRoundSchedule(vm.item.id, vm.roundNum).then(function success(data) {
				refreshMatches();
			}).catch(errorLogService.handleApiCallError);
		}

		function onSaveClick() {
			var matches = [];

			for (var i = 0; i < vm.matches.length; i++) {
				var match = vm.matches[i];
				matches.push({
					leagueId: vm.item.id,
					round: vm.roundNum,
					date: match.date,
					team1Id: match.team1.teamId,
					team2Id: match.team2.teamId
				});
			}

			leagueService.editRoundSchedule(vm.item.id, vm.roundNum, matches).then(function success(data) {
				vm.formDisabled = true;
			}).catch(errorLogService.handleApiCallError);
		}

		function getDisplayName() {
			return leagueService.getDisplayName(vm.item);
		}
	}
})();