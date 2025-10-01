// TWS Investment Platform - JavaScript Utilities

(function () {
    'use strict';

    // Initialize on document ready
    document.addEventListener('DOMContentLoaded', function () {
        initializeAlerts();
        initializeFormValidation();
        initializeLoadingSpinner();
    });

    // Auto-dismiss alerts after 5 seconds
    function initializeAlerts() {
        const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
        alerts.forEach(function (alert) {
            setTimeout(function () {
                const bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }, 5000);
        });
    }

    // Form validation enhancement
    function initializeFormValidation() {
        const forms = document.querySelectorAll('.needs-validation');
        Array.from(forms).forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }

    // Loading spinner utilities
    function initializeLoadingSpinner() {
        window.showLoading = function () {
            const spinner = document.getElementById('loadingSpinner');
            if (spinner) {
                spinner.classList.remove('d-none');
            }
        };

        window.hideLoading = function () {
            const spinner = document.getElementById('loadingSpinner');
            if (spinner) {
                spinner.classList.add('d-none');
            }
        };
    }

    // Password strength validator
    window.validatePasswordStrength = function (password) {
        const requirements = {
            length: password.length >= 8 && password.length <= 24,
            uppercase: /[A-Z]/.test(password),
            lowercase: /[a-z]/.test(password),
            number: /\d/.test(password),
            special: /[@$!%*?&#]/.test(password)
        };

        return {
            isValid: Object.values(requirements).every(v => v === true),
            requirements: requirements
        };
    };

    // API call helper with loading spinner
    window.apiCall = function (url, method, data, successCallback, errorCallback) {
        showLoading();

        const options = {
            method: method || 'GET',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            }
        };

        if (data && (method === 'POST' || method === 'PUT')) {
            options.body = JSON.stringify(data);
        }

        fetch(url, options)
            .then(response => {
                hideLoading();
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                if (successCallback) {
                    successCallback(data);
                }
            })
            .catch(error => {
                hideLoading();
                console.error('API call error:', error);
                if (errorCallback) {
                    errorCallback(error);
                } else {
                    showAlert('An error occurred. Please try again.', 'danger');
                }
            });
    };

    // Show alert message
    window.showAlert = function (message, type) {
        type = type || 'info';
        const alertHtml = `
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                <i class="fas fa-${getAlertIcon(type)} me-2"></i>
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;

        const container = document.querySelector('.container:first-of-type') || document.body;
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = alertHtml;
        container.insertBefore(tempDiv.firstElementChild, container.firstChild);

        // Auto-dismiss after 5 seconds
        setTimeout(function () {
            const alert = container.querySelector('.alert');
            if (alert) {
                const bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }
        }, 5000);
    };

    // Get icon for alert type
    function getAlertIcon(type) {
        const icons = {
            'success': 'check-circle',
            'danger': 'exclamation-circle',
            'warning': 'exclamation-triangle',
            'info': 'info-circle'
        };
        return icons[type] || 'info-circle';
    }

    // Confirm dialog helper
    window.confirmAction = function (message, callback) {
        if (confirm(message)) {
            callback();
        }
    };

    // Form submission with confirmation
    window.submitFormWithConfirmation = function (formId, message) {
        confirmAction(message, function () {
            document.getElementById(formId).submit();
        });
    };

    // Format currency
    window.formatCurrency = function (amount) {
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD'
        }).format(amount);
    };

    // Format date
    window.formatDate = function (dateString) {
        const date = new Date(dateString);
        return new Intl.DateTimeFormat('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        }).format(date);
    };

    // Debounce function for search inputs
    window.debounce = function (func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    };

    // Copy to clipboard
    window.copyToClipboard = function (text) {
        navigator.clipboard.writeText(text).then(function () {
            showAlert('Copied to clipboard!', 'success');
        }).catch(function (err) {
            console.error('Failed to copy text: ', err);
            showAlert('Failed to copy to clipboard', 'danger');
        });
    };

    // Scroll to element
    window.scrollToElement = function (elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    };

    // Toggle password visibility
    window.togglePasswordVisibility = function (inputId) {
        const input = document.getElementById(inputId);
        if (input) {
            input.type = input.type === 'password' ? 'text' : 'password';
        }
    };

})();
