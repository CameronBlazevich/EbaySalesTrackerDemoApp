//Handles loading jQuery templates dynamically from server
//and rendering them based upon tile data
var tileBinder = function () {
    var templateBase = '/Templates/',

    bind = function (tileDiv, data, renderer) {
        var tileName = tileDiv.attr('id');
        $.get(templateBase + tileName + '.html', function (templates) {
            $('body').append(templates);
            var acctTemplates = [
                tmpl(tileName, 'Small', data),
                tmpl(tileName, 'Medium', data),
                tmpl(tileName, 'Large', data)
            ];
            tileDiv.data().templates = acctTemplates;
            tileDiv.data().tileData = data;

            renderer(tileDiv);
        });
    },

    tmpl = function (tileName, size, data) {
        var template = $('#' + tileName + 'Template_' + size);

        if (data != null)

            return template.render(data);
        else
            return template.html();
    };

    return {
        bind: bind,

    };
}();
