var sceneStateManager = function () {
    var sceneId = 0,
    animateSpeed = 760,
    tiles,
    init = function (args) {
        tiles = args.tiles;
        var wellDiv = $('<div/>', { 'class': 'well', 'id': 'well' });
            wellDiv.appendTo('#container')
        var carouselDiv = $('<div/>', { 'class': 'carousel slide', 'id': 'myCarousel' });
            carouselDiv.appendTo('#well');
        var carouselInnerDiv = $('<div/>', { 'class': 'carousel-inner', 'id': 'carouselInner' });
            carouselInnerDiv.appendTo('#myCarousel');
            var carouselItemDiv = $('<div/>', { 'class': 'item', 'id': 'carouselItem' });
            var carouselRowDiv = $('<div/>', { 'class': 'row', 'id': 'carouselRow' });
            var leftScroll = $('<a/>', { 'class': 'left carousel-control', 'href': '#myCarousel', 'data-slide': 'prev' });
            var rightScroll = $('<a/>', { 'class': 'right carousel-control', 'href': '#myCarousel', 'data-slide': 'next' });
        //var carouselActiveItemDiv = $('<div/>', { 'class': 'active item', 'id': 'carouselItem' });
            var loopCount = 0;
            
        $(tiles).each(function (index) {
            //build tiles
            if (index == 0)
            {
                carouselItemDiv = $('<div/>', { 'class': 'active item', 'id': 'carouselItem' + loopCount });
                carouselItemDiv.appendTo(carouselInnerDiv);
            }

            if (index != 0 && index % 4 == 0)
            {
                loopCount++;
                carouselItemDiv = $('<div/>', { 'class': 'item', 'id': 'carouselItem' + loopCount });
                carouselItemDiv.appendTo(carouselInnerDiv);
                
            }
            carouselRowDiv = $('<div/>', { 'class': 'row', 'id': 'carouselRow'+loopCount });
            carouselRowDiv.appendTo('#carouselItem'+loopCount)
            var tileDiv = $('<div/>', { 'class': 'tile col-sm-3', 'text': this.name, 'id': this.tileId });
            //tileDiv.css('border-top', '5px solid ' + this.scenes[sceneId].borderColor);
            tileDiv.data(this);

            //moveTile(tileDiv, this.scenes[sceneId]);
            
            tileDiv.appendTo('#carouselRow'+loopCount);

            //if (index < 8) {
            //    tileDiv.addClass('top-row');
            //}

            //tileDiv.draggable({ opacity: 0.9, zIndex: 5000, revert: 'invalid', revertDuration: 500 });
            //tileDiv.droppable({
            //    tolerance: 'pointer',
            //    drop: function (event, ui) {
            //        swapTiles(ui.draggable, $(this));
            //    }
            //});
        });
        leftScroll.appendTo('#myCarousel');
        rightScroll.appendTo('#myCarousel');

        //$('#left').click(slideRight);
        //$('#right').click(slideLeft);
    },

    renderTiles = function (userId) {
        //Call data service delegate to get account information
        //Once information returns bindTiles will be invoked

        //calls to data service to get user inventory and render inventory tiles
        dataService.GetInventoryItems(renderInventoryTiles);

       //renderDefaultTiles();
    },

     renderInventoryTiles = function (data) {
         //$('div.tile[id^="Inventory"]').each(function () {
         //    var tileDiv = $(this);
         //    renderTile(data, tileDiv, 0);
         //});
         var i = 0;

         $.each(data, function (index, data) {
             var tileDiv = $('#InventoryItem' + data.Id);

             renderTile(data, tileDiv, 0);
         });

         //$.each(data, function (index, invItem) {
         //    renderTile(data, invItem.Id, 0);
         //});

         //$.each(data, function (index, invItem) {
         //    var test = jQuery('<div/>', {
         //        id: 'InventoryItem',
         //        //href: 'http://google.com',
         //        //title: 'Become a Googler',
         //        //rel: 'external',
         //        text: invItem.Description
         //    });
         //    renderTile(data, test, 0);
         //});

         //$("#container").html($("#InventoryItem").render(data));


     },

     renderTile = function (data, tile, fadeInAmount) {
         if (fadeInAmount > 0) tile.fadeOut(fadeInAmount);
         if (fadeInAmount > 0) {
             tile.fadeIn(fadeInAmount,
                 function () {
                     tileBinder.bind(tile, data, tileRenderer.render);
                 }
             );
         }
         else {
             tileBinder.bind(tile, data, tileRenderer.render);
         }
     },

    renderDefaultTiles = function () {
        $('#Video, #Quote, #SectorSummary, #MarketNews, #Currencies').each(function () {
            if ($(this).data().templates == null) {
                var tileDiv = $(this);
                tileBinder.bind(tileDiv, null, tileRenderer.render);
            }
        });
    },

     swapTiles = function (source, target) {
         var sourceScene = source.data().scenes[sceneId];
         var targetScene = target.data().scenes[sceneId];

         moveTile(target, sourceScene);
         moveTile(source, targetScene);

         swapScenes(sourceScene, targetScene);

         if (sceneId == 0) {
             //handle top row scrolling
             var sourceTopRow = source.hasClass('top-row');
             var targetTopRow = target.hasClass('top-row');
             if (sourceTopRow && !targetTopRow) {
                 source.removeClass('top-row');
                 target.addClass('top-row');
             } else if (!sourceTopRow && targetTopRow) {
                 target.removeClass('top-row');
                 source.addClass('top-row');
             }
         }

         //resize
         var sourceSize = sourceScene.size;
         var targetSize = targetScene.size;
         if (sourceSize != targetSize) {

             target.data().scenes[sceneId].size = sourceSize;
             source.data().scenes[sceneId].size = targetSize;

             tileRenderer.render(target);
             tileRenderer.render(source);
         }
     },

    slideLeft = function () {
        $('.top-row').animate({ 'left': '-=250px' }, 800, function () {
            $(this).data().scenes[sceneId].left -= 250;
        });
    },

    slideRight = function () {
        $('.top-row').animate({ 'left': '+=250px' }, 800, function () {
            $(this).data().scenes[sceneId].left += 250;
        });
    },
        //changeScene = function () {
        //    if (sceneId == 0) {
        //        sceneId = 1;
        //        $('.scroller').hide();
        //        $('#gridButton').delay(Math.floor(Math.random() * 450)).attr('disabled', false).addClass('enabled');
        //        $('#cloudButton').delay(Math.floor(Math.random() * 450)).attr('disabled', true).removeClass('enabled');

        //    } else if (sceneId == 1) {
        //        sceneId = 0;
        //        $('.scroller').show();
        //        $('#cloudButton').attr('disabled', false).addClass('enabled');
        //        $('#gridButton').attr('disabled', true).removeClass('enabled');
        //    }

        //    $('.tile').each(function () {
        //        var tile = $(this);
        //        moveTile(tile, tile.data().scenes[sceneId]);
        //        tileRenderer.render(tile, sceneId);
        //    });
        //},

    moveTile = function (tile, scene) {
        tile.animate({
            opacity: scene.opacity,
            top: scene.top,
            left: scene.left,
            height: scene.height,
            width: scene.width,
            zIndex: scene.z
        },
            {
                duration: animateSpeed,
                easing: "easeInOutExpo",
                step: function () { },
                complete: function () { }
            });
        //},

        //swapScenes = function (source, target) {
        //    var top = source.top,
        //        left = source.left,
        //        height = source.height,
        //        width = source.width,
        //        opacity = source.opacity,
        //        zindex = source.zindex;

        //    source.top = target.top;
        //    source.left = target.left;
        //    source.height = target.height;
        //    source.width = target.width;
        //    source.opacity = target.opacity;
        //    source.zindex = target.zindex;

        //    target.top = top;
        //    target.left = left;
        //    target.height = height;
        //    target.width = width;
        //    target.opacity = opacity;
        //    target.zindex = zindex;
    };



    return {
        init: init,
        //changeScene: changeScene,
        getTiles: function () { return tiles; },
        renderTiles: renderTiles,
        renderTile: renderTile
    };

}();
