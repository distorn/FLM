(function () {
	'use strict';

	angular
		.module("FLMClientApp.utils")
		.factory("dateUtils", dateUtils);

	function dateUtils() {
		var service = {};

		service.getDefaultDatePickerOptions = getDefaultDatePickerOptions;

		function getDefaultDatePickerOptions() {
			// TODO: switch Monday/Sunday as first day according to user's locale
			return {
				startingDay: 1,
				showWeeks: false
			};
		}

		return service;
	}
})();