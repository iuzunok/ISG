'use strict';
appGenel
    .controller('ngcYetkiVer', function ($scope, GenelService, KullaniciYetkiGrupService)
    {
        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.oKullaniciYetkiGrupAraModel = {};
        $scope.oKullaniciYetkiGrupAraModelPage = [];
        $scope.KullaniciKey = 0;

        $scope.KullaniciYetkiGrupGetir = function (KullaniciKey)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;

            KullaniciYetkiGrupService.KullaniciYetkiGrupGetir(KullaniciKey, function (data)
            {
                if (data != null)
                {
                    if (data.Durum == '')
                    {
                        $scope.oKullaniciYetkiGrupAraModel = data.Data;
                        $scope.ToplamKayitAdet = $scope.oKullaniciYetkiGrupAraModel.length;
                        $scope.AktifSayfaNo = 1;
                        $scope.SayfaKayitAdet = 50;
                        $scope.oKullaniciYetkiGrupAraModelPage = GenelService.Sayfala($scope, true, $scope.oKullaniciYetkiGrupAraModel, $scope.oKullaniciYetkiGrupAraModelPage);
                    }
                    else
                    {
                        $scope.HataVar = true;
                        $scope.HataAciklama = data.Aciklama;
                        $scope.oKullaniciYetkiGrupAraModelPage = null;
                    }
                }
                else
                {
                    $scope.oKullaniciYetkiGrupAraModelPage = null;
                    $scope.HataVar = true;
                    $scope.HataAciklama = 'Sistem hatası. data=null';
                }

                $scope.$parent.ArkaPlaniAcikMi = false;
            });
        };

        $scope.KullaniciYetkiGetir = function ()
        {
            $scope.$parent.ArkaPlaniAcikMi = true;

            $scope.KullaniciKey = $('#hdntxtKullaniciAd').val();
            // alert($scope.KullaniciKey);

            $scope.KullaniciYetkiGrupGetir($scope.KullaniciKey);

            $.get('/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + $scope.KullaniciKey, function (data)
            {
                // debugger;
                $scope.HataVar = false;
                $scope.$apply();
                var $divKullaniciDetay = $('#divKullaniciDetay');
                $divKullaniciDetay.html(data);
                $divKullaniciDetay.find('.cardPopUp').removeClass('cardPopUp');
                // $scope.$parent.ArkaPlaniAcikMi = false;
                $scope.$apply();
            })
                .fail(function (jqXHR, textStatus, errorThrown)
                {
                    alert('Hata: ' + jqXHR.responseText + " , " + textStatus + " , " + errorThrown);
                    $scope.$parent.ArkaPlaniAcikMi = false;
                });
        };

        $scope.RCIn = function ()
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/KullaniciYetkiGrup/KullaniciYetkiGrupKayit?Durum=I&Key=0&KullaniciKey=' + $scope.KullaniciKey, '80%', '90%', false, true, 'Kullanıcı Yetki Kayıt', '', '');
        };

        $scope.SayfalamaDegisti = function ()
        {
            $scope.oKullaniciYetkiGrupAraModelPage = GenelService.Sayfala($scope, true, $scope.oKullaniciYetkiGrupAraModel, $scope.oKullaniciYetkiGrupAraModelPage);
        };

        $scope.$parent.ArkaPlaniAcikMi = false;
    })

    .controller('ngcKullaniciYetkiGrupKayit', function ($scope, GenelService, YetkiGrupService)
    {
        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.KullaniciModel = {};
        $scope.YetkiGrupAd = '';
        $scope.oYetkiGrupModel = {};
        $scope.oYetkiGrupModelPage = [];
        $scope.oYetkiGrupModelSec = [];

        var Key = '';
        if (window.location.href.indexOf('?') > -1)
        {
            var Parametre = window.location.href.substr(window.location.href.indexOf('?') + 1, window.location.href.length);
            Key = Parametre.split('&')[1].split('=')[1];
        }
        $scope.Key = Key;

        YetkiGrupService.YetkiGrupAra($scope, function (data)
        {
            // debugger;
            // console.log(data);
            if (data != null)
            {
                if (data.Durum == '')
                {
                    // debugger;
                    $scope.oYetkiGrupModel = data.Data;
                    $scope.ToplamKayitAdet = $scope.oYetkiGrupModel.length;
                    $scope.AktifSayfaNo = 1;
                    $scope.SayfaKayitAdet = 50;
                    // Sayfala($scope, true);
                    $scope.oYetkiGrupModelPage = GenelService.Sayfala($scope, true, $scope.oYetkiGrupModel, $scope.oYetkiGrupModelPage);
                }
                else
                {
                    $scope.HataVar = true;
                    $scope.HataAciklama = data.Aciklama;
                    $scope.oYetkiGrupModelPage = null;
                }
            }
            else
            {
                $scope.oYetkiGrupModelPage = null;
                $scope.HataVar = true;
                $scope.HataAciklama = 'Sistem hatası. data=null';
            }

            $scope.$parent.ArkaPlaniAcikMi = false;
        });

        $scope.SayfalamaDegisti = function ()
        {
            // console.log('sayfa:' + $scope.AktifSayfaNo);
            $scope.oYetkiGrupModelPage = GenelService.Sayfala($scope, true, $scope.oYetkiGrupModel, $scope.oYetkiGrupModelPage);
        };

        $scope.$parent.ArkaPlaniAcikMi = false;

        $scope.YetkiGurupSecBirakTumYetkiGrupTumu = function ($event)
        {
            var toggleStatus = !$scope.isAllSelected;
            angular.forEach($scope.options, function (itm) { itm.selected = toggleStatus; });
        }

        $scope.YetkiGurupSecBirakTumYetkiGrup = function ($event, YetkiGrupKey)
        {
            // debugger;
            if ($event.target.checked)
            {
                angular.forEach($scope.oYetkiGrupModelPage, function (itm, index)
                {
                    if (itm.YetkiGrupKey == YetkiGrupKey)
                    {
                        $scope.oYetkiGrupModelPage[index].S = 1;
                        $scope.oYetkiGrupModelSec.push($scope.oYetkiGrupModelPage[index]);
                    }
                });
            }
            else
            {
                angular.forEach($scope.oYetkiGrupModelSec, function (itm, index)
                {
                    if (itm.YetkiGrupKey == YetkiGrupKey)
                        $scope.oYetkiGrupModelSec.splice(index, 1);
                });
            }
        }

        $scope.KullaniciYetkiKaydet = function ()
        {
            alert("daha bitmedi");
        };

        $scope.YetkiGrupFlitrele = function ()
        {
            //if ($scope.YetkiGrupAd.length > 2)
            //    alert("daha bitmedi veri getir:" + $scope.YetkiGrupAd.length);
            //else
            //    alert("daha bitmedi bekle:" + $scope.YetkiGrupAd.length);
        };

        //KullaniciYetkiGrupService.KullaniciGetir(Key, function (data)
        //{
        //    if (data != null)
        //    {
        //        if (data.Durum == '')
        //        {
        //            console.log(data.Data);
        //            $scope.KullaniciModel = data.Data[0];
        //        }
        //        else
        //        {
        //            $scope.HataVar = true;
        //            $scope.HataAciklama = data.Aciklama;
        //        }
        //    }
        //    else
        //    {
        //        $scope.HataVar = true;
        //        $scope.HataAciklama = 'Sistem hatası: data = null';
        //    }
        //});

        //var KullaniciTipNoGeldi = false;
        //var AktifPasifTipNoGeldi = false;
        //KullaniciService.KullaniciLookGetir('Yetki', 'KullaniciTipNo', function (data)
        //{
        //    console.log('KullaniciLookGetir Yetki KullaniciTipNo sonuç geldi');
        //    data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
        //    $scope.KullaniciModel.KullaniciTipNolar = data;

        //    KullaniciTipNoGeldi = true;
        //    if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
        //        $scope.$parent.ArkaPlaniAcikMi = false;
        //});

        //KullaniciService.KullaniciLookGetir('Yetki', 'AktifPasifTipNo', function (data)
        //{
        //    console.log('KullaniciLookGetir Yetki AktifPasifTipNo sonuç geldi');
        //    data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
        //    $scope.KullaniciModel.AktifPasifTipNoNolar = data;

        //    AktifPasifTipNoGeldi = true;
        //    if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
        //        $scope.$parent.ArkaPlaniAcikMi = false;
        //});

        //$scope.KullaniciKaydet = function (FormValid)
        //{
        //    if (!FormValid)
        //    {
        //        $scope.HataVar = true;
        //        $scope.HataAciklama = 'Zorunlu alanları giriniz';
        //        return;
        //    }
        //    $scope.HataVar = false;
        //    $scope.HataAciklama = '';

        //    $scope.$parent.ArkaPlaniAcikMi = true;
        //    KullaniciService.KullaniciKaydet($scope, Key, function (data)
        //    {
        //        if (data.Durum)
        //            parent.KayitGuncellendi(Key, data.Model);
        //        else if (data.Sonuc != 'Iptal')
        //        {
        //            // alert('Uyarı: ' + data.Sonuc);
        //            $scope.HataVar = true;
        //            $scope.HataAciklama = data.Sonuc;
        //        }
        //        $scope.$parent.ArkaPlaniAcikMi = false;
        //    });
        //};
    })


    .factory("KullaniciYetkiGrupService", ['$http', function ($http)
    {
        var fac = {};

        fac.KullaniciYetkiGrupGetir = function (Key, CallBack)
        {
            // debugger;
            $http.post("/Yetki/KullaniciYetkiGrup/KullaniciYetkiGrupAraSonuc", { KullaniciKey: Key })
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

        //fac.YetkiGrupAra = function ($scope, CallBack)
        //{
        //    $http.post("/Yetki/YetkiGrup/YetkiGrupAraSonuc", $scope.YetkiGrupModel)
        //        .then(function (response)
        //        {
        //            console.log('ara ok:');
        //            // console.log(response.data);
        //            CallBack(response.data);
        //        })
        //        .catch(function (response)
        //        {
        //            console.log('ara hata: ' + response.data);
        //            // alert("Hata : " + response.data);
        //            CallBack(null);
        //        });
        //};

        //fac.YetkiGrupSil = function ($scope, Key, CallBack)
        //{
        //    if (confirm("Yetki grup kaydını silmek istediğinize emin misiniz?"))
        //    {
        //        $scope.$parent.ArkaPlaniAcikMi = true;

        //        $http.get("/Yetki/YetkiGrup/YetkiGrupSil?Key=" + Key)
        //            .then(function (response)
        //            {
        //                $scope.ArkaPlaniAcikMi = false;
        //                console.log('sil sonucu success...');
        //                if (response.data.Durum == '')
        //                {
        //                    CallBack({ Durum: true, Key: Key, Sonuc: '' });
        //                }
        //                else
        //                {
        //                    CallBack({ Durum: false, Key: Key, Sonuc: response.data.Aciklama });
        //                }
        //            })
        //            .catch(function (response)
        //            {
        //                debugger;
        //                console.log('sil error...' + response.message);
        //                $scope.ArkaPlaniAcikMi = false;
        //                CallBack({ Durum: false, Key: Key, Sonuc: response.message });
        //            });
        //    }
        //    else
        //        CallBack({ Durum: false, Key: Key, Sonuc: 'Iptal' });
        //};

        //fac.YetkiGrupKaydet = function ($scope, Key, CallBack)
        //{
        //    if (confirm("Yetki grup kaydını güncellemek istediğinize emin misiniz?"))
        //    {
        //        $scope.$parent.ArkaPlaniAcikMi = true;

        //        $http({
        //            method: 'Post',
        //            url: '/Yetki/YetkiGrup/YetkiGrupKayit',
        //            // contentType: 'application/x-www-form-urlencoded; charset-UTF-8',
        //            data: $scope.YetkiGrupModel
        //        })
        //            .then(function (response)
        //            {
        //                if (response.data.Durum == '')
        //                    CallBack({ Durum: true, Key: Key, Sonuc: '', Model: $scope.YetkiGrupModel });
        //                else
        //                    CallBack({ Durum: false, Key: Key, Sonuc: response.data.Aciklama, Model: null });
        //            })
        //            .catch(function (response)
        //            {
        //                console.log('Sistem hatası: ' + response.message);
        //                CallBack({ Durum: false, Key: Key, Sonuc: response.message, Model: null });
        //            });
        //    }
        //    else
        //        CallBack({ Durum: false, Key: Key, Sonuc: 'Iptal', Model: null });
        //};

        return fac;
    }]);


//var Sayfala = function ($scope, SayfaYap)
//{
//    if (SayfaYap)
//    {
//        var begin = (($scope.AktifSayfaNo - 1) * $scope.SayfaKayitAdet);
//        var end = begin + $scope.SayfaKayitAdet;
//        $scope.oKullaniciYetkiGrupAraModelPage = $scope.oKullaniciYetkiGrupAraModel.slice(begin, end);
//    }
//    else
//        $scope.oKullaniciYetkiGrupAraModelPage = $scope.oKullaniciYetkiGrupAraModel;
//};