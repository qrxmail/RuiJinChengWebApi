// 浏览器的高度、宽度
var dh = 1080;
var dw = 1920;

// 页面初始化
$(function () {
    // 等比缩放
    dh = $(document).height();
    dw = $(document).width();
    setBodyScale();

    $("#fullScreenBtn").show();
    $("#exitFullScreenBtn").hide();

    // 实时显示时间
    time();
    setInterval("time()", 1000);

    $.getJSON("data.json", result => {
        // 待办任务数据设置
        setTaskData(result.taskData);

        // 操作统计数据设置
        setStatDataOper(result.operData);

        // 雷达图表
        setChartLeiDa(result.leidaData);

        // 柱状图表
        setChartZhuZhuang(result.chartZhuZhuangData);

        // 仪表盘图表
        setChartYiBiao(result.chartYiBiaoData);

        // 故障处理表格
        setBugData(result.bugData);

        // 折线图表
        setChartZheXian(result.chartZheXianData);

        //设备数量及价值统计数据设置
        setStatDataDevice(result.statDataDevice);

        // 环形图表：设备数量，价值占比统计
        setChartHuanXing(result.chartHuanXingData);

        // 地图图表：设备数据
        setChartDiTu(result.chartDiTuData);

    });
});

// 窗体改变大小事件
$(window).resize(function () {
    setBodyScale();
});

// 设置body的缩放比例
function setBodyScale() {

    // 当前窗体的高度
    var ww = $(window).width();

    // 计算缩放比例
    var r = ww / dw;
    $("#body").css("transform", "scale(" + r + ")");
}

// 全屏
function fullScreen() {
    var elem = document.documentElement;
    if (elem.webkitRequestFullScreen) {
        elem.webkitRequestFullScreen();
    } else if (elem.mozRequestFullScreen) {
        elem.mozRequestFullScreen();
    } else if (elem.requestFullScreen) {
        elem.requestFullscreen();
    } else {
        alert("浏览器不支持全屏API或已被禁用");
    }
    $("#fullScreenBtn").hide();
    $("#exitFullScreenBtn").show();
}

// 退出全屏
function exitFullScreen() {
    var elem = document;
    if (elem.webkitCancelFullScreen) {
        elem.webkitCancelFullScreen();
    } else if (elem.mozCancelFullScreen) {
        elem.mozCancelFullScreen();
    } else if (elem.cancelFullScreen) {
        elem.cancelFullScreen();
    } else if (elem.exitFullscreen) {
        elem.exitFullscreen();
    } else {
        alert("浏览器不支持全屏API或已被禁用");
    }
    $("#fullScreenBtn").show();
    $("#exitFullScreenBtn").hide();
}

// 实时时间
function time() {
    var vWeek, vWeek_s,
        vWeek = ["星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
    var date = new Date();
    year = date.getFullYear();
    month = date.getMonth() + 1;
    day = date.getDate();
    hours = date.getHours();
    minutes = date.getMinutes();
    seconds = date.getSeconds();
    vWeek_s = date.getDay();
    $("#time").html(year + "年" + month + "月" + day + "日" + "\t" + hours + ":" + minutes + ":" + seconds + "\t" + vWeek[vWeek_s]);
};

// 设置待办任务数据
function setTaskData(data) {
    var htmlStr = "";
    for (var i = 0; i < data.length; i++) {
        htmlStr += '<tr>' +
            '<td style = "width:70px;padding: 7px 8px;" >' + data[i].name + '</td>' +
            '<td style = "width:110px;padding: 7px 8px;">' + data[i].user + '</td>' +
            '<td style = "width:120px;padding: 7px 8px;" > ' + data[i].state + '</td> ' +
            '<td style = "width:100px;padding: 7px 8px;" > ' + data[i].branch + '</td> ' +
            '</tr>';
    }
    $("#tasktdody").html(htmlStr);
}

// 设置操作统计数据
function setStatDataOper(data) {
    // 未处理保修
    $("#divbaoxiu").html(data.baoxiu);
    // 今日报修
    $("#divjinribaoxiu").html(data.jinribaoxiu);
    // 故障数
    $("#divguzhang").html(data.guzhang);
    // 完成任务数
    $("#divwanchengrenwu").html(data.wanchengrenwu);
}

// 雷达图表
function setChartLeiDa(data) {

    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('left1'));

    // 指定图表的配置项和数据
    option = {
        //title: {
        //    text: '基础雷达图'
        //},
        tooltip: {},
        //legend: {
        //    data: ['预算分配（Allocated Budget）', '实际开销（Actual Spending）']
        //},
        radar: {
            shape: 'rect',
            radius: ["0%", "65%"],
            name: {
                textStyle: {
                    color: '#fff',
                    fontSize: 14,
                }
            },
            splitArea: {
                show: false,
                //areaStyle: {
                //    color: ["#2a4a93"]  // 图表背景网格的颜色
                //}
            },
            splitLine: {
                show: true,
                lineStyle: {
                    width: 1,
                    color: '#1FADDA' // 图表背景网格线的颜色
                }
            },
            indicator: [
                { name: '设备完好率', max: 100 },
                { name: '设备利用率', max: 100 },
                { name: '维护及时率', max: 100 },
                { name: '检修及时率', max: 100 },
                { name: '巡检及时率', max: 100 },
            ]
        },
        series: [{
            name: '预算 vs 开销（Budget vs spending）',
            type: 'radar',
            itemStyle: {
                normal: {
                    color: "rgba(0, 105, 255, 0.57)", // 图表中各个图区域的边框线拐点颜色
                    lineStyle: {
                        color: "white" // 图表中各个图区域的边框线颜色
                    },
                    areaStyle: {
                        type: 'default'
                    }
                }
            },
            data: [
                {
                    value: data,
                    name: '百分比（%）'
                },
            ]
        }]
    };

    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
}

// 柱状图表
function setChartZhuZhuang(data) {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('left2'));

    // 指定图表的配置项和数据
    var option = {
        title: {
            text: '',
            textStyle: {
                fontSize: 16,
                color: "#fff",
                fontStyle: "normal",
                fontWeight: "normal",
                fontFamily: "Arial"
            },
        },
        tooltip: {},
        //legend: {
        //    data: ['销量']
        //},
        xAxis: {
            data: data.xAxisData,
            axisLabel: {
                show: true,
                textStyle: {
                    color: "#fff"
                }
            },
        },
        yAxis: {
            splitLine: {
                show: false
            },
            axisLabel: {
                show: true,
                textStyle: {
                    color: "#fff"
                }
            },
        },
        series: [
            {
                name: '数量',
                type: 'bar',
                barWidth: 10,
                itemStyle: {
                    color: '#1FADDA',
                    barBorderRadius: [3, 3, 0, 0],
                },
                emphasis: {
                    itemStyle: {
                        color: '#83bff6',
                    }
                },
                data: data.seriesData,
            }
        ]
    };

    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
}

// 仪表盘图表
function setChartYiBiao(data) {
    // 基于准备好的dom，初始化echarts实例
    var myChart1 = echarts.init(document.getElementById('left3'));
    var myChart2 = echarts.init(document.getElementById('left4'));
    var myChart3 = echarts.init(document.getElementById('left5'));
    var myChart4 = echarts.init(document.getElementById('left6'));

    // 指定图表的配置项和数据
    option = {

        tooltip: {
            formatter: '{a}: {c}%'
        },
        series: [
            {
                radius: "100%",
                name: '完好率',
                type: 'gauge',
                startAngle: 180,        // 仪表盘起始角度,默认 225。圆心 正右手侧为0度，正上方为90度，正左手侧为180度。
                endAngle: 0,          // 仪表盘结束角度,默认 -45
                axisLine: {            // 坐标轴线
                    lineStyle: {       // 属性lineStyle控制线条样式
                        color: [[0.09, 'lime'], [0.82, '#1e90ff'], [1, '#ff4500']],
                        width: 1,
                        shadowColor: '#fff', //默认透明
                        shadowBlur: 5
                    }
                },
                axisLabel: {            // 刻度标签。
                    show: true,             // 是否显示标签,默认 true。
                    distance: 5,            // 标签与刻度线的距离,默认 5。
                    color: "#fff",          // 文字的颜色,默认 #fff。
                    fontSize: 8,           // 文字的字体大小,默认 5。
                    formatter: "{value}",   // 刻度标签的内容格式器，支持字符串模板和回调函数两种形式。 示例:// 使用字符串模板，模板变量为刻度默认标签 {value},如:formatter: '{value} kg'; // 使用函数模板，函数参数分别为刻度数值,如formatter: function (value) {return value + 'km/h';}
                },
                axisTick: {            // 坐标轴小标记
                    length: 10,        // 属性length控制线长
                    lineStyle: {       // 属性lineStyle控制线条样式
                        color: 'auto',
                        shadowColor: '#fff', //默认透明
                        shadowBlur: 5
                    }
                },
                splitLine: {           // 分隔线
                    length: 15,         // 属性length控制线长
                    lineStyle: {       // 属性lineStyle（详见lineStyle）控制线条样式
                        width: 1,
                        color: '#fff',
                        shadowColor: '#fff', //默认透明
                        shadowBlur: 5
                    }
                },
                pointer: {              // 仪表盘指针。
                    show: true,             // 是否显示指针,默认 true。
                    length: "65%",          // 指针长度，可以是绝对数值，也可以是相对于半径的百分比,默认 80%。
                    width: 2,               // 指针宽度,默认 8。
                },

                itemStyle: {            // 仪表盘指针样式。
                    color: "auto",          // 指针颜色，默认(auto)取数值所在的区间的颜色
                    opacity: 1,             // 图形透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
                    borderWidth: 0,         // 描边线宽,默认 0。为 0 时无描边。
                    borderType: "solid",    // 柱条的描边类型，默认为实线，支持 'solid', 'dashed', 'dotted'。
                    borderColor: "#000",    // 图形的描边颜色,默认 "#000"。支持的颜色格式同 color，不支持回调函数。
                    //shadowBlur: 5,         // (发光效果)图形阴影的模糊大小。该属性配合 shadowColor,shadowOffsetX, shadowOffsetY 一起设置图形的阴影效果。
                    //shadowColor: "#fff",    // 阴影颜色。支持的格式同color。
                },
                detail: {
                    show: true,             // 是否显示详情,默认 true。
                    offsetCenter: [0, -25],// 相对于仪表盘中心的偏移位置，数组第一项是水平方向的偏移，第二项是垂直方向的偏移。可以是绝对的数值，也可以是相对于仪表盘半径的百分比。
                    color: "auto",          // 文字的颜色,默认 auto。
                    fontSize: 15,           // 文字的字体大小,默认 15。
                    fontWeight: "bold",
                    formatter: "{value}%",  // 格式化函数或者字符串
                },
                data: [{ value: 98 }]
            }
        ]
    };
    // 使用刚指定的配置项和数据显示图表。
    option.series[0].data = data.y1.data;
    option.series[0].name = data.y1.name;
    myChart1.setOption(option);
    // 使用刚指定的配置项和数据显示图表。
    option.series[0].data = data.y2.data;
    option.series[0].name = data.y2.name;
    myChart2.setOption(option);
    // 使用刚指定的配置项和数据显示图表。
    option.series[0].data = data.y3.data;
    option.series[0].name = data.y3.name;
    myChart3.setOption(option);
    // 使用刚指定的配置项和数据显示图表。
    option.series[0].data = data.y4.data;
    option.series[0].name = data.y4.name;
    myChart4.setOption(option);
}

// 故障处理表格数据设置
function setBugData(data) {
    var htmlStr = "";
    for (var i = 0; i < data.length; i++) {
        htmlStr += '<tr>' +
            '<td style = "padding: 7px 5px; width: 9%" > ' + data[i].deviceName + '</td>' +
            '<td style="padding: 7px 5px; width: 9%">' + data[i].no + '</td>' +
            '<td style = "padding: 7px 5px; width: 14%" > ' + data[i].user + '</td> ' +
            '<td style = "padding: 7px 5px; width: 18% " >' + data[i].time + '</td> ' +
            '<td style = "padding: 7px 5px; width: 14%" > ' + data[i].level + '</td> ' +
            '<td style = "padding: 7px 5px; width: 14%" > ' + data[i].state + '</td> ' +
            '<td style = "padding: 7px 5px; width: 22%" > ' + data[i].timelen + '</td> ' +
            '</tr>';
    }
    $("#bugtbody").html(htmlStr);
}

// 折线图表
function setChartZheXian(data) {
    // 基于准备好的dom，初始化echarts实例
    var myChart1 = echarts.init(document.getElementById('right1'));
    var myChart2 = echarts.init(document.getElementById('right2'));
    var myChart3 = echarts.init(document.getElementById('right3'));

    // 指定图表的配置项和数据
    option = {
        title: {
            show: true,
            text: "",
            textStyle: {
                fontSize: 14,
                color: "#fff",
                fontStyle: "normal",
                fontWeight: "normal",
                fontFamily: "Arial"
            },
        },
        xAxis: {
            type: 'category',
            boundaryGap: true,
            axisLabel: {
                textStyle: {
                    color: '#fff',//坐标值得具体的颜色

                }
            },
            splitLine: {
                show: true, //网格线
                lineStyle: {
                    type: 'solid',
                    color: '#2E385D',
                    width: '1'
                }
            },
            data: data.xAxisData,
        },
        yAxis: {
            type: 'value',
            splitLine: {
                show: true, //网格线
                lineStyle: {
                    type: 'solid',
                    color: '#2E385D',
                    width: '1'
                }
            },
            splitArea: { show: false },//保留网格区域
            axisLabel: {
                textStyle: {
                    color: '#fff',//坐标值得具体的颜色

                }
            },
            axisLine: {
                lineStyle: {
                    type: 'solid',
                    color: '#2E385D',
                    width: '1'
                }
            },
        },
        series: [{
            data: [0, 2, 4, 6, 3, 6, 3, 5, 7, 8],
            type: 'line',
            areaStyle: {
                color: "rgba(22,212,229, 0.3)"
            },
            lineStyle: {
                color: "rgba(22,212,229, 1)"
            },
            itemStyle: {
                color: "rgba(22,212,229, 1)"
            }
        }]
    };

    // 使用刚指定的配置项和数据显示图表。
    option.title.text = "";
    option.series[0].data = data.seriesData1; //[0, 2, 4, 6, 3, 6, 3, 5, 7, 8];
    option.series[0].areaStyle = { color: "rgba(22,212,229, 0.3)" };
    option.series[0].lineStyle = { color: "rgba(22,212,229, 1)" };
    option.series[0].itemStyle = { color: "rgba(22,212,229, 1)" };
    myChart1.setOption(option);

    option.title.text = "";
    option.series[0].data = data.seriesData2; //[0, 20, 14, 26, 31, 61, 31, 52, 71, 78];
    option.series[0].areaStyle = { color: "rgba(255,235,123, 0.3)" };
    option.series[0].lineStyle = { color: "rgba(255,235,123, 1)" };
    option.series[0].itemStyle = { color: "rgba(255,235,123, 1)" };
    myChart2.setOption(option);

    option.title.text = "";
    option.series[0].data = data.seriesData3; //[10, 20, 51, 20, 72, 49, 10, 51, 72, 81];
    option.series[0].areaStyle = { color: "rgba(107,139,243, 0.3)" };
    option.series[0].lineStyle = { color: "rgba(107,139,243, 1)" };
    option.series[0].itemStyle = { color: "rgba(107,139,243, 1)" };
    myChart3.setOption(option);
}

//设备数量及价值统计数据设置
function setStatDataDevice(data) {
    $("#divdevicenum").html(data.devicenum);
    $("#divdevicecost").html(data.devicecost);
    $("#divdevicenumnow").html(data.devicenumnow);
    $("#divdevicecostnow").html(data.devicecostnow);
}

// 环形图表：设备数量，价值占比统计
function setChartHuanXing(data) {
    // 基于准备好的dom，初始化echarts实例
    var myChart1 = echarts.init(document.getElementById('pie1'));
    var myChart2 = echarts.init(document.getElementById('pie2'));

    // 指定图表的配置项和数据
    option = {
        tooltip: {
            trigger: 'item',
            formatter: '{a} <br/>{b}: {c} ({d}%)'
        },
        //legend: {
        //    orient: 'vertical',
        //    left: 10,
        //    data: ['直接访问', '邮件营销', '联盟广告', '视频广告', '搜索引擎']
        //},

        //环形图中间添加文字
        graphic: [{
            type: 'text',
            left: 'center',
            top: '45%',//通过不同top值可以设置上下显示
            style: {
                text: '45.23',
                textAlign: 'center',
                fill: '#1DD6E5', //文字的颜色
                width: 30,
                height: 30,
                fontSize: 20,
                fontFamily: "Microsoft YaHei"
            }
        }],
        series: [
            {
                name: '数量占比',
                type: 'pie',
                radius: ['50%', '75%'],
                avoidLabelOverlap: false,
                label: {
                    show: false,
                    position: 'center'
                },
                emphasis: {
                    //label: {
                    //    show: true,
                    //    fontSize: '30',
                    //    fontWeight: 'bold'
                    //}
                },
                labelLine: {
                    show: false
                },
                data: [
                    {
                        value: 31235,
                        name: '原数量',
                        itemStyle: {
                            color: "#1ccad5",
                            label: {
                                show: true,
                                position: 'inside'
                            },
                        }
                    },
                    {
                        value: 232,
                        name: '新增数量',
                        itemStyle: {
                            color: "#172e74",
                            label: {
                                show: true,
                                position: 'inside'
                            },
                        }
                    },
                ]
            }
        ]
    };

    // 使用刚指定的配置项和数据显示图表。
    option.graphic[0].style.text = "37.50";
    option.graphic[0].style.fill = "#1DD6E5";
    option.series[0].name = "数量占比";
    option.series[0].data = data.seriesData1;
    myChart1.setOption(option);

    option.graphic[0].style.text = "23.08";
    option.graphic[0].style.fill = "#FCB760";
    option.series[0].name = "价值占比";
    option.series[0].data = data.seriesData2;
    myChart2.setOption(option);
}

// 地图图表：设备数据
function setChartDiTu(data) {
    // 基于准备好的dom，初始化echarts实例
    var chart = echarts.init(document.getElementById('map'));
    chart.setOption({
        tooltip: {
            trigger: 'item',
            formatter: '{b}<br/>{c} 台'
        },
        series: [{
            type: 'map',
            map: '陕西',
            showLegendSymbol: false,    //去除地图指示点
            roam: false,  // 是否开启鼠标缩放和平移漫游。默认不开启。如果只想要开启缩放或者平移，可以设置成 'scale' 或者 'move'。设置成 true 为都开启
            label: {
                normal: {
                    show: true,         //显示省份标签
                    textStyle: {        //省份字体设置
                        fontSize: 14,
                        color: '#fff',
                    }
                },
                emphasis: {             //鼠标悬浮效果
                    show: true
                }
            },

            itemStyle: {
                normal: {
                    borderWidth: 0.5,//区域边框宽度
                    borderColor: '#0EFAFD',//区域边框颜色
                    areaColor: "rgba(50,113,236,0.35)", // 区域颜色
                },
                emphasis: {
                    show: true,
                }
            },
            data: data
        }]
    });
}