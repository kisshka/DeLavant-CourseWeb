$(document).ready(function () {
    function updatePartialView() {
        var selectedValue = $('input[name="accessType"]:checked').val();
        $('#SelectedAccessType').val(selectedValue);

        $.ajax({
            url: '/Course/Access',
            data: { accessType: selectedValue },
            type: 'POST',
            success: function (response) {
                $('#responseArea').html(response);
            }
        });
    };
    $('input[name="accessType"]').on('change', function () {
        updatePartialView();
    });
    updatePartialView();
});
