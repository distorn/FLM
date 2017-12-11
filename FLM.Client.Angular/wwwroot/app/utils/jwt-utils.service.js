(function () {
	'use strict';

	angular
		.module("FLMClientApp.utils")
		.factory("jwtUtils", jwtUtils);

	function jwtUtils(stringUtils) {
		var service = {};

		service.getDataFromToken = getDataFromToken;

		function getDataFromToken(token) {
			var data = {};
			if (typeof token !== 'undefined') {
				var encoded = token.split('.')[1]; // get payload
				data = JSON.parse(stringUtils.urlBase64Decode(encoded));
			}
			return data;
		}

		return service;
	}
})();