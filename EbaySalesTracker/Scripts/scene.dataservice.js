//Encapsulates data calls to server (AJAX calls)

var dataService = new function () {
    var serviceBase = '/DataService/',
        getInventoryItems = function (callback) {
            $.getJSON(serviceBase + 'GetInventoryItems', {}, function (data) {
                callback(data);
            });
        },
        
        getInventoryDetailsPartial = function (id, callback) {
            console.log("Here");
            $.getJSON('../InventoryItems/DetailsPartial', { id: id }, function (data) {
                callback(data);
            });

        },
          getInventoryItemById = function (id, callback) {
              $.getJSON(serviceBase + 'GetInventoryItemById', { id: id }, function (data) {
                  callback(data);
              });
          },
          getInventoryItemSalesOverTime = function (invItemId, callback) {
              $.getJSON(serviceBase + 'GetDataForAverageProfitOverTime', { id: invItemId }, function (data) {
                  callback(data);
              });
          },

        getItemSalesDataByMonth = function (id, callback) {
            $.getJSON(serviceBase + 'GetItemSalesDataByMonth', { id: id }, function (data) {
                    callback(data);               
            });
        },
        getHighestAverageProfitItem = function (callback) {
            $.getJSON(serviceBase + 'GetHighestAverageProfitItem', {}, function (data) {
                callback(data);
            });
        },
        getProfitByMonth = function (year, month,callback) {
            $.getJSON(serviceBase + 'GetProfitByMonth', {year:year, month:month}, function (data) {
                callback(data);
            });
        },
        getSalesByMonth = function (year, month, callback) {
            $.getJSON(serviceBase + 'GetSalesByMonth', { year: year, month: month }, function (data) {
                callback(data);
            });
        },
        getBestSellingItem = function (callback) {
            $.getJSON(serviceBase + 'GetBestSellingItem', {}, function (data) {
                callback(data);
            })
        },
        updateInventoryItem = function (id,description,cost,costChange,callback) {
            $.post('../InventoryItems/Edit', { Id: id, Description: description, Cost: cost, costChange: costChange }, callback);
        },
        deleteInventoryItem = function (id, callback) {
            $.post('../InventoryItems/Delete/' + id, { id: id }, callback);
        },
        getSalesSummaries = function ( callback) {
            $.get(serviceBase + 'GetSalesSummaries', {}, function (data) {
                callback(data);
            })
        },
        getProfitSummaries = function (callback) {
            $.get(serviceBase + 'GetProfitSummaries', {}, function (data) {
                callback(data);
            })
        },
        getFeesSummaries = function (callback) {
            $.get(serviceBase + 'GetFeesSummaries', {}, function (data) {
                callback(data);
            })
        };

    return {
        GetInventoryItems: getInventoryItems,
        GetInventoryDetailsPartial: getInventoryDetailsPartial,
        GetInventoryItemById: getInventoryItemById,
        GetItemSalesDataByMonth: getItemSalesDataByMonth,
        GetInventoryItemSalesOverTime: getInventoryItemSalesOverTime,
        GetBestSellingItem: getBestSellingItem,
        GetHighestAverageProfitItem: getHighestAverageProfitItem,
        GetProfitByMonth: getProfitByMonth,
        GetSalesByMonth: getSalesByMonth,
        UpdateInventoryItem: updateInventoryItem,
        DeleteInventoryItem: deleteInventoryItem,
        GetSalesSummaries: getSalesSummaries,
        GetProfitSummaries: getProfitSummaries,
        GetFeesSummarites: getFeesSummaries
    //    getMarketIndexes: getMarketIndexes,
    //    getQuote: getQuote,
    //    getTickerQuotes: getTickerQuotes
    };

}();