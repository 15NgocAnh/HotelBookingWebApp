// Format currency input with thousand separators
function formatCurrencyInput(input) {
    // Remove all non-digit characters
    let value = input.value.replace(/\D/g, '');
    
    // Add thousand separators
    if (value.length > 0) {
        value = parseInt(value, 10).toLocaleString('vi-VN');
    }
    
    // Update input value
    input.value = value;
}

// Format currency input on focus
function handleCurrencyFocus(input) {
    // Remove thousand separators when focusing
    input.value = input.value.replace(/\D/g, '');
}

// Format currency input on blur
function handleCurrencyBlur(input) {
    // Add thousand separators when losing focus
    formatCurrencyInput(input);
}

// Get numeric value from formatted currency
function getNumericValue(formattedValue) {
    return parseInt(formattedValue.replace(/\D/g, ''), 10) || 0;
}

// Initialize currency inputs
document.addEventListener('DOMContentLoaded', function() {
    // Find all currency inputs
    const currencyInputs = document.querySelectorAll('input[data-currency]');
    
    currencyInputs.forEach(input => {
        // Add event listeners
        input.addEventListener('input', () => formatCurrencyInput(input));
        input.addEventListener('focus', () => handleCurrencyFocus(input));
        input.addEventListener('blur', () => handleCurrencyBlur(input));
        
        // Initial format
        formatCurrencyInput(input);
    });
}); 