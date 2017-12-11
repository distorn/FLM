(function () {
	'use strict';

	angular
		.module("FLMClientApp.league")
		.factory("leagueService", leagueService);

	function leagueService($http, $q, EnvConfig, errorLogService, dateFieldConverter, pagingUtils) {
		var service = {};

		// - API communication -

		service.getLeagues = getLeagues;
		service.getLeagueById = getLeagueById;
		service.createLeague = createLeague;
		service.editLeague = editLeague;
		service.deleteLeague = deleteLeague;

		service.assignTeam = assignTeam;
		service.removeTeam = removeTeam;
		service.getLeagueTeams = getLeagueTeams;

		service.getRoundSchedule = getRoundSchedule;
		service.editRoundSchedule = editRoundSchedule;
		service.clearRoundSchedule = clearRoundSchedule;

		service.getStandings = getStandings;

		// - Data Utils -

		service.newItem = newItem;
		service.getDisplayName = getDisplayName;

		function newItem() {
			return {
				name: "",
				season: "",
				startDate: null,
				endDate: null,
				roundsCount: 0
			};
		}

		function getDisplayName(item) {
			return item.name + " " + item.season;
		}

		function getLeagues(pagingModel = null) {
			var config = {};

			if (pagingModel) {
				config.params = pagingUtils.getParamsFromModel(pagingModel);
			}

			return $http.get(EnvConfig.api + "api/league", config)
				.then(function (response) {
					return response.data;
				});
		}

		function getLeagueById(itemID) {
			return $http.get(EnvConfig.api + "api/league/" + itemID)
				.then(function (response) {
					var item = response.data.model;
					dateFieldConverter.convertFields(item, ["startDate", "endDate"]);
					return item;
				});
		}

		function createLeague(item) {
			return $http.post(EnvConfig.api + "api/league", item)
				.then(function (response) {
					return response.data;
				});
		}

		function editLeague(item) {
			return $http.put(EnvConfig.api + "api/league", item)
				.then(function (response) {
					return response.data;
				});
		}

		function deleteLeague(itemID) {
			return $http.delete(EnvConfig.api + "api/league/" + itemID)
				.then(function (response) {
					return response.data;
				});
		}

		function assignTeam(leagueId, teamId) {
			var url = "api/league/" + leagueId + "/assign-team/" + teamId;
			return $http.post(EnvConfig.api + url)
				.then(function (response) {
					return response.data;
				});
		}

		function removeTeam(leagueId, teamId) {
			var url = "api/league/" + leagueId + "/remove-team/" + teamId;
			return $http.delete(EnvConfig.api + url)
				.then(function (response) {
					return response.data;
				});
		}

		function getLeagueTeams(leagueId) {
			return $http.get(EnvConfig.api + "api/league/" + leagueId + "/teams")
				.then(function (response) {
					return response.data;
				});
		}

		function getRoundSchedule(leagueId, roundNum) {
			return $http.get(EnvConfig.api + "api/league/" + leagueId + "/schedule/" + roundNum)
				.then(function (response) {
					// convert date fields
					if (response.data && response.data.model) {
						var items = response.data.model;
						dateFieldConverter.convertFields(items, ["date"]);
					}
					return response.data;
				});
		}

		function editRoundSchedule(leagueId, roundNum, matches) {
			var data = { matches: matches };
			return $http.post(EnvConfig.api + "api/league/" + leagueId + "/schedule/" + roundNum, data)
				.then(function (response) {
					return response.data;
				});
		}

		function clearRoundSchedule(leagueId, roundNum) {
			return $http.delete(EnvConfig.api + "api/league/" + leagueId + "/schedule/" + roundNum)
				.then(function (response) {
					return response.data;
				});
		}

		function getStandings(leagueId) {
			return $http.get(EnvConfig.api + "api/league/" + leagueId + "/standings")
				.then(function (response) {
					return response.data;
				});
		}

		return service;
	}
})();