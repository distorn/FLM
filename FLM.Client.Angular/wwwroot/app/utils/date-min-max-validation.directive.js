(function () {
	'use strict';

	angular
		.module("FLMClientApp.utils")
		.directive("dateMinMaxValidation", dateMinMaxValidation);

	function dateMinMaxValidation() {
		var directive = {};

		directive.restrict = "A";
		directive.require = "ngModel";

		directive.link = function (scope, element, attrs, ctrl) {
			var options = scope.$eval(attrs.datepickerOptions);

			if (options.minDate) {
				ctrl.$validators.minDate = function (modelValue, viewValue) {
					return modelValue > options.minDate;
				}
			}

			if (options.maxDate) {
				ctrl.$validators.maxDate = function (modelValue, viewValue) {
					return modelValue < options.maxDate;
				}
			}
		}

		return directive;
	}
})();