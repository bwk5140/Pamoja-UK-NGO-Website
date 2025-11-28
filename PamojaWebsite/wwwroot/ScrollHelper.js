window.scrollTailwindCarousel = (trackId, slideIndex) => {
    const track = document.getElementById(trackId);
    if (track) {
        const slideWidth = track.offsetWidth;
        track.style.transform = `translateX(-${slideIndex * slideWidth}px)`;
    }
};
window.scrollToSection = function (elementId) {
    const el = document.getElementById(elementId);
    if (el) {
        try {
            el.scrollIntoView({ behavior: 'smooth', block: 'start' });
        } catch (err) {
            // Fallback for Safari/iOS
            const y = el.getBoundingClientRect().top + window.pageYOffset;
            window.scrollTo({ top: y, behavior: 'smooth' });
        }
    } else {
        console.warn(`Element with ID '${elementId}' not found.`);
    }
};

