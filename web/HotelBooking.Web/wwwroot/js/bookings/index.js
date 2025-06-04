document.addEventListener('DOMContentLoaded', function () {
    // Delete modal functionality
    const deleteModal = document.getElementById('deleteModal');
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');

            const modalBody = deleteModal.querySelector('.modal-body p');
            const deleteIdInput = deleteModal.querySelector('#deleteId');

            modalBody.textContent = `Are you sure you want to cancel the booking for room "${name}"? This action cannot be undone.`;
            deleteIdInput.value = id;
        });
    }

    // Check In modal functionality
    const checkInModal = document.getElementById('checkInModal');
    if (checkInModal) {
        checkInModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');

            const modalBody = checkInModal.querySelector('.modal-body p');
            const checkInIdInput = checkInModal.querySelector('#checkInId');

            modalBody.textContent = `Are you sure you want to check in the room "${name}"?`;
            checkInIdInput.value = id;
        });
    }

    // Check Out modal functionality
    const checkOutModal = document.getElementById('checkOutModal');
    if (checkOutModal) {
        checkOutModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');

            const modalBody = checkOutModal.querySelector('.modal-body p');
            const checkOutIdInput = checkOutModal.querySelector('#checkOutId');

            modalBody.textContent = `Are you sure you want to check out the room "${name}"?`;
            checkOutIdInput.value = id;
        });
    }

    // Create Invoice modal functionality
    const createInvoiceModal = document.getElementById('createInvoiceModal');
    if (createInvoiceModal) {
        createInvoiceModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');

            const modalBody = createInvoiceModal.querySelector('.modal-body p');
            const createInvoiceBookingIdInput = createInvoiceModal.querySelector('#createInvoiceBookingId');

            modalBody.textContent = `Are you sure you want to create invoice for booking room "${name}"?`;
            createInvoiceBookingIdInput.value = id;
        });
    }

    // Tab switching with smooth animation
    const tabButtons = document.querySelectorAll('[data-bs-toggle="tab"]');
    tabButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Add loading animation
            const targetPane = document.querySelector(this.getAttribute('data-bs-target'));
            if (targetPane) {
                targetPane.style.opacity = '0.5';
                setTimeout(() => {
                    targetPane.style.opacity = '1';
                }, 150);
            }
        });
    });

    // Add smooth hover effects to booking cards
    const bookingCards = document.querySelectorAll('.booking-card');
    bookingCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-2px)';
        });

        card.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0)';
        });
    });
});

function setCheckInNotes() {
    const notes = document.getElementById('checkInNotes').value;
    document.getElementById('checkInNotesInput').value = notes;
}

function setInvoiceNotes() {
    const notes = document.getElementById('invoiceNotes').value;
    document.getElementById('invoiceNotesInput').value = notes;
} 