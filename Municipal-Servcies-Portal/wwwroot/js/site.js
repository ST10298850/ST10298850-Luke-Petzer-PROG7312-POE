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

    // Store selected files in a Set for easy add/remove
    let selectedFiles = new Map();

    // Enhanced progress tracking including notifications
    function updateProgress() {
        let completedFields = 0;
        let totalFields = 3; // location, category, description are always required

        // Check required fields
        if (formFields.location.value.trim() !== '') completedFields++;
        if (formFields.category.value !== '') completedFields++;
        if (formFields.description.value.trim() !== '') completedFields++;

        // Optional attachment (count as complete if any file is present)
        let hasAttachment = selectedFiles.size > 0;
        if (hasAttachment) {
            completedFields++;
            totalFields++;
        }

        // Optional notification fields (only count if enabled)
        let notificationFields = 0;
        let completedNotifications = 0;
        if (emailCheckbox.checked) {
            notificationFields++;
            totalFields++;
            if (emailInput.value.trim() !== '') completedNotifications++;
        }
        if (phoneCheckbox.checked) {
            notificationFields++;
            totalFields++;
            if (phoneInput.value.trim() !== '') completedNotifications++;
        }
        completedFields += completedNotifications;

        // Calculate progress percentage
        let progressPercent = Math.round((completedFields / totalFields) * 100);
        progressPercent = Math.min(100, progressPercent);

        // Update progress bar with animation
        animateProgressBar(progressPercent);

        // Update progress label based on completion
        updateProgressLabel(completedFields, totalFields, progressPercent);
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

    function updateProgressLabel(completed, total, percent) {
        if (percent === 100) {
            progressLabel.innerHTML = 'Form Complete! <span style="color: #10B981;">✓</span>';
        } else if (completed >= 3) {
            progressLabel.innerHTML = 'Required Fields Complete <span style="color: #F59E0B;">+</span>';
        } else {
            const remaining = 3 - completed;
            progressLabel.textContent = `${remaining} Required Field${remaining !== 1 ? 's' : ''} Remaining`;
        }
    }

    // Add event listeners to all form fields
    formFields.location.addEventListener('input', debounce(updateProgress, 300));
    formFields.category.addEventListener('change', updateProgress);
    formFields.description.addEventListener('input', debounce(updateProgress, 300));

    // File upload functionality with progress tracking
    fileInput.addEventListener('change', function(e) {
        handleFiles(e.target.files);
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
        handleFiles(e.dataTransfer.files);
    });

    function handleFiles(fileList) {
        let added = false;
        for (let file of fileList) {
            if (validateFile(file) && !selectedFiles.has(file.name)) {
                selectedFiles.set(file.name, file);
                added = true;
            }
        }
        if (added) {
            displayFileList();
            updateProgress();
        }
    }

    function displayFileList() {
        fileInfo.innerHTML = '';
        if (selectedFiles.size === 0) {
            fileInfo.style.display = 'none';
            return;
        }
        fileInfo.style.display = 'block';
        selectedFiles.forEach((file, name) => {
            const fileItem = document.createElement('div');
            fileItem.className = 'file-item';
            fileItem.innerHTML = `
                <img src=\"~/images/icons/ic_attatched_file.svg\" alt=\"File Icon\" width=\"16\" height=\"16\" />
                <span class=\"file-name\">${file.name}</span>
                <span class=\"file-size\">${formatFileSize(file.size)}</span>
                <button type=\"button\" class=\"remove-file\" data-filename=\"${file.name}\">×</button>
            `;
            fileInfo.appendChild(fileItem);
        });
        // Add remove event listeners
        fileInfo.querySelectorAll('.remove-file').forEach(btn => {
            btn.addEventListener('click', function() {
                selectedFiles.delete(this.getAttribute('data-filename'));
                displayFileList();
                updateProgress();
            });
        });
    }

    // Override form submit to append all files in FormData
    const form = document.querySelector('.issue-form');
    form.addEventListener('submit', function(e) {
        if (selectedFiles.size > 0) {
            // Replace file input files with selectedFiles
            const dataTransfer = new DataTransfer();
            selectedFiles.forEach(file => dataTransfer.items.add(file));
            fileInput.files = dataTransfer.files;
        }
    });

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

// Home page "Report Issue" button functionality
document.addEventListener('DOMContentLoaded', function() {
    var btn = document.getElementById('ReportIssueBtn');
    if (btn) {
        btn.addEventListener('click', function() {
            window.location.href = '/Issue/Create';
        });
    }
});

// Home page "Report Issue" button functionality
document.addEventListener('DOMContentLoaded', function() {
    var btn = document.getElementById('LocalEventsBtn');
    if (btn) {
        btn.addEventListener('click', function() {
            window.location.href = '/LocalEvents/Index';
        });
    }
});
