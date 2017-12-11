(function () {
	'use strict';

	angular
		.module("FLMClientApp.match")
		.controller("MatchEditController", MatchEditController);

	function MatchEditController($state, lodash, matchService, playerService, errorLogService, dataItem, team1Players, team2Players) {
		var vm = this;

		// - Data -

		vm.item = dataItem;
		vm.team1Players = team1Players;
		vm.team2Players = team2Players;

		vm.scores = vm.item.scores;

		// - UI -
		vm.teams = [
			{ id: vm.item.team1Id, label: vm.item.team1FullName },
			{ id: vm.item.team2Id, label: vm.item.team2FullName }
		];

		vm.scoreInput = {};

		// - Methods -

		vm.getPlayersProvider = getPlayersProvider;
		vm.getPlayerDisplayName = getPlayerDisplayName;
		vm.resetScorePlayer = resetScorePlayer;
		vm.sortScoresByMinute = sortScoresByMinute;
		vm.getTeamScores = getTeamScores;
		vm.getEnemyTeamId = getEnemyTeamId;
		vm.addScore = addScore;
		vm.removeScore = removeScore;
		vm.isTotalScoresCountValid = isTotalScoresCountValid;
		vm.isScoresCountByTeamValid = isScoresCountByTeamValid;
		vm.isAllScoresValid = isAllScoresValid;
		vm.save = save;

		vm.sortScoresByMinute();

		function isAllScoresValid() {
			return vm.isScoresCountByTeamValid(true) && vm.isScoresCountByTeamValid(false);
		}

		function isTotalScoresCountValid() {
			return vm.scores.length == vm.item.team1Score + vm.item.team2Score;
		}

		function isScoresCountByTeamValid(isHomeTeam) {
			if (isHomeTeam) {
				return vm.getTeamScores(vm.item.team1Id).length === vm.item.team1Score;
			} else {
				return vm.getTeamScores(vm.item.team2Id).length === vm.item.team2Score;
			}
		}

		function sortScoresByMinute() {
			vm.scores = lodash.sortBy(vm.scores, function (o) { return o.minute; });
		}

		function getEnemyTeamId(teamId) {
			if (teamId === vm.item.team1Id) {
				return vm.item.team2Id;
			} else if (teamId === vm.item.team2Id) {
				return vm.item.team1Id;
			} else {
				// teamId not presented in this match
				return null;
			}
		}

		function removeScore(item) {
			lodash.remove(vm.scores, function (o) { return o == item; });
		}

		function addScore() {
			vm.scores.push({
				matchId: vm.item.id,
				teamId: vm.scoreInput.team.id,
				enemyTeamId: vm.getEnemyTeamId(vm.scoreInput.team.id),
				playerId: vm.scoreInput.player.playerId,
				playerFullName: playerService.getDisplayName(vm.scoreInput.player),
				isOG: vm.scoreInput.isOG,
				isPenalty: vm.scoreInput.isPenalty,
				minute: vm.scoreInput.minute,
			});

			vm.sortScoresByMinute();

			vm.scoreInput = {};
		}

		function getTeamScores(teamId) {
			var scores = [];

			for (var i = 0; i < vm.scores.length; i++) {
				var score = vm.scores[i];
				if (score.teamId === teamId) {
					scores.push(score);
				}
			}

			return (scores);
		}

		function resetScorePlayer() {
			vm.scoreInput.player = null;
		}

		function getPlayerDisplayName(player) {
			return player ? playerService.getDisplayName(player) : "";
		}

		function getPlayersProvider() {
			if (!vm.scoreInput.team) {
				// No Team selected
				return [];
			} else {
				if (vm.scoreInput.team.id === vm.item.team1Id) {
					// Team 1 scores
					return vm.scoreInput.isOG ? vm.team2Players : vm.team1Players;
				} else {
					// Team 2 scores
					return vm.scoreInput.isOG ? vm.team1Players : vm.team2Players;
				}
			}
		}

		function save() {
			var results = {
				id: vm.item.id,
				team1Score: vm.item.team1Score,
				team2Score: vm.item.team2Score,
				scores: vm.scores,
			};

			matchService.editMatch(results).then(function success(data) {
				vm.item = data.model;
				vm.scores = vm.item.scores;
				vm.sortScoresByMinute();
				//$state.go("match.list");
			}).catch(errorLogService.handleApiCallError);
		}
	}
})();