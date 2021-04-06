$(document).keyup(function (e) {
    if (e.shiftKey && e.keyCode == 49 ) { //Shift + 1
        $("#txtAccount").val(1);
        $("#txtPassword").val(789);
    }
    else if (e.shiftKey && e.keyCode == 50) { //Shift + 2
        $("#txtAccount").val(2);
        $("#txtPassword").val(123);
    }
    else if (e.shiftKey && e.keyCode == 51) { //Shift + 3
        $("#txtAccount").val(5);
        $("#txtPassword").val(456);
    }
    else  if (e.shiftKey && e.keyCode == 52) { //Shift + 4
        $("#txtAccount").val(7);
        $("#txtPassword").val(123);
    }
    else if (e.shiftKey && e.keyCode == 53) { //Shift + 5
        $("#txtAccount").val(8);
        $("#txtPassword").val(123);
    }
    else if (e.shiftKey && e.keyCode == 54) { //Shift + 6
        $("#txtAccount").val(10);
        $("#txtPassword").val(123);
    }
    else if (e.shiftKey && e.keyCode == 55) { //Shift + 7
        $("#txtAccount").val(11);
        $("#txtPassword").val(123);
    }
    else if (e.shiftKey && e.keyCode == 56) { //Shift + 8
        $("#txtAccount").val(12);
        $("#txtPassword").val(123);
    }
    else if (e.shiftKey && e.keyCode == 57) { //Shift + 9
        $("#txtAccount").val(13);
        $("#txtPassword").val(123);
    }
    else if (e.shiftKey && e.keyCode == 48) { //Shift + 0
        $("#txtAccount").val(14);
        $("#txtPassword").val(123);
    }
});
