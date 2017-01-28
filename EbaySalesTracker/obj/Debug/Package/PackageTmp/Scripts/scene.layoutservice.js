//Contains tile size and scene layout details
var sceneLayoutService = function () {
    var width,
    //scene1
    pad = 6,
    r1H = 210,
    //small
    s1Sh = 93,
    s1Sh2 = 93,
    s1Sw = 264,
    //medium
    s1Mh = 197,
    s1Mw = 365,
    s1Mw2 = 270,
    //large
    s1Lh = 340,
    s1Lw = 584,
    itop = 0,
    ileft = 0,
    items = { tiles: [] },
        
        populateLayout = function () {
            dataService.GetInventoryItems(setUpLayout)
        },
        setUpLayout = function (data) {
            $.each(data, function (index, data) {
                items.tiles.push({
                    name: 'Inventory Item ' + data.Id,
                    tileId: 'InventoryItem' + data.Id,
                    borderColor: '#dcdcdc'
                    
                    //scenes: [
                    //    { height: s1Mh, width: s1Mw, top: 0 + itop, left: 0 + ileft, opacity: 1, size: 1, borderColor: '#5E1B6B', z: 0 },
                    //    { height: 90, width: 210, top: 80 + itop, left: 250 + ileft, size: 0, borderColor: '#5E1B6B', z: '2000', opacity: .5 }
                    //]
                });
                ileft += s1Mw + 1;
                itop -= s1Mh;
            });
            sceneStateManager.init(items);
            sceneStateManager.renderTiles("bd7844bc-b2cb-4ccb-b489-09776e1f69c2");

        },
        get = function () {
            width = $('#content').width();
            populateLayout();

        };

    return {
        get: get
    };

}();