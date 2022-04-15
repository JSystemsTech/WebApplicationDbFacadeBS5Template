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
    $(initData.userSearchWidgetContainerId).onDataTableRowAction('manage-roles', function (e, data) {
        $(e.target).DataTable().serversideModal({
            size: 'md',
            url: data.ManageUserRolesUrl,
            title: 'Manage ' + data.Name + ' Roles',
            loading: 'Users Management for ' + data.Name,
            static: true,
            close: true,
            reload: true
        });
    }).onDataTableRowAction('manage-groups', function (e, data) {
        console.log(data);
    });

    $(initData.viewOnlineUsersButtonId).serversideModalButton({
        size: 'xl',
        url: initData.viewOnlineUsersUrl,
        title: 'Online Users',
        loading: 'Online Users Modal',
        static: true,
        close: true,
        reload: false
    }
    );
    $(initData.addAnnouncementId).serversideModalButton({
        size: 'lg',
        url: initData.addAnnouncementFormUrl,
        title: 'Add Announcement',
        loading: 'Add Announcement Modal',
        static: true,
        close: true,
        reload: true,
        onLoad: function (modal, el) {
            el.on('submit-success', function () {
                modal.hide();
            });
        }
    }
    );
    $(initData.previewAnnouncementId).serversideModalButton({
        size: 'xl',
        url: initData.previewAnnouncementUrl,
        title: 'Preview Announcement',
        loading: 'Preview Announcement Modal',
        static: true,
        close: true,
        reload: true
    }
    );
    $(initData.announcementManagementTableContainerId).onDataTableRowAction('Copy', function (e, data) {
        $(e.target).DataTable().serversideModal({
            size: 'lg',
            url: data.CopyAnnouncementUrl,
            title: 'Add Announcement',
            loading: 'Add Announcement Modal',
            static: true,
            close: true,
            reload: true,
            onLoad: function (modal, el) {
                el.on('submit-success', function () {
                    modal.hide();
                });
            }
        });
    }).onDataTableRowAction('Edit', function (e, data) {
        $(e.target).DataTable().serversideModal({
            size: 'lg',
            url: data.EditAnnouncementUrl,
            title: 'Edit Announcement',
            loading: 'Edit Announcement Modal',
            static: true,
            close: true,
            reload: true,
            onLoad: function (modal, el) {
                el.on('submit-success', function () {
                    modal.hide();
                    $(e.target).DataTable().ajax.reload();
                });
            }
        });
    });
}));
