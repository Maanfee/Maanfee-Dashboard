/*
 * JavaScript Library v0.0.3
 *
 *
 *
 * Date: 01-11-01
 */

window.AppJsInterop = {
    QuerySelector: (Selector) => {

        const Node = document.querySelector(Selector);

        var Obj = {
            TagName: Node.tagName,
            Selector: Selector,
            ClassName: Node.className,
            Id: Node.id,
            Name: Node.getAttribute("name"),
            Value: Node.value,
            Href: Node.href,
            Text: Node.innerHTML
        };

        //alert(Node.tagName);

        return JSON.stringify(Obj);
    },
    QuerySelectorAll: (Selector) => {

        const NodeList = document.querySelectorAll(Selector);

        let Objs = new Array();

        NodeList.forEach(function (elem) {
            var Obj = {
                TagName: Node.tagName,
                Selector: Selector,
                ClassName: elem.className,
                Id: elem.id,
                Name: elem.getAttribute("name"),
                Value: elem.value,
                Href: elem.href,
                Text: elem.innerHTML
            };
            Objs.push(Obj);
        });

        //alert(Obj.Text);

        //Objs.forEach(function (elem) {
        //    alert(elem.Text);
        //});

        return JSON.stringify(Objs);

        //    //if (!NodeList)
        //    //    throw "element not found";

        //    //alert(JSON.stringify(NodeList[0]));
        //    //alert(NodeList[0].innerHTML);

        //    return Array.from(NodeList);
    },
    // *********************** HTML/CSS ***********************
    Text: (Selector, Text) => {
        var Elements = document.querySelectorAll(Selector);

        let Texts = new Array();

        Elements.forEach(item => {
            if (Text && Text !== "") {
                item.innerHTML = Text;
            }
            else {
                Texts.push(item.innerHTML);
            }
        });

        if (Texts.length > 0) {
            return JSON.stringify(Texts);
        }

        return;
    },
    Css: (Selector, Property, Value) => {
        var Elements = document.querySelectorAll(Selector);

        //alert(Elements.length);

        let Texts = new Array();

        Elements.forEach(item => {
            if (Property && Property !== "" && Value && Value !== "") {
                item.style.setProperty(Property, Value);
            }
            else if (Property && Property !== "") {
                //Texts.push(item.innerHTML);
                Texts.push(getComputedStyle(item).getPropertyValue(Property));
            }
            else {
                return;
            }
        });

        if (Texts.length > 0) {
            return JSON.stringify(Texts);
        }

        return;
    },
    AddClass: (Selector, ClassName) => {
        var Elements = document.querySelectorAll(Selector);

        // Class List
        let ClassesArray = ClassName.split(" ");
        //  alert(ClassesArray[0]);

        // For get classes names
        let Classes = new Array();

        Elements.forEach(item => {
            if (ClassName && ClassName !== "") {
                ClassesArray.forEach(itemclass => {
                    item.classList.add(itemclass);
                });
                //item.classList.add(Class);
            }
            else {
                Classes.push(item.classList.toString());
            }
        });

        if (Classes.length > 0) {
            return JSON.stringify(Classes);
        }

        return;
    },
    RemoveClass: (Selector, ClassName) => {
        var Elements = document.querySelectorAll(Selector);

        Elements.forEach(item => {
            if (ClassName && ClassName !== "") {
                item.classList.remove(ClassName);
            }
        });

        return;
    },
    HasClass: (Selector, ClassName) => {
        var Elements = document.querySelectorAll(Selector);

        var Result = false;

        Elements.forEach(item => {
            if (ClassName && ClassName !== "") {
                if (item.classList.contains(ClassName)) {
                    Result = true;
                }
            }
        });

        return Result;
    },
    // *********************** Events ***********************

}

export function QuerySelector(Selector) {
    return AppJsInterop.QuerySelector(Selector);
}

export function QuerySelectorAll(Selector) {
    return AppJsInterop.QuerySelectorAll(Selector);
}

// *********************** HTML/CSS ***********************

export function Text(Selector, Text) {
    return AppJsInterop.Text(Selector, Text);
}

export function Css(Selector, Property, Value) {
    return AppJsInterop.Css(Selector, Property, Value);
}

export function AddClass(Selector, ClassName) {
    return AppJsInterop.AddClass(Selector, ClassName);
}

export function RemoveClass(Selector, ClassName) {
    return AppJsInterop.RemoveClass(Selector, ClassName);
}

export function HasClass(Selector, ClassName) {
    return AppJsInterop.HasClass(Selector, ClassName);
}

// *********************** Events ***********************



// ******************************************************

export function AddEventListener(dotnetHelper, options) {

    const Elements = document.querySelectorAll(options.currentSelector);
    const EventName = options.eventName;

    const Handler = function () {
        dotnetHelper.invokeMethodAsync('InvokeHandler', null);
    }

    if (Elements.length == 0)
        throw "element not found";

    Elements.forEach(function (elem) {
        elem.addEventListener(EventName, Handler);
    });

    return eventhandler(Elements, EventName, Handler);
}

function eventhandler(elements, eventName, handler) {
    let matches = elements;
    return {
        removeHandler: function () {
            matches.forEach(function (elem) {
                elem.removeEventListener(eventName, handler);
            });
        }
    };
}

// ******************************************************

export function OnClick(Selector) {

    const Node = document.querySelector(Selector);

    Node.click();

}

// ******************************************************

export function Val(Selector, Value) {
    var Elements = document.querySelectorAll(Selector);

    let Values = new Array();

    Elements.forEach(item => {
        // Value !== undefined ||
        if (Value && Value !== "") {
            item.innerHTML = Value;
        }
        else {
            Values.push(item.innerHTML);
        }
    });
    //return (newVal !== undefined ? item.innerHTML = newVal : item.innerHTML);

    if (Values.length > 0) {
        return JSON.stringify(Values);
    }

    return;
};

    //Classes.forEach(item => {
    //    alert(item);
    //});


        //var Arr = Classes.toString().replace(",", " ");
        //alert(Classes);