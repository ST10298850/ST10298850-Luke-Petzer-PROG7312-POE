// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Form Progress Bar and File Upload Functionality
document.addEventListener('DOMContentLoaded', function() {
    // Only run this code if we're on the issue creation page
    if (document.getElementById('attachment')) {
        initializeIssueForm();
    }
});

function initializeIssueForm() {
    const uploadArea = document.querySelector('.upload-area');
    const fileInput = document.getElementById('attachment');
    const fileInfo = document.getElementById('file-info');
    const fileName = document.querySelector('.file-name');
    const fileSize = document.querySelector('.file-size');
    
    // Progress bar elements
    const progressBar = document.querySelector('.form-progress-bar');
    const progressValue = document.querySelector('.form-progress-value');
    const progressLabel = document.querySelector('.form-progress-label');
    
    // Notification elements
    const emailCheckbox = document.getElementById('emailCheckbox');
    const phoneCheckbox = document.getElementById('phoneCheckbox');
    const emailInput = document.getElementById('notificationEmail');
    const phoneInput = document.getElementById('notificationPhone');
    
    // Form fields to track
    const formFields = {
        location: document.getElementById('location'),
        category: document.getElementById('category'),
        description: document.getElementById('description'),
        attachment: document.getElementById('attachment')
    };

    // Initialize notification functionality
    initializeNotifications();

    function initializeNotifications() {
        // Email checkbox functionality
        emailCheckbox.addEventListener('change', function() {
            const emailField = emailInput.closest('.notification-field');
            if (this.checked) {
                emailInput.disabled = false;
                emailField.classList.add('enabled');
                emailInput.focus();
            } else {
                emailInput.disabled = true;
                emailInput.value = '';
                emailField.classList.remove('enabled');
            }
            updateProgress();
        });

        // Phone checkbox functionality  
        phoneCheckbox.addEventListener('change', function() {
            const phoneField = phoneInput.closest('.notification-field');
            if (this.checked) {
                phoneInput.disabled = false;
                phoneField.classList.add('enabled');
                phoneInput.focus();
            } else {
                phoneInput.disabled = true;
                phoneInput.value = '';
                phoneField.classList.remove('enabled');
            }
            updateProgress();
        });

        // Track notification input changes for progress
        emailInput.addEventListener('input', debounce(updateProgress, 300));
        phoneInput.addEventListener('input', debounce(updateProgress, 300));
    }

    // Enhanced progress tracking including notifications
    function updateProgress() {
        let completedFields = 0;
        let totalRequiredFields = 3; // location, category, description are required
        let totalFields = 4; // including optional attachment
        let bonusFields = 0; // notification fields add bonus progress
        
        // Check required fields
        if (formFields.location.value.trim() !== '') completedFields++;
        if (formFields.category.value !== '') completedFields++;
        if (formFields.description.value.trim() !== '') completedFields++;
        
        // Check optional attachment
        let hasAttachment = formFields.attachment.files.length > 0;
        if (hasAttachment) completedFields++;
        
        // Check notification preferences (bonus progress)
        if (emailCheckbox.checked && emailInput.value.trim() !== '') bonusFields++;
        if (phoneCheckbox.checked && phoneInput.value.trim() !== '') bonusFields++;
        
        // Calculate progress percentage (notifications add 10% bonus each)
        let baseProgress = Math.round((completedFields / totalFields) * 85); // Max 85% for main fields
        let bonusProgress = bonusFields * 7.5; // 7.5% per notification method
        let progressPercent = Math.min(100, baseProgress + bonusProgress);
        
        // Update progress bar with animation
        animateProgressBar(progressPercent);
        
        // Update progress label based on completion
        updateProgressLabel(completedFields, totalRequiredFields, hasAttachment, bonusFields);
    }

    function animateProgressBar(targetPercent) {
        const currentWidth = parseInt(progressBar.style.width) || 0;
        const increment = targetPercent > currentWidth ? 1 : -1;
        
        if (currentWidth !== targetPercent) {
            let currentPercent = currentWidth;
            
            const animate = () => {
                currentPercent += increment;
                progressBar.style.width = currentPercent + '%';
                progressValue.textContent = currentPercent + '%';
                
                // Add visual feedback based on progress level
                if (currentPercent >= 100) {
                    progressBar.style.background = 'linear-gradient(90deg, #10B981, #059669)';
                    progressLabel.textContent = 'Form Complete!';
                } else if (currentPercent >= 85) {
                    progressBar.style.background = 'linear-gradient(90deg, #3B82F6, #1E40AF)';
                    progressLabel.textContent = 'Almost Done';
                } else if (currentPercent >= 60) {
                    progressBar.style.background = 'linear-gradient(90deg, #F59E0B, #D97706)';
                    progressLabel.textContent = 'Good Progress';
                } else if (currentPercent > 0) {
                    progressBar.style.background = 'linear-gradient(90deg, #EF4444, #DC2626)';
                    progressLabel.textContent = 'Getting Started';
                } else {
                    progressBar.style.background = '#E5E7EB';
                    progressLabel.textContent = 'Form Completion';
                }
                
                if ((increment > 0 && currentPercent < targetPercent) || 
                    (increment < 0 && currentPercent > targetPercent)) {
                    setTimeout(animate, 20);
                }
            };
            animate();
        }
    }

    function updateProgressLabel(completed, required, hasAttachment, bonusFields) {
        if (completed >= 4 && bonusFields >= 1) {
            progressLabel.innerHTML = 'Excellent! All fields complete <span style="color: #10B981;">✓</span>';
        } else if (completed >= 4) {
            progressLabel.innerHTML = 'Form Complete! <span style="color: #10B981;">✓</span>';
        } else if (completed >= required && bonusFields >= 1) {
            progressLabel.innerHTML = 'Great! Consider adding notifications <span style="color: #F59E0B;">+</span>';
        } else if (completed >= required) {
            progressLabel.innerHTML = 'Required Fields Complete <span style="color: #F59E0B;">+</span>';
        } else {
            const remaining = required - completed;
            progressLabel.textContent = `${remaining} Required Field${remaining !== 1 ? 's' : ''} Remaining`;
        }
    }

    // Add event listeners to all form fields
    formFields.location.addEventListener('input', debounce(updateProgress, 300));
    formFields.category.addEventListener('change', updateProgress);
    formFields.description.addEventListener('input', debounce(updateProgress, 300));

    // File upload functionality with progress tracking
    fileInput.addEventListener('change', function(e) {
        if (e.target.files.length > 0) {
            const file = e.target.files[0];
            if (validateFile(file)) {
                displayFileInfo(file);
                updateProgress(); // Update progress when file is added
            }
        }
    });

    // Handle drag and drop
    uploadArea.addEventListener('dragover', function(e) {
        e.preventDefault();
        uploadArea.classList.add('drag-over');
    });

    uploadArea.addEventListener('dragleave', function(e) {
        e.preventDefault();
        uploadArea.classList.remove('drag-over');
    });

    uploadArea.addEventListener('drop', function(e) {
        e.preventDefault();
        uploadArea.classList.remove('drag-over');
        
        const files = e.dataTransfer.files;
        if (files.length > 0) {
            const file = files[0];
            if (validateFile(file)) {
                fileInput.files = files;
                displayFileInfo(file);
                updateProgress(); // Update progress when file is dropped
            }
        }
    });

    function displayFileInfo(file) {
        fileName.textContent = file.name;
        fileSize.textContent = formatFileSize(file.size);
        fileInfo.style.display = 'block';
        uploadArea.style.display = 'none';
    }

    function formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }

    function validateFile(file) {
        const maxSize = 10 * 1024 * 1024; // 10MB
        const allowedTypes = ['image/png', 'image/jpeg', 'image/jpg', 'application/pdf'];
        
        if (file.size > maxSize) {
            alert('File size must be less than 10MB');
            return false;
        }
        
        if (!allowedTypes.includes(file.type)) {
            alert('Only PNG, JPG, and PDF files are allowed');
            return false;
        }
        
        return true;
    }

    // Global function to remove file
    window.removeFile = function() {
        fileInput.value = '';
        fileInfo.style.display = 'none';
        uploadArea.style.display = 'flex';
        updateProgress(); // Update progress when file is removed
    }

    // Debounce function to prevent too many progress updates
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    // Add smooth transition effect to progress bar
    progressBar.style.transition = 'width 0.3s ease-in-out, background 0.3s ease-in-out';

    // Initialize progress on page load
    updateProgress();

    // Add pulse animation for empty form
    if (progressBar.style.width === '0%' || !progressBar.style.width) {
        progressBar.classList.add('pulse-hint');
        setTimeout(() => progressBar.classList.remove('pulse-hint'), 3000);
    }
}

// Home page "Get Started" button functionality
document.addEventListener('DOMContentLoaded', function() {
    var btn = document.getElementById('getStartedBtn');
    if (btn) {
        btn.addEventListener('click', function() {
            window.location.href = '/Issue/Create';
        });
    }
});
