// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.
//$(function () {
//    $('button[data-toggle="ajax-modal').click(function (e) {
//        var url = $(this).data('url');
//        $.get(url).done(function (data) {
//            PlaceHolderElement.html(data);
//            PlaceHolderElement.find('.modal').modal('show');
//        })
//    })
//})

//$(document).ready(function () {
//    $('#myModal').modal({
//        backdrop: 'static',
//        keyboard: false
//    })
//});

//$(document).ready(function () {
//    $("#save-button").click(function (event) {
//        console.log("submit");
//        event.preventDefault();
//        console.log($(this));

//        if ($(this).valid()) {
//            $.ajax({
//                url: "http://localhost:5001/api/Room/add-new-room?Code=@room.Code&FeeAppliedDate=@room.FeeAppliedDate&RentFee=@room.RentFee&Status=1",
//                type: "POST",
//                headers: {
//                    "Authorization": "Bearer @myCookie",
//                },
//                success: function (result) {
//                    $("#myModal").fadeOut();
//                    // Reload the list of rooms on the main page
//                    //window.location.reload();
//                },
//                error: function (xhr, status, error) {
//                    alert("Error creating room: " + error);
//                },
//            });
//        }

//    });
//});


