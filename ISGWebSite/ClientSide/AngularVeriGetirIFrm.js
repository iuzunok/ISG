appGenel
    .controller('AngGenelController', function ()
    {
        // $scope.ArkaPlaniAcikMi = true;
        //$scope.$watch("ArkaPlaniAcikMi", function (newValue, oldValue, scope)
        //{
        //    console.log("Eski Değer: " + oldValue + " Yeni Değer: " + newValue);
        //}, true);
    })

    .controller('ngConKullaniciAra', function ($scope, KullaniciAramaService)
    {
        $scope.ArkaPlaniAcikMi = true;
        $scope.KullaniciModelAra = {
            // alan dolu gelmesi için
            // KullaniciAd: 'i'
        };
        $scope.KullaniciModelAraSonuc = {};
        $scope.KullaniciModelAraSonucPage = [];
        $scope.KullaniciModelKayit = {};

        // sıralama
        $scope.Sirala = function (SiraAlan)
        {
            // debugger;
            $scope.SiralamaAlan = SiraAlan;
            $scope.TersSira = !$scope.TersSira;
            // alert($scope.TersSira);

            // var order = !column.orderDesc ? 1 : -1;
            $scope.KullaniciModelAraSonuc.sort(function (a, b)
            {
                if ($scope.TersSira)
                    return b[SiraAlan].localeCompare(a[SiraAlan]);
                else
                    return a[SiraAlan].localeCompare(b[SiraAlan]);
            });

            Sayfala(true);
        };

        $scope.KullaniciAra = function ()
        {
            $scope.$parent.ArkaPlaniAcikMi = true;
            KullaniciAramaService.KullaniciAra($scope, function (data)
            {
                // debugger;
                $scope.KullaniciModelAraSonuc = data;
                if ($scope.KullaniciModelAraSonuc != null)
                {
                    // sayfalama
                    $scope.totalItems = $scope.KullaniciModelAraSonuc.length;
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 4;

                    $scope.$watch('currentPage + itemsPerPage', function ()
                    {
                        Sayfala(true);
                    });
                }
                else
                    $scope.KullaniciModelAraSonucPage = null;
                $scope.$parent.ArkaPlaniAcikMi = false;
            });
        };

        Sayfala = function (SayfaYap)
        {
            // debugger;
            if (SayfaYap)
            {
                var begin = (($scope.currentPage - 1) * $scope.itemsPerPage);
                var end = begin + $scope.itemsPerPage;
                $scope.KullaniciModelAraSonucPage = $scope.KullaniciModelAraSonuc.slice(begin, end);
            }
            else
                $scope.KullaniciModelAraSonucPage = $scope.KullaniciModelAraSonuc;
        }

        $scope.RCInY = function ()
        {
            $scope.ArkaPlaniAcikMi = true;
            var $arifPopUp = $('#arifPopUp');
            // var options = { "backdrop": "static", keyboard: true };
            // $('#argemModalPopUp').css('opacity', '100').show();

            oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciKayit?Durum=I&Key=0').width(800).height(500);

            ////removes the "active" class to .popup and .popup-content when the "Close" button is clicked 
            //$(".close, .popup-overlay").on("click", function ()
            //{
            //    $(".popup-overlay, .popup-content").removeClass("active");
            //    $arifPopUp.attr('src', '');
            //});

            // .width(600).height(400);


            $scope.ArkaPlaniAcikMi = false;
        };

        $scope.RCGuY = function (Key)
        {
            // debugger;
            $scope.ArkaPlaniAcikMi = true;
            var $arifPopUp = $('#arifPopUp');
            /*var options = { "backdrop": "static", keyboard: true };
            $('#argemModalPopUp').modal(options).modal('toggle')
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciKayit?Durum=U&Key=' + Key);*/
            oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciKayit?Durum=U&Key=' + Key).width(800).height(500);

            $scope.ArkaPlaniAcikMi = false;
        };

        $scope.RCOkY = function (Key)
        {
            $scope.ArkaPlaniAcikMi = true;
            var $arifPopUp = $('#arifPopUp');
            /*var options = { "backdrop": "static", keyboard: true };
            $('#argemModalPopUp').modal(options).modal('toggle')
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + Key);*/
            oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciOku?Durum=O&Key=' + Key).width(800).height(500);

            $scope.ArkaPlaniAcikMi = false;
        };

        $scope.RCSiY = function (Key)
        {
            $scope.ArkaPlaniAcikMi = true;
            var $arifPopUp = $('#arifPopUp');
            /*var options = { "backdrop": "static", keyboard: true };
            $('#argemModalPopUp').modal(options).modal('toggle')
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciOku?Durum=D&Key=' + Key);*/
            oArgemModal = ArgemModal.open({ width: "816px", height: "516px" });
            $arifPopUp.attr('src', '/Yetki/Kullanici/KullaniciOku?Durum=D&Key=' + Key).width(800).height(500);
            $scope.ArkaPlaniAcikMi = false;
        };

        $scope.RCSec = function (Key)
        {
            window.location.href = '/Yetki/Kullanici/Kullanici?Key=' + Key;
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

        $scope.ExportPDF = function ()
        {
            var docDefinition = {
                header: { text: 'Kullanıcı Listesi', margin: [250, 10, 0, 0] },
                content: [
                    {
                        table: {
                            headerRows: 1,
                            widths: [130, 100, 100, 80, 80],
                            body: [
                                ['Kullanıcı Adı', 'Adı', 'Soyadı', 'Tipi', 'Durumu'],
                                // [{ text: 'Bold value', bold: true }, 'Val 2', 'Val 3', 'Val 4']
                            ]
                        }
                    }
                ],
                footer: function (currentPage, pageCount)
                {
                    return { text: currentPage + '/' + pageCount, alignment: 'right', margin: [0, 0, 50, 0] };
                }
            };

            $scope.KullaniciModelAraSonuc.forEach(function (Veri)
            {
                docDefinition.content[0].table.body.push([Veri.KullaniciAd, Veri.Ad, Veri.Soyad, Veri.KullaniciTipNoUzunAd, Veri.AktifPasifTipNoUzunAd]);
            });

            pdfMake.createPdf(docDefinition).download('Kullanici.pdf');
        };

        $scope.ExportExcel = function ()
        {
            // debugger;
            Sayfala(false);
            $scope.$apply();
            var tblSonuc = document.getElementById('tblSonuc');
            var wb = XLSX.utils.table_to_book(tblSonuc, { sheet: "Sayfa1" });
            XLSX.writeFile(wb, 'Sonuc.xlsx');
            Sayfala(true);

            // direk $scope çalışıyor fakat kolon adları düzgün gelmiyor ve sıralama da yok
            var ws = XLSX.utils.json_to_sheet($scope.KullaniciModelAraSonuc);
            var wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, "Kullanıcı");
            XLSX.writeFile(wb, 'Sonuc.xlsx');
        };

        // sayfa ilk açıldığında otomotik yüklensin diye
        $scope.KullaniciAra();
    })

    .controller('ngConKullaniciKayit', function ($scope, KullaniciAramaService)
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
            KullaniciAramaService.KullaniciGetir(Key, function (data)
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

    .controller('ngConOkuma', function ($scope, KullaniciAramaService)
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
    })
