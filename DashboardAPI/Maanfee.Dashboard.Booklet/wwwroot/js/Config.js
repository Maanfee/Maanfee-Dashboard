/*
 * Magazine sample
*/

// Zoom in / Zoom out
function zoomTo(event) {

    //alert($(event.target).attr('class'));
    //alert($(event.target.nodeName).find("img").attr('class'));
    //alert(event.target.id + " | " + $(event.target).attr('class'));

    setTimeout(function () {

            if (event.target && $(event.target).hasClass('zoom-this')) {
            if ($('.magazine-viewport').data().regionClicked) {
                $('.magazine-viewport').data().regionClicked = false;
            }
            else {
                if ($('.magazine-viewport').zoom('value') == 1) {
                    $('.magazine-viewport').zoom('zoomIn', event);
                }
                else {
                    $('.magazine-viewport').zoom('zoomOut');
                }
            }
        }

    }, 10);

}

// Process click on a region
function regionClick(event) {

    var region = $(event.target);
    if (region.hasClass('region')) {
        $('.magazine-viewport').data().regionClicked = true;
        setTimeout(function () {
            $('.magazine-viewport').data().regionClicked = false;
        }, 100);

        var regionType = $.trim(region.attr('class').replace('region', ''));
        return processRegion(region, regionType);
    }

}

// Process the data of every region

function processRegion(region, regionType) {

    data = decodeParams(region.attr('region-data'));
    switch (regionType) {
        case 'link':
            window.open(data.url);
            break;
        case 'zoom':
            var regionOffset = region.offset(),
				viewportOffset = $('.magazine-viewport').offset(),
				pos = {
				    x: regionOffset.left - viewportOffset.left,
				    y: regionOffset.top - viewportOffset.top
				};
            $('.magazine-viewport').zoom('zoomIn', pos);
            break;
        case 'to-page':
            $('.magazine').turn('page', data.page);
            break;
    }
}

// Load large page
function loadLargePage(page, pageElement) {

}

// Load small page
function loadSmallPage(page, pageElement) {

}

// http://code.google.com/p/chromium/issues/detail?id=128488
function isChrome() {
    return navigator.userAgent.indexOf('Chrome') != -1;
}

function disableControls(page) {
    if (page == 1)
        $('.previous-button').hide();
    else
        $('.previous-button').show();

    if (page == $('.magazine').turn('pages2'))
        $('.next-button').hide();
    else
        $('.next-button').show();
}

// Set the width and height for the viewport
function resizeViewport() {

    var width = $(window).width(),
        height = $(window).height(),
        options = $('.magazine').turn('options');

    $('.magazine').removeClass('animated');

    $('.magazine-viewport').css({
        width: width,
        height: height
    }).zoom('resize');

    if ($('.magazine').turn('zoom') == 1) {
        var bound = calculateBound({
            width: options.width,
            height: options.height,
            boundWidth: Math.min(options.width, width),
            boundHeight: Math.min(options.height, height)
        });

        if (bound.width % 2 !== 0)
            bound.width -= 1;

        if (bound.width != $('.magazine').width() || bound.height != $('.magazine').height()) {

            $('.magazine').turn('size', bound.width, bound.height);

            if ($('.magazine').turn('page') == 1)
                $('.magazine').turn('peel', 'br');

            $('.next-button').css({ height: bound.height, backgroundPosition: '-38px ' + (bound.height / 2 - 32 / 2) + 'px' });
            $('.previous-button').css({ height: bound.height, backgroundPosition: '-4px ' + (bound.height / 2 - 32 / 2) + 'px' });
        }

        $('.magazine').css({ top: -bound.height / 2, left: -bound.width / 2 });
    }

    var magazineOffset = $('.magazine').offset();

    if (magazineOffset.top < $('.made').height())
        $('.made').hide();
    else
        $('.made').show();

    $('.magazine').addClass('animated');

}

// Width of the flipbook when zoomed in
function largeMagazineWidth() {
    return 2214;
}

// decode URL Parameters
function decodeParams(data) {

    var parts = data.split('&'), d, obj = {};

    for (var i = 0; i < parts.length; i++) {
        d = parts[i].split('=');
        obj[decodeURIComponent(d[0])] = decodeURIComponent(d[1]);
    }

    return obj;
}

// Calculate the width and height of a square within another square
function calculateBound(d) {

    var bound = { width: d.width, height: d.height };

    if (bound.width > d.boundWidth || bound.height > d.boundHeight) {

        var rel = bound.width / bound.height;

        if (d.boundWidth / rel > d.boundHeight && d.boundHeight * rel <= d.boundWidth) {

            bound.width = Math.round(d.boundHeight * rel);
            bound.height = d.boundHeight;

        }
        else {

            bound.width = d.boundWidth;
            bound.height = Math.round(d.boundWidth / rel);

        }
    }

    return bound;
}

// ********************************************************
