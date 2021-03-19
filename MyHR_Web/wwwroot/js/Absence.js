function ShowTime() {
    var NowDate = new Date();
    var d = NowDate.getDay();
    var dayNames = new Array("星期⽇", "星期⼀", "星期⼆", "星期三", "星期四", "星期五", "星期六");
    document.getElementById('showbox').innerHTML = '⽬前時間：' + NowDate.toLocaleString() + '（' +
        dayNames[d] + '）';
    setTimeout('ShowTime()', 1000);
}

function onClock() {
    



    window.alert("您已打卡成功!");
}

function offClock() {
    window.alert("您已打卡成功!");
}

