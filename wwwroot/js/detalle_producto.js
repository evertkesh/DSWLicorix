 
    const qtyInput = document.getElementById('qty-input');
    function increment() {
        qtyInput.value = parseInt(qtyInput.value) + 1;
    }
    function decrement() {
        if(parseInt(qtyInput.value) > 1) {
            qtyInput.value = parseInt(qtyInput.value) - 1;
        }
    }

    
    function switchTab(tab) {
        const infoBtn = document.getElementById('tab-btn-info');
        const reviewsBtn = document.getElementById('tab-btn-reviews');
        const infoContent = document.getElementById('tab-content-info');
        const reviewsContent = document.getElementById('tab-content-reviews');

        if (tab === 'info') {
            infoBtn.classList.add('active-tab-indicator', 'text-primary');
            infoBtn.classList.remove('text-on-surface-variant');
            reviewsBtn.classList.remove('active-tab-indicator', 'text-primary');
            reviewsBtn.classList.add('text-on-surface-variant');
            infoContent.classList.remove('hidden');
            reviewsContent.classList.add('hidden');
        } else {
            reviewsBtn.classList.add('active-tab-indicator', 'text-primary');
            reviewsBtn.classList.remove('text-on-surface-variant');
            infoBtn.classList.remove('active-tab-indicator', 'text-primary');
            infoBtn.classList.add('text-on-surface-variant');
            reviewsContent.classList.remove('hidden');
            infoContent.classList.add('hidden');
        }
    }

    
    setInterval(() => {
        const stockElement = document.getElementById('stock-counter');
        
        if (Math.random() > 0.7) {
            let currentMatch = stockElement.innerText.match(/\d+/);
            if (currentMatch) {
                let current = parseInt(currentMatch[0]);
                if (current > 5) {
                    stockElement.innerText = `En Stock: ${current - 1} unidades`;
                    stockElement.classList.add('text-error');
                    setTimeout(() => stockElement.classList.remove('text-error'), 1000);
                }
            }
        }
    }, 8000);