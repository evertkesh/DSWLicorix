        
        document.querySelectorAll('.group').forEach(card => {
            card.addEventListener('mouseenter', () => {
                const badge = card.querySelector('.discount-badge');
                if (badge) badge.style.transform = 'scale(1.1)';
            });
            card.addEventListener('mouseleave', () => {
                const badge = card.querySelector('.discount-badge');
                if (badge) badge.style.transform = 'scale(1)';
            });
        });

        
        window.addEventListener('scroll', () => {
            const header = document.querySelector('header');
            if (window.scrollY > 50) {
                header.classList.add('shadow-sm');
            } else {
                header.classList.remove('shadow-sm');
            }
        });