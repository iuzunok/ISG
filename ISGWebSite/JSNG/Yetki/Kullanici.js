'use strict';
appGenel
    .controller('ngcGenel', function ()
    {
    })

    .controller('ngcKullaniciAra', function ($scope, $window, GenelService, KullaniciService)
    {
        $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent)
        {
            $scope.$parent.$parent.CCS = Math.round(performance.now() - $scope.BasTime);
            // sayfada başka gridler yoksa bursı olmalı aşağısı olmamalı
            $scope.$parent.ArkaPlaniAcikMi = false;
        });

        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.SayfaKayitAdet = 50;
        $scope.KullaniciModel = {
            // alan dolu gelmesi için
            // KullaniciAd: 'i',
            AktifPasifTipNo: 101,

            ToplamKayitAdet: 0,
            SayfaKayitAdet: 50,
            AktifSayfaNo: 1,
            SiraTip: false,
            SiraAlan: 'Ad,Soyad'
        };
        $scope.KullaniciModelSonuc = {};

        $scope.Sirala = function (SiralamaAlan)
        {
            // debugger;
            $scope.KullaniciModel.SiraAlan = SiralamaAlan;
            $scope.KullaniciModel.SiraTip = !$scope.KullaniciModel.SiraTip;

            // birden fazla sayfa var ise vt sıralı çek
            if ($scope.KullaniciModel.ToplamKayitAdet > $scope.KullaniciModel.SayfaKayitAdet)
                $scope.KullaniciAramaYap('O', 1); // order
            else // tek sayfa ise dom da sırala
            {
                $scope.KullaniciModelSonuc.sort(function (a, b)
                {
                    if ($scope.KullaniciModel.SiraTip)
                        return b[SiralamaAlan].localeCompare(a[SiralamaAlan]);
                    else
                        return a[SiralamaAlan].localeCompare(b[SiralamaAlan]);
                });
            }
        };

        $scope.SayfalamaDegisti = function ()
        {
            $scope.KullaniciAramaYap('P', $scope.KullaniciModel.AktifSayfaNo); // paging
        };

        $scope.SayfaKayitAdetDegisti = function ()
        {
            $scope.KullaniciModel.SayfaKayitAdet = $scope.SayfaKayitAdet;
            $scope.KullaniciAramaYap('R', 1); // page sayfa kayıt sayısı
        };

        $scope.KullaniciAramaYap = function (Durum, AktifSayfaNo)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            $scope.BasTime = performance.now();

            KullaniciService.SrcKullaniciAra($scope, function (data)
            {
                if (data != null)
                {
                    if (data.Durum == '')
                    {
                        $scope.HataVar = false;
                        $scope.HataAciklama = '';
                        $scope.$parent.SCS = data.SCS;

                        $scope.KullaniciModelSonuc = data.Data;
                        $scope.KullaniciModel.ToplamKayitAdet = data.ToplamKayitAdet;
                        $scope.KullaniciModel.AktifSayfaNo = AktifSayfaNo;
                        if (Durum == 'B' || Durum == 'R')
                            $scope.KullaniciModel.SiraAlan = '';
                    }
                    else if (data.Durum == 'E')
                    {
                        $scope.HataVar = true;
                        $scope.HataAciklama = data.Aciklama;
                        $('#divSysError').removeClass('alert-info').addClass('alert-danger');
                        $scope.KullaniciModelSonuc = null;
                        $scope.$parent.ArkaPlaniAcikMi = false;
                    }
                    else
                    {
                        $scope.HataVar = true;
                        $scope.HataAciklama = data.Aciklama;
                        $scope.KullaniciModelSonuc = null;
                        $scope.$parent.ArkaPlaniAcikMi = false;

                        $scope.$parent.SCS = data.SCS;
                        $scope.BitTime = performance.now();
                        $scope.$parent.CCS = Math.round($scope.BitTime - $scope.BasTime);
                    }
                }
                else
                {
                    $scope.HataVar = true;
                    $scope.HataAciklama = 'Sistem hatası. data=null';
                }

                // sayfada başka gridler olursa bursı olmalı yukarısı olmamalı
                // $scope.$parent.ArkaPlaniAcikMi = false;
            });
        };

        $scope.RCIn = function ()
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            // var $arifPopUp = $('#arifPopUp');
            // oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            // $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciKayit?Durum=I&Key=0').width(800).height(500);
            // alert(1);
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/Kullanici/KullaniciKayit?Durum=I&Key=0', 800, 500, false, true, 'Yeni Kullanıcı Kayıt', '', '');
            // alert(oArgemModal);
            // $scope.$parent.ArkaPlaniAcikMi = false;
        };

        $scope.RCGu = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            //var $arifPopUp = $('#arifPopUp');
            //oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            //$arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciKayit?Durum=U&Key=' + Key).width(800).height(500);
            //$scope.ArkaPlaniAcikMi = false;
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/Kullanici/KullaniciKayit?Durum=U&Key=' + Key, 800, 500, false, true, 'Kullanıcı Güncelle', '', '');
        };

        $scope.RCOk = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            //var $arifPopUp = $('#arifPopUp');
            //oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            //$arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + Key).width(800).height(500);
            //$scope.ArkaPlaniAcikMi = false;
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + Key, 800, 500, false, true, 'Kullanıcı Oku', '', '');
        };

        $scope.RCSi = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            //var $arifPopUp = $('#arifPopUp');
            //oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            //$arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciOku?Durum=D&Key=' + Key).width(800).height(500);
            //$scope.ArkaPlaniAcikMi = false;
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/Kullanici/KullaniciOku?Durum=D&Key=' + Key, 800, 500, false, true, 'Kullanıcı Sil', '', '');
        };

        $scope.RCSec = function (Key)
        {
            window.location.href = '/Yetki/Kullanici/Kullanici?Key=' + Key;
        };

        $scope.RCDet = function ($event, Key)
        {
            var $TagArti = $($event.currentTarget);
            var $KullaniciDetay = $('.TmpKullaniciDetay_' + Key);
            var Aktif = $TagArti.hasClass('fa-fcGr');
            if (Aktif)
            {
                $TagArti.removeClass('fa-minus-square fa-fcGr').addClass('fa-plus-square fa-fcTu');
                if ($KullaniciDetay != null)
                    $KullaniciDetay.parent().hide();
            }
            else
            {
                $TagArti.removeClass('fa-plus-square fa-fcTu').addClass('fa-minus-square fa-fcGr');

                if ($KullaniciDetay.parent().data('DataVar') == 1)
                    $KullaniciDetay.parent().show();
                else
                {
                    var ArkaRenk = 'AramaSonucTek'
                    if ($TagArti.parent().parent().hasClass('AramaSonucCift'))
                        ArkaRenk = 'AramaSonucCift';

                    $TagArti.parent().parent().after('<tr class=' + ArkaRenk + '><td><span class="fa fa-refresh fa-spin fa-fs22 fa-fcTu"></td>' +
                        '<td>' +
                        '<span class="fa fa-pencil-square-o fa-fs22 fa-fcTu"></span>' +
                        '<span class="fa fa-trash fa-fs22 fa-fcTu"></span>' +
                        '</td>' +
                        '<td colspan=6 class="TmpKullaniciDetay_' + Key + '" ></td></tr>');

                    $.get('/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + Key, function (data)
                    {
                        $scope.HataVar = false;
                        $scope.$apply();
                        var $KullaniciDetay = $('.TmpKullaniciDetay_' + Key);
                        $KullaniciDetay.html(data);
                        $KullaniciDetay.parent().data('DataVar', 1);
                        $KullaniciDetay.prev().prev().find('span').removeClass();
                    })
                        .fail(function (jqXHR, textStatus, errorThrown)
                        {
                            alert('Hata: ' + jqXHR.responseText + " , " + textStatus + " , " + errorThrown);
                        });
                }
            }
        };

        $scope.ExportPDF = function ()
        {
            // $scope.$parent.ArkaPlaniAcikMi = true;
            $('.fa-file-pdf-o').addClass('fa-refresh fa-spin').removeClass('fa-file-pdf-o');

            var docDefinition = {
                defaultStyle: { fontSize: 10 },
                header: { text: 'Kullanıcı Listesi', margin: [250, 10, 0, 0] },
                content: [
                    {
                        table: {
                            headerRows: 1,
                            widths: [30, 120, 95, 95, 70, 70],
                            body: [
                                ['S.No', 'Kullanıcı Adı', 'Adı', 'Soyadı', 'Tipi', 'Durumu']
                            ]
                        }
                    }
                ],
                footer: function (AktifSayfaNo, SayfaAdet)
                {
                    return { text: AktifSayfaNo + '/' + SayfaAdet, alignment: 'right', margin: [0, 0, 50, 0] };
                }
            };

            $scope.KullaniciModelSonuc.forEach(function (Veri, index)
            {
                docDefinition.content[0].table.body.push([String(index + 1), Veri.KullaniciAd, Veri.Ad, Veri.Soyad, Veri.KullaniciTipNoUzunAd + "", Veri.AktifPasifTipNoUzunAd + ""]);
            });

            pdfMake.createPdf(docDefinition).download('Kullanici.pdf', function ()
            {
                $('.fa-refresh').addClass('fa-file-pdf-o').removeClass('fa-refresh fa-spin');
            });
            // pdfMake.createPdf(docDefinition).download('Kullanici.pdf');
        };

        $scope.ExportExcel = function ()
        {
            // $scope.KullaniciModelSonuc = GenelService.Sayfala($scope, false, $scope.KullaniciModelSonuc, $scope.KullaniciModelSonucPage);
            // $scope.$apply();
            var tblSonuc = document.getElementById('tblSonuc');
            var wb = XLSX.utils.table_to_book(tblSonuc, { sheet: "Sayfa1" });

            XLSX.writeFile(wb, 'Sonuc.xlsx', function (err)
            {
                debugger;
                if (!err) return alert("Saved to " + filename);
                alert("Error: " + (err.message || err));
            });

            /*XLSX.writeFile(wb, 'Sonuc.xlsx',

                function (err)
                {
                    debugger;
                    if (!err)
                        return alert("Saved to " + filename);
                    else
                        alert("Error: " + (err.message || err));
                }
            );*/
            // Sayfala($scope, true);
            // $scope.KullaniciModelSonuc = GenelService.Sayfala($scope, true, $scope.KullaniciModelSonuc, $scope.KullaniciModelSonuc);

            /*// direk $scope çalışıyor fakat kolon adları düzgün gelmiyor ve sıralama da yok
            var ws = XLSX.utils.json_to_sheet($scope.KullaniciModelSonuc);
            var wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, "Kullanıcı");
            XLSX.writeFile(wb, 'Sonuc.xlsx');*/
        };

        var KullaniciTipNoGeldi = false;
        var AktifPasifTipNoGeldi = false;

        KullaniciService.SrcKullaniciLookGetir('Yetki', 'AktifPasifTipNo', function (data)
        {
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModel.AktifPasifTipNoNolar = data;
            // $scope.KullaniciModel.AktifPasifTipNo = data[0].LookNo; // seçiniz seçili gelsin diye

            console.log('Yetki AktifPasifTipNo geldi');
            AktifPasifTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
            {
                $scope.$parent.ArkaPlaniAcikMi = false;
            }
        });

        KullaniciService.SrcKullaniciLookGetir('Yetki', 'KullaniciTipNo', function (data)
        {
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModel.KullaniciTipNolar = data;
            $scope.KullaniciModel.KullaniciTipNo = data[0].LookNo; // seçiniz seçili gelsin diye

            console.log('Yetki KullaniciTipNo geldi');
            KullaniciTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
            {
                $scope.$parent.ArkaPlaniAcikMi = false;
            }
        });

        // sayfa ilk açıldığında otomotik yüklensin diye
        $scope.KullaniciAramaYap('B', 1);
    })

    .controller('ngcKullaniciKayit', function ($scope, KullaniciService)
    {
        // debugger;
        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.KullaniciModel = {};

        var Key = '';
        if (window.location.href.indexOf('?') > -1)
        {
            var Parametre = window.location.href.substr(window.location.href.indexOf('?') + 1, window.location.href.length);
            Key = Parametre.split('&')[1].split('=')[1];
        }
        $scope.Key = Key;

        // if (Key > 0) // yeni kayıtsa veri çekmeye gerek yok. seçili alanlar yüklü gelsin diye gerek var
        // {
        KullaniciService.SrcKullaniciGetir(Key, function (data)
        {
            if (data != null)
            {
                if (data.Durum == '')
                {
                    console.log(data.Data);
                    $scope.KullaniciModel = data.Data[0];
                }
                else
                {
                    $scope.HataVar = true;
                    $scope.HataAciklama = data.Aciklama;
                    // todo:kaydet butonu pasif yap
                }
            }
            else
            {
                $scope.HataVar = true;
                $scope.HataAciklama = 'Sistem hatası: data = null';
                // todo:kaydet butonu pasif yap
                // alert('Sistem hatası: data=null');
            }
        });

        var KullaniciTipNoGeldi = false;
        var AktifPasifTipNoGeldi = false;
        KullaniciService.SrcKullaniciLookGetir('Yetki', 'KullaniciTipNo', function (data)
        {
            // debugger;
            console.log('Yetki KullaniciTipNo:');
            // console.log(data);
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModel.KullaniciTipNolar = data;

            KullaniciTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
                $scope.$parent.ArkaPlaniAcikMi = false;
        });

        KullaniciService.SrcKullaniciLookGetir('Yetki', 'AktifPasifTipNo', function (data)
        {
            // debugger;
            console.log('Yetki AktifPasifTipNo:');
            // console.log(data);
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModel.AktifPasifTipNoNolar = data;

            AktifPasifTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
                $scope.$parent.ArkaPlaniAcikMi = false;
        });

        $scope.KullaniciKaydet = function (FormValid)
        {
            if (!FormValid)
            {
                $scope.HataVar = true;
                $scope.HataAciklama = 'Zorunlu alanları giriniz';
                return;
            }
            $scope.HataVar = false;
            $scope.HataAciklama = '';

            $scope.$parent.ArkaPlaniAcikMi = true;
            KullaniciService.SrcKullaniciKaydet($scope, Key, function (data)
            {
                if (data.Durum)
                    parent.KayitGuncellendi(Key, data.Model);
                else if (data.Sonuc != 'Iptal')
                {
                    // alert('Uyarı: ' + data.Sonuc);
                    $scope.HataVar = true;
                    $scope.HataAciklama = data.Sonuc;
                }
                $scope.$parent.ArkaPlaniAcikMi = false;
            });
        };
    })

    .controller('ngcOkuma', function ($scope, KullaniciService)
    {
        $scope.RCSil = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            KullaniciService.SrcKullaniciSil($scope, Key, function (data)
            {
                if (data.Durum)
                    parent.KayitSilindi(Key);
                else if (data.Sonuc != 'Iptal')
                {
                    $scope.HataVar = true;
                    $scope.HataAciklama = data.Sonuc;
                }
                $scope.$parent.ArkaPlaniAcikMi = false;
            });
        };
    })


    .factory("KullaniciService", ['$http', function ($http)
    {
        var fac = {};

        fac.SrcKullaniciGetir = function (Key, CallBack)
        {
            $http.post("/Yetki/Kullanici/KullaniciGetir", { KullaniciKey: Key })
                .then(function (response)
                {
                    console.log('getir success...');
                    CallBack(response.data);
                })
                .catch(function (response)
                {
                    // debugger;
                    console.log('getir error...' + response.data);
                    // alert("Hata : " + response.data);
                    CallBack(null);
                });
        };

        fac.SrcKullaniciAra = function ($scope, CallBack)
        {
            $http.post("/Yetki/Kullanici/KullaniciAraSonuc", $scope.KullaniciModel)
                .then(function (response)
                {
                    console.log('ara ok:');
                    console.log(response.data);
                    CallBack(response.data);
                })
                .catch(function (response)
                {
                    console.log('ara hata: ');
                    console.log(response.data);
                    CallBack(response.data);
                });
        };

        fac.SrcKullaniciSil = function ($scope, Key, CallBack)
        {
            if (confirm("Kullanıcı kaydını silmek istediğinize emin misiniz?"))
            {
                $scope.ArkaPlaniAcikMi = true;

                $http.get("/Yetki/Kullanici/KullaniciSil?Key=" + Key)
                    .then(function (response)
                    {
                        // debugger;
                        $scope.ArkaPlaniAcikMi = false;
                        console.log('sil sonucu success...');
                        if (response.data.Durum == '')
                        {
                            CallBack({ Durum: true, Key: Key, Sonuc: '' });
                        }
                        else
                        {
                            CallBack({ Durum: false, Key: Key, Sonuc: response.data.Aciklama });
                        }
                    })
                    .catch(function (response)
                    {
                        debugger;
                        // debugger;
                        console.log('sil error...' + response.message);
                        $scope.ArkaPlaniAcikMi = false;
                        CallBack({ Durum: false, Key: Key, Sonuc: response.message });
                    });
            }
            else
                CallBack({ Durum: false, Key: Key, Sonuc: 'Iptal' });
        };

        fac.SrcKullaniciKaydet = function ($scope, Key, CallBack)
        {
            if (confirm("Kullanıcı kaydını güncellemek istediğinize emin misiniz?"))
            {
                $scope.ArkaPlaniAcikMi = true;

                $http({
                    method: 'Post',
                    url: '/Yetki/Kullanici/KullaniciKayit',
                    // contentType: 'application/x-www-form-urlencoded; charset-UTF-8',
                    data: $scope.KullaniciModel
                })
                    .then(function (response)
                    {
                        if (response.data.Durum == '')
                            CallBack({ Durum: true, Key: Key, Sonuc: '', Model: $scope.KullaniciModel });
                        else
                            CallBack({ Durum: false, Key: Key, Sonuc: response.data.Aciklama, Model: null });
                    })
                    .catch(function (response)
                    {
                        console.log('Sistem hatası: ' + response.message);
                        CallBack({ Durum: false, Key: Key, Sonuc: response.message, Model: null });
                    });
            }
            else
                CallBack({ Durum: false, Key: Key, Sonuc: 'Iptal', Model: null });
        };

        fac.SrcKullaniciLookGetir = function (TabloAd, AlanAd, CallBack)
        {
            var Parametre = { TabloAd: TabloAd, AlanAd: AlanAd };
            $http.get("/Yetki/Look/LookGetir", { params: Parametre })
                .then(function (response)
                {
                    CallBack(response.data);
                })
                .catch(function (response)
                {
                    alert("Sistem hatası: " + response.data);
                    CallBack(null);
                });
        }

        return fac;
    }]);

//var Sayfala = function ($scope, SayfaYap)
//{
//    if (SayfaYap)
//    {
//        var begin = (($scope.AktifSayfaNo - 1) * $scope.SayfaKayitAdet);
//        var end = begin + $scope.SayfaKayitAdet;
//        $scope.oKullaniciAraModelPage = $scope.oKullaniciAraModel.slice(begin, end);
//    }
//    else
//        $scope.oKullaniciAraModelPage = $scope.oKullaniciAraModel;
//};

var KayitGuncellendi = function (Key, Model)
{
    var $scope = GetScope('ngcKullaniciAra');

    // arama listesinide güncelle
    var ix = -1;
    for (var i = 0; i < $scope.KullaniciModelSonuc.length; i++)
    {
        if ($scope.KullaniciModelSonuc[i].KullaniciKey == Key)
        {
            ix = i;
            break;
        }
    }
    if (ix > -1)
    {
        $scope.KullaniciModelSonuc[ix].KullaniciAd = Model.KullaniciAd;
        $scope.KullaniciModelSonuc[ix].Ad = Model.Ad;
        $scope.KullaniciModelSonuc[ix].Soyad = Model.Soyad;
        $scope.KullaniciModelSonuc[ix].KullaniciTipNo = Model.KullaniciTipNo;
        $scope.KullaniciModelSonuc[ix].AktifPasifTipNo = Model.AktifPasifTipNo;
    }
    $scope.$apply();

    ArgemModalPopUpKapat();
};

var KayitSilindi = function (Key)
{
    debugger;
    var $scope = GetScope('ngcKullaniciAra');

    // arama sonucu listesinden de sil
    var ix = -1;
    for (var i = 0; i < $scope.KullaniciModelSonuc.length; i++)
    {
        if ($scope.KullaniciModelSonuc[i].KullaniciKey == Key)
        {
            ix = i;
            break;
        }
    }
    if (ix > -1)
    {
        $scope.KullaniciModelSonuc.splice(ix, 1);
        $scope.KullaniciModel.ToplamKayitAdet--;

        // if ($scope.KullaniciModelSonuc.length == 1 && $scope.AktifSayfaNo > 1) // sayfadaki son kayıt ise önceki sayfa varsa ona git
        // $scope.AktifSayfaNo--;

        // Sayfala($scope, true);
        // $scope.KullaniciModelSonuc = GenelService.Sayfala($scope, true, $scope.KullaniciModelSonuc, $scope.KullaniciModelSonuc);
        // $scope.$apply();
    }
    ArgemModalPopUpKapat();
};