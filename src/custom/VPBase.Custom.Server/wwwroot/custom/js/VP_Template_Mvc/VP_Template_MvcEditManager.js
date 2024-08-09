var VP_Template_MvcEditManager = function () {
	function init() {
		AdminInputHelper.ApplyDefaultBehaviour("#page-content");
	}

	return {
		Init: init
	};
}();

GlobalManager.AddValidation($('#title-input'), $('#title-required'), "error", true, function (callback) {
    var title = $('#title-input').val().trim();
    if (title === "") {
        callback({ isValid: false, errors: ["Field is required"] });
        return;
    }

    const req = $.ajax('/Custom/VP_Template_Mvc/ValidateTitle?id=' + $('#VP_Template_MvcId').val() + '&title=' + title, { method: 'POST' });

    req.done(function (data) {
        callback(data);
    });

});

GlobalManager.AddValidation($('#category-input'), $('#category-required'), "warn", false, function (callback) {
    var category = $('#category-input').val().trim();
    if (category === "") {
        callback({ isValid: true });
        return;
    }

    const req = $.ajax('/Custom/VP_Template_Mvc/ValidateCategory?id=' + $('#VP_Template_MvcId').val() + '&category=' + category, { method: 'POST' });

    req.done(function (data) {
        callback(data);
    });

});