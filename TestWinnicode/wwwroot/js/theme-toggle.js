document.addEventListener("DOMContentLoaded", () => {
    const toggleButton = document.getElementById("theme-toggle");
    const themeIcon = toggleButton.querySelector(".theme-icon");
    const darkMode = window.matchMedia("(prefers-color-scheme: dark)").matches;

    // Set initial icon based on current theme
    if (darkMode) {
        themeIcon.src = "/images/sun-icon.png";
    }

    toggleButton.addEventListener("click", (e) => {
        e.preventDefault();

        if (document.body.classList.contains("dark-mode")) {
            document.body.classList.remove("dark-mode");
            themeIcon.src = "/images/moon-icon.png";
        } else {
            document.body.classList.add("dark-mode");
            themeIcon.src = "/images/sun-icon.png";
        }
    });
});
