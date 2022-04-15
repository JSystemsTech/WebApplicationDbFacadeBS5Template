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

    var loginCarouselEl = $(initData.carouselId)[0];
    var loginCarousel = new bootstrap.Carousel(loginCarouselEl, {
        keyboard: false,
        interval: false,
        wrap: false,
        touch: false
    });
    $(initData.acknowlagePrivacyPolicyId).one('click', function () {
        loginCarousel.next();
    });
    $(initData.acknowlageCookiePolicyId).one('click', function () {
        loginCarousel.next();
    });
}));
