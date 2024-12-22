$(document).ready(function() {
    $('#loginForm').submit(function(event) {
        event.preventDefault(); 

        const formData = {
            username: $('#username').val(),
            password: $('#password').val()
        };

        $.ajax({
            type: 'POST',
            url: '/api/login', 
            data: JSON.stringify(formData),
            contentType: 'application/json',
            success: function(response) {
                // Assuming your API sends a token on successful login
                localStorage.setItem('authToken', response.token); 

                $('#message').html('<p>Login successful!</p>');
                window.location.href = '/dashboard'; 
            },
            error: function(xhr, status, error) {
                $('#message').html('<p>Login failed: ' + error + '</p>'); 
            }
        });
    });
});