 function filterCategories() {
        const input = document.getElementById('search-post');
    const filter = input.value.trim().toLowerCase();
    const select = document.getElementById('post-list');
    const options = select.querySelectorAll('option');

        options.forEach(option => {
            const text = option.textContent.trim().toLowerCase();
    if (text.includes(filter)) {
        option.style.display = '';
            } else {
        option.style.display = 'none';
            }
        });
    }