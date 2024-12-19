$(document).ready(function () {
    // Show additional fields on initial registration button click
    $('#initialRegisterButton').on('click', function (event) {
        event.preventDefault();
        $('#additionalFields').fadeIn();
    });

    // Submit final registration form
    $('#finalRegisterButton').on('click', function (event) {
        event.preventDefault();

        // Gather all form data
        const formData = new FormData();
            formData.append('username', $('#register-username').val());
            formData.append('email', $('#register-email').val());
            formData.append('password', $('#register-password').val());
            formData.append('confirmPassword', $('#confirm-password').val());
            formData.append('visibilityNo', $('#visibility-no').val());
            formData.append('intentNo', $('#intent-no').val());
            formData.append('postalCode', $('#postal-code').val());
            formData.append('street1', $('#street1').val());
            formData.append('street2', $('#street2').val());
            formData.append('city', $('#city').val());
            formData.append('state', $('#state').val());
            formData.append('country', $('#country').val());
            formData.append('birthday', $('#birthday').val());
            formData.append('profileImage', $('#profile-image')[0].files[0]);

        // Basic validation
        if (!formData.password || formData.password !== formData.confirmPassword) {
            $('#register-message').html('<p class="error">Passwords do not match!</p>');
            return;
        }

        // AJAX call to backend
        $.ajax({
            type: 'POST',
            url: '/api/register', // Replace with your backend endpoint
            data: JSON.stringify(formData),
            contentType: 'application/json',
            success: function (response) {
                $('#register-message').html('<p>Registration successful!</p>');
                window.location.href = '/login'; // Redirect on success
            },
            error: function (xhr, status, error) {
                $('#register-message').html('<p>Registration failed: ' + error + '</p>');
            }
        });
    });
});
