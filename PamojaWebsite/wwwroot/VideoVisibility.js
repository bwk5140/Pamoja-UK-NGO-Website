window.observeVideoVisibility = (videoId) => {
    const video = document.getElementById(videoId);
    if (!video) return;

    const observer = new IntersectionObserver(
        (entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    video.play();
                } else {
                    video.pause();
                }
            });
        },
        { threshold: 0.5 }
    );

    observer.observe(video);
};