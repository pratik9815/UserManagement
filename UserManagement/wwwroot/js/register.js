

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('togglePassword').addEventListener('click', togglePassword);
    document.getElementById('toggleConfirmPassword').addEventListener('click', toggleConfirmPassword);
});
// Function to toggle password visibility
function togglePassword() {
    var passwordField = document.getElementById("password");
    var eyeIcon = document.getElementById("eyeIcon");

    if (passwordField.type === "password") {
        passwordField.type = "text";
        eyeIcon.classList.remove("fa-eye");
        eyeIcon.classList.add("fa-eye-slash");
    } else {
        passwordField.type = "password";
        eyeIcon.classList.remove("fa-eye-slash");
        eyeIcon.classList.add("fa-eye");
    }
}

// Function to toggle confirm password visibility
function toggleConfirmPassword() {
    var confirmPasswordField = document.getElementById("confirmPassword");
    var eyeIconConfirm = document.getElementById("eyeIconConfirm");

    if (confirmPasswordField.type === "password") {
        confirmPasswordField.type = "text";
        eyeIconConfirm.classList.remove("fa-eye");
        eyeIconConfirm.classList.add("fa-eye-slash");
    } else {
        confirmPasswordField.type = "password";
        eyeIconConfirm.classList.remove("fa-eye-slash");
        eyeIconConfirm.classList.add("fa-eye");
    }
}