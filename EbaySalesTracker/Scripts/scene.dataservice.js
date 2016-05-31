//Encapsulates data calls to server (AJAX calls)

var dataService = new function () {
    var serviceBase = '/DataService/',
        getInventoryItems = function (callback) {
            $.getJSON(serviceBase + 'GetInventoryItems', {}, function (data) {
                callback(data);
            });
        
    //    getMarketIndexes = function (callback) {
    //        $.getJSON(serviceBase + 'GetMarketIndexes', function (data) {
    //            callback(data);
    //        });
    //    },

    //    getQuote = function (sym, callback) {
    //        $.getJSON(serviceBase + 'GetQuote', { symbol: sym }, function (data) {
    //            callback(data);
    //        });
    //    },

    //    getTickerQuotes = function (callback) {
    //        $.getJSON(serviceBase + 'GetTickerQuotes', function (data) {
    //            callback(data);
    //        });
        };

    return {
        GetInventoryItems: getInventoryItems,
    //    getMarketIndexes: getMarketIndexes,
    //    getQuote: getQuote,
    //    getTickerQuotes: getTickerQuotes
    };

}();