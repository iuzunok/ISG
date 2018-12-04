'use strict';
var appGenel = angular.module('ngaGenel', ['ui.bootstrap'])
    .controller('ngcGenel', function ()
    {
    })

    .controller('ngcYetkiVer', function ($scope)
    {
        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.oKullaniciYetkiGrupModel = {};
        $scope.oKullaniciYetkiGrupModelPage = [];
        $scope.KullaniciKey = 0;
        $scope.$parent.ArkaPlaniAcikMi = false;

        $scope.KullaniciYetkiGetir = function ()
        {
            $scope.$parent.ArkaPlaniAcikMi = true;

            $scope.KullaniciKey = $('#hdntxtKullaniciAd').val();
            // alert($scope.KullaniciKey);

            $.get('/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + $scope.KullaniciKey, function (data)
            {
                // debugger;
                $scope.HataVar = false;
                $scope.$apply();
                var $divKullaniciDetay = $('#divKullaniciDetay');
                $divKullaniciDetay.html(data);
                $divKullaniciDetay.find('.cardPopUp').removeClass('cardPopUp');
                $scope.$parent.ArkaPlaniAcikMi = false;
                $scope.$apply();
            })
                .fail(function (jqXHR, textStatus, errorThrown)
                {
                    alert('Hata: ' + jqXHR.responseText + " , " + textStatus + " , " + errorThrown);
                    $scope.$parent.ArkaPlaniAcikMi = false;
                });
        };
    });