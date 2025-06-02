class AuthService {
    constructor() {
        this.baseUrl = '/api';
        this.accessToken = localStorage.getItem('accessToken');
        this.refreshToken = localStorage.getItem('refreshToken');
    }

    setTokens(accessToken, refreshToken) {
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('refreshToken', refreshToken);
    }

    clearTokens() {
        this.accessToken = null;
        this.refreshToken = null;
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
    }

    async makeAuthenticatedRequest(url, options = {}) {
        if (!this.accessToken) {
            window.location.href = '/Account/Login';
            return;
        }

        const headers = {
            'Authorization': `Bearer ${this.accessToken}`,
            'Refresh-Token': this.refreshToken,
            'Content-Type': 'application/json',
            ...options.headers
        };

        try {
            const response = await fetch(`${this.baseUrl}${url}`, {
                ...options,
                headers
            });

            // Kiểm tra xem có token mới không
            const newAccessToken = response.headers.get('New-Access-Token');
            const newRefreshToken = response.headers.get('New-Refresh-Token');

            if (newAccessToken && newRefreshToken) {
                // Cập nhật token mới
                this.setTokens(newAccessToken, newRefreshToken);

                // Thử lại request ban đầu với token mới
                headers['Authorization'] = `Bearer ${newAccessToken}`;
                return fetch(`${this.baseUrl}${url}`, {
                    ...options,
                    headers
                });
            }

            if (response.status === 401) {
                this.clearTokens();
                window.location.href = '/Account/Login';
                return;
            }

            return response;
        } catch (error) {
            console.error('API request failed:', error);
            throw error;
        }
    }

    async login(email, password) {
        try {
            const response = await fetch(`${this.baseUrl}/auth/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ email, password })
            });

            if (!response.ok) {
                throw new Error('Login failed');
            }

            const data = await response.json();
            this.setTokens(data.token, data.refreshToken);
            return data;
        } catch (error) {
            console.error('Login failed:', error);
            throw error;
        }
    }

    async logout() {
        this.clearTokens();
        window.location.href = '/Account/Login';
    }
}

// Tạo instance global
window.authService = new AuthService(); 