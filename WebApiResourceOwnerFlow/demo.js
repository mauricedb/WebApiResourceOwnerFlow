$(function () {

    $('#btnUnsecure').click(function () {
        $.ajax('/api/demo').then(
            function (data) {
                $('#result').text(data);
            },
            function (err) {
                $('#result').text(JSON.stringify(err));
            }
        );

    });

    $('#btnLogin').click(function () {
        $.ajax('/token', {
            type: 'post',
            data: {
                username: 'maurice',
                password: 'pass',
                grant_type: 'password'
            }
        }).then(function(e) {
            sessionStorage.token_type = e.token_type;
            sessionStorage.access_token = e.access_token;
        });
    });

    $('#btnSecure').click(function () {
        $.ajax('/api/demo', {
            headers: {
                Authorization: sessionStorage.token_type + ' ' + sessionStorage.access_token
            }
        }).then(
            function (data) {
                $('#result').text(data);
            },
            function (err) {
                $('#result').text(JSON.stringify(err));
            }
        );
    });
});
