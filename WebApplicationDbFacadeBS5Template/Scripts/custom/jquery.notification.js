/*!
 * jQuery notification
 *
 * Copyright (c) 2022 Jonathan McGuire
 * Released under the MIT license
 */

(function (factory) {
	if (typeof define === "function" && define.amd) {
		define(["jquery"], factory);
	} else if (typeof module === "object" && module.exports) {
		module.exports = factory(require("jquery"));
	} else {
		factory(jQuery);
	}
}(function ($) {
	var data = $('body').data();
	var addNotification = function (options) {
		options = $.extend(true, {
			title: '',
			message: '',
			type: 'info',
			icon: 'fa-info-circle',
			delay: 6000,
			autohide: true,
			html: false
		}, options);

		var titleEl = $('<strong class="me-auto"></strong>');
		titleEl.text(options.title);
		var closeEl = $('<a href="" data-bs-dismiss="toast" aria-label="Close" style="color:unset;"><i class="fas fa-times-circle"></i></a>');
		var header = $('<div class="toast-header py-1">');
		header.addClass('bg-' + options.type).addClass('text-inverse-' + options.type);
		header.append(titleEl).append(closeEl);
		var body = $('<div class="toast-body"></div>');
		var bodyRow = $('<div class="row"></div>');
		var bodyIconCol = $('<div class="col-auto align-self-center"><i class="fa fa-2x ' + options.icon + '"></i></div>');
		bodyIconCol.addClass('text-' + options.type);
		var bodyContentCol = $('<div class="col small"></div>');
		bodyRow.append(bodyIconCol).append(bodyContentCol);
		body.append(bodyRow);
		if (options.html) {
			bodyContentCol.html(options.message);
		} else {
			bodyContentCol.css('overflow-wrap','anywhere');
			bodyContentCol.text(options.message);
		}
		var toastEl = $('<div class="toast" role="alert" aria-live="assertive" aria-atomic="true"></div>');
		toastEl.append(header).append(body);
		$('[data-bs="notification"]').append(toastEl);
		var toast = new bootstrap.Toast(toastEl[0], {
			delay: options.delay,
			autohide: options.autohide,
		});
		toast.show();
	};

	$.Notification = {
		add: function (options) { addNotification(options); },
		primary: function (options) { addNotification($.extend(true, options, { type: 'primary', icon: 'fa-info-circle' })); },
		secondary: function (options) { addNotification($.extend(true, options, { type: 'secondary', icon: 'fa-info-circle' })); },
		info: function (options) { addNotification($.extend(true, options, { type: 'info', icon: 'fa-info-circle' })); },
		warning: function (options) { addNotification($.extend(true, options, { type: 'warning', icon: 'fa-exclamation-circle' })); },
		danger: function (options) { addNotification($.extend(true, options, { type: 'danger', icon: 'fa-exclamation-triangle' })); },
		success: function (options) { addNotification($.extend(true, options, { type: 'success', icon: 'fa-check-circle' })); },
		error: function (ex) { addNotification({ title:ex.name, message: ex.message, type: 'warning', icon: 'fa-exclamation-circle', autohide: false }); },
	};
	$('[data-bs="error"]').each(function (i, el) {
		$.Notification.error($(el).data());
	});
}));