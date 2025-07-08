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

$(document).ready(function () {
    var table = $('#artikelTable').DataTable({
        scrollY: "500px",
        scrollCollapse: true,
        paging: true,
        order: [[3, "desc"]],
        columnDefs: [
            { orderable: false, targets: [0, 4] },
            { orderable: true, targets: [1, 2, 3] }
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

    $('#statusFilter').on('change', function () {
        var selected = $(this).val();
        table.column(2).search(selected).draw();
    });

    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

document.addEventListener("DOMContentLoaded", function () {
        const kategoriSub = {
                1: [{id: 11, name: "AI" }, {id: 12, name: "Perangkat Lunak" }],
            2: [{id: 21, name: "Gizi" }, {id: 22, name: "Kesehatan Mental" }]
        };

            document.getElementById("kategori").addEventListener("change", function () {
            const sub = document.getElementById("subkategori");
            sub.innerHTML = '<option value="">Pilih Subkategori</option>';
            const selected = kategoriSub[this.value] || [];
            selected.forEach(s => {
                const opt = document.createElement("option");
            opt.value = s.id;
            opt.textContent = s.name;
            sub.appendChild(opt);
            });
        });

        const quill = new Quill('#editor', {
                theme: 'snow',
            placeholder: 'Tulis artikel Anda di sini...',
            modules: {
                toolbar: [
            [{header: [1, 2, 3, false] }],
            ['bold', 'italic', 'underline'],
            ['link', 'blockquote', 'code-block'],
            [{list: 'ordered' }, {list: 'bullet' }],
            [{indent: '-1' }, {indent: '+1' }],
            ['clean']
            ]
            }
        });

        document.querySelector("form").onsubmit = function () {
            document.getElementById("IsiArtikel").value = quill.root.innerHTML;
    };
});
    