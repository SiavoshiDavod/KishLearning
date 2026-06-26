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
    SetGauge('فضای سرور', 'CapacityDisk', result.totalSize, result.totalSize - result.totalFreeSpace, 'Gaugechart');
}

function chartMultiLineSite() {
    pw.post('/Dashboard/MultiLineSite', null, 'json', SetCountValueMultiLineChart, null, false);
}

function RegistrationTable() {
    pw.post('/UsersAdmin/LoadList?_search=true&nd=1569562579153&rows=10&page=1&sidx=id&sord=desc&_=1569562578807', null, 'json', SetRegistrationTable, null, false);
}

function JoinUsTable() {
    pw.post('/JoinUs/LoadList?_search=true&nd=1569562583005&rows=10&page=1&sidx=Id&sord=desc&_=1569562582043', null, 'json', SetJoinUsTable, null, false);
}

function ZarinpalPaymentTable() {
    // pw.post('', null, 'json', SetZarinpalPaymentTableTable, null, false);
}

function SetJoinUsTable(result) {
    var id = "10JoinUs";
    drawTaleByGridValue(result, id);
}

function SetRegistrationTable(result) {
    var id = "10Registration";
    drawTaleByGridValue(result, id);
}
function drawTaleByGridValue(obj, id) {
    var tbl = "<table class='table table-bordered ' style='font-size: 10px;'>";//<thead> <tr> <th>نام فایل</th> <th> دانلود</th> </tr> </thead>";
    if (obj !== null) {
        tbl += "<thead>";
        for (x in obj.Rows[0]) {
            if (x === "Name" || x === "Family" || x === "date_register_Shamsi" || x === "Email" || x === "Mobile" || x === "CreatedDateShamsi" || x === "Description") {
                tbl += "<th>";
                switch (x) {
                    case "Name":
                        tbl += "نام";
                        break;
                    case "Family":
                        tbl += "فامیلی";
                        break;
                    case "date_register_Shamsi":
                        tbl += "تاریخ";
                        break;
                    case "CreatedDateShamsi":
                        tbl += "تاریخ";
                        break;
                    case "Email":
                        tbl += "ایمیل";
                        break;
                    case "Mobile":
                        tbl += "موبایل";
                        break;
                    case "Description":
                        tbl += "توضیحات";
                        break;
                }

                tbl += "</th>";
            }
        }
        tbl += "</thead>";
        for (var i = 0; i < obj.Rows.length; i++) {
            tbl += "<tr>";
            for (x in obj.Rows[i]) {
                if (x === "Name" || x === "Family" || x === "date_register_Shamsi" || x === "Email" || x === "Mobile" || x === "CreatedDateShamsi" || x === "Description") {
                    tbl += "<td>";
                    tbl += obj.Rows[i][x];
                    tbl += "</td>";
                }

            }
            tbl += "</tr>";
            //tbl += "<tr> <td>" + res.data[i].name + "</td> <td><a target='_blank' href='" + res.data[i].url + "'><span class='fa fa-download'></span></a></td> </tr>";
        }
        tbl += "</table>";

        $('#' + id).html(tbl);
    }
}

function SetZarinpalPaymentTableTable(result) {
    var id = "10Payment";
}

var scanQualityNames = ['بازدید سایت', 'کتاب', 'مقاله', 'دوره های آفلاین', 'کلاسهای آنلاین', 'نمایش ویدیو', 'ویدیو پولی', 'ادوبی'];

function SetCountValueMultiLineChart(result) {

    if (result === null) {
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
    dataPoints.push({ name: scanQualityNames[0], data: dataPointSite });
    dataPoints.push({ name: scanQualityNames[1], data: dataPointBook });
    dataPoints.push({ name: scanQualityNames[2], data: dataPointPaper });
    dataPoints.push({ name: scanQualityNames[3], data: dataPointCourse });
    dataPoints.push({ name: scanQualityNames[4], data: dataPointOnline });
    dataPoints.push({ name: scanQualityNames[5], data: dataPointVideo });
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
                rotation: -45
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
           
        });
}