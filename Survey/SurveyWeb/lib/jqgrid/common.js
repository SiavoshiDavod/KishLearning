//گرفتن آی دی های جی کیو گرید، فقط صفحه جاری
function getJqgridRowIds(jqgridName) {
    s = jQuery("#" + jqgridName).jqGrid('getGridParam', 'selarrrow');
    return s;
}

//متدی که فرمتر جی کیوگرید را اورراید می کند
function jqgridCellValueNumberFormatter(cellvalue, options, rowObject) {
    if (cellvalue != null) {
        var formatted = cellvalue + '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(formatted))
            formatted = formatted.replace(rgx, '$1,$2');

        return formatted;
    }
    return "";
}

//لود مسیر جدید جی کیو گرید
function loadJqGridUrl(gridName, url, otherParams) {

    if (url == null || !url) url = '';

    if (gridName == null || gridName == '') return null;

    var gridUrl = jQuery("#" + gridName).jqGrid('getGridParam', 'url');
    //debugger;
    if (gridUrl.indexOf('?') != -1) {
        url = url + gridUrl.substring(gridUrl.indexOf('?')) + (otherParams && otherParams != null ? '&' + otherParams : '');
    } else {
        url = url + (otherParams && otherParams != null ? '?' + otherParams : '?');
    }
    var postData = jQuery("#" + gridName).jqGrid('getGridParam', 'postData');
    $.each(postData, function (key, value) {
        if (url.substr(url.indexOf('?')).length == 1) {
            url += key + "=" + encodeURIComponent(value);
        } else {
            url += "&" + key + "=" + encodeURIComponent(value);
        }
    });
    return url;
}
//دوباره فراخوانی جی کیو گرید
function reloadJqGrid(gridName, newUrl, page) {
    if (newUrl != null && page != null)
        $("#" + gridName).jqGrid().setGridParam({ url: newUrl, page: page, datatype: 'json' }).trigger("reloadGrid");
    else if (newUrl != null)
        $("#" + gridName).jqGrid().setGridParam({ url: newUrl, datatype: 'json' }).trigger("reloadGrid");
    else if (page != null)
        $("#" + gridName).jqGrid().setGridParam({ page: page, datatype: 'json' }).trigger("reloadGrid");
    else
        $("#" + gridName).jqGrid().setGridParam({ datatype: 'json' }).trigger("reloadGrid");

}
//گرفتن اطلاعات یک ردیف در جی کیوگرید
function getJqGridRowData(gridName, rowId) {

    var rowData = jQuery('#' + gridName).jqGrid('getRowData', rowId);
    return rowData;
}
//دوباره تطبیق عرض گرید
function resizeJqgrids() {
    $('div[id^="gbox_"]').each(function () {
        var id = $(this).attr('id');
        var newid = id.replace('gbox_', '');
        $('#' + newid).jqGrid('setGridWidth', $('#' + id).parent().width() - 5, false);
    });
}
//دوباره تطبیق عرض گرید مشخص
function resizeSelectedJqgrid(jqgridName) {
    $('#' + jqgridName).jqGrid('setGridWidth', $('#' + jqgridName).parent().width() - 5, false);
}
/**** Functions to handle custom columns for jqGrid *************/
function processGridColumns($theGrid) {
    var gridId = $theGrid.attr('id'),
        gridPagerId = '#' + gridId + 'Pager';

    $theGrid.navGrid(gridPagerId).navButtonAdd(gridPagerId, // Add a button to show the column chooser
        {
            caption: "",
            buttonicon: "ui-icon-calculator",
            title: "انتخاب ستون ها",
            onClickButton: function () {
                $(this).jqGrid('columnChooser',
                    {
                        classname: 'column-chooser',
                        done: function (perm) {
                            if (perm) {
                                this.jqGrid("remapColumns", perm, true);

                                // Collecting the column states to store in local storage
                                var colModels = this.jqGrid('getGridParam', 'colModel');
                                var columns = new Array();

                                $.each(perm, function (i, columnIndex) {
                                    columns.push({
                                        field: colModels[i].index,
                                        isHidden: colModels[i].hidden,
                                        pos: columnIndex
                                    });
                                });

                                // Store in local storage
                                saveObjectInLocalStorage('ls-' + $(this).attr('id'), columns);
                                // End of collecting the column states to store in local storage
                            }
                        }
                    });
            }
        }).navButtonAdd(gridPagerId, // Add the clear settings button to the pager
        {
            caption: "",
            buttonicon: "ui-icon-closethick",
            title: "پاک کردن تنظیمات ستون ها",
            onClickButton: function () {
                removeObjectFromLocalStorage('ls-' + $(this).attr('id'));
                window.location.reload();
            }
        });
}

/**************** Local storage helpers *************/
/**
 * @param string storageItemName : A key to store the value under its name in the broswe's storage (local storage)
 * @param value: any javascrit object 
 * @returns nothing 
 */
function saveObjectInLocalStorage(storageItemName, value) {
    if (window.localStorage !== undefined) {
        window.localStorage.setItem(storageItemName, JSON.stringify(value));
    }
}
/**
 * Removes the item from the local storage
 * @param string storageItemName : The key that the value is stored under its name
 * @returns nothing 
 */
function removeObjectFromLocalStorage(storageItemName) {
    if (window.localStorage !== undefined) {
        window.localStorage.removeItem(storageItemName);
    }
}
/**
 * Fetches the value from the local storage or null if not found
 * @param string storageItemName : The key that the value is stored under its name
 * @returns object : javascript object
 */
function getObjectFromLocalStorage(storageItemName) {
    if (window.localStorage !== undefined) {
        return $.parseJSON(window.localStorage.getItem(storageItemName));
    }
    return null;
}
/**************** End of Local storage helpers *************/
function restoreGridColumns($theGrid) {
    var perm = getObjectFromLocalStorage('ls-' + $theGrid.attr('id'));

    if (null === perm || undefined == perm || '' === perm) {
        return;
    }

    var colModels = $theGrid.jqGrid('getGridParam', 'colModel');
    var columns = [];

    if (perm.length > 0) {
        columns[0] = 0;
        $.each(perm, function (icol, columnIndex) {
            if (icol > 0) {
                var pos = columnIndex.index;
                var field = columnIndex.field;

                $.each(colModels, function (i, colModel) {
                    var colModelIndex = colModel.index;

                    if (field === colModelIndex) {
                        columns[icol] = i;

                        if ($.parseJSON(columnIndex.isHidden)) {
                            $theGrid.jqGrid('hideCol', [field]);
                        }
                    }
                });
            }
        });

        $theGrid.jqGrid("remapColumns", columns, true);
    }
};
/**** End of the functions to handle custom columns for jqGrid *************/
//چاپ جی کیو گرید
function printJqgridList(gridName, url, otherParams, isExcel) {

    var rowCount = jQuery("#" + gridName).jqGrid('getGridParam', 'reccount');
    if (rowCount == 0) {
        showError('هیچ ردیفی برای چاپ وجود ندارد');
        return false;
    }

    var url = loadJqGridUrl(gridName, url, otherParams);
    if (url == null || url == '') {
        showError('هیچ ردیفی برای چاپ وجود ندارد');
        return false;
    }
    if (isExcel && isExcel == true) {
        //debugger;
        loadSelectedColumnsForExcelInJqgrid(gridName, url, function (newurl) {
            window.open(newurl);
        });
        return false;
    }
    window.open(url);
}

function loadSelectedColumnsForExcelFromArray(arrayColumns, arrayNames, url, callback) {

    var colModels = arrayColumns;
    var colNames = arrayNames;
    var html = '<div class="_sanatyarExcelModalSortablePlace deleted"><h6>ستون های حذف شده</h6><ul  class="connectedSortable"></ul></div>';
    html += '<div class="_sanatyarExcelModalSortablePlace selected"><h6>ستون های انتخاب شده</h6><ul class="connectedSortable">';
    $.each(colModels, function (i, item) {
        if (item.hidden == false && item.name != 'act' && item.name != 'cb') {

            html += '<li class="ui-state-default" val="' + item.name + '">' + colNames[i] + '</li>';
        }
    });
    html += '</ul></div>';

    var htmlDialog = '<div class="_sanatyarExcelModalColumnSelector">' + html + '</div>';

    $(htmlDialog).dialog({
        modal: true,
        width: 450,
        height: 500,
        title: 'انتخاب ستون ها برای اکسل',
        open: function () {
            $("._sanatyarExcelModalSortablePlace>ul").sortable({
                connectWith: ".connectedSortable"
            }).disableSelection();
            var height = $("._sanatyarExcelModalColumnSelector .selected ul").css('height');
            $("._sanatyarExcelModalColumnSelector .deleted ul").css('height', height);
        },
        buttons: {
            'اکسل': function () {
                var lis = $(this).find('._sanatyarExcelModalSortablePlace.selected').find('ul>li');
                var selectedColumns = '';
                $.each(lis, function (i, item) {

                    if (selectedColumns != '')
                        selectedColumns += ',';
                    selectedColumns += $(item).attr('val');
                });
                var newurl = url + '&selectedColumnNames=' + selectedColumns;


                if (callback != null)
                    callback(newurl);
            }
        }

    });
    return false;
}

function loadSelectedColumnsForExcelInJqgrid(gridName, url, callback) {
    //debugger;
    var colModels = $("#" + gridName).jqGrid('getGridParam', 'colModel');
    var colNames = $("#" + gridName).jqGrid('getGridParam', 'colNames');
    loadSelectedColumnsForExcelFromArray(colModels, colNames, url, callback);
    //var html = '<div class="_sanatyarExcelModalSortablePlace"><h6>ستون های حذف شده</h6><ul  class="connectedSortable"></ul></div>';
    //html += '<div class="_sanatyarExcelModalSortablePlace selected"><h6>ستون های انتخاب شده</h6><ul class="connectedSortable">';
    //$.each(colModels, function (i, item) {
    //    if (item.hidden == false && item.name != 'act' && item.name != 'cb') {

    //        html += '<li class="ui-state-default" val="' + item.name + '">' + colNames[i] + '</li>';
    //    }
    //});
    //html += '</ul></div>';


    //var htmlDialog = '<div class="_sanatyarExcelModalColumnSelector">' + html + '</div>';

    //$(htmlDialog).dialog({
    //    modal: true,
    //    width: 450,
    //    height: 500,
    //    title: 'انتخاب ستون ها برای اکسل',
    //    open: function () {
    //        $("._sanatyarExcelModalSortablePlace>ul").sortable({
    //            connectWith: ".connectedSortable"
    //        }).disableSelection();
    //    },
    //    buttons: {
    //        'اکسل': function () {
    //            var lis = $('._sanatyarExcelModalSortablePlace.selected').find('ul>li');
    //            var selectedColumns = '';
    //            $.each(lis, function (i, item) {
    //                if (selectedColumns != '')
    //                    selectedColumns += ',';
    //                selectedColumns += $(item).attr('val');
    //            });
    //            url += '&selectedColumnNames=' + selectedColumns;
    //            //console.log('ur=' + url);
    //            if (callback != null)
    //                callback(url);
    //        }
    //    }

    //});
    //return false;
}