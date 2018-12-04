'use strict';
var ngAppLogin = angular.module('ngaGenel', ['ui.bootstrap'])
    .controller('ngcGenel', function ()
    {
    })

    .controller('ngcYetkiGrupAra', function ($scope, YetkiGrupService)
    {
        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.oYetkiGrupAraModel = {};
        $scope.oYetkiGrupAraModelPage = [];

        $scope.Sirala = function (SiraAlan)
        {
            $scope.SiralamaAlan = SiraAlan;
            $scope.TersSira = !$scope.TersSira;

            $scope.oYetkiGrupAraModel.sort(function (a, b)
            {
                if ($scope.TersSira)
                    return b[SiraAlan].localeCompare(a[SiraAlan]);
                else
                    return a[SiraAlan].localeCompare(b[SiraAlan]);
            });

            Sayfala($scope, true);
        };

        $scope.SayfalamaDegisti = function ()
        {
            // console.log('sayfa:' + $scope.AktifSayfaNo);
            Sayfala($scope, true);
        };

        $scope.YetkiGrupAra = function ()
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            YetkiGrupService.YetkiGrupAra($scope, function (data)
            {
                if (data != null)
                {
                    if (data.Durum == '')
                    {
                        $scope.oYetkiGrupAraModel = data.Data;
                        $scope.ToplamKayitAdet = $scope.oYetkiGrupAraModel.length;
                        $scope.AktifSayfaNo = 1;
                        $scope.SayfaKayitAdet = 50;
                        Sayfala($scope, true);
                    }
                    else
                    {
                        $scope.HataVar = true;
                        $scope.HataAciklama = data.Aciklama;
                        $scope.oYetkiGrupAraModelPage = null;
                    }
                }
                else
                {
                    $scope.oYetkiGrupAraModelPage = null;
                    $scope.HataVar = true;
                    $scope.HataAciklama = 'Sistem hatası. data=null';
                }

                $scope.$parent.ArkaPlaniAcikMi = false;
            });
        };

        $scope.RCIn = function ()
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            //var $arifPopUp = $('#arifPopUp');
            //oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            //$arifPopUp.attr('src', '/Yetki/YetkiGrup/YetkiGrupKayit?Durum=I&Key=0').width(800).height(500);
            //$scope.ArkaPlaniAcikMi = false;
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/YetkiGrup/YetkiGrupKayit?Durum=I&Key=0', 800, 500, false, true, 'Yeni Kullanıcı Kayıt', '', '');
        };

        $scope.RCGu = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            //var $arifPopUp = $('#arifPopUp');
            //oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            //$arifPopUp.attr('src', '/Yetki/YetkiGrup/YetkiGrupKayit?Durum=U&Key=' + Key).width(800).height(500);
            //$scope.ArkaPlaniAcikMi = false;
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/YetkiGrup/YetkiGrupKayit?Durum=U&Key=' + Key, 800, 500, false, true, 'Yeni Kullanıcı Kayıt', '', '');
        };

        $scope.RCOk = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            //var $arifPopUp = $('#arifPopUp');
            //oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            //$arifPopUp.attr('src', '/Yetki/YetkiGrup/YetkiGrupOku?Durum=O&Key=' + Key).width(800).height(500);
            //$scope.ArkaPlaniAcikMi = false;
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/YetkiGrup/YetkiGrupOku?Durum=O&Key=' + Key, 800, 500, false, true, 'Yeni Kullanıcı Kayıt', '', '');
        };

        $scope.RCSi = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            //var $arifPopUp = $('#arifPopUp');
            //oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            //$arifPopUp.attr('src', '/Yetki/YetkiGrup/YetkiGrupOku?Durum=D&Key=' + Key).width(800).height(500);
            //$scope.ArkaPlaniAcikMi = false;
            oArgemModal = ArgemMPUtil.PopUpAc($scope, '/Yetki/YetkiGrup/YetkiGrupOku?Durum=D&Key=' + Key, 800, 500, false, true, 'Yeni Kullanıcı Kayıt', '', '');
        };

        $scope.RCSec = function (Key)
        {
            window.location.href = '/Yetki/YetkiGrup/YetkiGrup?Key=' + Key;
        };

        $scope.ExportPDF = function ()
        {
            var docDefinition = {
                defaultStyle: { fontSize: 10 },
                header: { text: 'Yetki Grup Listesi', margin: [250, 10, 0, 0] },
                content: [
                    {
                        table: {
                            headerRows: 1,
                            widths: [50, '*'],
                            body: [
                                ['S.No', 'Grup Adı']
                            ]
                        }
                    }
                ],
                footer: function (AktifSayfaNo, SayfaAdet)
                {
                    return { text: AktifSayfaNo + '/' + SayfaAdet, alignment: 'right', margin: [0, 10, 50, 0] };
                }
            };

            $scope.oYetkiGrupAraModel.forEach(function (Veri, index)
            {
                docDefinition.content[0].table.body.push([String(index + 1), Veri.YetkiGrupAd]);
            });

            pdfMake.createPdf(docDefinition).download('YetkiGrup.pdf');
        };

        $scope.ExportExcel = function ()
        {
            Sayfala($scope, false);
            $scope.$apply();
            var tblSonuc = document.getElementById('tblSonuc');
            var wb = XLSX.utils.table_to_book(tblSonuc, { sheet: "Sayfa1" });
            XLSX.writeFile(wb, 'Sonuc.xlsx');
            Sayfala($scope, true);

            // direk $scope çalışıyor fakat kolon adları düzgün gelmiyor ve sıralama da yok
            var ws = XLSX.utils.json_to_sheet($scope.oYetkiGrupAraModel);
            var wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, "Kullanıcı");
            XLSX.writeFile(wb, 'YetkiGrup.xlsx');
        };

        // sayfa ilk açıldığında otomotik yüklensin diye
        $scope.YetkiGrupAra();
    })

    .controller('ngcYetkiGrupKayit', function ($scope, YetkiGrupService)
    {
        $scope.$parent.ArkaPlaniAcikMi = true;
        $scope.YetkiGrupModel = {};

        var Key = '';
        if (window.location.href.indexOf('?') > -1)
        {
            var Parametre = window.location.href.substr(window.location.href.indexOf('?') + 1, window.location.href.length);
            Key = Parametre.split('&')[1].split('=')[1];
        }
        $scope.Key = Key;

        if (Key > 0) // yeni kayıtsa veri çekmeye gerek yok
        {
            YetkiGrupService.YetkiGrupGetir(Key, function (data)
            {
                $scope.$parent.ArkaPlaniAcikMi = false;
                if (data != null)
                {
                    if (data.Durum == '')
                    {
                        console.log(data.Data);
                        $scope.YetkiGrupModel = data.Data[0];
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
        }
        else
            $scope.$parent.ArkaPlaniAcikMi = false;

        $scope.YetkiGrupKaydet = function (FormValid)
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
            YetkiGrupService.YetkiGrupKaydet($scope, Key, function (data)
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

    .controller('ngcOkuma', function ($scope, YetkiGrupService)
    {
        $scope.RCSil = function (Key)
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            YetkiGrupService.YetkiGrupSil($scope, Key, function (data)
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


    .factory("YetkiGrupService", ['$http', function ($http)
    {
        var fac = {};

        fac.YetkiGrupGetir = function (Key, CallBack)
        {
            $http.post("/Yetki/YetkiGrup/YetkiGrupAraSonuc", { YetkiGrupKey: Key })
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

        fac.YetkiGrupAra = function ($scope, CallBack)
        {
            $http.post("/Yetki/YetkiGrup/YetkiGrupAraSonuc", $scope.YetkiGrupModel)
                .then(function (response)
                {
                    console.log('ara ok:');
                    // console.log(response.data);
                    CallBack(response.data);
                })
                .catch(function (response)
                {
                    console.log('ara hata: ' + response.data);
                    // alert("Hata : " + response.data);
                    CallBack(null);
                });
        };

        fac.YetkiGrupSil = function ($scope, Key, CallBack)
        {
            if (confirm("Yetki grup kaydını silmek istediğinize emin misiniz?"))
            {
                $scope.$parent.ArkaPlaniAcikMi = true;

                $http.get("/Yetki/YetkiGrup/YetkiGrupSil?Key=" + Key)
                    .then(function (response)
                    {
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
                        console.log('sil error...' + response.message);
                        $scope.ArkaPlaniAcikMi = false;
                        CallBack({ Durum: false, Key: Key, Sonuc: response.message });
                    });
            }
            else
                CallBack({ Durum: false, Key: Key, Sonuc: 'Iptal' });
        };

        fac.YetkiGrupKaydet = function ($scope, Key, CallBack)
        {
            if (confirm("Yetki grup kaydını güncellemek istediğinize emin misiniz?"))
            {
                $scope.$parent.ArkaPlaniAcikMi = true;

                $http({
                    method: 'Post',
                    url: '/Yetki/YetkiGrup/YetkiGrupKayit',
                    // contentType: 'application/x-www-form-urlencoded; charset-UTF-8',
                    data: $scope.YetkiGrupModel
                })
                    .then(function (response)
                    {
                        if (response.data.Durum == '')
                            CallBack({ Durum: true, Key: Key, Sonuc: '', Model: $scope.YetkiGrupModel });
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

        return fac;
    }]);


var Sayfala = function ($scope, SayfaYap)
{
    if (SayfaYap)
    {
        var begin = (($scope.AktifSayfaNo - 1) * $scope.SayfaKayitAdet);
        var end = begin + $scope.SayfaKayitAdet;
        $scope.oYetkiGrupAraModelPage = $scope.oYetkiGrupAraModel.slice(begin, end);
    }
    else
        $scope.oYetkiGrupAraModelPage = $scope.oYetkiGrupAraModel;
};

var KayitGuncellendi = function (Key, Model)
{
    var $scope = GetScope('ngcYetkiGrupAra');

    // arama listesinide güncelle
    var ix = -1;
    for (var i = 0; i < $scope.oYetkiGrupAraModel.length; i++)
    {
        if ($scope.oYetkiGrupAraModel[i].YetkiGrupKey == Key)
        {
            ix = i;
            break;
        }
    }
    if (ix > -1)
    {
        $scope.oYetkiGrupAraModel[ix].YetkiGrupAd = Model.YetkiGrupAd;
    }
    $scope.$apply();

    ArgemModalPopUpKapat();
};

var KayitSilindi = function (Key)
{
    //    debugger;
    var $scope = GetScope('ngcYetkiGrupAra');

    // arama sonucu listesinden de sil
    var ix = -1;
    for (var i = 0; i < $scope.oYetkiGrupAraModel.length; i++)
    {
        if ($scope.oYetkiGrupAraModel[i].YetkiGrupKey == Key)
        {
            ix = i;
            break;
        }
    }
    if (ix > -1)
    {
        $scope.oYetkiGrupAraModel.splice(ix, 1);
        $scope.ToplamKayitAdet--;

        if ($scope.oYetkiGrupAraModelPage.length == 1 && $scope.AktifSayfaNo > 1) // sayfadaki son kayıt ise önceki sayfa varsa ona git
            $scope.AktifSayfaNo--;

        Sayfala($scope, true);
        $scope.$apply();
    }
    ArgemModalPopUpKapat();
};