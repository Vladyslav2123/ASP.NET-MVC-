'use strict';

$(document).on(function () {
	let toasts = JSON.Serialize(messages);
	toasts.forEach(t => {
		if (t.type === 'success') {
			toastr.success(t.content);
		}
		if (t.type === 'error') {
			toastr.error(t.content);
		}
	});
})