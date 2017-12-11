(function () {
	'use strict';

	angular
		.module("FLMClientApp.player")
		.config(configRoutes);

	function configRoutes($stateProvider, $urlRouterProvider) {
		$urlRouterProvider.when("/player", "/player/list");

		$stateProvider
			.state("player", {
				url: "/player",
				templateUrl: "app/player/player.html"
			})
			.state("player.list", {
				url: "/list",
				templateUrl: "app/player/player-list.html",
				controller: "PlayerListController as ctrl",
			})
			.state("player.create", {
				url: "/create",
				templateUrl: "app/player/player-edit.html",
				controller: "PlayerEditController as ctrl",
				resolve: {
					dataItem: function (playerService) {
						return playerService.newItem();
					},
					mode: function () { return "create" }
				}
			})
			.state("player.edit", {
				url: "/edit/:itemID",
				templateUrl: "app/player/player-edit.html",
				controller: "PlayerEditController as ctrl",
				resolve: {
					dataItem: function (playerService, $stateParams) {
						return playerService.getPlayerById($stateParams.itemID);
					},
					mode: function () { return "edit" }
				}
			})
			.state("player.detail", {
				url: "/detail/:itemID",
				templateUrl: "app/player/player-detail.html",
				controller: "PlayerDetailsController as ctrl",
				resolve: {
					dataItem: function (playerService, $stateParams) {
						return playerService.getPlayerById($stateParams.itemID);
					}
				}
			})
			;
	};
})();