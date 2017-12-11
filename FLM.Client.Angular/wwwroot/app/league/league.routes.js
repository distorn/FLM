(function () {
	'use strict';

	angular
		.module("FLMClientApp.league")
		.config(configRoutes);

	function configRoutes($stateProvider, $urlRouterProvider) {
		$urlRouterProvider.when("/league", "/league/list");

		$stateProvider
			.state("league", {
				url: "/league",
				templateUrl: "app/league/league.html"
			})
			.state("league.list", {
				url: "/list",
				templateUrl: "app/league/league-list.html",
				controller: "LeagueListController as ctrl",
			})
			.state("league.create", {
				url: "/create",
				templateUrl: "app/league/league-edit.html",
				controller: "LeagueEditController as ctrl",
				resolve: {
					dataItem: function (leagueService) {
						return leagueService.newItem();
					},
					mode: function () { return "create" }
				}
			})
			.state("league.edit", {
				url: "/edit/:itemID",
				templateUrl: "app/league/league-edit.html",
				controller: "LeagueEditController as ctrl",
				resolve: {
					dataItem: function (leagueService, $stateParams) {
						return leagueService.getLeagueById($stateParams.itemID);
					},
					mode: function () { return "edit" }
				}
			})
			.state("league.detail", {
				url: "/detail/:itemID",
				templateUrl: "app/league/league-detail.html",
				controller: "LeagueDetailsController as ctrl",
				resolve: {
					dataItem: function (leagueService, $stateParams) {
						return leagueService.getLeagueById($stateParams.itemID);
					}
				}
			})
			.state("league.edit-teams", {
				url: "/edit/:itemID/teams",
				templateUrl: "app/league/league-edit-teams.html",
				controller: "LeagueEditTeamsController as ctrl",
				resolve: {
					dataItem: function (leagueService, $stateParams) {
						return leagueService.getLeagueById($stateParams.itemID);
					}
				}
			})
			.state("league.edit-schedule", {
				url: "/edit/:itemID/schedule/:roundNum",
				templateUrl: "app/league/league-edit-schedule.html",
				controller: "LeagueEditScheduleController as ctrl",
				resolve: {
					dataItem: function (leagueService, $stateParams) {
						return leagueService.getLeagueById($stateParams.itemID);
					}
				}
			})
			;
	};
})();