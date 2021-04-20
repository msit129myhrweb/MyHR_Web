$(document).keyup(function (e) {
    if (e.shiftKey && e.keyCode == 49 ) { //Shift + 1
        $("#idProcess").val("電訪個性外向，專業技能OK，已通知主管二次電訪。");
    }
    else if (e.shiftKey && e.keyCode == 50) { //Shift + 2
        $("#idProcess").val("主管電訪通過，已通知現場面試");
    }
    else if (e.shiftKey && e.keyCode == 51) { //Shift + 3
        $("#idProcess").val("現場面試通過，主管已同意，待 HR 談妥薪資。");
    }
    else  if (e.shiftKey && e.keyCode == 52) { //Shift + 4
        $("#idProcess").val("該員薪資需求 38K-43k，我方告知願以 41K 聘任，待該員考慮後回覆。");
    }
    else if (e.shiftKey && e.keyCode == 53) { //Shift + 5
        $("#idProcess").val("該員回覆同意薪資 41K，已安排報到。");
    }
    else if (e.shiftKey && e.keyCode == 54) { //Shift + 6
        $("#idProcess").val("該員面試後無意願");
    }
    else if (e.shiftKey && e.keyCode == 55) { //Shift + 7
        $("#idProcess").val("該員經驗不符合職務需求");
    }
    else if (e.shiftKey && e.keyCode == 56) { //Shift + 8
        $("#idProcess").val("");
    }
    else if (e.shiftKey && e.keyCode == 57) { //Shift + 9
        $("#idProcess").val("哈哈9");
    }
    else if (e.shiftKey && e.keyCode == 48) { //Shift + 0
        $("#idProcess").val("哈哈10");
    }
    //else if (e.shiftKey && e.keyCode == 81) { //Shift + Q
    //    $("#txtAccount").val(15);
    //    $("#txtPassword").val(5768325);
    //}
    //else if (e.shiftKey && e.keyCode == 87) { //Shift + W
    //    $("#txtAccount").val(17);
    //    $("#txtPassword").val(54742);
    //}
});
