document.addEventListener('DOMContentLoaded', function() {
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            const priceInputs = form.querySelectorAll('input[data-currency]');
            priceInputs.forEach(input => {
                // Remove all non-digit characters
                const numericValue = input.value.replace(/[.,]/g, '');
                input.value = numericValue;
            });
        });
    });
}); 