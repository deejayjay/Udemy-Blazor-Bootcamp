/* 
    Code to show toastr notifications
    Ref: https://github.com/CodeSeven/toastr
*/
function showToastr(type, message) {
    if (type.toLowerCase() === "success") {
        // Display a success toast, with a title
        toastr.success(message, 'Operation Successful', { timeout: 3000 });
    } else if (type.toLowerCase() === "error") {
        toastr.error(message, 'Operation Failed', { timeout: 3000 });
    }
}

/*
    Code to show success/error sweet alerts
    Ref: https://sweetalert2.github.io/
*/
function showSweetAlert(type, title, message) {
    if (type.toLowerCase() === "success") {
        Swal.fire({
            title: title,
            text: message,
            icon: 'success'
        })
    } else if (type.toLowerCase() === "error") {
        Swal.fire({
            title: title,
            text: message,
            icon: 'error'
        })
    }
}