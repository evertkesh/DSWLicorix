   
    document.querySelectorAll('.product-card-hover').forEach(card => {
        card.addEventListener('mouseenter', () => {
            const btn = card.querySelector('.add-to-cart-btn');
            btn.style.opacity = '1';
            btn.style.transform = 'translateY(0)';
        });
        card.addEventListener('mouseleave', () => {
            const btn = card.querySelector('.add-to-cart-btn');
            btn.style.opacity = '0';
            btn.style.transform = 'translateY(16px)';
        });
    });

    
    const slider = document.querySelector('input[type="range"]');
    if (slider) {
        slider.addEventListener('input', (e) => {
            const val = e.target.value;
            const display = e.target.nextElementSibling.querySelector('span:last-child');
            display.innerText = `$${val}${val >= 1000 ? '+' : ''}`;
        });
    }