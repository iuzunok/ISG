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

    ArgemModalPopUpKapat();
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

    ArgemModalPopUpKapat();
};


var ArgemModalPopUpKapat = function (data)
{
    oArgemModal.close();
    // $('#argemModalPopUp').modal('toggle');
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







var oArgemModal;
var ArgemModal = (function ()
{
    var method = {},
        $overlay,
        $modal,
        $content;
    // $close;

    // Center the modal in the viewport
    method.center = function ()
    {
        var top, left;
        top = Math.max($(window).height() - $modal.outerHeight(), 0) / 2;
        left = Math.max($(window).width() - $modal.outerWidth(), 0) / 2;
        $modal.css({
            top: top + $(window).scrollTop(),
            left: left + $(window).scrollLeft()
        });
    };

    // Open the modal
    method.open = function (settings)
    {
        // debugger;
        // $content.empty().append(settings.content);
        $modal.css({ width: settings.width || 'auto', height: settings.height || 'auto' });
        method.center();
        $(window).bind('resize.modal', method.center);
        $modal.show();
        $overlay.show();
        return this;
    };

    // Close the modal
    method.close = function ()
    {
        // debugger;
        $modal.hide();
        $overlay.hide();
        // $content.empty();
        $content.find('#arifPopUp').attr('src', '');
        $(window).unbind('resize.modal');
    };

    // debugger;
    // Generate the HTML and add it to the document
    $overlay = $('<div id="ArgemModalOverlay"></div>');
    $modal = $('<div id="ArgemModal"></div>');
    $content = $('<div id="ArgemModalContent"><iframe id="arifPopUp" src="" frameborder="0" scrolling="no"></iframe></div>');
    // $close = $('<a id="close" href="#">close</a>');

    $modal.hide();
    $overlay.hide();
    // $modal.append($content, $close);
    $modal.append($content);

    $(document).ready(function ()
    {
        $('body').append($overlay, $modal);
    });

    /*$close.click(function (e)
    {
        e.preventDefault();
        method.close();
    });*/

    return method;
}());

/*
// Wait until the DOM has loaded before querying the document
$(document).ready(function ()
{

    $.get('ajax.html', function (data)
    {
        modal.open({ content: data });
    });

    $('a#howdy').click(function (e)
    {
        modal.open({ content: "Hows it going?" });
        e.preventDefault();
    });
});*/