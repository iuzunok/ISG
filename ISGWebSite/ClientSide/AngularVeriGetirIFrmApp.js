'use strict';
var appGenel = angular.module('AngGenelApp', [])

    .factory("KullaniciAramaService", ['$http', function ($http)
    {
        // debugger;
        var fac = {};

        fac.KullaniciGetir = function ($scope, Key, CallBack)
        {
            // debugger;
            //$scope.ArkaPlaniAcikMi = true;
            // alert(Key);
            $http.post("/Yetki/Kullanici/KullaniciAraSonuc", { KullaniciKey: Key })
                .then(function (response)
                {
                    // debugger;
                    // $scope.ArkaPlaniAcikMi = false;
                    console.log('kullanıcı getir success...');
                    // $scope.KullaniciModelKayit = response.data;
                    CallBack(response.data[0]);
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
            $scope.$parent.ArkaPlaniAcikMi = true;
            $http.post("/Yetki/Kullanici/KullaniciAraSonuc", $scope.KullaniciModelAra)
                .then(function (response)
                {
                    // debugger;
                    $scope.$parent.ArkaPlaniAcikMi = false;
                    console.log('kullanıcı ara success...');
                    $scope.KullaniciModelAraSonuc = response.data;
                })
                .catch(function (response)
                {
                    // debugger;
                    $scope.$parent.ArkaPlaniAcikMi = false;
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
                // alert($scope.KullaniciModelKayit.KullaniciKey);
                // console.log('giden form data:');
                // console.log($scope.KullaniciModelKayit);

                $http({
                    method: 'Post',
                    url: '/Yetki/Kullanici/KullaniciKayit',
                    // contentType: 'application/x-www-form-urlencoded; charset-UTF-8',
                    data: $scope.KullaniciModelKayit
                })
                    .then(function (response)
                    {
                        // console.log('kullanıcı girişi sonucu success...');
                        // console.log(response.data);
                        if (response.data.IslemDurum == 'OK')
                            CallBack({ Durum: true, Key: Key, Sonuc: '', Model: $scope.KullaniciModelKayit });
                        else
                            CallBack({ Durum: false, Key: Key, Sonuc: response.data.IslemAciklama, Model: null });
                    })
                    .catch(function (response)
                    {
                        console.log('Sistem hatası: ' + response.data);
                        CallBack({ Durum: false, Key: Key, Sonuc: response.data, Model: null });
                    });
            }
            else
                CallBack({ Durum: false, Key: Key, Sonuc: 'Iptal', Model: null });
        };

        fac.KullaniciLookGetir = function (TabloAd, AlanAd, CallBack)
        {
            // var t0 = performance.now();
            var Parametre = { TabloAd: TabloAd, AlanAd: AlanAd };
            $http.get("/Yetki/Look/LookGetir", { params: Parametre })
                .then(function (response)
                {
                    // var t1 = performance.now();
                    // console.log(AlanAd + ": took " + (t1 - t0) + " ms.");
                    // console.log('KullaniciLookGetir ' + AlanAd + ' success...');
                    CallBack(response.data);
                })
                .catch(function (response)
                {
                    // debugger;
                    // console.log('Parametre getir (' + AlanAd + ') hatası.' + response.data);
                    alert("Sistem hatası: " + response.data);
                    CallBack(null);
                });
        }
        return fac;
    }]);
