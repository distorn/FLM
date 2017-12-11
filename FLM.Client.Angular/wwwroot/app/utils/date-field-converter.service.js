(function () {
	'use strict';

	angular
		.module("FLMClientApp.utils")
		.factory("dateFieldConverter", dateFieldConverter);

	function dateFieldConverter() {
		var service = {};
		service.iso8601RegEx = /(19|20|21)\d\d([-/.])(0[1-9]|1[012])\2(0[1-9]|[12][0-9]|3[01])T(\d\d)([:/.])(\d\d)([:/.])(\d\d)/;

		service.convertField = convertField;
		service.convertFields = convertFields;

		function convertField(item, propertyName) {
			var value = item[propertyName];
			if (typeof value === "string" && value.match(service.iso8601RegEx)) {
				var dateValue = new Date(value);
				item[propertyName] = dateValue;
			} else {
				console.warn("Can't convert field [", propertyName, "] to Date. Item: ", item);
			}
		}

		function convertFields(object, propertyNames) {
			if (object.constructor === Array) {
				// object is array, convert fields for every child item
				for (var itemIndex = 0; itemIndex < object.length; itemIndex++) {
					var item = object[itemIndex];
					for (var propIndex = 0; propIndex < propertyNames.length; propIndex++) {
						service.convertField(item, propertyNames[propIndex]);
					}
				}
			} else {
				// simple object passed, just convert properties
				for (var i = 0; i < propertyNames.length; i++) {
					service.convertField(object, propertyNames[i]);
				}
			}
		}

		return service;
	}
})();