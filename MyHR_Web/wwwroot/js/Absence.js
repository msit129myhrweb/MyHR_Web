/*list*/
function ShowTime() {
    var NowDate = new Date();
    var d = NowDate.getDay();
    var dayNames = new Array("星期⽇", "星期⼀", "星期⼆", "星期三", "星期四", "星期五", "星期六");
    document.getElementById('showbox').innerHTML = '⽬前時間：' + NowDate.toLocaleString() + '（' +
        dayNames[d] + '）';
    setTimeout('ShowTime()', 1000);
}

$(document).ready(function() {

    $("#btnOn").click(function () {
        var id = $("#id").value();
        let d = new date();



        if (id != null) {
            $.ajax({
                type: "POST",
                url: "",
                data: "{CEmployeeId=id,COn=d}",
                success: function () {

                }

            })
        }
    })
})
function onClock() {
    var req = new XMLHttpRequest();
    req.open("get", "/Absence/List");
    req.onload = function () {
        var now = new Date();
        var nowtime = now.toLocaleString();
        var content = document.getElementById("showClock");
        content.innerHTML = nowtime + "\n您已打卡成功!\n下班別忘記打卡!";
    };
    req.send();
}

function offClock() {
    var req = new XMLHttpRequest();
    req.open("get", "/Absence/List");
    req.onload = function () {
        var now = new Date();
        var nowtime = now.toLocaleString();
        var content = document.getElementById("showClock");
        content.innerHTML = nowtime + "\n您已打卡成功!\n今天辛苦你了,趕快回家休息吧~";
    };
    req.send();
}

function showHistory() {
    $.ajax({
        url: "/Absence/List",
        type: "GET",
        success: function (data) {
            var txt = "<table  id='example2' class='table table - bordered table - hover'><tr><th>上班<th>下班<th>狀態";
            var ons = data.getElementById("#on");
            var offs = data.getElementById("#off");
            var status = data.getElementById("#status");
            for (var i = 0; i < ons.length; i++) {
                txt += "<tr><td>" + ons[i].firstChild.nodeValue;
                txt += "<td>" + offs[i].firstChild.nodeValue;
                txt += "<td>" + status[i].firstChild.nodeValue;
            }
            $("#showClock").html(txt);
        }
    });
}

/*create*/
//document.addEventListener("DOMContentLoaded", function () {
//    document.getElementById("#submit").addEventListener("click", function () {
//        var result = [];
//        var clocks = document.getElementsByName("checkbox");

//        for (var i = 0; i < clocks.length; i++) {
//            var clock = clocks.item(i);
//            if (clock.checked) {
//                result.push(clock.value);
//            }
//        }
//        window.alert(result.toLocaleString());
//    }, false)
//}, false)


//$(document).ready(function () {
//    $("#btnOn").click(function () {
//        var id = $("#id").value();
//        let d = new date();


        
//        if (id!=null) {
//            $.ajax({
//                type: "POST",
//                url: "",
//                async: false,
//                data: "{CEmployeeId=id,COn=d}",
//                success: function () {
                    
//                }

//            })
//        }
//    })
//})