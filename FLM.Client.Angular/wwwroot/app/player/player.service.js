(function () {
	'use strict';

	angular
		.module("FLMClientApp.player")
		.factory("playerService", playerService);

	function playerService($http, $q, EnvConfig, errorLogService, dateFieldConverter, pagingUtils) {
		var service = {};

		// - API communication -

		service.getPlayers = getPlayers;
		service.getPlayerById = getPlayerById;
		service.createPlayer = createPlayer;
		service.editPlayer = editPlayer;
		service.deletePlayer = deletePlayer;
		service.lookupPlayer = lookupPlayer;

		// - Data Utils -

		service.newItem = newItem;
		service.getDisplayName = getDisplayName;

		function newItem() {
			return {
				firstName: "",
				lastName: "",
				dateOfBirth: null,
			};
		}

		function getDisplayName(item) {
			return item.lastName + " " + item.firstName;
		}

		function getPlayers(pagingModel = null) {
			var config = {};

			if (pagingModel) {
				config.params = pagingUtils.getParamsFromModel(pagingModel);
			}

			return $http.get(EnvConfig.api + "api/player", config)
				.then(function (response) {
					return response.data;
				});
		}

		function getPlayerById(itemID) {
			return $http.get(EnvConfig.api + "api/player/" + itemID)
				.then(function (response) {
					var item = response.data.model;
					dateFieldConverter.convertFields(item, ["dateOfBirth"]);
					return item;
				});
		}

		function createPlayer(item) {
			return $http.post(EnvConfig.api + "api/player", item)
				.then(function (response) {
					return response.data;
				});
		}

		function editPlayer(item) {
			return $http.put(EnvConfig.api + "api/player", item)
				.then(function (response) {
					return response.data;
				});
		}

		function deletePlayer(itemID) {
			return $http.delete(EnvConfig.api + "api/player/" + itemID)
				.then(function (response) {
					return response.data;
				});
		}

		function lookupPlayer(searchCriteria) {
			return $http.get(EnvConfig.api + "api/player/lookup/" + searchCriteria)
				.then(function (response) {
					return response.data;
				});
		}

		return service;
	}
})();