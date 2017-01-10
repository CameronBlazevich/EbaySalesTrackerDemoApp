

var helper = new function () {
    var elliptify = function (value, totalDisplayLength)  {
        if (value.length > totalDisplayLength) {
           return value.substr(0, totalDisplayLength-3) + '...';
        }
        else {
            return value;
        }
    },
        monetaryRound = function (number) {
            return parseFloat(Math.round(number*100)/100).toFixed(2);
        };

    return {
        elliptify: elliptify,
        monetaryRound: monetaryRound
    };
}();