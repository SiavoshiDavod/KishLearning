var intervalIds = [];
var ctrlDown = false;

var pw = new Object();


pw.unAuthorize = function () {
    pw.notificationWarning('دسترسی', 'شما دسترسی لازم جهت این کار را ندارید.<br>امکان دارد نیاز به ورود مجدد به سیستم داشته باشید');
    pw.closeLoading();

}

pw.error = function () {
    pw.notificationSingleError('خطا', 'متاسفانه عملیات با شکست مواجه شد.');
    pw.closeLoading();
}


//var contentUrl = "/Home/Dashboard";

String.prototype.lines = function () {
    return this.split(/\r*\n/);
}
String.prototype.lineCount = function () {
    return this.lines().length;
}
Array.prototype.RemoveArrayItem = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === val) {
            this.splice(i, 1);
            break;
        }
    }
};
Array.prototype.RemoveDuplicates = function (arr) {
    var temp = {};
    for (var i = 0; i < arr.length; i++) temp[arr[i]] = true;
    var r = [];
    for (var k in temp) r.push(k);
    return r;
};

pw.changeUrl = function (url, title) {
    if (typeof (history.pushState) != "undefined") {
        var obj = { Title: title, Url: url };
        history.pushState(obj, obj.Title, obj.Url);
    } else {
        alert("مروگر شما از Html5 پشتبانی نمی کند، لطفا از مرورگرهای بروز استفاده نمایید.");
    }
}

pw.getCoords = function (elem) {
    var box = elem.getBoundingClientRect();

    var body = document.body;
    var docEl = document.documentElement;

    var scrollTop = window.pageYOffset || docEl.scrollTop || body.scrollTop;
    var scrollLeft = window.pageXOffset || docEl.scrollLeft || body.scrollLeft;

    var clientTop = docEl.clientTop || body.clientTop || 0;
    var clientLeft = docEl.clientLeft || body.clientLeft || 0;

    var top = box.top + scrollTop - clientTop;
    var left = box.left + scrollLeft - clientLeft;

    return { top: Math.round(top), left: Math.round(left) };
}

pw.getContent = function (url, content, callback, title, addToHistory) {

    if (title === undefined || title === null || title.length === 0) {
        title = window.document.title;
    }

    if (addToHistory === undefined || addToHistory === null) {
        addToHistory = true;
    }

    if (content === undefined || content === null || content.length === 0) {
        content = 'body';
    } else {
        content = '#' + content;
    }
    $.ajax({
        url: url,
        type: "Get",
        datatype: "html",
        beforeSend: function () {
            pw.openLoading();
        },
        success: function (result) {
            if (addToHistory === true) {
                pw.changeUrl(window.location.origin + '/home/index?url=' + url, title);
            }

            $(content).empty();
            $(content).html(result);
            $(content).removeClass('k-textbox');
            $("html, body").animate({
                scrollTop: 0
            }, 500);
            pw.closeLoading();
            preventSubmit();
            $('.noSorting').removeClass('header');
            $('[data-toggle="tooltip"]').tooltip();
            setTimeout(function () {
                if (callback !== undefined && callback !== null)
                    window[callback]();

                var t = new Array();
                $('.breadcrumb-2 a').each(function (i, a) {
                    t.push(a.innerText);
                });

                if (t.length > 0) {
                    window.document.title = 'سامانه ستاک | ' + t.join(' | ');
                }

                $(content + ' input[type=text]').focus(function () { $(this).select(); });
                $(content + ' input[type=number]').focus(function () { $(this).select(); });
                $(content + ' input[type=textarea]').focus(function () { $(this).select(); });

            }, 500);

        },
        fail: function (res) {
            pw.error();
        },
        error: function (res) {
            pw.error();
        },
        complete: function (jqXHR) {
            if (jqXHR.state == 401) {
                pw.unAuthorize();
            }
        },
        timeout: 60000
    });
}


pw.post = function (url, data, dataType, successFunction, failFunction, viewLoading) {

    if (url === undefined || url === null || url.length === 0)
        return;
    if (data === undefined || data === null || data.length === 0)
        data = {};
    if (dataType === undefined || dataType === null || dataType.length === 0)
        dataType = 'json';

    if (viewLoading === undefined || viewLoading === null)
        pw.openLoading();

    $.ajax({
        url: url,
        dataType: dataType,
        data: data,
        type: 'post',
        success: function (response) {
            if (successFunction !== undefined && successFunction !== null)
                successFunction(response);
            pw.closeLoading();

            if (response != null && response.getMessage !== undefined && response.getMessage !== '') {
                if (response.state === true) {
                    if (response.getMessage == '')
                        pw.notificationSuccess('عملیات با موفقیت انجام شد.');
                    else
                        pw.notificationSuccess(response.Title, response.getMessage);
                } else {
                    pw.notificationError(response.Title, response.getMessage);
                }
            }
        },
        fail: function () {
            if (failFunction !== undefined && failFunction !== null && failFunction.length > 0)
                failFunction();
            pw.closeLoading();
        },
        error: function (res) {
            pw.error();
        },
        complete: function (jqXHR) {
            if (jqXHR.state == 401) {
                pw.unAuthorize();
            }
        },
        timeout: 30000
    })

}

pw.submitForm = function (formId, afterSubmit, viewLoading, validate) {
    if (formId === undefined || formId === null || formId.length === 0)
        return;
    if (validate === undefined || validate === null || validate.length === 0)
        validate = true;
    //Rahmati

    var plaque=$('.plaqueNumberHiddenValueForValidate');
    if (validate) {
        $('#' + formId).removeData("validator");
        $('#' + formId).removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse('#' + formId);
        var message = $('#' + formId).validate();
        var validForm = $('#' + formId).valid();
        if (!validForm) {
            return;
        } else if (plaque.length && Number(plaque.val() <= 100000000)) {
            $('.text-danger').html("فیلد شماره پلاک اشتباه است");
            return;
        }
        else {
            $('.text-danger').empty();
        }
    }

    var url = $('#' + formId).attr('action');

    if (url === undefined || url === null || url.length === 0)
        url = window.location.href;

    data = new FormData($('#' + formId)[0]);

    var dataType = $('#' + formId).data('datatype');

    if (dataType === undefined || dataType === null || dataType.length === 0)
        dataType = 'json';

    var type = $('#' + formId).data('type');

    if (type === undefined || type === null || type.length === 0)
        type = 'post';

    if (viewLoading === undefined || viewLoading === null)
        viewLoading = true;

    if (viewLoading)
        pw.openLoading();

    $('#' + formId + ' .error-content').hide();

    $.ajax({
        url: url,
        dataType: dataType,
        data: data,
        type: type,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.state === false) {
                $('#' + formId + ' .error-content .error-box ul').empty();
                $.each(response.messages, function (i, v) {
                    $('#' + formId + ' .error-content .error-box ul').append('<li>' + v + '</li>');
                });
                $('#' + formId + ' .error-content').show();
            } else {
                if (afterSubmit !== undefined && afterSubmit !== null && afterSubmit.length > 0)
                    afterSubmit(response);
                else {
                    if ((window[formId + '_AfterSubmit'] !== undefined))
                        window[formId + '_AfterSubmit'](response);
                }

                if (response.getMessage == '')
                    pw.notificationSuccess('عملیات با موفقیت انجام شد.');
                else
                    pw.notificationSuccess(response.Title, response.getMessage);
            }

            if (viewLoading)
                pw.closeLoading();
        },
        fail: function () {
            if (viewLoading)
                pw.closeLoading();
        },
        error: function (res) {
            pw.error();
        },
        complete: function (jqXHR) {
            if (jqXHR.state == 401) {
                pw.unAuthorize();
            }
        },
        timeout: 30000
    });

}


// Change
pw.alert = function (title, message, successFunction) {
    Swal({
        title: title,
        text: message,
        //type: 'info',
        confirmButtonText: "تایید",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.value !== undefined && result.value) {
            if (successFunction !== undefined && successFunction !== null) {
                successFunction();
            }
        }
    });
}

///INPUT TYPES:email,url,password,textarea,select,radio,checkbox,file,range
pw.prompt = function (title, message, successFunction, inputType, placeholder, isltr) {
    if (inputType == null || inputType == undefined) {
        inputType = 'text';
    }
    if (placeholder == null || placeholder == undefined) {
        placeholder = 'اطلاعات را وارد کنید';
    }
    swal({
        title: title,
        text: message,
        type: 'text',
        input: inputType,
        showCancelButton: true,
        closeOnConfirm: true,
        confirmButtonText: "تایید",
        cancelButtonText: "انصراف",
        inputPlaceholder: placeholder,
        preConfirm: (inputValue) => {
            if (inputValue === "") {
                swal.showValidationMessage(message);
            }
        }
    }).then((result) => {
        if (result.value !== undefined && result.value !== "") {
            if (successFunction !== undefined && successFunction !== null) {
                successFunction(result.value);
                //swal.close();
            }
        }
    });
    if (inputType != null || inputType != undefined) {
        $(".swal2-input").css("direction", "ltr")
    }
}

pw.alertHtml = function (title, message, successFunction) {
    Swal({
        title: title,
        text: message,
        type: 'text',
        showCancelButton: true,
        confirmButtonText: "تایید",
        html: true,
    }).then((result) => {
        if (result.value !== undefined && result.value) {
            if (successFunction !== undefined && successFunction !== null) {
                successFunction();
            }
        }
    })
}

pw.confirm = function (title, message, successFunction, calcelFunction) {

    Swal({
        title: title,
        text: message,
        //type: 'question',
        showCancelButton: true,
        //confirmButtonColor: '#3BAFDA',
        //cancelButtonColor: '#d33',
        cancelButtonText: "خیر",
        confirmButtonText: "بله"
    }).then((result) => {
        if (result.dismiss === Swal.DismissReason.cancel) {
            if (calcelFunction !== undefined && calcelFunction !== null) {
                calcelFunction();
            }
        }
        else if (result.value !== undefined && result.value) {
            if (successFunction !== undefined && successFunction !== null) {
                successFunction();
            }
        }
    })
}
///POPUP TYPES : success,error,warning,info,question
pw.toast = function (POPUPTYPES, message, position, timer, backgroundColor) {
    if (POPUPTYPES == null || POPUPTYPES == undefined) {
        POPUPTYPES = 'success';
    }
    if (position == null || position == undefined) {
        position = 'top-end';
    }
    if (timer == null || timer == undefined) {
        timer = 3000;
    }
    if (backgroundColor == null || timer == backgroundColor) {
        backgroundColor = 'deepskyblue';
    }
    const toast = Swal.mixin({
        toast: true,
        position: position,
        showConfirmButton: false,
        timer: timer
    });

    toast({
        background: backgroundColor,
        type: POPUPTYPES,
        title: message
    });
}
// change


pw.modal = function (name, title, url, data, size, noFooter, successFunction, cancelFunction, htmlResultFunction, className) {

    if (className === undefined || className === null)
        className = "";

    var existBackdrop = $('.modal-backdrop').length;

    if (noFooter === undefined || noFooter === null)
        noFooter = false;

    $('#' + name).remove();
    var modal = '<div class="modal fade in {className}" id="{Name}" role="dialog" aria-labelledby="myModalLabel"><div class="modal-window modal-dialog {Size}" id="{Name}_dialog" role="document"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title" id="myModalLabel">{Title}</h4></div><div class="modal-body"></div>{Footer}</div></div></div><div id="{Name}_Coord"></div>';

    var footer = '<div class="modal-footer"><button type="button" id="' + name + '_successBtn" class="btn btn-primary save">ثبت</button>&nbsp;<button type="button"  id="' + name + '_closeBtn" class="btn btn-default" data-dismiss="modal">انصراف</button></div>';

    modal = modal.replace('{Name}', name);
    modal = modal.replace('{Name}', name);
    modal = modal.replace('{Name}', name);
    modal = modal.replace('{className}', className);
    modal = modal.replace('{Title}', title);
    modal = modal.replace('{Size}', size);
    if (noFooter === false) {
        modal = modal
            .replace('{Footer}', footer);
    } else {
        modal = modal.replace('{Footer}', '');
    }

    $('body').append(modal);

    if (noFooter === false) {
        if (successFunction !== undefined && successFunction !== null && successFunction !== '') {
            $('#' + name + '_successBtn').click(function () {
                eval(successFunction)();
            });
        }
        if (cancelFunction !== undefined && cancelFunction !== null && cancelFunction !== '') {
            $('#' + name + '_closeBtn').click(function () {
                eval(cancelFunction)();
            });
        }
    }

    $.ajax({
        url: url,
        type: "Get",
        data: data,
        datatype: "html",
        beforeSend: function () {
            pw.openLoading();
        },
        success: function (result) {

            $('#' + name + ' .modal-body').empty();
            if (htmlResultFunction !== undefined && htmlResultFunction !== null && htmlResultFunction !== '') {
                htmlResultFunction(result);
            } else {
                $('#' + name + ' .modal-body').html(result);
                $('#' + name + ' input').removeClass('k-textbox');
            }

            pw.closeLoading();
            $('#' + name).modal('show');
            $('#' + name).on('hidden.bs.modal', function () {
                $('#' + name).remove();
            });

            if (noFooter === false && (successFunction !== undefined && successFunction !== null && successFunction !== '')) {
                $('#' + name + ' form:first input').keydown(function (e) {
                    if (e.keyCode === 13) {
                        $('#' + name + '_successBtn').click();
                    }
                });
            }

            $('[data-toggle="tooltip"]').tooltip();

            $('#' + name + ' form:first').attr("onsubmit", "return false;");

            var firstInput = $('#' + name + ' form:first .form-control:first');

            if (firstInput === undefined)
                firstInput = $('#' + name + ' form:first input[type="text"]:first');
            if (firstInput === undefined)
                firstInput = $('#' + name + ' form:first input[type="textarea"]:first');

            if (firstInput !== undefined) {
                setTimeout(function () {
                    firstInput.focus();
                    firstInput.select();
                    $('#' + name + ' input[type=text]').focus(function () { $(this).select(); });
                    $('#' + name + ' input[type=number]').focus(function () { $(this).select(); });
                    $('#' + name + ' input[type=textarea]').focus(function () { $(this).select(); });
                }, 500);
            }

            $('#' + name + ' .modal-body')
                .css('max-height', (window.innerHeight - 215) + 'px')
                .css('overflow-y', 'auto');

            if (existBackdrop > 0) {
                $('.modal-backdrop:last').addClass('modal-backdrop-hide');
            }
        },
        error: function () {
            pw.error();
        },
        fail: function () {
            pw.error();
        },
        complete: function (jqXHR) {
            if (jqXHR.state == 401) {
                pw.unAuthorize();
            }
        },
        timeout: 60000
    });

}

pw.notificationSuccess = function (title, message, messagePosition) {
    var messageType = 'success';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';
    //pw.toast(messageType, message, messagePosition, 60000, "limegreen");
    //.autoHideNotify(messageType, messagePosition, title, message);
}

pw.notificationError = function (title, message, messagePosition) {
    var messageType = 'error';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';
    //pw.toast(messageType, message, messagePosition, 60000, 'tomato');

    //.notify(messageType, messagePosition, title, message);
}

pw.notificationSingleError = function (title, message, messagePosition) {

    if ($('.notifyjs-metro-single').length > 0)
        return;

    var messageType = 'single';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Remind = function (title, message, messagePosition) {
    var messageType = 'satak_Remind';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Plaque = function (title, message, messagePosition) {
    var messageType = 'satak_Plaque';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_OverDose = function (title, message, messagePosition) {
    var messageType = 'satak_OverDose';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_User = function (title, message, messagePosition) {
    var messageType = 'satak_User';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Ticket = function (title, message, messagePosition) {
    var messageType = 'satak_Ticket';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Temperature = function (title, message, messagePosition) {
    var messageType = 'satak_Temperature';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_MetaData = function (title, message, messagePosition) {
    var messageType = 'satak_MetaData';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Message = function (title, message, messagePosition) {
    var messageType = 'satak_Message';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Location = function (title, message, messagePosition) {
    var messageType = 'satak_Location';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Label = function (title, message, messagePosition) {
    var messageType = 'satak_Label';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Humidity = function (title, message, messagePosition) {
    var messageType = 'satak_Humidity';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Files = function (title, message, messagePosition) {
    var messageType = 'satak_Files';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Error = function (title, message, messagePosition) {
    var messageType = 'satak_Error';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}
pw.notificationSatak_Done = function (title, message, messagePosition) {
    var messageType = 'satak_Done';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';

    //.notify(messageType, messagePosition, title, message);
}



pw.notificationInfo = function (title, message, messagePosition) {
    var messageType = 'info';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';
    //pw.toast(messageType, message, messagePosition, 60000);
    //.autoHideNotify(messageType, messagePosition, title, message);
}

pw.notificationWarning = function (title, message, messagePosition) {
    var messageType = 'warning';

    if (messagePosition === undefined || messagePosition === null || messagePosition === '')
        messagePosition = 'top left';
    //pw.toast(messageType, message, messagePosition, 60000);
    //.autoHideNotify(messageType, messagePosition, title, message);
}

pw.openLoading = function () {
    $('#loading').show();
}

pw.closeLoading = function () {
    $('#loading').hide();
}

pw.openModal = function (name) {
    $('#' + name).modal('show');
}

pw.closeModal = function (name) {
    $('#' + name).modal('hide');
}

pw.reloadGrid = function (name) {
    $('#' + name).DataTable().page(1).draw();
}

pw.fullScreen = function () {
    if (!document.fullscreenElement &&
        !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.msRequestFullscreen) {
            document.documentElement.msRequestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    } else {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
    }
}

pw.login = function () {
    $('#modal-login .errors')
        .empty()
        .hide();

    pw.openLoading();
    var email = $.trim($('#login_Email').val());
    var password = $.trim($('#login_Password').val());
    var requestVerificationToken = $('#modal-login input[name=__RequestVerificationToken]').val();

    if (email.length <= 0) {
        $('#login_Email').focus();
        return;
    }
    if (password.length <= 0) {
        $('#login_Password').focus();
        return;
    }


    $.ajax({
        url: '/Home/LoginAsync',
        type: 'post',
        dataType: 'json',
        data: {
            email: email,
            password: password,
            __RequestVerificationToken: requestVerificationToken
        },
        success: function (response) {
            if (response.state) {
                pw.notificationSuccess(response.message);
                pw.closeModal('modal-login');
                pw.closeLoading();
            } else {
                $('#modal-login .errors')
                    .html(response.getMessage)
                    .show();

                pw.closeLoading();
            }
        },
        fail: function () {
            pw.error();
        },
        complete: function (jqXHR) {
            if (jqXHR.state == 401) {
                pw.unAuthorize();
            }
        },
        timeout: 60000
    })
};

//$(function () {
//    preventLink();
//    GetCurrentUserName();
//});

function CurrentUserChangePassword() {
    var success = function (result) {
        pw.submitForm('ChangeUserFrm', function (response) {
            if (response.state) {
                pw.closeModal('ChangePasswordModal');
            } else {
                pw.notificationError("", response.getMessage);
            }
        });
    }
    pw.modal('ChangePasswordModal', 'تغییر کلمه عبور', '/Users/CurrentUserChangePassword', null, 'modal-lg', false, success);
}

function GetCurrentUserName() {
    $.ajax({
        url: '/Users/GetCurrentUserName',
        type: "Get",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $('#CurrentUserName').val(result);
            }
        },
        complete: function (jqXHR) {
            if (jqXHR.state == 401) {
                pw.unAuthorize();
            }
        },
        timeout: 60000
    });
}
function preventLink() {
    $('.link').click(function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        var callback = $(this).data('callback');


        if (url === undefined || url === null || url.length === 0)
            return true;
        pw.getContent(url, 'main-content', callback);
        return false;
    });

}

function preventSubmit() {
    $("form input[type=submit]").click(function (e) {
        e.preventDefault();
        var formId = $(this).parents("form").attr('id');
        pw.submitForm(formId);
        return false;
    })
}

pw.alarm = function (title, message, type) {
    switch (type) {
        case 'info':
            pw.notificationInfo(title, message);
            break;
        case 'success':
            pw.notificationSuccess(title, message);
            break;
        case 'warning':
            pw.notificationWarning(title, message);
            break;
        case 'error':
            pw.notificationError(title, message);
            break;
    }
}

pw.setCookie = function (name, value, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = name + "=" + value + ";" + expires + ";path=/";
}

pw.getCookie = function (name) {
    var name = name + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

pw.dataTable = new Object();

pw.dataTable.simple = function (tableName) {
    $('#' + tableName).DataTable({
        "language": { "url": "js/plugins/datatables/Persian.json" }
    });
}

pw.snackbar = function (message) {
    return;
    var msg_snack = Msg.factory({
        preset: "snackbar",
        autoclose_delay: 3000
    });
    msg_snack.show(message);
}


pw.toPersianDateTimePicker = function (id, groupId, fromDate, toDate) {
    var container = $('#' + id).parent();

    if (fromDate === undefined || fromDate === null || fromDate === '') {
        fromDate = false;
    }
    if (toDate === undefined || toDate === null || toDate === '') {
        toDate = false;
    }
    if (groupId === undefined || groupId === null || groupId === '') {
        groupId = '';
    }

    var value = $('#' + id).val();

    $('#' + id)
        .attr('id', '_' + id)
        .attr('name', '_' + id);

    $(container).append('<input id="' + id + '" name="' + id + '" type="hidden" value="' + value + '" />');

    $('#_' + id)
        .MdPersianDateTimePicker({
            dateFormat: 'yyyy-MM-dd',
            enableTimePicker: false,
            englishNumber: true,
            targetTextSelector: '#_' + id,
            targetDateSelector: '#' + id,
            fromDate: fromDate,
            toDate: toDate,
            groupId: groupId
        });

    if (value !== null && value !== '1/1/0001 12:00:00 AM' && value !== '') {
        var date = new Date(value.replace('ق.ظ', 'AM').replace('ب.ظ', 'PM'));
        var newDate = toJalaali(date.getFullYear(), date.getMonth() + 1, date.getDate());
        $('#_' + id).val(newDate.jy + '/' + newDate.jm + '/' + newDate.jd);
    } else {
        $('#_' + id).val('');
    }

    $('.glyphicon-calendar').click(function () {
        $(this).parent().prev().focus();
    });
}

pw.existValueInOptions = function (elementId, value, callBack) {
    for (i = 0; i < document.getElementById(elementId).length; ++i) {
        if (document.getElementById(elementId).options[i].value == value) {
            eval(callBack);
            return;
        }
    }
    $('#' + elementId).val($('#' + elementId + ' option:first').val());
}


pw.lookup = function (name, title, lookupAction, url, callBack) {
    var lookupName = name + "_Name";
    var formName = name + "_FormName";

    pw.modal(name + 'Modal', title, lookupAction, { name: lookupName, formName: formName, url: url, callbackFunction: callBack }, 'modal-lg', true);
}


pw.logout = function () {
    pw.post('/home/logout', {}, 'json', function () {
        window.location = '/home/login';
    });
}


pw.select2 = new Object();
pw.select2.change = function (id, val) {
    $("#" + id).val(val).trigger('change');
}

pw.getGridSelectedItems = function (grdName) {
    var values = $('#' + grdName + '_Checkbox_Values').val();
    if (values === undefined || values.length === 0)
        return null;

    var result = values.split(',').splice(0);
    result.RemoveArrayItem("");
    return result;
}

pw.setGridSelectedValues = function (grdName, value) {
    $('#' + grdName + '_Checkbox_Values').val(value);
}


pw.getCurrentPageIds = function (grdName) {

    var items = $('#' + grdName + ' tr td input');
    var result = new Array();
    $.each(items, function (i, v) {
        result.push(v.value);
    });
    return result.join(',');
}


function showPicture(img) {
    pw.modal('showPictureModal', 'نمایش تصویر اسکن', '/Scanners/ShowPicture', { url: img }, 'modal-lg', true);;
}

$(function () {
    $(document).keydown(function (e) {
        if (e.keyCode == 17 || e.keyCode == 91) ctrlDown = true;
    }).keyup(function (e) {
        if (e.keyCode == 17 || e.keyCode == 91) ctrlDown = false;
    });

    $('a.action-link').click(function (e) {
        e.preventDefault();
        var url = $(this).attr('data-href');
        if (url === undefined || url === null || url === '#' || url === '')
            return true;

        $('#sidebar-menu li').find('.active').removeClass('active');
        $(this).parent().addClass('active');
        $(this).addClass('active');

        pw.getContent(url, 'main-content');
        return false;
    });
    var refreshUrl = window.location.href.toLowerCase().split(window.location.origin + "/home/index?url=")[1];
    if (refreshUrl !== undefined && refreshUrl !== null && refreshUrl.indexOf('home/index') === -1) {
        pw.openLoading();
        pw.getContent(refreshUrl.replace("&fullscreen=true", "").replace("&fullscreen=false", ""), "main-content");

    }
    setTimeout(function () {
        pw.closeLoading();
    },
    500);
    //$.fn.dataTable.ext.errMode = 'none';

});


function alarmCount() {
    var _count = parseInt($('#alarmCount').html());
    _count++;
    $('#alarmCount').html(_count);
    $('.notifications').effect('shake');
    $('#alarmsMenu').effect('shake');

    pw.snackbar('<i class="fa fa-bell" style="font-size: 32px;margin-left:5px;"></i> ' + 'شما یک پیام دارید، لطفا پیام ها را مشاهده نمایید.');
}




$(window).resize(function () {
    var modals = $('.modal-window');
    $.each(modals, function (i, v) {
        $('#' + v.id + ' .modal-body').css('max-height', (window.innerHeight - 215) + 'px');
    });

    var _height = $(document).innerHeight() - 109;
    $('#main-content').css('height', _height + 'px');

});



$(window).on('popstate', function (e) {
    try {
        var url = document.location.href.split('?url=')[1];
        if (url !== undefined && url !== null && url.length > 0)
            pw.getContent(url, 'main-content', null, null, false);
    } catch (e) {
        console.log(e.message);
    }
});

pw.GetPlaqueImageScanInModal = function (scanId) {
    if (scanId == null || scanId == 0 || scanId == undefined) {
        return;
    }
    pw.ShowImageInModal('/Scan/GetPlaqueImage?scanId=' + scanId);
}

pw.GetThumbnailScanInModal = function (scanId) {
    if (scanId == null || scanId == 0 || scanId == undefined) {
        return;
    }
    pw.ShowImageInModal('/Scan/GetThumbnail?scanId=' + scanId,scanId);
}

pw.getOperatorImageInModal = function (operatorId) {
    if (operatorId == null || operatorId == 0 || operatorId == undefined) {
        return;
    }
    pw.ShowImageInModal('/Operators/GetImage?operatorId=' + operatorId);
}

pw.ShowImageInModal = function (url,id) {
    Swal.fire({
        imageUrl: url,
        imageAlt: 'تصویر ندارد',
        animation: false,
        onOpen: function () {
            if (isNaN(parseInt(id)))
                return;
            $('.swal2-image').click(function () {
                swal.close();
                pw.getThumbnailWithLabel(id);
            });
        }
    });

}

pw.getThumbnailWithLabel = function (scanId) {
    pw.modal('showThumbnailWithLabel', 'نمایش اسکن', '/Scan/GetThumbnailWithLabel', { scanId: scanId }, 'modal-full', true, null, null, null, "DahboardModal");
}

pw.PostImage = function (fdata, url) {
    pw.openLoading();
    $.ajax({
        type: 'post',
        url: url,
        data: fdata,
        processData: false,
        contentType: false
    }).done(function (response) {
        pw.closeLoading();
        if (response != null && response.getMessage !== undefined && response.getMessage !== '') {
            if (response.state === true) {
                pw.notificationSuccess(response.Title, response.getMessage);
            } else {
                pw.notificationError(response.Title, response.getMessage);
            }
            $(":file").filestyle('destroy');
            swal.close();
        }
    });
}

pw.GetScanFileUrlInModal = function (scanId, trackingCode) {
    if (scanId == null || scanId == 0 || scanId == undefined) {
        return;
    }
    var htmlResultFunction = function (res) {
        var tbl = "<table class='table table-bordered table-hover table-striped text-center align-middle' style='font-size: 20px;'><thead> <tr> <th>نام فایل</th> <th> دانلود</th> </tr> </thead>";
        if (res !== null) {
            for (var i = 0; i < res.data.length; i++) {
                tbl += "<tr> <td>" + res.data[i].name + "</td> <td><a target='_blank' href='" + res.data[i].url + "'><span class='fa fa-download'></span></a></td> </tr>";
            }
        }
        tbl += "</table>";
        $('.modal-body').html(tbl);
    };
    pw.modal('ScanFileInModal', 'دانلود فایل های اسکن', '/Scan/GetAllScanFileUrlAsync', { scanId: scanId, trackingCode: trackingCode }, 'modal-md', true, null, null, htmlResultFunction);
}

pw.formSubmitByEnter = function (formId) {
    $('#' + formId + ' input[type=text]').keydown(function (e) {
        if (e.keyCode === 13) {
            $('#' + formId + '_SearchBtn').click();
        }
    });

    $('#' + formId + ' input[type=email]').keydown(function (e) {
        if (e.keyCode === 13) {
            $('#' + formId + '_SearchBtn').click();
        }
    });

    $('#' + formId + ' input[type=number]').keydown(function (e) {
        if (e.keyCode === 13) {
            $('#' + formId + '_SearchBtn').click();
        }
    });
}

pw.toPersianDate = function (y, m, d) {
    var date = toJalaali(y, m, d);
    return date.jy + '/' + date.jm + '/' + date.jd;
}


pw.toGregorian = function (y, m, d) {
    var date = toGregorian(y, m, d);
    return date.gy + '/' + date.gm + '/' + date.gd;
}