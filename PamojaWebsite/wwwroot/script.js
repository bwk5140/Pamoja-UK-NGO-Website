// Main JavaScript for Pamoja Website

// Initialize Feather Icons
document.addEventListener('DOMContentLoaded', function () {
    feather.replace();
});

// Smooth scrolling for anchor links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Statistics counter animation
function animateCounter(element, target, duration = 2000) {
    let start = 0;
    const increment = target / (duration / 16);

    const updateCounter = () => {
        start += increment;
        if (start < target) {
            element.textContent = Math.floor(start);
            requestAnimationFrame(updateCounter);
        } else {
            element.textContent = target;
        }
    };

    updateCounter();
}

// Intersection Observer for animations
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('animate-fade-in');
        }
    });
}, observerOptions);

// Add fade-in class to elements
document.addEventListener('DOMContentLoaded', function () {
    const animatedElements = document.querySelectorAll('.service-card, .value-card, .stat-card');
    animatedElements.forEach(el => {
        observer.observe(el);
    });
});

// Mobile menu functionality
function toggleMobileMenu() {
    const mobileMenu = document.getElementById('mobileMenu');
    if (mobileMenu) {
        mobileMenu.classList.toggle('hidden');
    }
}

// Form validation
function validateContactForm(form) {
    const email = form.querySelector('input[type="email"]');
    const message = form.querySelector('textarea');
    let isValid = true;

    if (email && !email.value.includes('@')) {
        email.classList.add('border-red-500');
        isValid = false;
    }

    if (message && message.value.trim().length < 10) {
        message.classList.add('border-red-500');
        isValid = false;
    }

    return isValid;
}

// Image lazy loading
function lazyLoadImages() {
    const images = document.querySelectorAll('img[data-src]');

    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.getAttribute('data-src');
                img.removeAttribute('data-src');
                observer.unobserve(img);
            }
        });
    });

    images.forEach(img => imageObserver.observe(img));
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function () {
    lazyLoadImages();

    // Add loading animation to statistics
    const statElements = document.querySelectorAll('.stat-number');
    statElements.forEach(el => {
        const target = parseInt(el.getAttribute('data-target'));
        animateCounter(el, target);
    });
});

// Service cards interaction
document.querySelectorAll('.service-card').forEach(card => {
    card.addEventListener('mouseenter', function () {
        this.style.transform = 'translateY(-10px) scale(1.02)';
    });

    card.addEventListener('mouseleave', function () {
        this.style.transform = 'translateY(0px) scale(1)';
    });
});

// Newsletter subscription
function subscribeNewsletter(email) {
    // Simulate API call
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve({ success: true, message: 'Thank you for subscribing!' });
        }, 1000);
    });
}
// Back to top button
function scrollToTop() {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        }

// Add CSS for animations
const style = document.createElement('style');
    style.textContent = `
    @keyframes fadeIn {
        from { opacity: 0; transform: translateY(30px); }
        to { opacity: 1; transform: translateY(0); }
    }
    
    .animate-fade-in {
        animation: fadeIn 0.8s ease-out forwards;
    }
    
    .service-card, .value-card, .stat-card {
        opacity: 0;
    }
`;
    document.head.appendChild(style);

    // Handle contact form submission
    document.addEventListener('DOMContentLoaded', function () {
        const contactForm = document.getElementById('contactForm');
        if (contactForm) {
            contactForm.addEventListener('submit', function (e) {
                e.preventDefault();
                if (validateContactForm(this)) {
                    // Simulate form submission
                    const submitBtn = this.querySelector('button[type="submit"]');
                    const originalText = submitBtn.textContent;

                    submitBtn.textContent = 'Sending...';
                    submitBtn.disabled = true;

                    setTimeout(() => {
                        submitBtn.textContent = 'Message Sent!';
                        this.reset();

                        setTimeout(() => {
                            submitBtn.textContent = originalText;
                            submitBtn.disabled = false;
                        }, 2000);
                    }, 1000);
                }
            });
        }
    });