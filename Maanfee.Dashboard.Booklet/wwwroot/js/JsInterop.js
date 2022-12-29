
function AddStyle(Url) {
    var Link = document.createElement("link");

    Link.rel = "stylesheet";
    Link.href = Url;

    document.body.appendChild(Link);

    return Link;
}

function RemoveStyle(Url) {

    var element = document.querySelector('link[href="' + Url + '"]');
    if (typeof (element) != 'undefined' && element != null) {
        // Exists.
        element.parentNode.removeChild(element);
    }

}

function AddScript(Url) {
    var js = document.createElement("script");

    js.type = "text/javascript";
    js.src = Url;

    document.body.appendChild(js);

    //var js = document.createElement("script");
    //js.type = "text/javascript";
    //js.src = "_content/Maanfee.Dashboard.Views.Booklet/js/Config.js";
    //document.body.appendChild(js);

    return js;
}

function RemoveScript(Url) {

    var element = document.querySelector('script[src="' + Url + '"]');
    if (typeof (element) != 'undefined' && element != null) {
        // Exists.
        element.parentNode.removeChild(element);
    }

}

export function Init() {

    RemoveStyle("_content/Maanfee.Dashboard.Views.Booklet/css/BookletStyle.css");
    RemoveScript("_content/Maanfee.Dashboard.Views.Booklet/js/JQuery.js");
    RemoveScript("_content/Maanfee.Dashboard.Views.Booklet/js/Turn.js");
    RemoveScript("_content/Maanfee.Dashboard.Views.Booklet/js/Zoom.js");
    RemoveScript("_content/Maanfee.Dashboard.Views.Booklet/js/Config.js");
    RemoveScript("_content/Maanfee.Dashboard.Views.Booklet/js/Init.js");

    // Load BookletStyle.css
    AddStyle("_content/Maanfee.Dashboard.Views.Booklet/css/BookletStyle.css").addEventListener('load', () => {
        // **************
        // Load jquery.js
        AddScript("_content/Maanfee.Dashboard.Views.Booklet/js/JQuery.js").addEventListener('load', () => {
            // Load turn.js
            AddScript("_content/Maanfee.Dashboard.Views.Booklet/js/Turn.js").addEventListener('load', () => {
                // Load zoom.min.js
                AddScript("_content/Maanfee.Dashboard.Views.Booklet/js/Zoom.js").addEventListener('load', () => {
                    // Load Config
                    AddScript("_content/Maanfee.Dashboard.Views.Booklet/js/Config.js").addEventListener('load', () => {
                        // Load Init.js
                        AddScript("_content/Maanfee.Dashboard.Views.Booklet/js/Init.js").addEventListener('load', () => {
                            InitBooklet();
                            //    Alert();
                        });
                    });
                });
            });
        });
        // **************
    });

}

export function Dispose() {
    // Destroy any previous bindings
    var flipbook = $('.magazine');

    if (flipbook.turn('is')) {
        flipbook.turn('destroy').remove();
        $(window).unbind('keydown');
        $(window).unbind('resize');
    }
}
