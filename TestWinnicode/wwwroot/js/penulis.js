document.addEventListener("DOMContentLoaded", function () {
    // === SIDEBAR TOGGLE ===
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

    // === QUILL EDITOR ===
    const quillElement = document.getElementById("editor");
    const isiArtikelField = document.getElementById("IsiArtikel");

    if (quillElement && isiArtikelField) {
        const quill = new Quill('#editor', {
            theme: 'snow',
            placeholder: 'Tulis artikel Anda di sini...',
            modules: {
                toolbar: [
                    [{ header: [1, 2, 3, false] }],
                    ['bold', 'italic', 'underline'],
                    ['link', 'blockquote', 'code-block'],
                    [{ list: 'ordered' }, { list: 'bullet' }],
                    [{ indent: '-1' }, { indent: '+1' }],
                    ['clean']
                ]
            }
        });

        // Simpan isi Quill ke input hidden saat submit
        const form = document.querySelector("form");
        if (form) {
            form.addEventListener("submit", function (e) {
                e.preventDefault(); // Stop default submit dulu
                isiArtikelField.value = quill.root.innerHTML;
                this.submit(); // Submit ulang setelah field terisi
            });

        }
    }

    // === DROPDOWN KATEGORI → SUBKATEGORI ===
    const kategoriSelect = document.getElementById("kategori");
    const subkategoriSelect = document.getElementById("subkategori");

    if (kategoriSelect && subkategoriSelect) {
        kategoriSelect.addEventListener("change", function () {
            const kategoriId = this.value;
            subkategoriSelect.innerHTML = '<option value="">Pilih Subkategori</option>';

            if (kategoriId) {
                fetch(`/Penulis/GetSubKategori?kategoriId=${kategoriId}`)
                    .then(response => response.json())
                    .then(data => {
                        data.forEach(sub => {
                            const opt = document.createElement("option");
                            opt.value = sub.id;
                            opt.textContent = sub.nama;
                            subkategoriSelect.appendChild(opt);
                        });
                    })
                    .catch(error => console.error("Gagal mengambil subkategori:", error));
            }
        });
    }
});
