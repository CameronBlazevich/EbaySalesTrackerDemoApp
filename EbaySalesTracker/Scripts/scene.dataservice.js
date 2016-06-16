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

        getItemSalesDataByMonth = function (id, callback) {
            $.getJSON(serviceBase + 'GetItemSalesDataByMonth', { id: id }, function (data) {                
                    callback(data);               
            });
        };

    return {
        GetInventoryItems: getInventoryItems,
        GetInventoryDetailsPartial: getInventoryDetailsPartial,
        GetInventoryItemById: getInventoryItemById,
        GetItemSalesDataByMonth: getItemSalesDataByMonth
    //    getMarketIndexes: getMarketIndexes,
    //    getQuote: getQuote,
    //    getTickerQuotes: getTickerQuotes
    };

}();