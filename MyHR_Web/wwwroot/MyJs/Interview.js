//$(document).on("click", ".open-AddBookDialog", function () {
//    var myBookId = $(this).data('id');       
//    // As pointed out in comments, 
//    // it is superfluous to have to manually call the modal.
//    // $('#addBookDialog').modal('show');
//});

//function updateDpet() {
//    // 初始化部門修改信息下拉列表
//    $('#updateDeptMd').on('show.bs.modal', function (e) {
//        var id = $(e.relatedTarget).data('orderid'); //根據上面a標籤中傳遞的data-orderid取值,這裏也可以通過data-id取值
//        //var data =  $('#updateDeptMd').data(); 也可以直接取data對象再取值
//        console.log(id);
//        // 到後臺獲取部門信息
//        $.ajax({
//            url: '/dept/queryId',
//            type: 'POST',
//            data: { id: id },
//            dataType: 'json',
//            success: function (data) {
//                console.log(data);
//                /*查詢empId*/
//                $('#updateDeptId').val(data.id);
//                $("#updateDeptName").val(data.name);
//            },
//            error: function () {
//                alert('出錯了');
//            }
//        });
//    });
//}

// 條件查詢
$(".filter").change(function () {
    cate = $("#Filter_Status").val();
    dept = $("#Filter_Dept").val();
    console.log(cate);
    $.ajax({
        type: "get",
        dataType: "html",
        url: "/Interview/Filter",
        data: { cate: cate, dept: dept },
        success: function (data) {
            console.log("ajax");
            $("#idInterview").html(data);
        },
        error: function (msg) {
            alert("error:" + msg);
        }
    })
});

// 進編輯頁
function fun(id) {
    $("div").remove("#tabpage");
    edit(id);
    detail(id);
    processCreate(id);
}

// 歷程區塊
function detail(id) {
    $.ajax({
        type: "get",
        dataType: "html",
        url: "/Interview/Details",
        data: { id: id },
        success: function (data) {
            console.log("detail");
            $('#tab-process').html(data);
        },
        error: function (msg) {
            alert("error:" + msg);
        }
    })
}

// 面試者區塊
function edit(id) {
    $.ajax({
        type: "get",
        dataType: "html",
        url: "/Interview/Edit",
        data: { id: id },
        success: function (data) {
            console.log("edit");
            $('#tab-user').html(data);
        },
        error: function (msg) {
            alert("error:" + msg);
        }
    })
}

//function editSubmit(id) {        
//    var result = confirm("確認修改?");        
//    if (result) {
//        $.ajax({
//            type: "post",
//            dataType: "html",
//            url: "/Interview/Edit",
//            data: { i: obj },
//            success: function (data) {
//                console.log("edit");                    
//            },
//            error: function (msg) {
//                alert("error:" + msg);
//            }
//        });
//    }
//}

// 歷程新增區塊
function processCreate(id) {
    $.ajax({
        type: "get",
        dataType: "html",
        url: "/Interview/ProcessCreate",
        data: { id: id },
        success: function (data) {
            $('#tab-processCreate').html(data);
        },
        error: function (msg) {
            alert("error:" + msg);
        }
    });
}

