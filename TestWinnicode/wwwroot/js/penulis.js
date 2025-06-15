document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById('sidebarMenu');
    const toggleBtn = document.querySelector('[data-bs-target="#sidebarMenu"]');
    const overlay = document.getElementById('sidebarOverlay');

    if (toggleBtn && sidebar && overlay) {
        toggleBtn.addEventListener('click', () => {
            sidebar.classList.toggle("show");
            overlay.classList.toggle("show");
        });

        overlay.addEventListener('click', () => {
            sidebar.classList.remove("show");
            overlay.classList.remove("show");
        });
    }
});
