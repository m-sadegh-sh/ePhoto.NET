
$(function() {
    var location = window.location.pathname.toLowerCase();

    if (location.indexOf("returnurl=") !== -1)
        location = location.substring(0, location.indexOf("returnurl="));

    if (location.indexOf("#") !== -1)
        location = location.substring(0, location.indexOf("#"));

    if (location.indexOf("?") === location.length - 1)
        location = location.substring(0, location.length - 1);

    var getTrimmedLocation = function (loc) {
        var slashes = loc.split("/").length;

        if (slashes < 3)
            return "\\";

        if (slashes === 3)
            return loc;

        return getTrimmedLocation(loc.substring(0, loc.lastIndexOf("/")));
    };

    var trimmedLocation = getTrimmedLocation(location);

    $("[data-activate=true] a").each(function(index, item) {
        var $item = $(item);

        if (typeof $item.attr("href") === "undefined" || $item.attr("href") === "#")
            return;

        var itemUrl = $item.get(0).pathname.toLowerCase();

        if (location === itemUrl) {
            if ($item.closest("nav, .nav").data("parent") === true) {
                $item.parent().addClass("active");
            } else
                $item.addClass("active");
        }

        if (location === itemUrl || itemUrl.indexOf(trimmedLocation) === 0) {
            var $menu = $item.closest("ul.collapse");
            var $toggler = $menu.prev();
            $menu.addClass("in").attr("aria-expanded", "true");
            $toggler.removeClass("collapsed").attr("aria-expanded", "true");
        }
    });
});