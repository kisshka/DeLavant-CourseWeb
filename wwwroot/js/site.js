function filterCategories() {
  const input = document.getElementById("search-post");
  const filter = input.value.trim().toLowerCase();
  const select = document.getElementById("post-list");
  const options = select.querySelectorAll("option");

  options.forEach((option) => {
    const text = option.textContent.trim().toLowerCase();
    if (text.includes(filter)) {
      option.style.display = "";
    } else {
      option.style.display = "none";
    }
  });
}

$(document).ready(function () {
  var Closed = false;
  $(".admin-menu").addClass("hidden");
  $(".hamburger").click(function () {
    if (Closed) {
      $(this).removeClass("open");
      $(this).addClass("closed");
      $(".admin-menu").addClass("hidden"); // Скрываем
      Closed = false;
    } else {
      $(this).removeClass("closed");
      $(this).addClass("open");
      $(".admin-menu").removeClass("hidden"); // Показываем
      Closed = true;
    }
  });
});

$(document).ready(function () {
    $('input[name="accessType"]').on('change', function () {
        var selectedValue = $(this).val();
        $('#SelectedAccessType').val(selectedValue);
        $.ajax({
            url: '/Course/Access',
            data: { accessType: selectedValue },
            type: 'POST',
            success: function (response) {
                $('#responseArea').html(response);
            },
            error: function (xhr, status, error) {
                alert('Ошибка! Проверьте консоль.');
                console.log(status + ': ' + error);
            }
        });
    });
    $('input[name="accessType"][value="Common"]').trigger('change');
});