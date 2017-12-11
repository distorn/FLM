(function () {
	'use strict';

	angular
		.module("FLMClientApp.team")
		.config(configRoutes);

	function configRoutes($stateProvider, $urlRouterProvider) {
		$urlRouterProvider.when("/team", "/team/list");

		$stateProvider
			.state("team", {
				url: "/team",
				templateUrl: "app/team/team.html"
			})
			.state("team.list", {
				url: "/list",
				templateUrl: "app/team/team-list.html",
				controller: "TeamListController as ctrl",
			})
			.state("team.detail", {
				url: "/detail/:itemID",
				templateUrl: "app/team/team-detail.html",
				controller: "TeamDetailsController as ctrl",
				resolve: {
					dataItem: function (teamService, $stateParams) {
						return teamService.getTeamById($stateParams.itemID);
					}
				}
			})
			.state("team.create", {
				url: "/create",
				templateUrl: "app/team/team-edit.html",
				controller: "TeamEditController as ctrl",
				resolve: {
					dataItem: function (teamService) {
						return teamService.newItem();
					},
					mode: function () { return "create" }
				}
			})
			.state("team.edit", {
				url: "/edit/:itemID",
				templateUrl: "app/team/team-edit.html",
				controller: "TeamEditController as ctrl",
				resolve: {
					dataItem: function (teamService, $stateParams) {
						return teamService.getTeamById($stateParams.itemID);
					},
					mode: function () { return "edit" }
				}
			})
			.state("team.edit-players", {
				url: "/edit/:itemID/players",
				templateUrl: "app/team/team-edit-players.html",
				controller: "TeamEditPlayersController as ctrl",
				resolve: {
					dataItem: function (teamService, $stateParams) {
						return teamService.getTeamById($stateParams.itemID);
					}
				}
			})
			;
	};
})();