window.ModalPopUpKapat = function ()
{
    $('#argemModalPopUp').modal('hide');
};

$(function ()
{
    $(".anchorOku").click(function ()
    {
        // debugger;
        var $buttonClicked = $(this);
        var SayfaAd = $buttonClicked.attr('data-sayfaad');
        $('#argemModalLabel').html(SayfaAd);
        var id = $buttonClicked.attr('data-id');
        // var id = "0";
        var Durum = $buttonClicked.attr('data-durum');
        var options = { "backdrop": "static", keyboard: true };

        var $ifmp = $('._armp_Iframe');
        $ifmp.attr('src', URLOku + '?Durum=' + Durum + '&Key=' + id);

        $('#argemModalPopUp').modal(options);
        $('#argemModalPopUp').modal('show');


        /*// debugger;
        var $buttonClicked = $(this);
        var SayfaAd = $buttonClicked.attr('data-sayfaad');
        $('#argemModalLabel').html(SayfaAd);
        var id = $buttonClicked.attr('data-id');
        var Durum = $buttonClicked.attr('data-durum');
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: URLOku,
            contentType: "application/json; charset=utf-8",
            data: { "Durum": Durum, "Key": id },
            datatype: "json",
            cache: false,
            success: function (data)
            {
                // debugger;
                $('#argemModalContent').html(data);
                // alert($('#myModalContent').html());
                $('#argemModalPopUp').modal(options);
                $('#argemModalPopUp').modal('show');

            },
            error: function (request, status, error)
            {
                alert('Sistem Hatası:\n' + request.responseText);
            }
        });*/
    });

    $(".anchorGuncelle").click(function ()
    {
        alert(1);
        // debugger;
        var $buttonClicked = $(this);
        var SayfaAd = $buttonClicked.attr('data-sayfaad');
        $('#argemModalLabel').html(SayfaAd);
        var id = $buttonClicked.attr('data-id');
        // var id = "0";
        var Durum = $buttonClicked.attr('data-durum');
        var options = { "backdrop": "static", keyboard: true };

        var $ifmp = $('._armp_Iframe');
        $ifmp.attr('src', URLGuncelle + '?Durum=' + Durum + '&Key=' + id);

        $('#argemModalPopUp').modal(options);
        $('#argemModalPopUp').modal('show');

        /*// debugger;
        var $buttonClicked = $(this);
        var SayfaAd = $buttonClicked.attr('data-sayfaad');
        $('#argemModalLabel').html(SayfaAd);
        var id = $buttonClicked.attr('data-id');
        var Durum = $buttonClicked.attr('data-durum');
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: URLGuncelle,
            contentType: "application/json; charset=utf-8",
            data: { "Durum": Durum, "Key": id },
            datatype: "json",
            success: function (data)
            {
                // debugger;
                $('#argemModalContent').html(data);
                // alert($('#myModalContent').html());
                $('#argemModalPopUp').modal(options);
                $('#argemModalPopUp').modal('show');

            },
            error: function (request, status, error)
            {
                alert('Sistem Hatası:\n' + request.responseText);
            }
        });*/
    });

    $(".anchorYeniKayit").click(function ()
    {
        // debugger;
        var $buttonClicked = $(this);
        var SayfaAd = $buttonClicked.attr('data-sayfaad');
        $('#argemModalLabel').html(SayfaAd);
        // var id = $buttonClicked.attr('data-id');
        var id = "0";
        var Durum = $buttonClicked.attr('data-durum');
        var options = { "backdrop": "static", keyboard: true };

        var $ifmp = $('._armp_Iframe');
        $ifmp.attr('src', URLGuncelle + '?Durum=' + Durum + '&Key=' + id);

        $('#argemModalPopUp').modal(options);
        $('#argemModalPopUp').modal('show');

        // return;
        /*$.ajax({
            type: "GET",
            url: URLGuncelle,
            contentType: "application/json; charset=utf-8",
            data: { "Durum": Durum, "Key": id },
            datatype: "json",
            success: function (data)
            {
                // debugger;
                // $('#argemModalContent').html(data);
                $('#argemModalPopUp').modal(options);
                $('#argemModalPopUp').modal('show');

            },
            error: function (request, status, error)
            {
                alert('Sistem Hatası:\n' + request.responseText);
            }
        });*/
    });

    /*$("#closbtn").click(function ()
    {
        $('#myModal').modal('hide');
    });*/
});


function fnIframeBoyutAyarla()
{
    var cssMpIF = '_armp_Iframe';
    // var cssMp = '_armp_Iframe';

    var $mp = $('#argemModalPopUp');
    var $ifmp = $('.' + cssMpIF);

    // var $ifmp = $mp.find('.' + cssIframe);
    $ifmp
        .css({ 'height': ($ifmp.parent().height() - $mp.find('.modal-header').outerHeight()) });
}

