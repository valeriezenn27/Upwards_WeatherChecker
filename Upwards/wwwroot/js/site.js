// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('input:file').on("change", function () {
    $('input:submit').prop('disabled', !$(this).val());
});

function on() {
    document.getElementById("overlay").style.display = "flex";
}

function off() {
    document.getElementById("overlay").style.display = "none";
}