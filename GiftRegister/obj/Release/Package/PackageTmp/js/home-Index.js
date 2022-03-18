console.log('test1')
var module = angular.module("homeIndex", []);
console.log('test2')
module.config(function ($routeProvider) {

    $routeProvider.when("/", {
        controller: "newGiftsReceivedController",
        templateUrl: "/templates/newGiftView.html"
    });
    $routeProvider.when("/newGiftGiven", {
        controller: "newGiftsReceivedController",
        templateUrl: "/templates/newGiftGivenView.html"
    });

    $routeProvider.when("/newGiftReceived", {
        controller: "newGiftsReceivedController",
        templateUrl: "/templates/newGiftReceivedView.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });
});


module.factory("dataService", function ($http, $q) {

    var _gifts = "";
    var _isInit = false;
    var _currentname = [];
    var _isReady = function () {

        return _isInit;
    }

    var _getGifts = function () {

        var deferred = $q.defer();

        $http.get("/Home/GetCurrentName")
           .then(function (result) {
               //Successful

               angular.copy(result.data, _gifts);
               _isInit = true;
               deferred.resolve();
           },
           function () {
               //Error
               deferred.reject();
           });
        return deferred.promise;
    };
    var _addGiftReceived = function (newGiftReceived) {

        var receive = $q.defer();
        $http.post("/api/Gifts", newGiftReceived)
             .then(function (result) {
                 //success
                 var newlyCreatedReceivedGift = result.data;
                 _gifts.splice(0, 0, newlyCreatedReceivedGift);
                 receive.resolve(newlyCreatedReceivedGift);

             },
             function () {
                 //error
                 receive.reject();
             });
        return receive.promise;
    }


    var _addGiftGiven = function (newGiftGiven) {

        var give = $q.defer();

        $http.post("/api/Gifts", newGiftGiven)
             .then(function (result) {
                 //success
                 var newlyCreatedGivenGift = result.data;
                 _gifts.splice(0, 0, newlyCreatedGivenGift);
                 give.resolve(newlyCreatedGivenGift);

             },
             function () {
                 //error
                 give.reject();
             });
        return give.promise;
    }


    return {
        gifts: _gifts,
        addGiftReceived: _addGiftReceived,
        getGifts: _getGifts,
        addGiftGiven: _addGiftGiven,
        isReady: _isReady
    };
});

(function (app) {
    "use strict";
    app.controller("newGiftsReceivedController", function ($scope, $http, $window, dataService, $timeout, $q) {
        //$scope.onlyNumbers = /^[1-9]+[0-9]*$/;
        $scope.newGiftReceived = {};
        $scope.newGiftGiven = {};
        $scope.newGiftReceived.NameOfEmployee = dataService.gifts;
        dataService.getGifts()
                .then(function () {
                    //success
                },
                   function () {
                       //error
                       //  alert("could not load gifts");
                   })
                .then(function () {
                    $scope.isBusy = false;
                });

        $scope.Receive = function () {

            dataService.addGiftReceived($scope.newGiftReceived)
            .then(function () {
                //success
                $window.location = "#/"
            },
            function () {
                //error
                //alert("Could not save the new gift");
            }
            )

        };

        $scope.Give = function () {

            dataService.addGiftGiven($scope.newGiftGiven)
            .then(function () {
                //success
                $window.location = "#/"
            },
            function () {
                //error
                //alert("Could not save the new gift");
            }
            )

        };

    });

    app.directive('receivedatepicker', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                element.datepicker({
                    dateFormat: 'yy-mm-dd',
                    onSelect: function (date) {
                        scope.newGiftReceived.DateGiftWasReceived = date;
                        scope.$apply();
                    }
                });
            }
        };
    });

    app.directive('givedatepicker', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                element.datepicker({
                    dateFormat: 'yy-mm-dd',
                    onSelect: function (date) {
                        scope.newGiftGiven.DateGiftWasGiven = date;
                        scope.$apply();
                    }
                });
            }
        };
    });

})(module);
