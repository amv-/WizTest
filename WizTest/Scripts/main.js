var Wizard = {}

Wizard.ajaxComplete = function(formid) {
    console.log('wizard pass');
    Wizard.initNavigation();
}
Wizard.initNavigation = function () {
    var index = $('#wizstep').val();
    $wizButtons = $('.wiz-prev, .wiz-cancel, .wiz-next');
    $wizButtons.hide();
    $wizButtons.removeClass('hidden');
    if (index == 0) {
        $('.wiz-next, .wiz-cancel').fadeIn(400);
    }
    if (index > 0) {
        $wizButtons.fadeIn(400);
    }
}

$(document).ajaxComplete(function () {
    $.validator.unobtrusive.parse("form");
    console.log('form parsing done');
});