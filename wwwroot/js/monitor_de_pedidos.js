        
        window.addEventListener('DOMContentLoaded', () => {
            const progressLine = document.querySelector('.absolute.top-4.left-0.w-\\[75\\%\\]');
            if (progressLine) {
                progressLine.style.width = '0%';
                setTimeout(() => {
                    progressLine.style.width = '75%';
                }, 500);
            }
        });