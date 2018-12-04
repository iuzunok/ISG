'use strict';
var ngAppLogin = angular.module('ngaLogin', [])

    .controller('ngcLogin', function ($scope, $http)
    {
        $scope.HataVar = false;
        // ilk değerleri atamak için
        $scope.KullaniciLoginModel = {
            KullaniciAd: 'iuzunok@argemyazilim.com',
            Parola: '123',
            KullaniciAdimiHatirla: 1
        };

        $scope.SistemeGirisYap = function (FormValid)
        {
            if (!FormValid)
            {
                $scope.HataVar = true;
                $scope.HataAciklama = 'Zorunlu alanları giriniz';
                return;
            }
            $scope.HataVar = false;
            $scope.HataAciklama = '';

            $scope.GirisButonKapat = true;

            $http({
                method: 'Post',
                url: '/Yetki/Login/SistemeGirisYap',
                // contentType: 'application/x-www-form-urlencoded; charset-UTF-8',
                data: $scope.KullaniciLoginModel
            })
                .then(function (response)
                {
                    if (response.data.Durum == '')
                        window.location.href = '/Yetki/Login/AnaSayfa';
                    else
                    {
                        $scope.GirisButonKapat = false;
                        $scope.HataVar = true;
                        $scope.HataAciklama = response.data.Aciklama;
                    }
                })
                .catch(function (response)
                {
                    $scope.KullaniciLoginModel.Parola = null;
                    console.log('error...' + response.data);
                    $scope.GirisButonKapat = false;
                    $scope.HataVar = true;
                    $scope.HataAciklama = response.data;
                });
        };
    });



