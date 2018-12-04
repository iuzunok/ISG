/// <reference path="ArgemJSUtil.js" />
// ArgemPopUp.js

(function ($, window, Util)
{
    var
        // $ = jQuery, Util = ArgemJSUtil, //Bu satır sadece Intellisense çalışsın diye var deploydan önce comment edilsin.
        cssMp = 'armp',
        cssTitle = 'armp_title',
        cssTitleGradient = 'armp_title_gradient',
        // cssOverlay = 'armp_overlay',
        cssOverlay = 'ArkaPlanGizleGoster',
        cssClose = 'armp_buton-close',
        cssMax = 'armp_buton_max',
        cssRestore = 'armp_buton_restore',
        cssYenile = 'armp_buton_yenile',
        // cssBackground = 'armp_background',
        // cssButon = 'armp_buton',
        MPNameSpace = '.argem.mp', //namespace for event bindings
        cssIframeBg = '_armp_layer',
        cssIframe = '_armp_Iframe',
        cssConstraint = '_armp_constraint',
        BrowserTip = ArgemJSUtil.BrowserVersiyonBul(),
        $scope = null,
        ArgemMPUtil = {};

    function fnOrtala($mp)
    {
        //Modal popup penceresini yatay ve dikey ortala
        var width = $(window).width(),
            height = $(window).height();

        //Modal Pop up maximum olarak ayarladı ise resize sırasında yine maximumda kalsın.
        if ($mp.data('Max'))
        {
            $mp
                .css({
                    'width': width - 30,
                    'height': height - 30
                });
        }

        $mp.css({
            'position': 'absolute',
            'top': (Math.max(0, height - $mp.outerHeight())) / 2 + $(window).scrollTop(),
            'left': (Math.max(0, width - $mp.outerWidth())) / 2 + $(window).scrollLeft()
        });
    }

    function fnIframeBoyutAyarla($mp)
    {
        var $ifmp = $mp.find('.' + cssIframe);
        $ifmp.css({
            'width': $mp.width(),
            'height': ($ifmp.parent().height() - $mp.find('.' + cssTitle).outerHeight()),
            'top': 0,
            'left': 0
        });

        // if ($.browser.msie)
        if (BrowserTip.name == 'IE')
        {
            // if ($.browser.version == '8.0' || $.browser.version == '9.0')
            if (BrowserTip.version == '8.0' || BrowserTip.version == '9.0')
                $ifmp.css({ 'top': 25 });
        }
    }

    function fnBoyutAyarla($mp)
    {
        var $ifmp = $mp.find('.' + cssIframe),
            $window = $(window),
            $ifwin,
            $ifdoc,
            baslikYukseklik,
            yukseklik,
            genislik;

        var Genislik = $mp.width();
        var Yukseklik = $mp.height();
        // alert(Genislik + ' ' + Yukseklik + ' ' + $ifmp.width() + ' ' + $ifmp.height());
        $ifmp.width(Genislik).height(Yukseklik - 30);
        // alert(Genislik + ' ' + Yukseklik + ' ' + $ifmp.width() + ' ' + $ifmp.height());

        // fnIframeBoyutAyarla($mp);

        //if ($mp.data('OtomatikBoyutla'))
        //{
        //    $mp.data('OtomatikBoyutla', false);
        //    try
        //    {
        //        $ifwin = $($ifmp[0].contentWindow);
        //        $ifdoc = $($ifmp[0].contentWindow.document);
        //        genislik = Math.min($window.width(), Math.max($ifwin.width(), $ifdoc.width() + 25));

        //        if ($mp.width() < genislik)
        //        {
        //            $mp
        //                .css({
        //                    'width': genislik
        //                });
        //        }

        //        baslikYukseklik = $mp.find('.' + cssTitle).outerHeight();
        //        yukseklik = $ifdoc.height() + baslikYukseklik;

        //        if (yukseklik > $window.height())
        //            yukseklik = $window.height();

        //        //to do: Iframe'in yüksekliğini her zaman ayarla
        //        $ifmp
        //            .css({
        //                'height': yukseklik - baslikYukseklik,
        //                'top': 0,
        //                'left': 0
        //            });

        //        $mp
        //            .css({
        //                'height': yukseklik
        //            });
        //    }
        //    catch (ex)
        //    {
        //        //Handle errors here
        //    }

        //    fnOrtala($mp);
        //}

        if ($mp.data('Ortala'))
        {
            $mp.data('Ortala', false);
            fnOrtala($mp);
        }
    }

    ArgemMPUtil.Max = function ()
    {
        var $mp = $('.' + cssMp);
        $mp.find('.' + cssMax).hide().end()
            .find('.' + cssRestore).css({ 'display': 'inline-block' }).end()
            .data({
                'RestoreGenislik': $mp.width(),
                'RestoreYukseklik': $mp.height(),
                'Max': true
            })
            .css({
                'width': $(window).width() - 40,
                'height': $(window).height() - 40
            })
            .resizable({
                disabled: true
            })
            .draggable({
                disabled: true
            });

        fnOrtala($mp);
        fnIframeBoyutAyarla($mp);
    };

    ArgemMPUtil.Yenile = function ()
    {
        var $mp = $('.' + cssMp);
        var $ifmp = $mp.find('.' + cssIframe);
        $ifmp.attr('src', $ifmp.attr('src'));
    };

    ArgemMPUtil.Restore = function ()
    {
        var $mp = $('.' + cssMp);
        $mp.data('Max', false)
            .css({
                'width': $mp.data('RestoreGenislik'),
                'height': $mp.data('RestoreYukseklik')
            })
            .draggable({
                'disabled': false
            })
            .resizable({
                'disabled': false
            })
            .find('.' + cssMax).show()
            .end()
            .find('.' + cssRestore)
            .hide();

        fnOrtala($mp);
        fnIframeBoyutAyarla($mp);
    };

    ArgemMPUtil.Close = function ()
    {
        // debugger;
        if ($scope != null)
        {
            $('.ArkaPlanProgress').show();
            $scope.$parent.ArkaPlaniAcikMi = false;
            $scope.$apply();
        }

        /*$('.' + cssOverlay).fadeOut(100, function ()
        {
            $(this).hide();
            $('.ArkaPlanProgress').show();
        });*/
        $('.' + cssConstraint).fadeOut(100, function ()
        {
            $(this).remove();
            $(window).unbind(MPNameSpace);
            $('input,radio,a,button,textarea,iframe,select').attr('tabindex', '1');
        });
    };

    ArgemMPUtil.PopUpAc = function ($pscope, SayfaURL, Genislik, Yukseklik, OtomatikBoyutla, Ortala, Baslik, OnOpenRunJSScript, OnCloseRunJSScript)
    {
        $scope = $pscope;
        if (OnOpenRunJSScript != '')
            eval(OnOpenRunJSScript);

        $('input,radio,a,button,textarea,iframe,select').attr('tabindex', '-1');

        var $window = $(window);
        // alert($window.width() + ' ' + $window.height() + ' ' + Genislik + ' ' + Yukseklik );
        if (Genislik > $window.width() - 10)
            Genislik = $window.width() - 10;
        if (Yukseklik > $window.height() - 10)
            Yukseklik = $window.height() - 10;
        // alert($window.width() + ' ' + $window.height() + ' ' + Genislik + ' ' + Yukseklik);

        var $iframelayer = $('.' + cssIframeBg),
            // Tema = Util.Theme,
            // bgClass = cssBackground,
            $parentdiv = $('.' + cssConstraint),
            // $parentdiv = $('.ArkaPlanGizleGoster'),
            $overlay = $('.' + cssOverlay),
            $mp,
            $ifmp,
            z = 100000,
            // browserversion = parseInt($.browser.version, 0),
            // browserversion = parseInt(Browser.version, 0),
            mode = window.document.documentMode || 0,
            // setExpr = $.browser.msie && ((browserversion < 8 && !mode) || mode < 8),
            setExpr = BrowserTip.name == 'IE' && ((BrowserTip.version < 8 && !mode) || mode < 8),
            // ie6 = $.browser.msie && (browserversion === 6) && !mode,
            ie6 = BrowserTip.name == 'IE' && (BrowserTip.version === 6) && !mode,
            // IE issues: 'about:blank' fails on HTTPS and javascript:false is s-l-o-w
            iframeSrc = /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank',
            expr = setExpr && (!$.boxModel || ($('object,embed').length > 0));

        //// debugger;
        //// if ($overlay.size() < 1)
        //if ($overlay.length == 0)
        //{
        //    // if ($.browser.msie)
        //    //if (Browser.name == 'IE')
        //    //{
        //    //    $iframelayer = $('<iframe class=' + cssIframeBg + ' style="z-index:' + z + ';display:none;border:none;margin:0;padding:0;position:absolute;width:100%;height:100%;top:0;left:0" src="' + iframeSrc + '"></iframe>');
        //    //    $iframelayer.css('opacity', 0.0);
        //    //}
        //    //else
        //    //{
        //    $iframelayer = $('<div class="' + cssIframeBg + '" style="display:none"></div>');
        //    //}

        //    $overlay = $('<div class="' + cssOverlay + ' ' + cssOverlay  + '" style="z-index:' + (z += 1) + ';cursor:wait;display:none;border:none;margin:0;padding:0;width:100%;height:100%;top:0;left:0;position:fixed;"></div>');
        //    $overlay.css({ 'opacity': 0.6 });

        //    // ie7 must use absolute positioning in quirks mode and to account for activex issues (when scrolling)
        //    if (ie6 || expr)
        //    {
        //        // give body 100% height
        //        if ($.boxModel)
        //        {
        //            $('html,body').css('height', '100%');
        //        }

        //        $($iframelayer, $overlay).css('position', 'absolute');
        //    }

        //    $('body').append($iframelayer).append($overlay);
        //}

        $overlay.css({ 'display': 'block' })
            .fadeTo(0, 0)
            .fadeTo('fast', 0.3);

        //IFrame'i Yükle
        // if ($parentdiv.size() < 1)
        if ($parentdiv.length == 0)
        {
            // debugger;
            $parentdiv = $(
                '<div class="' + cssConstraint + ' ArgemModalOverlay" style="z-index:' + (z += 1) + ';position:absolute;left:0;top:0;margin:0px;padding:0px;width:100%;height:100%">' +
                '   <div class="' + cssMp + ' ' + cssTitleGradient + '" style="width:' + Genislik + 'px; height:' + Yukseklik + 'px; margin:0px; overflow:hidden; display:none;">' +
                '       <div style="background-color:#ffffff; width:100%; height:100%; margin:0px; padding:0px; border:none 0px Transparent">' +
                '           <div class="' + cssTitle + '" style="margin:0px;padding:0px;width:100%">' +
                '               <table cellpadding="0" cellspacing="0" style="width:100%;padding:0px;border-collapse:collapse;border:none 0px Transparent">' +
                '                   <tbody>' +
                '                   <tr>' +
                '                       <td style="text-align:left">' +
                '                           <em style="cursor:default;pointer:default;white-space:nowrap">&nbsp;' + Baslik + '</em>' +
                '                       </td>' +
                '                       <td style="text-align: right; white-space: nowrap">' +
                '                           <a class="' + cssYenile + '" title="' + Util.CI.Popup.Yenile + '"><span class="fa fa-refresh"></span></a>' +
                '                           <a class="' + cssRestore + '" title="' + Util.CI.Popup.Restore + '" style="display:none"><span class="fa fa-window-restore"></span></a>' +
                '                           <a class="' + cssMax + '" title="' + Util.CI.Popup.EkraniKapla + '" style="display:inline-block"><span class="fa fa-window-maximize"></span></a>' +
                '                           <a class="' + cssClose + '" title="' + Util.CI.Popup.Kapat + '" style="display:inline-block"><span class="fa fa-window-close"></span></a>' +
                '                       </td>' +
                '                   </tr>' +
                '                   </tbody>' +
                '               </table>' +
                '           </div>' +
                '           <iframe class="' + cssIframe + '" style="position:relative; display:block; margin: 0px; padding: 0px; border: none 0; width: 100%; top:0px; left:0px" scrolling="0" frameborder="0" marginHeight="0px" marginWidth="0px" src="' + SayfaURL + '"></iframe>' +
                '       </div>' +
                '   </div>' +
                '</div>');

            // debugger;
            $parentdiv.appendTo('body');
            $mp = $parentdiv.children('.' + cssMp);
            $ifmp = $mp.find('.' + cssIframe);

            $(window).bind('resize' + MPNameSpace, function ()
            {
                $ifmp.css({ 'visibility': 'hidden' });
                fnOrtala($mp);
                $ifmp.css({ 'visibility': 'visible' });
                fnIframeBoyutAyarla($mp);
            });

            $mp.find('.' + cssRestore + ',.' + cssMax + ',.' + cssClose + ',.' + cssYenile).mousedown(function ()
            {
                return false;
            });

            $mp.find('.' + cssYenile).click(function ()
            {
                ArgemMPUtil.Yenile();
                return false;
            });

            $mp.find('.' + cssRestore).click(function ()
            {
                ArgemMPUtil.Restore();
                return false;
            });

            $mp.find('.' + cssMax).click(function ()
            {
                ArgemMPUtil.Max();
                return false;
            });

            $mp.find('.' + cssClose).click(function ()
            {
                if (OnCloseRunJSScript != '')
                {
                    eval(OnCloseRunJSScript);
                }

                ArgemMPUtil.Close();
                return false;
            });

            $mp.data({
                'OtomatikBoyutla': OtomatikBoyutla,
                'Ortala': Ortala
            })
                .draggable({
                    handle: $('.' + cssTitle).disableSelection(),
                    containment: 'parent',
                    opacity: 0.35,
                    start: function ()
                    {
                        $ifmp.hide();
                    },
                    stop: function ()
                    {
                        $ifmp.show();
                    }
                })
                .resizable({
                    'minHeight': 250,
                    'minWidth': 250,
                    'handles': 'all',
                    containment: 'parent',
                    start: function (event, ui)
                    {
                        $ifmp.css({ 'visibility': 'hidden' });
                    },
                    stop: function (event, ui)
                    {
                        $ifmp.css({ 'visibility': 'visible' });
                        fnIframeBoyutAyarla($mp);
                    }
                })
                .find('.' + cssTitle)
                .dblclick(function ()
                {
                    if ($mp.data('Max'))
                    {
                        ArgemMPUtil.Restore();
                    }
                    else
                    {
                        ArgemMPUtil.Max();
                    }
                });

            // debugger;
            // $ifmp.load(function ()
            $ifmp.on('load', function ()
            {
                if (!$mp.is(':visible'))
                {
                    $('.ArkaPlanProgress').hide();

                    ////Modalpopup'ı Göster.
                    $mp.show();
                    //$mp.fadeIn(600, function ()
                    //{
                    //    //Lütfen bekleyiniz resmini gizle.
                    //    $overlay.css({ 'background-image': 'none' });
                    //});
                }

                fnBoyutAyarla($mp);
            });
        }

        return this;
    };

    window.ArgemMPUtil = ArgemMPUtil;

}(jQuery, window, ArgemJSUtil));