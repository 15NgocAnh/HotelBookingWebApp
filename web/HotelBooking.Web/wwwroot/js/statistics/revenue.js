let revenueChart;

document.addEventListener('DOMContentLoaded', function() {
    initializeCharts();
    
    document.getElementById('timeRange').addEventListener('change', function() {
        loadData();
    });
});

function initializeCharts() {
    // Initialize Revenue Trend Chart
    const trendCtx = document.getElementById('revenueChart').getContext('2d');
    const trendData = JSON.parse(document.getElementById('revenueData').value);
    
    revenueChart = new Chart(trendCtx, {
        type: 'line',
        data: {
            labels: trendData.map(d => d.date),
            datasets: [{
                label: 'Daily Revenue',
                data: trendData.map(d => d.revenue),
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
                        callback: function(value) {
                            return value.toLocaleString() + ' ₫';
                        }
                    }
                }
            }
        }
    });
}

function loadData() {
    const days = document.getElementById('timeRange').value;
    window.location.href = `?days=${days}`;
} 