function toggleMobileSearch() {
    var searchDiv = document.getElementById("mobile-search");
    if (searchDiv.classList.contains("d-none")) {
        searchDiv.classList.remove("d-none");
    } else {
        searchDiv.classList.add("d-none");
    }
}
