        document.addEventListener('scroll', () => {
            const header = document.querySelector('header');
            if (window.scrollY > 50) {
                header.classList.add('shadow-sm');
                header.style.backgroundColor = 'rgba(251, 249, 248, 0.95)';
                header.style.backdropFilter = 'blur(10px)';
            } else {
                header.classList.remove('shadow-sm');
                header.style.backgroundColor = '#fbf9f8';
                header.style.backdropFilter = 'none';
            }
    });
        document.querySelectorAll('[data-event-carousel]').forEach((carousel) => {
            const track = carousel.querySelector('.event-carousel-track');
            const slides = Array.from(carousel.querySelectorAll('.event-carousel-slide'));
            const dotsContainer = carousel.querySelector('.event-carousel-dots');
            const nextButton = carousel.querySelector('.event-carousel-next');
            const prevButton = carousel.querySelector('.event-carousel-prev');
            const interval = Number(carousel.dataset.interval) || 5000;

            if (!track || slides.length === 0) {
                return;
            }

            let currentIndex = 0;
            let timerId;

            const dots = slides.map((_, index) => {
                const dot = document.createElement('button');
                dot.type = 'button';
                dot.className = 'event-carousel-dot';
                dot.setAttribute('aria-label', `Ver evento ${index + 1}`);
                dot.addEventListener('click', () => {
                    showSlide(index);
                    restartTimer();
                });
                dotsContainer?.appendChild(dot);
                return dot;
            });

            const updateDots = () => {
                dots.forEach((dot, index) => {
                    dot.setAttribute('aria-current', index === currentIndex ? 'true' : 'false');
                });
            };

            const showSlide = (index) => {
                currentIndex = (index + slides.length) % slides.length;
                track.style.transform = `translateX(-${currentIndex * 100}%)`;
                updateDots();
            };

            const nextSlide = () => showSlide(currentIndex + 1);
            const previousSlide = () => showSlide(currentIndex - 1);
            const startTimer = () => {
                if (slides.length > 1) {
                    timerId = window.setInterval(nextSlide, interval);
                }
            };
            const stopTimer = () => window.clearInterval(timerId);
            const restartTimer = () => {
                stopTimer();
                startTimer();
            };

            nextButton?.addEventListener('click', () => {
                nextSlide();
                restartTimer();
            });
            prevButton?.addEventListener('click', () => {
                previousSlide();
                restartTimer();
            });
            carousel.addEventListener('mouseenter', stopTimer);
            carousel.addEventListener('mouseleave', startTimer);

            showSlide(0);
            startTimer();
        });