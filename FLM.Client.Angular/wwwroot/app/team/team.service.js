(function () {
	'use strict';

	angular
		.module("FLMClientApp.team")
		.factory("teamService", teamService);

	function teamService($http, $q, EnvConfig, errorLogService, dateFieldConverter, pagingUtils) {
		var service = {};

		// - API communication -

		service.getTeams = getTeams;
		service.getTeamById = getTeamById;
		service.createTeam = createTeam;
		service.editTeam = editTeam;
		service.deleteTeam = deleteTeam;

		service.assignPlayer = assignPlayer;
		service.removePlayer = removePlayer;
		service.getTeamPlayers = getTeamPlayers;
		service.getTeamLeagues = getTeamLeagues;

		service.lookupTeam = lookupTeam;

		// - Data Utils -

		service.newItem = newItem;
		service.getDisplayName = getDisplayName;

		function newItem() {
			return {
				name: "",
				city: "",
				foundationYear: (new Date()).getFullYear(),
			};
		}

		function getDisplayName(item) {
			return item.city + " " + item.name;
		}

		function getTeams(pagingModel = null) {
			var config = {};

			if (pagingModel) {
				config.params = pagingUtils.getParamsFromModel(pagingModel);
			}

			return $http.get(EnvConfig.api + "api/team", config)
				.then(function (response) {
					return response.data;
				});
		}

		function getTeamById(itemID) {
			return $http.get(EnvConfig.api + "api/team/" + itemID)
				.then(function (response) {
					var item = response.data.model;

					return item;
				});
		}

		function createTeam(item) {
			return $http.post(EnvConfig.api + "api/team", item)
				.then(function (response) {
					return response.data;
				});
		}

		function editTeam(item) {
			return $http.put(EnvConfig.api + "api/team", item)
				.then(function (response) {
					return response.data;
				});
		}

		function deleteTeam(itemID) {
			return $http.delete(EnvConfig.api + "api/team/" + itemID)
				.then(function (response) {
					return response.data;
				});
		}

		function assignPlayer(teamId, playerId, number) {
			var url = "api/team/" + teamId + "/assign-player/" + playerId + "/" + number;
			return $http.post(EnvConfig.api + url)
				.then(function (response) {
					return response.data;
				});
		}

		function removePlayer(teamId, playerId) {
			var url = "api/team/" + teamId + "/remove-player/" + playerId;
			return $http.delete(EnvConfig.api + url)
				.then(function (response) {
					return response.data;
				});
		}

		function getTeamPlayers(teamId) {
			return $http.get(EnvConfig.api + "api/team/" + teamId + "/players")
				.then(function (response) {
					return response.data;
				});
		}

		function getTeamLeagues(teamId) {
			return $http.get(EnvConfig.api + "api/team/" + teamId + "/leagues")
				.then(function (response) {
					return response.data;
				});
		}

		function lookupTeam(searchCriteria) {
			return $http.get(EnvConfig.api + "api/team/lookup/" + searchCriteria)
				.then(function (response) {
					return response.data;
				});
		}

		return service;
	}
})();