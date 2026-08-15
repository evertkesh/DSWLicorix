    
    function cambiarCantidad(idProducto, delta, stock) {
        const input = document.getElementById('qty_' + idProducto);
        if (!input) return;
        let valor = parseInt(input.value || '1', 10);
        if (isNaN(valor)) valor = 1;
        valor = valor + delta;
        if (valor < 1) valor = 1;
        if (valor > stock) valor = stock;
        input.value = valor;
        
        const form = document.getElementById('formQty_' + idProducto);
        if (form) form.submit();
    }

    
    window.addEventListener('DOMContentLoaded', () => {
        const tableRows = document.querySelectorAll('tbody tr');
        tableRows.forEach((row, index) => {
            row.style.opacity = '0';
            row.style.transform = 'translateY(10px)';
            setTimeout(() => {
                row.style.transition = 'all 0.6s ease';
                row.style.opacity = '1';
                row.style.transform = 'translateY(0)';
            }, 100 * index);
        });
    });