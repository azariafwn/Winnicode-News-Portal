document.addEventListener("DOMContentLoaded", function () {
    // Inisialisasi Quill
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

        const form = document.querySelector("form");
        form.addEventListener("submit", function (e) {
            const isiField = document.getElementById("IsiArtikel");
            isiField.value = quill.root.innerHTML;
        });
    }

    // Kategori → SubKategori
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
