function contractor_grd_EditDataTableItem(id) {
    var success = function (res) {
        pw.submitForm('createContractorFrm',
            function (response) {
                pw.closeModal('editContractorModal');
                pw.reloadGrid('contractor_grd');
            });

    }
    pw.modal('editContractorModal', 'ویرایش پیمانکار', '/Contractors/Create', { id: id }, 'modal-md', false, success);
}

//(url, data, dataType, successFunction, failFunction, viewLoading)
function contractor_grd_DeleteDataTableItem(id) {
    var success = function () {
        pw.post('/Contractors/Delete',
            { id: id },
            'json',
            function (response) {
                pw.reloadGrid('contractor_grd');
            });

    }
    pw.confirm('حذف', 'آیا مطمئن هستید حذف شود؟', success);
}


function removeContractorUser(contractorId) {
    var success = function () {
        pw.post('/Contractors/RevokeUserFromContractorAsync', { contractorId: contractorId }, 'json', function (response) {

            if (response.state) {
                pw.reloadGrid('contractor_grd');
            }

        }, '', true);
    }
    pw.confirm('حذف', 'آیا مطمئن هستید حذف شود؟', success);
}
///////////////////////////////////////////////////////
function openVideoSelectorAsModal(event, idElement, nameElement) {
    event.preventDefault();
    $('<div id=VideoSelectorDiv">').dialog({
        modal: true,
        open: function () {
            var thisCtl = $(this);
            $.ajax({
                url: '/VideoFile/Selector',
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
        title: "انتخاب ویدیو",
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
////////////////////////////////////////////////////////////////////////////////////////


function CapacityDisk() {
    pw.post('/Dashboard/Capacity', null, 'json', SetCapacityDisk, null, false);
}

function OnlineUserCount() {
    pw.post('/Dashboard/OnlineUserCount', null, 'json', SetOnlineUserCount, null, false);
}
function SetOnlineUserCount(result) {
    $("#OnlineUserCount").html(result.count);
}

function AllUserCount() {
    pw.post('/Dashboard/AllUserCount', null, 'json', SetAllUserCount, null, false);
}
function SetAllUserCount(result) {
    $("#AllUserCount").html(result.count);
}

function SetCapacityDisk(result) {
    SetGauge('فضای سرور', 'CapacityDisk', result.totalSize, result.totalSize-result.totalFreeSpace, 'Gaugechart');
}

function chartMultiLineSite() {
    pw.post('/Dashboard/MultiLineSite', null, 'json', SetCountValueMultiLineChart, null, false);
}
var scanQualityNames = ['بازدید سایت', 'کتاب', 'مقاله', 'دوره های آفلاین', 'کلاسهای آنلاین', 'نمایش ویدیو', 'ویدیو پولی','ادوبی'];

function SetCountValueMultiLineChart(result) {

    if (result == null) {
        $('#chartMultiLine').html('<p class="text-center">رکوردی یافت نشد</p>');
        return;
    }

    var xAxisData = [];
    var dataPoints = [];
    var dataPointSite = [];
    var dataPointBook = [];
    var dataPointPaper = [];
    var dataPointCourse = [];
    var dataPointOnline = [];
    var dataPointVideo = [];
    var dataPointVideoNotFree = [];
    var dataPointAdobe = [];
    //Site, Book, Paper, Course, Online,Video, VideoNotFree, Adobe

    for (var i = 0; i < result.length; i++) {
        xAxisData.push(result[i].Date);
        dataPointSite.push([result[i].Date, result[i].Site]);
        dataPointBook.push([result[i].Date, result[i].Book]);
        dataPointPaper.push([result[i].Date, result[i].Paper]);
        dataPointCourse.push([result[i].Date, result[i].Course]);
        dataPointOnline.push([result[i].Date, result[i].Online]);
        dataPointVideo.push([result[i].Date, result[i].Video]);
        dataPointVideoNotFree.push([result[i].Date, result[i].VideoNotFree]);
        dataPointAdobe.push([result[i].Date, result[i].Adobe]);
    }
    dataPoints.push( { name: scanQualityNames[0], data: dataPointSite});
    dataPoints.push( { name: scanQualityNames[1], data: dataPointBook});
    dataPoints.push( { name: scanQualityNames[2], data: dataPointPaper});
    dataPoints.push( { name: scanQualityNames[3], data: dataPointCourse});
    dataPoints.push( { name: scanQualityNames[4], data: dataPointOnline});
    dataPoints.push( { name: scanQualityNames[5], data: dataPointVideo});
   // dataPoints.push( { name: scanQualityNames[6], data: dataPointVideoNotFree});
    dataPoints.push({ name: scanQualityNames[7], data: dataPointAdobe });

    Highcharts.chart('chartMultiLine', {

        chart: {
            zoomType: 'x'
        },

        title: {
            text: 'نمودار بازدید سایت'
        },

        //subtitle: {
        //    text: 'Using the Boost module'
        //},

        tooltip: {
            valueDecimals: 0
        },

        xAxis: {
            //categories: xAxisData,
            // type: 'datetime',
            //showFirstLabel: true,
            //showLastLabel: true
            labels: {
                step: result.length - 1,
                rotation: -45,
            }
        },
        yAxis: {
            title: {
                text: ' تعداد'
            }
        },
        series: dataPoints,
        exporting: {
            // enabled: false,
            buttons: {
                contextButton: {
                    menuItems: ["viewFullscreen", "printChart"]
                }
            }
        }
    });
}



function SetGauge(titleF, title, max, result, div) {
    var plotBands3 = max / 3;
    var plotBands1 = max / 1.5;

    if (result > max) {
        max = result;
    }
    var min = 0;
    if (min > result) {
        min = result;
    }


    Highcharts.chart(div, {
        chart: {
            type: 'gauge',
            plotBackgroundColor: null,
            plotBackgroundImage: null,
            plotBorderWidth: 0,
            plotShadow: false
        },

        title: {
            text: titleF
        },

        pane: {
            startAngle: -150,
            endAngle: 150,
            background: [{
                backgroundColor: {
                    linearGradient: {
                        x1: 0, y1: 0, x2: 0, y2: 1
                    },
                    stops: [
                        [0, '#FFF'],
                        [1, '#333']
                    ]
                },
                borderWidth: 0,
                outerRadius: '109%'
            }, {
                backgroundColor: {
                    linearGradient: {
                        x1: 0, y1: 0, x2: 0, y2: 1
                    },
                    stops: [
                        [0, '#333'],
                        [1, '#FFF']
                    ]
                },
                borderWidth: 1,
                outerRadius: '107%'
            }, {
                // default background
            }, {
                backgroundColor: '#DDD',
                borderWidth: 0,
                outerRadius: '105%',
                innerRadius: '103%'
            }]
        },

        // the value axis
        yAxis: {
            min: min,
            max: max,

            minorTickInterval: 'auto',
            minorTickWidth: 1,
            minorTickLength: 10,
            minorTickPosition: 'inside',
            minorTickColor: '#666',

            tickPixelInterval: 30,
            tickWidth: 2,
            tickPosition: 'inside',
            tickLength: 10,
            tickColor: '#666',
            labels: {
                step: 2,
                rotation: 'auto'
            },
            title: {
                text: ' '
            },
            plotBands: [{
                from: min,
                to: plotBands3,
                color: '#55BF3B' // green
            }, {
                from: plotBands3,
                to: plotBands1,
                color: '#DDDF0D' // yellow
            }, {
                from: plotBands1,
                to: max,
                color: '#DF5353' // red
            }]
        },

        series: [{
            name: title,
            data: [result],
            tooltip: {
                valueSuffix: ' '
            }
        }],
        exporting: {
            // enabled: false,
            buttons: {
                contextButton: {
                    menuItems: ["viewFullscreen", "printChart"]
                }
            }
        }
    },
        // Add some life
        function (chart) {
            if (!chart.renderer.forExport) {
                //    $('#btn').click(function () {
                //        var point = chart.series[0].points[0];
                //        point.update(result);
                //    });
                var change = false;
                //if (title == "Humidity" || title == "Temprature") {
                //    change = true;
                //}
                if (change) {
                    var interval_id = setInterval(function () {
                        try {
                            if (title == "Humidity") {
                                var point = chart.series[0].points[0];
                                if (ScannerHumidityValue > max) {
                                    ScannerHumidityValue = max;
                                }
                                if (ScannerHumidityValue < min) {
                                    ScannerHumidityValue = min;
                                }
                                point.update(ScannerHumidityValue);
                            } else if (title == "Temprature") {
                                var point = chart.series[0].points[0];
                                if (ScannerTempratureValue > max) {
                                    ScannerTempratureValue = max;
                                }
                                if (ScannerTempratureValue < min) {
                                    ScannerTempratureValue = min;
                                }
                                point.update(ScannerTempratureValue);
                            } else {
                                var point = chart.series[0].points[0],
                                    newVal,
                                    inc = Math.round((Math.random() - 0.5) * 20);

                                newVal = point.y + inc;
                                if (newVal < 0 || newVal > max) {
                                    newVal = point.y - inc;
                                }

                                point.update(newVal);
                            }
                        } catch (e) {

                        }
                    }, 3000);

                    intervalIds.push(interval_id);
                }

            }
        });
}