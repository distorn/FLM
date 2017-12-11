(function () {
	'use strict';

	angular
		.module("FLMClientApp.match")
		.controller("MatchDetailsController", MatchDetailsController);

	function MatchDetailsController($state, matchService, errorLogService, dataItem, lodash) {
		var vm = this;

		// - Data -

		vm.item = dataItem;
		vm.scores = vm.item.scores;

		// - Methods -

		vm.sortScoresByMinute = sortScoresByMinute;
		vm.getTeamScores = getTeamScores;

		vm.sortScoresByMinute();

		function sortScoresByMinute() {
			vm.scores = lodash.sortBy(vm.scores, function (o) { return o.minute; });
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
	}
})();