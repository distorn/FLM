(function () {
	'use strict';

	angular
		.module("FLMClientApp.match")
		.factory("matchService", matchService);

	function matchService($http, $q, EnvConfig, errorLogService, dateFieldConverter, pagingUtils) {
		var service = {};

		// - API communication -

		service.getMatches = getMatches;
		service.getMatchById = getMatchById;
		service.createMatch = createMatch;
		service.editMatch = editMatch;
		service.deleteMatch = deleteMatch;

		// - Data Utils -

		service.newItem = newItem;
		service.newFilter = newFilter;
		service.getDisplayName = getDisplayName;

		function newItem() {
			return {
				team1Id: null,
				team2Id: null,
				date: null,
			};
		}

		function newFilter() {
			return {
				leagueId: null,
				teamId: null,
				minDate: null,
				maxDate: null,
			};
		}

		function getDisplayName(item) {
			return item.firstName + " " + item.lastName;
		}

		function getMatches(pagingModel = null, filter = null) {
			var config = {};

			if (pagingModel) {
				config.params = pagingUtils.getParamsFromModel(pagingModel);
			}

			return $http.post(EnvConfig.api + "api/match/search", filter, config)
				.then(function (response) {
					return response.data;
				});
		}

		function getMatchById(itemID) {
			return $http.get(EnvConfig.api + "api/match/" + itemID)
				.then(function (response) {
					var item = response.data.model;
					dateFieldConverter.convertFields(item, ["date"]);
					return item;
				});
		}

		function createMatch(item) {
			return $http.post(EnvConfig.api + "api/match", item)
				.then(function (response) {
					return response.data;
				});
		}

		function editMatch(item) {
			return $http.put(EnvConfig.api + "api/match", item)
				.then(function (response) {
					return response.data;
				});
		}

		function deleteMatch(itemID) {
			return $http.delete(EnvConfig.api + "api/match/" + itemID)
				.then(function (response) {
					return response.data;
				});
		}

		return service;
	}
})();