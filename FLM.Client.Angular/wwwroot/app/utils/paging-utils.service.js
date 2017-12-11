(function () {
	'use strict';

	angular
		.module("FLMClientApp.utils")
		.factory("pagingUtils", pagingUtils);

	function pagingUtils(EnvConfig) {
		var service = {};

		service.newPagingModel = newPagingModel;
		service.updatePagingModel = updatePagingModel;
		service.getParamsFromModel = getParamsFromModel;

		function newPagingModel() {
			return {
				itemsPerPage: EnvConfig.defaultItemsPerPage,
				currentPage: 1,
				totalItems: 0,
			};
		}

		function updatePagingModel(pagingModel, serverData) {
			pagingModel.currentPage = serverData.pageNumber;
			pagingModel.totalItems = serverData.itemCount;
			pagingModel.itemsPerPage = serverData.pageSize;
		}

		function getParamsFromModel(pagingModel) {
			return {
				pageSize: pagingModel.itemsPerPage,
				pageNumber: pagingModel.currentPage
			};
		}

		return service;
	}
})();