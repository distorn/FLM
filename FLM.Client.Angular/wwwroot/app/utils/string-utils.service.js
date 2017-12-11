(function () {
	'use strict';

	angular
		.module("FLMClientApp.utils")
		.factory("stringUtils", stringUtils);

	function stringUtils() {
		var service = {};

		service.urlBase64Decode = urlBase64Decode;

		function urlBase64Decode(str) {
			var output = str.replace('-', '+').replace('_', '/');
			switch (output.length % 4) {
				case 0:
					break;
				case 2:
					output += '==';
					break;
				case 3:
					output += '=';
					break;
				default:
					throw 'Illegal base64url string!';
			}
			return window.atob(output);
		}

		return service;
	}
})();