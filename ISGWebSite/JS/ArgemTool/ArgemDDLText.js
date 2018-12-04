/// <reference path="ArgemJSUtil.js" />

// ArgemDDLText.js

(function (window, $, Util, undefined)
{
    var cssContainer = 'ardt_container',
        cssLi = 'ardt_list_item',
        cssLiSelected = 'ardt_list_item_selected',
        cssLiHover = 'ardt_list_item_hover',
        cssUl = 'ardt_ul',
        cssMessage = 'ardt_mesaj',
        cssPopup = 'ardt_popup',
        cssInput = 'ardt_input',
        cssVeriGetir = 'ardt_get_data',
        cssTemizle = 'ardt_clear_data',
        cssBg = 'ardt_background',
        cssBgGrad = 'ardt_bg_gradient',
        DDLTextUtil = {};

    /*private function START*/

    function mesajYaz(ddlText, strMesaj)
    {
        // debugger;
        var $ddltext = $(ddlText).parents('.' + cssContainer);
        var $popup = $('#' + ddlText.id + '_popup');

        $popup.find('.' + cssUl).hide();
        $popup.find('.' + cssMessage).html(strMesaj).show();
        $popup
            .css({
                'width': $ddltext.width(),
                'top': ($ddltext.offset().top + $ddltext.outerHeight() + 2),
                'left': $ddltext.offset().left,
                'overflow': 'auto'
            })
            .slideDown('fast');
    }

    function kontrol(ddlText)
    {
        var result = true;
        if (ddlText.Required)
        {
            if (ddlText.value === '' || $('#' + ddlText.HdnAlanId).val() === '')
            {
                result = false;
            }
        }
        return result;
    }

    function listeGizle(ddlText)
    {
        if (ddlText === undefined)
            $('.' + cssPopup).hide('fast');
        else
            $('.' + cssPopup).not('#' + ddlText.id + '_popup').hide('fast');
    }

    function scrollAyarla($popup)
    {
        var $secilili = $popup.find('.' + cssLiSelected);
        if ($secilili.length > 0)
        {
            if ($popup.offset().top > $secilili.offset().top)
            {
                $popup.scrollTop($popup.scrollTop() + $secilili.offset().top - $popup.offset().top);
            }
            else if ($secilili.offset().top + $secilili.outerHeight() > $popup.offset().top + $popup.innerHeight())
            {
                $popup.scrollTop($popup.scrollTop() + $secilili.offset().top + $secilili.outerHeight() - $popup.offset().top - $popup.innerHeight());
            }
        }
    }

    function listeGoster(ddlText)
    {
        var popupID = '#' + ddlText.id + '_popup',
            $popup = $(popupID),
            divargemddltext = $(ddlText).parents('.' + cssContainer);

        $('.' + cssPopup).not(popupID).hide('fast');
        $popup.find('.' + cssMessage).hide();
        $popup.find('.' + cssUl).show();

        $popup
            .css({
                'width': divargemddltext.width(),
                'top': divargemddltext.offset().top + divargemddltext.outerHeight() + 2,
                'left': divargemddltext.offset().left
            })
            .slideDown('fast', function ()
            {
                scrollAyarla($popup);
            });
    }

    function tumunuDegistir(pstrVeri, pstrNeyi, pstrNeyle)
    {
        pstrVeri = pstrVeri.replace(pstrNeyi, pstrNeyle);
        while (pstrVeri.indexOf(pstrNeyi) > -1)
        {
            pstrVeri = pstrVeri.replace(pstrNeyi, pstrNeyle);
        }
        pstrVeri = pstrVeri.replace(pstrNeyi, pstrNeyle);
        return pstrVeri;
    }

    function veriUpCase(pstrVeri)
    {
        return tumunuDegistir(tumunuDegistir(pstrVeri, "i", "İ"), "ı", "I").toUpperCase();
    }

    function filterdata(ddlText)
    {
        var upper = veriUpCase(ddlText.value),
            show = false,
            $ul = $('#' + ddlText.id + '_popup .' + cssUl);

        $ul.children().each(function ()
        {
            if (veriUpCase($(this).text()).indexOf(upper) === -1)
            {
                $(this).removeClass(cssLiSelected).hide();
            }
            else
            {
                $(this).show();
                show = true;
            }
        });


        if (show)
        {
            $ul.show();
        }
    }

    function veriIsaretle($ul, Durum)
    {
        var CSSClass = cssLiSelected,// + ' ' + cssLiSelected + $ul.data('Theme'),
            $secilili = $ul.children('.' + cssLiSelected).removeClass(CSSClass);

        // if ($secilili.size() === 0)
        if ($secilili.length == 0)
        {
            $secilili = $ul.children(':visible:first').addClass(CSSClass);
        }
        else
        {
            if (Durum === 'DOWN')
            {
                if ($secilili.nextAll(':visible').length > 0)
                {
                    $secilili.nextAll(':visible:first').addClass(CSSClass);
                }
                else
                {
                    $ul.children(':visible:first').addClass(CSSClass);
                }
            }
            else if (Durum === 'UP')
            {
                if ($secilili.prevAll(':visible').length > 0)
                {
                    $secilili.prevAll(':visible:first').addClass(CSSClass);
                }
                else
                {
                    $ul.children(':visible:last').addClass(CSSClass);
                }
            }
        }

        scrollAyarla($ul.parent());
    }

    function ozelDDLOnClick(ddlText, secilenli)
    {
        var CSSClass = cssLiSelected;//  + ' ' + cssLiSelected + ddlText.Theme;
        ddlText.value = $(secilenli).html();

        // debugger;
        var Key = $(secilenli).data('deger');
        $("#" + ddlText.HdnAlanId).val(Key);
        // ddlText.ScopeAd.Key = Key;
        // alert(ddlText.HdnAlanId + ' : ' + $("#" + ddlText.HdnAlanId).val() + ' : ' + Key);

        $(secilenli).addClass(CSSClass).siblings().removeClass(CSSClass);
        Util.ZorunluCssAyarla(ddlText, kontrol(ddlText));
        ddlText.focus();

        if (ddlText.OnClientSelect !== undefined)
        {
            Util.myEval(ddlText.OnClientSelect);
        }
    }

    function veriGetir(ddlText)
    {
        // debugger;
        var jsUtil = Util.CI.DDLText;
        // Theme = ddlText.Theme;

        ddlText.focus();
        $.ajax({
            type: "POST",
            // url: ddlText.WebServiceURL + '/' + ddlText.WebServiceWebMethodAd,
            // data: '{"' + ddlText.ParametreAd + '":"' + ddlText.value + '"' + ddlText.EkPapametere + '}',
            url: ddlText.ControlName,
            data: '{"' + ddlText.ParametreAd + '":"' + ddlText.value + '"' + ddlText.EkPapametere + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function ()
            {
                mesajYaz(ddlText, jsUtil.WaitMessage);
            },
            success: function (data)
            {
                // debugger;
                var $popup = $('#' + ddlText.id + '_popup'),
                    secilideger = $('#' + ddlText.HdnAlanId).val(),
                    $ul = $popup.find('.' + cssUl),
                    liCSS = cssBgGrad + ' ' + cssLi,// + ' ' + cssLi + Theme,
                    liSelectedCSS = cssLiSelected,// + ' ' + cssLiSelected + Theme,
                    // jsonVeri = (typeof odata.d) === 'string' ? $.parseJSON(odata.d) : odata.d,
                    jsonVeri = data,
                    i;

                $('#' + ddlText.HdnAlanId).val('');
                $ul.children().remove();

                /*if (jsonVeri.TOKENS != null && jsonVeri.TOKENS != '')
                    alert(jsonVeri.TOKENS);*/
                if (jsonVeri.Durum != '')
                    // alert(jsonVeri.Aciklama);
                    mesajYaz(ddlText, jsUtil.NoRecordFound);
                else
                {
                    for (i = 0; i < jsonVeri.Data.length; i += 1)
                    {
                        $ul.append(
                            $('<li>' + jsonVeri.Data[i].Text + '</li>')
                                .addClass(liCSS)
                                .addClass((secilideger !== '' && jsonVeri.Data[i].Key === secilideger) ? liSelectedCSS : '')
                                .data({ 'deger': jsonVeri.Data[i].Key })
                                .on('mouseover mouseleave', function (event)
                                {
                                    // debugger;
                                    var cssclass = cssLiHover;// + ' ' + cssLiHover + $(this).parent().data('Theme');
                                    if (event.type === 'mouseover')
                                    {
                                        $(this).addClass(cssclass);
                                    }
                                    else
                                    {
                                        $(this).removeClass(cssclass);
                                    }
                                })
                        );
                    }
                }

                $ul.children().click(function (e)
                {
                    if ($(this).is('li.' + cssLi))
                    {
                        e.preventDefault();
                        ozelDDLOnClick(ddlText, this);
                    }
                });

                // if ($ul.children().size() === 0)
                if ($ul.children().length == 0)
                {
                    mesajYaz(ddlText, jsUtil.NoRecordFound);
                }
                else
                {
                    listeGoster(ddlText);
                }
            },
            error: function (result)
            {
                alert(result.status + ' ' + result.statusText + '\n\r' + result.responseText);
            }
        });
    }

    function stringFormat(str)
    {
        var i;
        for (i = 1; i < arguments.length; i += 1)
        {
            str = str.replace('{' + (i - 1) + '}', arguments[i]);
        }
        return str;
    }

    /*private function END*/

    /*public functions START*/

    DDLTextUtil.TextKontrol = function (source, args)
    {
        var argemDDLText = $('#' + source.controltovalidate).get(0);
        args.IsValid = kontrol(argemDDLText);

        Util.ZorunluCssAyarla(argemDDLText, args.IsValid);
    };

    DDLTextUtil.Clear = function (ddlTextID)
    {
        var ddlText = $('#' + ddlTextID).get(0);
        $("#" + ddlText.HdnAlanId).val('');
        $('#' + ddlTextID + ">ul>li").remove();
        $(ddlText).val('').focus();
        listeGizle();
    };

    DDLTextUtil.Enable = function (ddlTextId)
    {
    }

    DDLTextUtil.Disable = function (ddlTextId)
    {
    }

    DDLTextUtil.Init = function ()
    {
        // debugger;
        var jsUtil = Util.CI.DDLText,
            keyCodes = Util.KeyCodes;

        $(document).bind('click.argemddltext', function (e)
        {
            if ($(e.target).closest('.' + cssContainer).length === 0)
            {
                listeGizle();
            }
            else if ($(e.target).hasClass(cssInput))
            {
                listeGizle($(e.target));
            }
        });

        $('.' + cssInput)
            .attr('autocomplete', 'off')
            .each(function ()
            {
                var ddlText = this,
                    MinCharAdet = ddlText.MinCharAdet,
                    // Theme = ddlText.Theme,
                    YetersizKarakter = jsUtil.YetersizKarakter,
                    // popupCSS = cssPopup,// + ' ' + cssPopup + Theme,
                    mesajCSS = cssMessage + ' ' + cssBgGrad,// + ' ' + cssMessage + Theme,
                    ulCSS = cssUl,// + ' ' + cssUl + Theme,
                    $popup = $('<div id="' + ddlText.id + '_popup" class="' + cssPopup + '">' +
                        '<div class="' + mesajCSS + '"></div>' +
                        '<ul class="' + ulCSS + '"></ul>' +
                        '</div > ');

                //$popup.children('.' + cssUl).data({ 'Theme': Theme });
                $('body').append($popup);
                $(ddlText).addClass(cssInput);

                $('#' + ddlText.id + '_VeriTemizle')
                    // .addClass(cssTemizle + Theme + ' ' + cssBg + Theme)
                    .addClass(cssTemizle + ' ' + cssBg)
                    .attr("title", jsUtil.Temizle)
                    .click(function ()
                    {
                        ddlText.value = '';
                        $('#' + ddlText.HdnAlanId).val('');
                        // $(this).parent().parent().find('#');
                        listeGizle();
                    });

                $('#' + ddlText.id + '_VeriGetir')
                    .click(function ()
                    {
                        if (MinCharAdet > ddlText.value.length)
                        {
                            mesajYaz(ddlText, stringFormat(YetersizKarakter, MinCharAdet));
                        }
                        else
                        {
                            //show popup
                            var $popup = $('#' + ddlText.id + '_popup');
                            // if ($popup.is(':visible') || $popup.find('.' + cssLi).size() < 1)
                            if ($popup.is(':visible') || $popup.find('.' + cssLi).length < 1)
                            {
                                veriGetir(ddlText);
                            }
                            else
                            {
                                listeGoster(ddlText);
                            }

                            ddlText.focus();
                        }
                    })
                    // .addClass(cssVeriGetir + Theme + ' ' + cssBg + Theme)
                    .addClass(cssVeriGetir + ' ' + cssBg)
                    .attr('title', jsUtil.VeriGetir);
            })
            .keydown(function (e)
            {
                var ddltext = this,
                    $popup = $('#' + ddltext.id + '_popup');

                switch (e.keyCode)
                {
                    // liste açıkken ENTER a basılınca submit yapma.                                                                                                                                                                                                                                                                                                                                                                           
                    case keyCodes.ENTER:
                        if ($popup.is(':visible'))
                        {
                            return false;
                        }
                        break;
                    case keyCodes.TAB:
                        if ($popup.is(':visible'))
                        {
                            listeGizle();
                        }
                        break;
                    case keyCodes.ESC:
                        listeGizle();
                        break;
                    default:
                }
            })
            .keyup(function (e)
            {
                var ddltext = this,
                    $popup = $('#' + ddltext.id + '_popup'),
                    MinCharAdet = ddltext.MinCharAdet,
                    YetersizKarakter = jsUtil.YetersizKarakter,
                    $ul = $popup.find('.' + cssUl),
                    $secilili = $ul.children('.' + cssLiSelected);

                switch (e.keyCode)
                {
                    case keyCodes.UP:
                        e.preventDefault();
                        if ($popup.is(':visible'))
                        {
                            veriIsaretle($ul, 'UP');
                        }
                        else
                        {
                            listeGoster(ddltext);
                            if ($ul.children().length === 0)
                            {
                                if (MinCharAdet > ddltext.value.length)
                                {
                                    $ul.children().remove();
                                    mesajYaz(ddltext, stringFormat(YetersizKarakter, MinCharAdet));
                                }
                                else
                                {
                                    veriGetir(ddltext);
                                }
                            }
                        }
                        break;
                    case keyCodes.DOWN:
                        e.preventDefault();
                        if ($popup.is(':visible'))
                        {
                            veriIsaretle($ul, 'DOWN');
                        }
                        else
                        {
                            listeGoster(ddltext);
                            if ($ul.children().length === 0)
                            {
                                if (MinCharAdet > ddltext.value.length)
                                {
                                    $ul.children().remove();
                                    mesajYaz(ddltext, stringFormat(YetersizKarakter, MinCharAdet));
                                }
                                else
                                {
                                    veriGetir(ddltext);
                                }
                            }
                        }
                        break;
                    case keyCodes.HOME:
                        break;
                    case keyCodes.END:
                        break;
                    case keyCodes.TAB:
                        break;
                    case keyCodes.LEFT:
                        break;
                    case keyCodes.RIGHT:
                        break;
                    case keyCodes.ESC:
                        e.preventDefault();
                        listeGizle();
                        return false;
                    case keyCodes.ENTER:
                        if ($popup.is(':visible'))
                        {
                            if ($secilili.length)
                            {
                                e.preventDefault();
                                ozelDDLOnClick(ddltext, $secilili);
                                listeGizle();
                            }
                            else
                            {
                                listeGoster(ddltext);
                            }
                            return false;
                        }
                        break;
                    default:
                        if (MinCharAdet > ddltext.value.length)
                        {
                            $('#' + ddltext.HdnAlanId).val('');
                            $ul.children().remove();
                            mesajYaz(ddltext, stringFormat(YetersizKarakter, MinCharAdet));
                        }
                        else
                        {
                            if ($ul.children().length > 0)
                            {
                                filterdata(ddltext);

                                if ($ul.children(':visible').length === 0)
                                {
                                    veriGetir(ddltext);
                                    //mesajYaz(ddltext, jsUtil.NoRecordFound);
                                }
                                else
                                {
                                    listeGoster(ddltext);
                                }
                            }
                            else
                            {
                                veriGetir(ddltext);
                            }
                        }
                }
            });


        //alert($('.' + cssLi).length);
        //$('.' + cssLi).on('mouseover mouseleave', function (event)
        //{
        //    debugger;
        //    var cssclass = cssLiHover;// + ' ' + cssLiHover + $(this).parent().data('Theme');
        //    if (event.type === 'mouseover')
        //    {
        //        $(this).addClass(cssclass);
        //    }
        //    else
        //    {
        //        $(this).removeClass(cssclass);
        //    }
        //});

    };

    /*public functions END*/

    window.ArgemDDLTextUtil = DDLTextUtil;

}(window, jQuery, ArgemJSUtil));