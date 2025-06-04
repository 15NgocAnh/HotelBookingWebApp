let roomTypeChart;
let roomStatusChart;

document.addEventListener('DOMContentLoaded', function() {
    initializeCharts();
    
    document.getElementById('timeRange').addEventListener('change', function() {
        loadData();
    });
});

function initializeCharts() {
    // Initialize Room Type Chart
    const typeCtx = document.getElementById('roomTypeChart').getContext('2d');
    const typeData = JSON.parse(document.getElementById('roomTypeData').value);
    
    roomTypeChart = new Chart(typeCtx, {
        type: 'doughnut',
        data: {
            labels: typeData.map(t => t.roomType),
            datasets: [{
                data: typeData.map(t => t.total),
                backgroundColor: [
                    '#4e73df',
                    '#1cc88a',
                    '#36b9cc',
                    '#f6c23e',
                    '#e74a3b'
                ],
                hoverBackgroundColor: [
                    '#2e59d9',
                    '#17a673',
                    '#2c9faf',
                    '#dda20a',
                    '#be2617'
                ],
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }]
        },
        options: {
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            },
            cutout: '70%'
        }
    });

    // Initialize Room Status Chart
    const statusCtx = document.getElementById('roomStatusChart').getContext('2d');
    const statusData = JSON.parse(document.getElementById('roomStatusData').value);
    
    roomStatusChart = new Chart(statusCtx, {
        type: 'doughnut',
        data: {
            labels: statusData.map(s => s.status),
            datasets: [{
                data: statusData.map(s => s.count),
                backgroundColor: [
                    '#1cc88a',  // Available
                    '#4e73df',  // Booked
                    '#36b9cc',  // Cleaning
                    '#e74a3b'   // Maintenance
                ],
                hoverBackgroundColor: [
                    '#17a673',
                    '#2e59d9',
                    '#2c9faf',
                    '#be2617'
                ],
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }]
        },
        options: {
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            },
            cutout: '70%'
        }
    });
}

function loadData() {
    const days = document.getElementById('timeRange').value;
    window.location.href = `?days=${days}`;
} 