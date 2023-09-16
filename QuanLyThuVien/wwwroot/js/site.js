// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Hàm xử lý form delete khi submit
function handleSubmitForm() {
    const modal = document.querySelector(".modal")
    modal.classList.add("d-block")
    modal.onclick = function (e) {
        if (e.target.value == "close") {
            modal.classList.remove("d-block")
        } else if (e.target.value == "submit") {
            document.getElementById("form-delete").submit();
        }
    }
}