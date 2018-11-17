'use strict';
var appGenel = angular.module('AngGenelApp', [])
    .controller('AngGenelController', function ($scope, $http, KullaniciAramaService, KullaniciLookService)
    {
        $scope.ArkaPlaniAcikMi = true;
        $scope.KullaniciModelAra = {
            // alan dolu gelmesi için
            // KullaniciAd: 'i'
        };
        $scope.KullaniciModelAraSonuc = {};
        $scope.KullaniciModelKayit = {};

        $scope.KullaniciAra = function ()
        {
            // $scope.ArkaPlaniAcikMi = true;
            // $scope.$apply();
            KullaniciAramaService.KullaniciAra($scope);
        };

        $scope.RCGu = function (Key)
        {
            $scope.ArkaPlaniAcikMi = true;
            $('.tempArgemModalSil').hide();
            $.get('/Yetki/Kullanici/KullaniciKayit?Durum=U&Key=' + Key,
                function (data)
                {
                    $('#argemModalLabel').html("Kullanıcı Güncelle");
                    $("#argemModalContent").html(data);
                    var options = { "backdrop": "static", keyboard: true };
                    $('#argemModalPopUp')
                        .modal(options)
                        .modal('toggle')
                        .on('shown.bs.modal', function ()
                        {
                            $(this).off('shown.bs.modal');

                            $('.tempArgemModalKaydet')
                                .show()
                                .on('click', function (event)
                                {
                                    event.preventDefault();
                                    $('.tempArgemModalKaydet').hide();
                                    $('.tempArgemModalKaydet').off('click');

                                    KullaniciAramaService.KullaniciSil($scope, Key, function (data)
                                    {
                                        console.log(data);
                                        if (data.Durum)
                                        {
                                            // arama sonucu listesinden de sil
                                            var ix = -1;
                                            for (var i = 0; i < $scope.KullaniciModelAraSonuc.length; i++)
                                            {
                                                if ($scope.KullaniciModelAraSonuc[i].KullaniciKey == data.Key)
                                                {
                                                    ix = i;
                                                    break;
                                                }
                                            }
                                            if (ix > -1)
                                                $scope.KullaniciModelAraSonuc.splice(ix, 1);

                                            $("#argemModalContent").html('');
                                            $('#argemModalPopUp').modal('toggle');
                                        }
                                    });
                                });

                            KullaniciAramaService.KullaniciGetir($scope, Key, function (data)
                            {
                                if (data != null)
                                {
                                    $scope.KullaniciModelKayit = data;
                                    $scope.ArkaPlaniAcikMi = false;
                                    // $scope.$apply();
                                }
                                else
                                    alert('hata var');
                            });
                        });

                    // $scope.ArkaPlaniAcikMi = false;
                    // $scope.$apply();
                })
                .fail(function (jqXHR, textStatus, errorThrown)
                {
                    debugger;
                    $('#argemModalLabel').html("Sistem hatası");
                    // $("#argemModalContent").html(jqXHR.responseText);
                    $("#argemModalContent").html(textStatus + "<br>" + errorThrown);
                    $('#argemModalPopUp').modal('toggle');
                    $scope.ArkaPlaniAcikMi = false;
                    $scope.$apply();

                    // alert(jqXHR.responseText);
                    // console.log(textStatus);
                    // console.log(errorThrown);
                });
        };

        $scope.RCOk = function (Key)
        {
            $scope.ArkaPlaniAcikMi = true;
            $('.tempArgemModalKaydet').hide();
            $('.tempArgemModalSil').hide();
            $.get('/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + Key,
                function (data)
                {
                    // debugger;
                    $('#argemModalLabel').html("Kullanıcı Detayı");
                    $("#argemModalContent").html(data);
                    var options = { "backdrop": "static", keyboard: true };
                    $('#argemModalPopUp')
                        .modal(options)
                        .modal('toggle');
                    $scope.ArkaPlaniAcikMi = false;
                    $scope.$apply();
                })
                .fail(function (jqXHR, textStatus, errorThrown)
                {
                    debugger;
                    $('#argemModalLabel').html("Sistem hatası");
                    // $("#argemModalContent").html(jqXHR.responseText);
                    $("#argemModalContent").html(textStatus + "<br>" + errorThrown);
                    $('#argemModalPopUp').modal('toggle');
                    $scope.ArkaPlaniAcikMi = false;
                    $scope.$apply();

                    // alert(jqXHR.responseText);
                    // console.log(textStatus);
                    // console.log(errorThrown);
                });
        };

        $scope.RCSi = function (Key)
        {
            $scope.ArkaPlaniAcikMi = true;
            $('.tempArgemModalKaydet').hide();
            // $('.tempArgemModalSil').hide();
            $.get('/Yetki/Kullanici/KullaniciOku?Durum=D&Key=' + Key,
                function (data)
                {
                    $('#argemModalLabel').html("Kullanıcı Sil");
                    $("#argemModalContent").html(data);
                    var options = { "backdrop": "static", keyboard: true };
                    $('#argemModalPopUp')
                        .modal(options)
                        .modal('toggle')
                        .on('shown.bs.modal', function ()
                        {
                            $(this).off('shown.bs.modal');

                            $('.tempArgemModalSil')
                                .show()
                                .on('click', function (event)
                                {
                                    event.preventDefault();
                                    $('.tempArgemModalSil').hide();
                                    $('.tempArgemModalSil').off('click');

                                    KullaniciAramaService.KullaniciSil($scope, Key, function (data)
                                    {
                                        console.log(data);
                                        if (data.Durum)
                                        {
                                            // arama sonucu listesinden de sil
                                            var ix = -1;
                                            for (var i = 0; i < $scope.KullaniciModelAraSonuc.length; i++)
                                            {
                                                if ($scope.KullaniciModelAraSonuc[i].KullaniciKey == data.Key)
                                                {
                                                    ix = i;
                                                    break;
                                                }
                                            }
                                            if (ix > -1)
                                                $scope.KullaniciModelAraSonuc.splice(ix, 1);

                                            $("#argemModalContent").html('');
                                            $('#argemModalPopUp').modal('toggle');
                                        }
                                    });
                                });
                        })

                    $scope.ArkaPlaniAcikMi = false;
                    $scope.$apply();
                })
                .fail(function (jqXHR, textStatus, errorThrown)
                {
                    debugger;
                    $('#argemModalLabel').html("Sistem hatası");
                    // $("#argemModalContent").html(jqXHR.responseText);
                    $("#argemModalContent").html(textStatus + "<br>" + errorThrown);
                    $('#argemModalPopUp').modal('toggle');
                    $scope.ArkaPlaniAcikMi = false;
                    $scope.$apply();

                    // alert(jqXHR.responseText);
                    // console.log(textStatus);
                    // console.log(errorThrown);
                });
        };

        // ilk sayfa açıldığında veriler yüklensin diye
        // hata var şimdilik kapat
        // KullaniciAramaService.KullaniciAra($scope);
        // $scope.ArkaPlaniAcikMi = false;

        var KullaniciTipNoGeldi = false;
        var AktifPasifTipNoGeldi = false;
        KullaniciLookService.KullaniciLookGetir('Yetki', 'KullaniciTipNo', function (data)
        {
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModelAra.KullaniciTipNolar = data;
            $scope.KullaniciModelAra.KullaniciTipNo = data[0].LookNo;

            // debugger;
            KullaniciTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
                $scope.ArkaPlaniAcikMi = false;
        });

        KullaniciLookService.KullaniciLookGetir('Yetki', 'AktifPasifTipNo', function (data)
        {
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModelAra.AktifPasifTipNoNolar = data;
            $scope.KullaniciModelAra.AktifPasifTipNo = data[0].LookNo;

            // debugger;
            AktifPasifTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
                $scope.ArkaPlaniAcikMi = false;
        });

        $scope.KullaniciModelAraSonuc = {};
        $scope.KullaniciAra = function ()
        {
            // debugger;
            KullaniciAramaService.KullaniciAra($scope);
        }
    })

    .controller('AngOkumaController', function ($scope, $http)
    {
        alert(1);
        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.SilmeGoster = false;

        var Parametre = '';
        if (window.location.href.indexOf('?') > -1)
            Parametre = window.location.href.substr(window.location.href.indexOf('?'), window.location.href.length);

        $http.get("/Yetki/Kullanici/KullaniciOkuAng" + Parametre)
            .then(function (response)
            {
                // debugger;
                console.log('kullanıcı oku sonucu success...');
                if (response.data.IslemDurum != 'H')
                {
                    console.log(response.data);
                    $scope.KullaniciModel = response.data;
                    $scope.SilmeGoster = true;
                }
                else
                {
                    $scope.HataVar = true;
                    $scope.HataAciklama = response.data;
                }
                $scope.$parent.ArkaPlaniAcikMi = false;
            })
            .catch(function (response)
            {
                debugger;
                console.log('kullanıcı oku error...' + response.data);
                $scope.HataVar = true;
                $scope.HataAciklama = response.data;
                $scope.$parent.ArkaPlaniAcikMi = false;
            });

    })

    .factory("KullaniciAramaService", ['$http', function ($http)
    {
        var fac = {};

        fac.KullaniciGetir = function ($scope, Key, CallBack)
        {
            // debugger;
            //$scope.ArkaPlaniAcikMi = true;
            $http.post("/Yetki/Kullanici/KullaniciAraSonuc", { KullaniciKey: Key })
                .then(function (response)
                {
                    // debugger;
                    // $scope.ArkaPlaniAcikMi = false;
                    console.log('kullanıcı getir success...');
                    // $scope.KullaniciModelKayit = response.data;
                    CallBack(response.data);
                })
                .catch(function (response)
                {
                    debugger;
                    // $scope.ArkaPlaniAcikMi = false;
                    console.log('kullanıcı getir error...' + response.data);
                    alert("Hata : " + response.data);
                    CallBack(null);
                });
        };

        fac.KullaniciAra = function ($scope)
        {
            // debugger;
            $scope.ArkaPlaniAcikMi = true;
            $http.post("/Yetki/Kullanici/KullaniciAraSonuc", $scope.KullaniciModelAra)
                .then(function (response)
                {
                    // debugger;
                    $scope.ArkaPlaniAcikMi = false;
                    console.log('kullanıcı ara success...');
                    $scope.KullaniciModelAraSonuc = response.data;
                })
                .catch(function (response)
                {
                    // debugger;
                    $scope.ArkaPlaniAcikMi = false;
                    console.log('kullanıcı ara error...' + response.data);
                    alert("Hata : " + response.data);
                });
        };


        fac.KullaniciSil = function ($scope, Key, CallBack)
        {
            if (confirm("Kullanıcı kaydını silmek istediğinize emin misiniz?"))
            {
                $scope.ArkaPlaniAcikMi = true;

                $http.get("/Yetki/Kullanici/KullaniciSil?Key=" + Key)
                    .then(function (response)
                    {
                        // debugger;
                        $scope.ArkaPlaniAcikMi = false;
                        console.log('kullanıcı sil sonucu success...');
                        if (response.data.IslemDurum != 'H')
                        {
                            CallBack({ Durum: true, Key: Key, Sonuc: '' });
                            // console.log(response.data);
                            // alert("OK silidn");
                            // todo:modalı kapatıp arkaplandan da sil
                        }
                        else
                        {
                            // $scope.HataVar = true;
                            // $scope.HataAciklama = response.data.IslemAciklama;
                            CallBack({ Durum: false, Key: Key, Sonuc: response.data.IslemAciklama });
                        }
                    })
                    .catch(function (response)
                    {
                        debugger;
                        console.log('kullanıcı sil error...' + response.data);
                        // $scope.HataVar = true;
                        // $scope.HataAciklama = response.data;
                        $scope.ArkaPlaniAcikMi = false;
                        CallBack({ Durum: false, Key: Key, Sonuc: response });
                    });
            }
            else
                CallBack({ Durum: false, Key: Key, Sonuc: 'Iptal' });
        };

        fac.KullaniciKaydet = function ($scope, Key, CallBack)
        {
            if (confirm("Kullanıcı kaydını güncellemek istediğinize emin misiniz?"))
            {
                $scope.ArkaPlaniAcikMi = true;

                $http({
                    method: 'Post',
                    url: '/Yetki/Kullanici/KullaniciKayit',
                    data: $scope.KullaniciModelAra
                })
                    .then(function (response)
                    {
                        // debugger;
                        console.log('kullanıcı girişi sonucu success...');
                        if (response.data == 'OK')
                            window.location.href = '/Account/AnaSayfa';
                        else
                        {
                            $scope.GirisButonKapat = false;
                            $scope.HataVar = true;
                            $scope.HataAciklama = response.data;
                        }
                    })
                    .catch(function (response)
                    {
                        // debugger;
                        $scope.LoginViewModel.parola = null;
                        console.log('error...' + response.data);
                        $scope.GirisButonKapat = false;
                        $scope.HataVar = true;
                        $scope.HataAciklama = response.data;
                    });
            }
            else
                CallBack({ Durum: false, Key: Key, Sonuc: 'Iptal' });
        };

        return fac;
    }])

    .factory("KullaniciLookService", ['$http', function ($http)
    {
        var fac = {};
        fac.KullaniciLookGetir = function (TabloAd, AlanAd, CallBack)
        {
            var Parametre = { TabloAd: TabloAd, AlanAd: AlanAd };
            $http.get("/Yetki/Look/LookGetir", { params: Parametre })
                .then(function (response)
                {
                    // debugger;
                    console.log('KullaniciLookGetir ' + AlanAd + ' success...');
                    CallBack(response.data);
                })
                .catch(function (response)
                {
                    // debugger;
                    console.log('KullaniciLookGetir ' + AlanAd + ' error...' + response.data);
                    alert("Hata : " + response.data);
                    CallBack(null);
                });
        }
        return fac;
    }]);
