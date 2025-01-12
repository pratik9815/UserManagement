$.validator.addMethod("regex", function (value, element) {
    return value === "" || /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/.test(value);
});

$.validator.unobtrusive.adapters.addBool("regex")