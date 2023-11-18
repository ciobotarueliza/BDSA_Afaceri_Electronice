window.Product = (function ($) {

    let editUrl = "/Product/Edit/{0}";
    let deleteUrl = "/Product/Delete/{0}"

    function _onDelete() {

        let selectedId = getSelectedId();

        $.ajax({
            url: stringFormat(deleteUrl, selectedId),
            data: { "id": selectedId },
            type: "delete",
            success: function (data) {
                if (data.success)
                    $(jid(selectedId)).remove();
            },
            error: function (context, status, message) {
                alert(context, status, message);
            }
        });

    }

    function _onChangeFile(event) {

        if (!event || !event.target || !event.target.files)
            return;

        let file = event.target.files[0];
        $(jid("fileUploadlabel")).text(file.name);
    }

    function _onEditBtnPress() {

        let selectedId = getSelectedId();
        let url = stringFormat(editUrl, selectedId);
        window.open(url, "_self");
    }

    return {
        onDelete: _onDelete,
        onEditBtnPress: _onEditBtnPress,
        onChangeFile: _onChangeFile
    }

}($));