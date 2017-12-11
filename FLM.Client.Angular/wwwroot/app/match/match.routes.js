(function () {
	'use strict';

	angular
		.module("FLMClientApp.match")
		.config(configRoutes);

	function configRoutes($stateProvider, $urlRouterProvider) {
		$urlRouterProvider.when("/match", "/match/list/by-league/");

		$stateProvider
			.state("match", {
				url: "/match",
				templateUrl: "app/match/match.html"
			})
			.state("match.listByLeague", {
				url: "/list/by-league/:leagueID",
				templateUrl: "app/match/match-list.html",
				controller: "MatchListController as ctrl",
				resolve: {
					leagues: function (leagueService) {
						return leagueService.getLeagues().then(function (data) {
							return data.model;
						});
					},
					team: function () { return null; },
				}
			})
			.state("match.listByTeam", {
				url: "/list/by-team/:teamID",
				templateUrl: "app/match/match-list.html",
				controller: "MatchListController as ctrl",
				resolve: {
					leagues: function () { return null; },
					team: function (teamService, $stateParams) {
						if ($stateParams.teamID) {
							return teamService.getTeamById($stateParams.teamID);
						} else {
							return null;
						}
					},
				}
			})
			.state("match.edit", {
				url: "/edit/:itemID",
				templateUrl: "app/match/match-edit.html",
				controller: "MatchEditController as ctrl",
				resolve: {
					dataItem: function (matchService, $stateParams) {
						return matchService.getMatchById($stateParams.itemID);
					},
					team1Players: function (teamService, dataItem) {
						return teamService.getTeamPlayers(dataItem.team1Id).then(function (data) {
							return data.model;
						});
					},
					team2Players: function (teamService, dataItem) {
						return teamService.getTeamPlayers(dataItem.team2Id).then(function (data) {
							return data.model;
						});
					},
				}
			})
			.state("match.detail", {
				url: "/detail/:itemID",
				templateUrl: "app/match/match-detail.html",
				controller: "MatchDetailsController as ctrl",
				resolve: {
					dataItem: function (matchService, $stateParams) {
						return matchService.getMatchById($stateParams.itemID);
					}
				}
			})
			;
	};
})();