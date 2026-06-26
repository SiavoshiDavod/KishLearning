
function openVideoSelectorAsModal(url, event, idElement, nameElement) {
    event.preventDefault();
    $('<div id=VideoSelectorDiv">').dialog({
        modal: true,
        open: function () {
            var thisCtl = $(this);
            $.ajax({
                url: url,
                success: function (res) {
                    $(thisCtl).html(res);
                }
            });
        },
        close: function (e) {
            var code = $('#VideoSelectorHiddenFieldId').val();
            var name = $('#VideoSelectorHiddenFieldName').val();
            if (code != null && code != "") {
                $(nameElement).val(name).trigger('change');
                $(idElement).val(code).trigger('change');
            }

            $(this).empty();
            $(this).dialog('destroy');
        },
        height: 450,
        width: 450,
        // hide: { effect: "none", duration: 150 },
        //show: { effect: "none", duration: 150 },
        //position: 'center',
        title: "انتخاب فایل",
        resizable: true
    });
}

function Mypost(url, data, dataType, successFunction, failFunction, viewLoading) {

    if (url === undefined || url === null || url.length === 0)
        return;
    if (data === undefined || data === null || data.length === 0)
        data = {};
    if (dataType === undefined || dataType === null || dataType.length === 0)
        dataType = 'json';

    if (viewLoading === undefined || viewLoading === null)
        //pw.openLoading();

        $.ajax({
            url: url,
            dataType: dataType,
            data: data,
            type: 'post',
            success: function (response) {
                if (successFunction !== undefined && successFunction !== null)
                    successFunction(response);
                //pw.closeLoading();

                if (response == true) {

                    SwalSuccessMessage('عملیات با موفقیت انجام شد');
                    //pw.notificationSuccess(response.Title, response.getMessage);
                } else {
                    SwalErrorMessage("error");
                    //pw.notificationError(response.Title, response.getMessage);
                }
            },
            fail: function () {
                if (failFunction !== undefined && failFunction !== null && failFunction.length > 0)
                    failFunction();
                // pw.closeLoading();
            }
        })

}
 function ResetPass(id) {
          swal({
              title: "بازیابی کلمه‌عبور",
              text: "رمز عبور جدید را وارد کنید",
              //type: 'text',
              input: "password",
              showCancelButton: true,
              //closeOnConfirm: true,
              confirmButtonText: "تایید",
              cancelButtonText: "انصراف",
              inputPlaceholder: "رمز عبور",
              preConfirm: (inputValue) => {
                  if (inputValue === "") {
                      swal.showValidationMessage("رمز عبور جدید را وارد کنید");
                  }
              }
          }).then((result) => {
              if (result.value !== undefined && result.value !== "") {
                  Mypost("/UsersAdmin/ResetPass", { id: id, pass: result.value });
              }
          });
          return false;
      }