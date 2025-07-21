$(document).ready(function () {
    // === TABEL PENULIS (jika ada) ===
    if ($('#artikelTable').length) {
        const penulisTable = $('#artikelTable').DataTable({
            order: [[4, "desc"]],
            columnDefs: [
                { orderable: false, targets: [0, 5] },
                { orderable: true, targets: [1, 2, 3, 4] }
            ],
            language: {
                search: "Cari:",
                lengthMenu: "Tampilkan _MENU_ entri",
                info: "Menampilkan _START_ sampai _END_ dari _TOTAL_ entri",
                paginate: {
                    first: "Pertama",
                    last: "Terakhir",
                    next: "›",
                    previous: "‹"
                },
                zeroRecords: "Tidak ada data ditemukan"
            },
            initComplete: function () {
                $('#artikelTable_wrapper .dataTables_length').appendTo('#tableControls');
                $('#artikelTable_wrapper .dataTables_filter').appendTo('#tableControls');
                $('#artikelTable_wrapper .dataTables_info').appendTo('#tableFooterControls');
                $('#artikelTable_wrapper .dataTables_paginate').appendTo('#tableFooterControls');
            }
        });

        // Nomor urut kolom No
        penulisTable.on('order.dt search.dt', function () {
            penulisTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

        // Filter status
        $('#statusFilter').on('change', function () {
            penulisTable.column(3).search(this.value).draw();
        });
    }

    // === TABEL EDITOR (jika ada) ===
    if ($('#artikelEditorTable').length) {
        const editorTable = $('#artikelEditorTable').DataTable({
            order: [[4, "desc"]], // Kolom Last Access
            columnDefs: [
                { orderable: false, targets: [0, 5] },
                { orderable: true, targets: [1, 2, 3, 4] }
            ],
            language: {
                search: "Cari:",
                lengthMenu: "Tampilkan _MENU_ entri",
                info: "Menampilkan _START_ sampai _END_ dari _TOTAL_ entri",
                paginate: {
                    first: "Pertama",
                    last: "Terakhir",
                    next: "›",
                    previous: "‹"
                },
                zeroRecords: "Tidak ada data ditemukan"
            },
            initComplete: function () {
                $('#artikelEditorTable_wrapper .dataTables_length').appendTo('#tableControls');
                $('#artikelEditorTable_wrapper .dataTables_filter').appendTo('#tableControls');
                $('#artikelEditorTable_wrapper .dataTables_info').appendTo('#tableFooterControls');
                $('#artikelEditorTable_wrapper .dataTables_paginate').appendTo('#tableFooterControls');
            }
        });

        // Nomor urut kolom No
        editorTable.on('order.dt search.dt', function () {
            editorTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

        // Filter status
        $('#statusFilter').on('change', function () {
            editorTable.column(3).search(this.value).draw();
        });
    }

    // === LOGIKA BARU UNTUK FORM PENOLAKAN INLINE ===
    // GUARD: Cek apakah kita berada di halaman yang memiliki tombol-tombol ini
    if ($('#showRejectionFormBtn').length) {

        const mainActionButtons = $('#mainActionButtons');
        const rejectionFormContainer = $('#rejectionFormContainer');
        const showRejectionFormBtn = $('#showRejectionFormBtn');
        const cancelRejectionBtn = $('#cancelRejectionBtn');
        const submitRejectionBtn = $('#submitRejectionBtn');
        const statusInput = $('#Status');
        const commentTextarea = $('#KomentarEditor'); // asp-for="KomentarEditor" akan menghasilkan id="KomentarEditor"

        // Ketika tombol "Artikel Ditolak" utama diklik
        showRejectionFormBtn.on('click', function () {
            mainActionButtons.hide(); // Sembunyikan tombol aksi utama
            rejectionFormContainer.show(); // Tampilkan form komentar
        });

        // Ketika tombol "Batal" di form komentar diklik
        cancelRejectionBtn.on('click', function () {
            rejectionFormContainer.hide(); // Sembunyikan form komentar
            mainActionButtons.show(); // Tampilkan kembali tombol aksi utama
        });

        // Ketika tombol "Kirim Penolakan" (yang tipe-nya submit) diklik
        submitRejectionBtn.on('click', function (event) {
            // 1. Validasi komentar tidak boleh kosong
            if (commentTextarea.val().trim() === '') {
                alert('Harap isi alasan penolakan.');
                event.preventDefault(); // Mencegah form untuk submit jika validasi gagal
                return;
            }

            // 2. Set status menjadi 'Ditolak' sebelum form disubmit
            statusInput.val('Ditolak');

            // Form akan tersubmit secara otomatis karena tombol ini bertipe "submit"
        });
    }
});
