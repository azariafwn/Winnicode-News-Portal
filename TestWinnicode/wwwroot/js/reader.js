function likeDislike(beritaId, isLike) {
    fetch('/Reader/LikeDislike', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ beritaId: beritaId, isLike: isLike })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById("likeCount").innerText = `(${data.newLikeCount})`;
                document.getElementById("dislikeCount").innerText = `(${data.newDislikeCount})`;
            }
        });
}
