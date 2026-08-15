        
        document.querySelectorAll('button').forEach(button => {
            button.addEventListener('mousedown', () => {
                button.classList.add('scale-95');
            });
            button.addEventListener('mouseup', () => {
                button.classList.remove('scale-95');
            });
        });

        
        const inputs = document.querySelectorAll('input[type="text"], input[type="tel"]');
        inputs.forEach(input => {
            input.addEventListener('focus', () => {
                input.parentElement.querySelector('label').classList.replace('text-on-surface-variant', 'text-primary');
            });
            input.addEventListener('blur', () => {
                input.parentElement.querySelector('label').classList.replace('text-primary', 'text-on-surface-variant');
            });
        });