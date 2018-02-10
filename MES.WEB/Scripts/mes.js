function deleteElement(event, element) {
    $.ajaxSetup({ cache: false });
    event.preventDefault();
    $.get(element.href, function (data) {
        if (data.Succedeed) {
            $(element).closest("tr").remove();
            alertGood(data.Message);
        } else {
            alertBad(data.Message);
        }
    });
};

function deleteElementFromList(event, element) {
    $.ajaxSetup({ cache: false });
    event.preventDefault();
    $.get(element.href, function (data) {
        if (data.Succedeed) {
            unloadModal(data);
            $("#results2").html('<div class="callout callout-success">' +
                '<h4>' + data.Message +'</h4 ></div >');
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
    $('#alert-message').html(result).delay(2500).slideUp(300);
    $('#alert-message').show();
}

function alertBad(message) {
    var result = '<div class="alert alert-danger alert-dismissable">' +
        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>' +
        '<h4><i class="icon fa fa-ban"></i> Не сохранено!</h4>' + message + '</div>';
    $('#alert-message').html(result).delay(9500).slideUp(300);
    $('#alert-message').show();
};





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


$(function() {
    $.ajaxSetup({ cache: false });
    $(".addtn").click(function(e) {

        e.preventDefault();
        $.get(this.href,
            function(data) {
                $('#dialogContent').html(data);
                $('#modDialog').modal('show');

            });
    });
});

$(function () {
    $.ajaxSetup({ cache: false });
    $(".charts").click(function (e) {

        var startDate = $('#StartDate').val();
        startDate = encodeURIComponent(startDate);
        var endDate = $('#EndDate').val();

        e.preventDefault();
        $.get(this.href + '?startDate=' +
            startDate +
            '&endDate=' +
            endDate,
            function (data) {
                $('#dialogContent').html(data);
                $('#modDialog').modal('show');

            });
    });
});

function unloadModal(data) {
    if (data.Succedeed===true) {
        $('#modDialog').modal('hide');
        $("#results").load(data.Accessory);
        alertGood(data.Message);
    } else if (data.Succedeed === undefined) {}
    else {
        $('#modDialog').modal('hide');       
        $("#results").load(data.Accessory);
        alertBad(data.Message);
    }
};

function unloadPartial(data) {
    if (data.Succedeed === true) {
        $('#modDialog').modal('hide');
        $("#results2").load(data.Accessory);
        alertGood(data.Message);
    } else if (data.Succedeed === undefined) { }
    else {
        $('#modDialog').modal('hide');
        $("#results2").load(data.Accessory);
        alertBad(data.Message);
    }
};

//Date picker
$('#StartDate').datepicker({
    language: "ru",
    orientation: "bottom auto",
    todayHighlight: true,
    todayBtn: "linked"

});


$('#EndDate').datepicker({
    language: "ru",
    orientation: "bottom auto",
    todayHighlight: true,
    todayBtn: "linked"
});

//Обновить при выборе даты
$('#StartDate').on('changeDate',
    function(ev) {
        view();
    });

$('#EndDate').on('changeDate',
    function(ev) {
        view();
    });


