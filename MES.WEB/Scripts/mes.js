function deleteElement(event, element) {
    $.ajaxSetup({ cache: false });
    event.preventDefault();
    $.get(element.href, function (data) {
        
        //alert(data.Message);
        if (data.Succedeed) {
            //view();
            $(element).closest("tr").remove();
            //var a = '<span id="span_one">Brower RDU </span>' + data.Message + '</span>';
            //$("#messageTrue").innerHTML = a;
            //$("#messageTrue").show();
            alertGood(data.Message);
        } else {
            alertBad(data.Message);
        }
    });
};

//Alert
function alertGood(message) {
    var result = '<div class="alert alert-success alert-dismissable">' +
        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>' +
        '<h4><i class="icon fa fa-check"></i> Успешно!</h4>' + message + '</div>';
    $('#alert-message').html(result);
    setTimeout(function() {
            $('#alert-message').hide();
        },
        3000);
}

function alertBad(message) {
    var result = '<div class="alert alert-danger alert-dismissable">' +
        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>' +
        '<h4><i class="icon fa fa-ban"></i> Неудачно!</h4>' + message + '</div>';
    $('#alert-message').html(result);
    setTimeout(function () {
            $('#alert-message').hide();
        },
        3000);
}





//$(function () {
//    $.ajaxSetup({ cache: false });
//    $(".text-info").click(function (e) {

//        e.preventDefault();
//        $.get(this.href,
//            function (data) {
//                $('#dialogContent').html(data);
//                $('#modDialog').modal('show');
//            });
//    });
//});


//$(function() {
//    $.ajaxSetup({ cache: false });
//    $("#addId").click(function(e) {

//        e.preventDefault();
//        $.get(this.href,
//            function(data) {
//                $('#dialogContent').html(data);
//                $('#modDialog').modal('show');
//            });
//    });
//});
  
