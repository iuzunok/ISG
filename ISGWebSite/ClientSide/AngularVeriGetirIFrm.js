appGenel
    .controller('AngGenelController', function ($scope, $http, KullaniciAramaService)
    {
        // $scope.ArkaPlaniAcikMi = true;
        /*$scope.$watch("ArkaPlaniAcikMi", function (newValue, oldValue, scope)
        {
            console.log("Eski Değer: " + oldValue + " Yeni Değer: " + newValue);
        }, true);*/
    })

    .controller('ngConKullaniciAra', function ($scope, $http, KullaniciAramaService)
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
            $scope.$parent.ArkaPlaniAcikMi = true;
            KullaniciAramaService.KullaniciAra($scope);
        };

        $scope.RCInY = function ()
        {
            $scope.ArkaPlaniAcikMi = true;
            var $arifPopUp = $('#arifPopUp');
            var options = { "backdrop": "static", keyboard: true };
            $('#argemModalPopUp').modal(options).modal('toggle')
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciKayit?Durum=I&Key=0');
            $scope.ArkaPlaniAcikMi = false;
        };

        $scope.RCGuY = function (Key)
        {
            $scope.ArkaPlaniAcikMi = true;
            var $arifPopUp = $('#arifPopUp');
            var options = { "backdrop": "static", keyboard: true };
            $('#argemModalPopUp').modal(options).modal('toggle')
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciKayit?Durum=U&Key=' + Key);
            $scope.ArkaPlaniAcikMi = false;
        };

        $scope.RCOkY = function (Key)
        {
            $scope.ArkaPlaniAcikMi = true;
            var $arifPopUp = $('#arifPopUp');
            var options = { "backdrop": "static", keyboard: true };
            $('#argemModalPopUp').modal(options).modal('toggle')
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + Key);
            $scope.ArkaPlaniAcikMi = false;
        };

        $scope.RCSiY = function (Key)
        {
            $scope.ArkaPlaniAcikMi = true;
            var $arifPopUp = $('#arifPopUp');
            var options = { "backdrop": "static", keyboard: true };
            $('#argemModalPopUp').modal(options).modal('toggle')
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciOku?Durum=D&Key=' + Key);
            $scope.ArkaPlaniAcikMi = false;
        };

        // var t0 = performance.now();
        var KullaniciTipNoGeldi = false;
        var AktifPasifTipNoGeldi = false;

        KullaniciAramaService.KullaniciLookGetir('Yetki', 'AktifPasifTipNo', function (data)
        {
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModelAra.AktifPasifTipNoNolar = data;
            $scope.KullaniciModelAra.AktifPasifTipNo = data[0].LookNo;

            AktifPasifTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
            {
                // var t1 = performance.now();
                // console.log("genel look getir " + (t1 - t0) + " milliseconds.");
                $scope.$parent.ArkaPlaniAcikMi = false;
            }
        });

        KullaniciAramaService.KullaniciLookGetir('Yetki', 'KullaniciTipNo', function (data)
        {
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModelAra.KullaniciTipNolar = data;
            $scope.KullaniciModelAra.KullaniciTipNo = data[0].LookNo;

            // debugger;
            KullaniciTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
            {
                // var t1 = performance.now();
                // console.log("genel look getir " + (t1 - t0) + " ms.");
                $scope.$parent.ArkaPlaniAcikMi = false;
            }
        });


        // sayfa ilk açıldığında otomotik yüklensin diye
        // KullaniciAramaService.KullaniciAra($scope);
    })

    .controller('ngConKullaniciKayit', function ($scope, $http, KullaniciAramaService)
    {
        // debugger;
        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.KullaniciModelKayit = {};

        var Key = '';
        if (window.location.href.indexOf('?') > -1)
        {
            var Parametre = window.location.href.substr(window.location.href.indexOf('?') + 1, window.location.href.length);
            Key = Parametre.split('&')[1].split('=')[1];
        }
        $scope.Key = Key;

        if (Key > 0) // yeni kayıtsa veri çekmeye gerek yok
        {
            KullaniciAramaService.KullaniciGetir($scope, Key, function (data)
            {
                $scope.$parent.ArkaPlaniAcikMi = true;
                if (data != null)
                {
                    console.log(data);
                    $scope.KullaniciModelKayit = data;
                }
                else
                {
                    alert('Sistem hatası: data=null');
                }
            });
        }

        var KullaniciTipNoGeldi = false;
        var AktifPasifTipNoGeldi = false;
        KullaniciAramaService.KullaniciLookGetir('Yetki', 'KullaniciTipNo', function (data)
        {
            console.log('KullaniciLookGetir Yetki KullaniciTipNo sonuç geldi');
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModelKayit.KullaniciTipNolar = data;

            // debugger;
            KullaniciTipNoGeldi = true;
            if (KullaniciTipNoGeldi && AktifPasifTipNoGeldi)
                $scope.$parent.ArkaPlaniAcikMi = false;
        });

        KullaniciAramaService.KullaniciLookGetir('Yetki', 'AktifPasifTipNo', function (data)
        {
            console.log('KullaniciLookGetir Yetki KullaniciTipNo sonuç geldi');
            data.unshift({ LookNo: 0, UzunAd: 'Seçiniz...' });
            $scope.KullaniciModelKayit.AktifPasifTipNoNolar = data;

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
            KullaniciAramaService.KullaniciKaydet($scope, Key, function (data)
            {
                // alert('data.Durum:' + data.Durum);
                if (data.Durum)
                    parent.KayitGuncellendi(Key, data.Model);
                else if (data.Sonuc != 'Iptal')
                    alert('Uyarı: ' + data.Sonuc);
                $scope.$parent.ArkaPlaniAcikMi = false;
            });
        };
    })

    .controller('ngConOkuma', function ($scope, $http, KullaniciAramaService)
    {
        $scope.RCSil = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            KullaniciAramaService.KullaniciSil($scope, Key, function (data)
            {
                if (data.Durum)
                    parent.KayitSilindi(Key);
                else if (data.Sonuc != 'Iptal')
                    alert('Uyarı: ' + data.Sonuc);
                $scope.$parent.ArkaPlaniAcikMi = false;
            });
        };
    });

function KullaniciSil(Key)
{
    alert('çöp fonksyon');
}

