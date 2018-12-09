'use strict';
var appGenel = angular.module('ngaGenel', ['ui.bootstrap'])
    .controller('ngcGenel', function ()
    {
        $scope.SCS = 0;
        $scope.CCS = 0;
    })

    .directive('onFinishRender', function ($timeout)
    {
        return {
            restrict: 'A',
            link: function (scope, element, attr)
            {
                if (scope.$last === true)
                {
                    $timeout(function ()
                    {
                        scope.$emit('ngRepeatFinished');
                    });
                }
            }
        }
    })

    /*.directive('myPostRepeatDirective', function ()
   {
       return function ($scope, element, attrs)
       {
           if ($scope.$last)
           {
               $scope.$parent.$parent.CCS = Math.round(performance.now() - $scope.BasTime);
           }
       };
   })*/


    .factory("GenelService", ['$http', function ($http)
    {
        var fac = {};

        fac.Sayfala = function ($scope, SayfaYap, Model, ModelPage)
        {
            if (SayfaYap)
            {
                var begin = (($scope.AktifSayfaNo - 1) * $scope.SayfaKayitAdet);
                var end = begin + $scope.SayfaKayitAdet;
                ModelPage = Model.slice(begin, end);
            }
            else
                ModelPage = Model;

            return ModelPage;
        };

        return fac;
    }]);
