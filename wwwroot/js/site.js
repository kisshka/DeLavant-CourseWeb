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
