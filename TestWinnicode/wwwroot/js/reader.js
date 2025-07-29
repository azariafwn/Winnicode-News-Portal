function likeDislike(beritaId, isLike) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch('/Reader/LikeDislike', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ beritaId: beritaId, isLike: isLike })
    })
        .then(response => {
            if (response.status === 401) {
                window.location.href = '/Account/Login?ReturnUrl=' + window.location.pathname;
                return null;
            }
            return response.json();
        })
        .then(data => {
            if (data && data.success) {
                // 1. Update jumlah (count)
                document.getElementById("likeCount").innerText = `(${data.newLikeCount})`;
                document.getElementById("dislikeCount").innerText = `(${data.newDislikeCount})`;

                // 2. Update gambar ikon berdasarkan status baru
                const likeIconTop = document.getElementById('likeIconTop');
                const dislikeIconTop = document.getElementById('dislikeIconTop');
                const likeIconBottom = document.getElementById('likeIconBottom');
                const dislikeIconBottom = document.getElementById('dislikeIconBottom');

                // Cek elemen sebelum mengubah src untuk menghindari error
                if (!likeIconTop || !dislikeIconTop || !likeIconBottom || !dislikeIconBottom) {
                    console.error("Satu atau lebih elemen ikon tidak ditemukan. Periksa ID pada file .cshtml");
                    return;
                }

                // Logika ini akan menggunakan variabel global yang sudah benar
                if (data.newUserStatus === true) { // User me-like
                    likeIconTop.src = likeIconFilled;
                    likeIconBottom.src = likeIconFilled;
                    dislikeIconTop.src = dislikeIconDefault;
                    dislikeIconBottom.src = dislikeIconDefault;
                } else if (data.newUserStatus === false) { // User me-dislike
                    likeIconTop.src = likeIconDefault;
                    likeIconBottom.src = likeIconDefault;
                    dislikeIconTop.src = dislikeIconFilled;
                    dislikeIconBottom.src = dislikeIconFilled;
                } else { // User netral (membatalkan)
                    likeIconTop.src = likeIconDefault;
                    likeIconBottom.src = likeIconDefault;
                    dislikeIconTop.src = dislikeIconDefault;
                    dislikeIconBottom.src = dislikeIconDefault;
                }
            } else if (data) {
                console.error("Gagal melakukan aksi:", data.message);
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

async function shareArticle(title, url) {
    // Gunakan Web Share API jika didukung browser
    if (navigator.share) {
        try {
            await navigator.share({
                title: title,
                text: `Baca artikel menarik ini di Winnicode: ${title}`,
                url: url,
            });
            console.log('Artikel berhasil dibagikan');
        } catch (err) {
            console.error('Gagal berbagi:', err);
        }
    } else {
        // Fallback: Salin ke clipboard jika Web Share API tidak didukung
        try {
            await navigator.clipboard.writeText(url);
            // Tampilkan notifikasi toast
            showShareToast('Link artikel telah disalin ke clipboard!');
        } catch (err) {
            console.error('Gagal menyalin link:', err);
            showShareToast('Gagal menyalin link.', true);
        }
    }
}

function showShareToast(message, isError = false) {
    const toast = document.createElement('div');
    toast.textContent = message;
    toast.className = 'share-toast';
    if (isError) {
        toast.style.backgroundColor = '#dc3545'; // Merah untuk error
    }

    document.body.appendChild(toast);

    // Hapus toast setelah 3 detik
    setTimeout(() => {
        toast.remove();
    }, 3000);
}