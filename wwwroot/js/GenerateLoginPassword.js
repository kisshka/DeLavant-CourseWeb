$(document).ready(function () {
    $('#generateBtn').click(function (event) {
        event.preventDefault();

        // Получаем URL из data-атрибута кнопки
        var generateUrl = $(this).data('generate-url');

        $.ajax({
            url: generateUrl,
            method: 'POST',
            success: function (response) {
                if (response.success) {
                    $('#usernameField').val(response.username);
                    $('#passwordField').val(response.password);
                    $('#confirmPasswordField').val(response.password);
                } else {
                    alert('Ошибка генерации!');
                }
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});

function toggleVisibility(fieldID) {
    var field = document.getElementById(fieldID);
    var button = field.nextElementSibling;
    if (field.type === 'password') {
        field.type = 'text';
        button.textContent = 'Скрыть пароль';
    } else {
        field.type = 'password';
        button.textContent = 'Показать пароль';
    }
}