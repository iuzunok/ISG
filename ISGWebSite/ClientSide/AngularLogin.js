var appGenel = angular.module('AngLoginApp', [])
    .controller('AngLoginController', function ($scope, $http)
    {
        $scope.HataVar = false;
        // ilk değerleri atamak için
        $scope.LoginViewModel = {
            KullaniciAd: 'iuzunok@argemyazilim.com',
            Parola: '123',
            KullaniciAdimiHatirla: 1
        };

        $scope.SistemeGirisYap = function ()
        {
            $scope.GirisButonKapat = true;
            $http({
                method: 'Post',
                url: '/Yetki/Kullanici/SistemeGirisYap',
                data: $scope.LoginViewModel
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
                    // alert("Hata : " + response.data);
                    $scope.GirisButonKapat = false;
                    $scope.HataVar = true;
                    $scope.HataAciklama = response.data;
                });
        };

        // $scope.KullaniciAd = "iuzunok@argemyazilim.com";
        // $scope.LoginViewModel.Parola = "123";
        // $scope.ArkaPlanKapat = false;
    });