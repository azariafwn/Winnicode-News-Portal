document.addEventListener("DOMContentLoaded", function () {
    const navbar = document.getElementById("categoryNavbar");
    const moreDropdown = document.getElementById("moreDropdown");
    const moreCategoriesList = document.getElementById("moreCategoriesList");

    function handleNavbarOverflow() {
        const navbarWidth = navbar.offsetWidth;
        let totalWidth = 0;
        const items = Array.from(navbar.children);

        // Bersihkan dropdown dulu
        moreCategoriesList.innerHTML = "";

        items.forEach(item => {
            if (item !== moreDropdown) {
                item.classList.remove("d-none");
            }
        });

        items.forEach(item => {
            if (item !== moreDropdown) {
                totalWidth += item.offsetWidth;
                if (totalWidth > navbarWidth) {
                    item.classList.add("d-none");
                    moreDropdown.classList.remove("d-none");
                    const clone = item.cloneNode(true);
                    clone.classList.remove("d-none");
                    moreCategoriesList.appendChild(clone);
                }
            }
        });

        // Sembunyikan dropdown jika tidak diperlukan
        if (moreCategoriesList.children.length === 0) {
            moreDropdown.classList.add("d-none");
        }
    }

    // Panggil saat load dan saat resize
    handleNavbarOverflow();
    window.addEventListener("resize", handleNavbarOverflow);
});
