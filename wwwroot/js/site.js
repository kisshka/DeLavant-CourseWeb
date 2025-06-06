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
  $(".overlay").addClass("hidden");
  $(".hamburger").click(function () {
    if (Closed) {
      $(this).removeClass("open");
      $(this).addClass("closed");
      $(".overlay").addClass("hidden"); // Скрываем
      Closed = false;
    } else {
      $(this).removeClass("closed");
      $(this).addClass("open");
      $(".overlay").removeClass("hidden"); // Показываем
      Closed = true;
    }
  });
});
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

function autoResize(elem) {
    elem.style.height = 'auto';
    elem.style.height = (elem.scrollHeight-7) + 'px';
}

document.querySelectorAll(".menu-item").forEach((item) => {
        item.addEventListener("click", function () {
          // Удаляем активный класс у всех элементов
          document.querySelectorAll(".menu-item").forEach((el) => {
            el.classList.remove("active");
          });

          // Добавляем активный класс текущему элементу
          this.classList.add("active");

          // Перемещаем красный фон
          const index = parseInt(this.dataset.index);
          const highlight = document.querySelector(".highlight");
          highlight.style.transform = `translateY(-50%) translateX(${
            index * 100
          }%)`;
        });
      });