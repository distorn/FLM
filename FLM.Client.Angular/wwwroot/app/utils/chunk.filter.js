(function () {
	'use strict';

	angular
		.module("FLMClientApp.utils")
		.filter("chunk", chunkFilter);

	function chunkFilter(lodash) {
		return lodash.memoize(lodash.chunk);
	}
})();