(function(app) {
    "use strict";

    app.controller("waterController", waterController);

    function waterController($scope) {
        
        //default values
        $scope.bucket1 = 3;
        $scope.bucket2 = 5;
        $scope.water = 4;
        $scope.nrSolutions = 2;

        //can send a request only if quantity of water wanted is <= max( buckets )
        $scope.disabled = false;
        $scope.$watchGroup(['bucket1', 'bucket2', 'water'], function () {
            $scope.disabled = !($scope.water <= Math.max($scope.bucket1, $scope.bucket2));
        });

        //format textboxes
        $scope.options = {
            format: "# liters",
            decimals: 0
        }
        $scope.optionsSolutions = {
            format: "# solutions",
            decimals: 0
        }
        
        $scope.searchSolution = function () {
            //clear current content
            $("#solutions").html("");

            var uri = "/api/watersolver";
            var params =
            {
                bucket1: $scope.bucket1,
                bucket2: $scope.bucket2,
                quantity: $scope.water,
                nrSolutions: $scope.nrSolutions
            };
            // Send an AJAX request
            $.getJSON(uri, params)
                .done(function (data) {
                    // On success, 'data' contains a list of solutions.
                    if (data.length === 0) {
                        $('<h3>', { class: "text-center", text: "No solution found!" }).appendTo($('#solutions'));
                    } else {
                        $.each(data, function (key, item) {
                            // Add a path for solution.
                            $('<div>', { class: "text-center label-info lead", text: "New Solution" }).appendTo($('#solutions'));
                            //$('<div>', { text: formatState(item) }).appendTo($('#solutions'));
                            formatState(item);
                        });
                    }
                });
        }
    }

    function formatBucketItem(item, maxValue) {
        if (item == 0) {
            $('<div>', { class: "bucket bucket-empty", text: item }).appendTo($('#solutions'));
        } else if (item == maxValue) {
            $('<div>', { class: "bucket bucket-full", text: item }).appendTo($('#solutions'));
        } else {
            $('<div>', { class: "bucket bucket-half", text: item }).appendTo($('#solutions'));
        }
    }
    
    function formatState(item) {
        if (item.parent != undefined && item.parent != null) {
            return formatState(item.parent) + $('<div>', { class: "bucket bucket-action", text: " --- " + item.operation + " ---> " }).appendTo($('#solutions')) + formatBucketItem(item.bucket1, $("#bucket1").val()) + formatBucketItem(item.bucket2, $("#bucket2").val());
        } else {
            return formatBucketItem(item.bucket1, $("#bucket1").val()) + formatBucketItem(item.bucket2, $("#bucket2").val());
        }
    }

})(angular.module('waterApp', ["kendo.directives"]));