(function () {
	'use strict';

	var app = angular.module("FLMClientApp", [

		"FLMClientApp.config",
		"FLMClientApp.auth",
		"FLMClientApp.league",
		"FLMClientApp.team",
		"FLMClientApp.player",
		"FLMClientApp.match",

		"ngMessages",
		"ngSanitize",
		"ngLodash",

		"ui.router",
		"ui.bootstrap",
		"ui.select"
	]);
})();