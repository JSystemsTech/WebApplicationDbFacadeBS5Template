(function (factory) {
    if (typeof define === "function" && define.amd) {
        define(["jquery"], factory);
    } else if (typeof module === "object" && module.exports) {
        module.exports = factory(require("jquery"));
    } else {
        factory(jQuery);
    }
}(function ($) {
    var initData = window.initData();
    //console.log(initData);
    var toggle = function (value) {
        var selected = $('.options a[data-value="' + value + '"]');
        var option = $('.selected a[data-value="' + value + '"]');
        console.log(option);
        console.log(selected);
        option.toggleClass('show').toggleClass('annimating');
        selected.toggleClass('show').toggleClass('annimating');
        setTimeout(function () {
            option.toggleClass('annimating');
            selected.toggleClass('annimating');
        }, 500);
    };
    $('.options a,.selected a').click(function () {
        if (!$(this).hasClass('annimating')) {
            var data = $(this).data();
            toggle(data.value);
        }        
    });
}));
