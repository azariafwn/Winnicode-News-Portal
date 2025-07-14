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
});
