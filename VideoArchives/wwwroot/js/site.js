// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showUploadForm() {
    stopVideo();
    document.getElementById('uploadFormContainer').classList.remove('hidden');
    document.getElementById('videoContent').classList.add('hidden');
    document.getElementById('noArchives').classList.add('hidden');
    document.getElementById('clickArchives').classList.add('hidden');
}

async function uploadVideo() {
    const form = document.getElementById('uploadForm');
    const formData = new FormData(form);

    const response = await fetch('https://localhost:7257/api/Videos/upload', {
        method: 'POST',
        body: formData
    });

    if (response.ok) {
        const videoList = await response.json();
        const videoTableBody = document.getElementById('videoTableBody');

        if (videoTableBody) {

            // Populate table with updated list of videos
            videoList.forEach(video => {
                const newVideoRow = document.createElement('tr');
                newVideoRow.setAttribute('onclick', `playVideo('${video.filePath}')`);
                newVideoRow.innerHTML = `<td>${video.fileName}</td><td>${video.fileSize}</td>`;
                videoTableBody.appendChild(newVideoRow);
            });

            // Reset form and toggle visibility
            form.reset();
            document.getElementById('uploadFormContainer').classList.add('hidden');
            document.getElementById('videoContent').classList.remove('hidden');
            document.getElementById('clickArchives').classList.remove('hidden');
        } else {
            console.error('videoTableBody element not found.');
        }
    } else {
        alert('Failed to upload video.');
    }
}

function playVideo(filePath) {
    const baseUrl = window.location.origin; // Get the base URL of the application
    const videoUrl = `https://localhost:7257/${filePath}`;
    const videoPlayer = document.getElementById('videoPlayer');
    videoPlayer.src = videoUrl;
    videoPlayer.play();
}

function stopVideo() {
    const videoPlayer = document.getElementById('videoPlayer');
    videoPlayer.pause();
}