/*!
 * Bootstrap 4 multi dropdown navbar ( https://bootstrapthemes.co/demo/resource/bootstrap-4-multi-dropdown-navbar/ )
 * Copyright 2017.
 * Licensed under the GPL license
 */


$(document).ready(function ()
{
    $('.dropdown-menu a.dropdown-toggle').on('click', function (e)
    {
        var $el = $(this);
        var $parent = $(this).offsetParent(".dropdown-menu");
        if (!$(this).next().hasClass('show'))
        {
            $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
        }
        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show');

        $(this).parent("li").toggleClass('show');

        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e)
        {
            $('.dropdown-menu .show').removeClass("show");
        });

        if (!$parent.parent().hasClass('navbar-nav'))
        {
            $el.next().css({ "top": $el[0].offsetTop, "left": $parent.outerWidth() - 4 });
        }

        return false;
    });
});










function GetScope(ctrlName)
{
    var sel = 'div[ng-controller="' + ctrlName + '"]';
    return angular.element(sel).scope();
}



// modal işlemleri

var KayitSilindi = function (Key)
{
    // debugger;
    // alert('Kullanıcı silindi Key:' + Key);
    var $scope = GetScope('ngConKullaniciAra');
    // $scope.KullaniciModelAraSonuc

    // arama sonucu listesinden de sil
    var ix = -1;
    for (var i = 0; i < $scope.KullaniciModelAraSonuc.length; i++)
    {
        if ($scope.KullaniciModelAraSonuc[i].KullaniciKey == Key)
        {
            ix = i;
            break;
        }
    }
    if (ix > -1)
        $scope.KullaniciModelAraSonuc.splice(ix, 1);
    $scope.$apply();

    ModalPopUpKapat();
};

var KayitGuncellendi = function (Key, Model)
{
    // debugger;
    var $scope = GetScope('ngConKullaniciAra');

    // arama listesinide güncelle
    var ix = -1;
    for (var i = 0; i < $scope.KullaniciModelAraSonuc.length; i++)
    {
        if ($scope.KullaniciModelAraSonuc[i].KullaniciKey == Key)
        {
            ix = i;
            break;
        }
    }
    if (ix > -1)
    {
        // debugger;
        $scope.KullaniciModelAraSonuc[ix].KullaniciAd = Model.KullaniciAd;
        $scope.KullaniciModelAraSonuc[ix].Ad = Model.Ad;
        $scope.KullaniciModelAraSonuc[ix].Soyad = Model.Soyad;
        $scope.KullaniciModelAraSonuc[ix].KullaniciTipNo = Model.KullaniciTipNo;
        $scope.KullaniciModelAraSonuc[ix].AktifPasifTipNo = Model.AktifPasifTipNo;
    }
    $scope.$apply();

    ModalPopUpKapat();
};


var ModalPopUpKapat = function (data)
{
    $('#argemModalPopUp').modal('toggle');
};

var KullaniciGuncelle = function ()
{
    if (confirm('değişiklikler kaydedilsin mi'))
    {
        $scope = angular.element(document.getElementById('divKullaniciKayit')).scope();
        // alert($scope);
        alert(angular);
        angular.KullaniciAramaService.KullaniciKaydet($scope, 0, function (data)
        {
            alert(data);
        })
        return true;
    }
    else
        return false;
};




function getIframeWindow(iframe_object)
{
    var doc;

    if (iframe_object.contentWindow)
    {
        return iframe_object.contentWindow;
    }

    if (iframe_object.window)
    {
        return iframe_object.window;
    }

    if (!doc && iframe_object.contentDocument)
    {
        doc = iframe_object.contentDocument;
    }

    if (!doc && iframe_object.document)
    {
        doc = iframe_object.document;
    }

    if (doc && doc.defaultView)
    {
        return doc.defaultView;
    }

    if (doc && doc.parentWindow)
    {
        return doc.parentWindow;
    }

    return undefined;
}




//window.ModalPopUpKapat = function ()
//{
//    $('#argemModalPopUp').modal('hide');
//};

$(function ()
{
    //    $(".anchorOku").click(function ()
    //    {
    //        // debugger;
    //        var $buttonClicked = $(this);
    //        var SayfaAd = $buttonClicked.attr('data-sayfaad');
    //        $('#argemModalLabel').html(SayfaAd);
    //        var id = $buttonClicked.attr('data-id');
    //        // var id = "0";
    //        var Durum = $buttonClicked.attr('data-durum');
    //        var options = { "backdrop": "static", keyboard: true };

    //        var $ifmp = $('._armp_Iframe');
    //        $ifmp.attr('src', URLOku + '?Durum=' + Durum + '&Key=' + id);

    //        $('#argemModalPopUp').modal(options);
    //        $('#argemModalPopUp').modal('show');


    //        /*// debugger;
    //        var $buttonClicked = $(this);
    //        var SayfaAd = $buttonClicked.attr('data-sayfaad');
    //        $('#argemModalLabel').html(SayfaAd);
    //        var id = $buttonClicked.attr('data-id');
    //        var Durum = $buttonClicked.attr('data-durum');
    //        var options = { "backdrop": "static", keyboard: true };
    //        $.ajax({
    //            type: "GET",
    //            url: URLOku,
    //            contentType: "application/json; charset=utf-8",
    //            data: { "Durum": Durum, "Key": id },
    //            datatype: "json",
    //            cache: false,
    //            success: function (data)
    //            {
    //                // debugger;
    //                $('#argemModalContent').html(data);
    //                // alert($('#myModalContent').html());
    //                $('#argemModalPopUp').modal(options);
    //                $('#argemModalPopUp').modal('show');

    //            },
    //            error: function (request, status, error)
    //            {
    //                alert('Sistem Hatası:\n' + request.responseText);
    //            }
    //        });*/
    //    });

    //    $(".anchorGuncelle").click(function ()
    //    {
    //        alert(1);
    //        // debugger;
    //        var $buttonClicked = $(this);
    //        var SayfaAd = $buttonClicked.attr('data-sayfaad');
    //        $('#argemModalLabel').html(SayfaAd);
    //        var id = $buttonClicked.attr('data-id');
    //        // var id = "0";
    //        var Durum = $buttonClicked.attr('data-durum');
    //        var options = { "backdrop": "static", keyboard: true };

    //        var $ifmp = $('._armp_Iframe');
    //        $ifmp.attr('src', URLGuncelle + '?Durum=' + Durum + '&Key=' + id);

    //        $('#argemModalPopUp').modal(options);
    //        $('#argemModalPopUp').modal('show');

    //        /*// debugger;
    //        var $buttonClicked = $(this);
    //        var SayfaAd = $buttonClicked.attr('data-sayfaad');
    //        $('#argemModalLabel').html(SayfaAd);
    //        var id = $buttonClicked.attr('data-id');
    //        var Durum = $buttonClicked.attr('data-durum');
    //        var options = { "backdrop": "static", keyboard: true };
    //        $.ajax({
    //            type: "GET",
    //            url: URLGuncelle,
    //            contentType: "application/json; charset=utf-8",
    //            data: { "Durum": Durum, "Key": id },
    //            datatype: "json",
    //            success: function (data)
    //            {
    //                // debugger;
    //                $('#argemModalContent').html(data);
    //                // alert($('#myModalContent').html());
    //                $('#argemModalPopUp').modal(options);
    //                $('#argemModalPopUp').modal('show');

    //            },
    //            error: function (request, status, error)
    //            {
    //                alert('Sistem Hatası:\n' + request.responseText);
    //            }
    //        });*/
    //    });

    //$(".anchorYeniKayit").click(function ()
    //{
    //    //// debugger;
    //    var $buttonClicked = $(this);
    //    //var SayfaAd = $buttonClicked.attr('data-sayfaad');
    //    //$('#argemModalLabel').html(SayfaAd);
    //    var id = "0";
    //    var Durum = $buttonClicked.attr('data-durum');
        
    //    //var $ifmp = $('._armp_Iframe');
    //    // $ifmp.attr('src', URLGuncelle + '?Durum=' + Durum + '&Key=' + id);

    //    var options = { "backdrop": "static", keyboard: true };
    //    // $('#argemModalPopUp2').modal(options);
    //    // $('#argemModalPopUp2').modal('show');

    //    // return;
    //    $.ajax({
    //        type: "GET",
    //        url: URLGuncelle,
    //        contentType: "application/json; charset=utf-8",
    //        data: { "Durum": Durum, "Key": id },
    //        datatype: "json",
    //        success: function (data)
    //        {
    //            // debugger;
    //            $('#argemModalContent2').html(data);
    //            $('#argemModalPopUp2').modal(options);
    //            // $('#argemModalPopUp2').modal('show');
    //        },
    //        error: function (request, status, error)
    //        {
    //            alert('Sistem Hatası:\n' + request.responseText);
    //        }
    //    });
    //});

    /*$("#closbtn").click(function ()
    {
        $('#myModal').modal('hide');
    });*/
});


//function fnIframeBoyutAyarla()
//{
//    var cssMpIF = '_armp_Iframe';
//    // var cssMp = '_armp_Iframe';

//    var $mp = $('#argemModalPopUp');
//    var $ifmp = $('.' + cssMpIF);

//    // var $ifmp = $mp.find('.' + cssIframe);
//    $ifmp
//        .css({ 'height': ($ifmp.parent().height() - $mp.find('.modal-header').outerHeight()) });
//}

