
//function Alert() {

//    alert("KOOOOOOOOOOOOO !!!");

//}

function InitBooklet() {

    $('.magazine-viewport').hide();

    // *******************************
    var flipbook = $('.magazine');

    // Check if the CSS was already loaded
    //if (flipbook.width() == 0 || flipbook.height() == 0) {
    //    setTimeout(JSMethod, 10);
    //    return;
    //}

    // Create the flipbook
    flipbook.turn({
        width: $(window).width() * 0.65,
        height: $(window).height() * 0.94,
        // Duration in millisecond
        duration: 1000,
        // Hardware acceleration
        acceleration: !isChrome(),
        // Enables gradients
        gradients: true,
        // Auto center this flipbook
        autoCenter: false,
        // Elevation from the edge of the flipbook when turning a page
        elevation: 50,
        // Direction 
        direction: 'rtl',
        // Events
        when: {
            turning: function (event, page, view) {
                //var book = $(this),
                //    currentPage = book.turn('page'),
                //    pages = book.turn('pages');

                //$(".page-number-odd").text(currentPage + 1);
                //$(".page-number-even").text(currentPage);
                //$('.page-number').text(currentPage);

                page -= 2;

                //page number
                if (page % 2 == 0) {
                    $('.page-number-even').text(page + 1);
                    $('.page-number-odd').text(page);
                }
                else {
                    $('.page-number-even').text(page);
                    $('.page-number-odd').text(page - 1);
                }
                //disableControls(page);
            },
            turned: function (event, page, view) {

                $(this).turn('center');

                //if (page == 1) {
                //    $(this).turn('peel', 'br');
                //}
                //disableControls(page);
            },
            missing: function (event, pages) {

            }
        }
    });

    // Using arrow keys to turn the page
    $(document).keydown(function (e) {

        setTimeout(function () {

            if ($('.magazine-viewport').data().regionClicked) {
                $('.magazine-viewport').data().regionClicked = false;
            }
            else {
                // Another Method
                //if ($('.magazine-viewport').find("div").hasClass('zoom-in')) {
                //    $('.magazine-viewport').zoom('zoomOut');
                //    e.preventDefault();
                //}
                if ($('.magazine-viewport').zoom('value') == 1) {
                    //$('.magazine-viewport').zoom('zoomIn');
                }
                else {
                    $('.magazine-viewport').zoom('zoomOut');    // وقتی صفحه ورق می خورد از زوم خارج می گردد
                }
            }
        }, 500);

        var previous = 37, next = 39, esc = 27;

        switch (e.keyCode) {
            case previous:

                // left arrow
                $('.magazine').turn('previous');
                //e.preventDefault();   // کلید راست و چپ را غیر فعال می کند

                break;
            case next:

                //right arrow
                $('.magazine').turn('next');
                //e.preventDefault();   // کلید راست و چپ را غیر فعال می کند

                break;
            case esc:

                $('.magazine-viewport').zoom('zoomOut');
                //e.preventDefault();

                break;
        }

    });

    // *******************************

    // Zoom.js
    $('.magazine-viewport').zoom({
        flipbook: $('.magazine'),
        max: function () {
            return largeMagazineWidth() / $('.magazine').width();
        },

        when: {
            swipeLeft: function () {
                $(this).zoom('flipbook').turn('next');
            },

            swipeRight: function () {
                $(this).zoom('flipbook').turn('previous');
            },

            resize: function (event, scale, page, pageElement) {
                if (scale == 1)
                    loadSmallPage(page, pageElement);
                else
                    loadLargePage(page, pageElement);
            },

            zoomIn: function () {
                $('.made').hide();
                $('.magazine').removeClass('animated').addClass('zoom-in');
                $('.zoom-icon').removeClass('zoom-icon-in').addClass('zoom-icon-out');

                if (!window.escTip && !$.isTouch) {
                    escTip = true;

                    $('<div />', { 'class': 'exit-message' }).
                        html('<div>Press ESC to exit</div>').
                        appendTo($('body')).
                        delay(2000).
                        animate({ opacity: 0 }, 500, function () {
                            $(this).remove();
                        });
                }
            },

            zoomOut: function () {
                $('.exit-message').hide();
                $('.made').fadeIn();
                $('.zoom-icon').removeClass('zoom-icon-out').addClass('zoom-icon-in');
                setTimeout(function () {
                    $('.magazine').addClass('animated').removeClass('zoom-in');
                    resizeViewport();
                }, 0);

            }
        }
    });

    // Zoom event
    if ($.isTouch)
        $('.magazine-viewport').bind('zoom.doubleTap', zoomTo);
    else
        $('.magazine-viewport').bind('zoom.tap', zoomTo);
    // *******************************

    $(window).resize(function () {
        resizeViewport();
    }).bind('orientationchange', function () {
        resizeViewport();
    });
    // *******************************

    // Regions
    if ($.isTouch) {
        $('.magazine').bind('touchstart', regionClick);
    }
    else {
        $('.magazine').click(regionClick);
    }
    // *******************************

    // Events for the next button
    $('.next-button').bind($.mouseEvents.over, function () {

        $(this).addClass('next-button-hover');

    }).bind($.mouseEvents.out, function () {

        $(this).removeClass('next-button-hover');

    }).bind($.mouseEvents.down, function () {

        $(this).addClass('next-button-down');

    }).bind($.mouseEvents.up, function () {

        $(this).removeClass('next-button-down');

    }).click(function () {

        $('.magazine').turn('next');

    });

    // Events for the previous button
    $('.previous-button').bind($.mouseEvents.over, function () {

        $(this).addClass('previous-button-hover');

    }).bind($.mouseEvents.out, function () {

        $(this).removeClass('previous-button-hover');

    }).bind($.mouseEvents.down, function () {

        $(this).addClass('previous-button-down');

    }).bind($.mouseEvents.up, function () {

        $(this).removeClass('previous-button-down');

    }).click(function () {

        $('.magazine').turn('previous');

    });

    // *******************************

    resizeViewport();

    $('.magazine').addClass('animated');

    // *******************************

    // Zoom icon
    $('.zoom-icon').bind('mouseover', function () {

        if ($(this).hasClass('zoom-icon-in'))
            $(this).addClass('zoom-icon-in-hover');

        if ($(this).hasClass('zoom-icon-out'))
            $(this).addClass('zoom-icon-out-hover');

    }).bind('mouseout', function () {

        if ($(this).hasClass('zoom-icon-in'))
            $(this).removeClass('zoom-icon-in-hover');

        if ($(this).hasClass('zoom-icon-out'))
            $(this).removeClass('zoom-icon-out-hover');

    }).bind('click', function () {

        if ($(this).hasClass('zoom-icon-in'))
            $('.magazine-viewport').zoom('zoomIn');
        else if ($(this).hasClass('zoom-icon-out'))
            $('.magazine-viewport').zoom('zoomOut');
    });

    //// Exit Icon
    //$('.Exit-icon').bind('click', function () {

    //    if (document.exitFullscreen) {
    //        document.exitFullscreen();
    //    }
    //    else if (document.mozCancelFullScreen) {
    //        document.mozCancelFullScreen();
    //    }
    //    else if (document.webkitCancelFullScreen) {
    //        document.webkitCancelFullScreen();
    //    }
    //    else if (document.msExitFullscreen) {
    //        document.msExitFullscreen();
    //    }

    //});

    // Show Page Number
    //$(".magazine").bind("turning", function (event, page, view) {
    //    //var book = $(this);
    //    //var currentPage = book.turn('page');
    //    //var pages = book.turn('pages');
    //    page -= 2;

    //    //page number
    //    if (page % 2 == 0) {
    //        $('.page-number-even').text(page + 1);
    //        $('.page-number-odd').text(page);
    //    } else {
    //        $('.page-number-even').text(page);
    //        $('.page-number-odd').text(page - 1);
    //    }
    //});

    $('.magazine-viewport').show();
}
