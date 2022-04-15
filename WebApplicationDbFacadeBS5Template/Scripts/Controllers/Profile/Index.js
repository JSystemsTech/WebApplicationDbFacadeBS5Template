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
    console.log(initData);
    if (initData.edit === true) {
        $(initData.nameEditButtonId).serversideModalButton({
            size: 'md',
            url: initData.nameEditUrl,
            title: 'Edit Name',
            loading: 'Edit Name Modal',
            static: true,
            close: true,
            reload: true,
            onLoad: function (modal, el) {
                el.on('submit-success', function () {
                    modal.hide();
                    $.ReloadPage();
                });
            }
        });
    }
    
    var tabElms = $('a[data-bs-toggle="list"][data-content-url]');
    tabElms.each(function (i, tabElm) {
        var trigger = $(tabElm).hasClass('active') ? 'show.bs.tab init' : 'show.bs.tab';
        $(tabElm).one(trigger, function (event) {
            var data = $(event.target).data();
            var container = $($(event.target).attr('href'));
            if (container.find('[tab-content-container]').length > 0) {
                container = container.find('[tab-content-container]');
            }
            $.get(data.contentUrl).done(function (response) {
                container.html(response);
                container.OnLoadContent();
            });

        });
    });

    $('a[data-bs-toggle="list"][data-content-url].active').trigger('init');
}));
