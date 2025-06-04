let bookingTrendChart;
let bookingStatusChart;

document.addEventListener('DOMContentLoaded', function() {
    initializeCharts();
    
    document.getElementById('timeRange').addEventListener('change', function() {
        loadData();
    });
});

function initializeCharts() {
    // Initialize Booking Trend Chart
    const trendCtx = document.getElementById('bookingTrendChart').getContext('2d');
    const dailyData = JSON.parse(document.getElementById('bookingData').value);
    
    bookingTrendChart = new Chart(trendCtx, {
        type: 'line',
        data: {
            labels: dailyData.map(d => d.date),
            datasets: [{
                label: 'Daily Bookings',
                data: dailyData.map(d => d.count),
                borderColor: '#4e73df',
                backgroundColor: 'rgba(78, 115, 223, 0.05)',
                borderWidth: 2,
                pointRadius: 3,
                pointBackgroundColor: '#4e73df',
                pointBorderColor: '#fff',
                pointHoverRadius: 5,
                pointHoverBackgroundColor: '#4e73df',
                pointHoverBorderColor: '#fff',
                fill: true
            }]
        },
        options: {
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            }
        }
    });

    // Initialize Booking Status Chart
    const statusCtx = document.getElementById('bookingStatusChart').getContext('2d');
    const statusData = JSON.parse(document.getElementById('bookingStatusData').value);
    
    bookingStatusChart = new Chart(statusCtx, {
        type: 'doughnut',
        data: {
            labels: statusData.labels,
            datasets: [{
                data: statusData.values,
                backgroundColor: [
                    '#1cc88a',  // Completed
                    '#4e73df',  // Pending
                    '#e74a3b'   // Cancelled
                ],
                hoverBackgroundColor: [
                    '#17a673',
                    '#2e59d9',
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