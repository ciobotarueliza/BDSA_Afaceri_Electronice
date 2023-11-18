var onClickRowTimer;

$(document).ready(function () {

    let tableRowSelector = jid("table") + " tbody tr";

    $(tableRowSelector).click(function () {
        if (onClickRowTimer) clearTimeout(onClickRowTimer);
        let me = this;

        onClickRowTimer = setTimeout(function () {
            let selected = $(me).hasClass("tableSelectedRow");
            $(tableRowSelector).removeClass("tableSelectedRow");
            if (!selected)
                $(me).addClass("tableSelectedRow");
        }, 250);
    });

    // Disable scroll when focused on a number input.
    $('body').on('focus', 'input[type=number]', function (e) {
        $(this).on('wheel', function (e) {
            e.preventDefault();
        });
    });

    // Restore scroll on number inputs.
    $('body').on('blur', 'input[type=number]', function (e) {
        $(this).off('wheel');
    });

    // Disable up and down keys.
    $('body').on('keydown', 'input[type=number]', function (e) {
        if (e.which == 38 || e.which == 40)
            e.preventDefault();
    });
});

function jid(id) {
    return "#" + id.replace(/(:|\.|\[|\])/g, "\\$1");
}

function getSelectedRow() {
    return $(jid("table") + " tbody tr.tableSelectedRow");
}

function getSelectedId() {
    let selected = getSelectedRow();

    if (!selected || selected.length !== 1) {
        alert("You must select a row");
        return null;
    }

    return selected[0].id;
}

function stringFormat() {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i += 1) {
        var reg = new RegExp('\\{' + i + '\\}', 'gm');
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};