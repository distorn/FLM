(function () {
	'use strict';

	angular
		.module("FLMClientApp.utils")
		.directive("datepickerLocaldate", datepickerLocaldate);

	function datepickerLocaldate() {
		var directive = {};

		directive.restrict = "A";
		directive.require = "ngModel";

		// FIX timezone issues with angular-ui datepicker input
		// based on https://gist.github.com/weberste/354a3f0a9ea58e0ea0de

		directive.link = function link(scope, element, attr, ctrl) {
			// called with a JavaScript Date object when picked from the datepicker
			ctrl.$parsers.push(function (viewValue) {
				// undo the timezone adjustment we did during the formatting
				if (viewValue) {
					viewValue.setMinutes(viewValue.getMinutes() - viewValue.getTimezoneOffset());
				}
				return viewValue;
			});
		}

		return directive;
	}
})();