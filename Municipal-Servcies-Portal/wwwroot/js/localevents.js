// Local Events Search Enhancement - Optional AJAX with Debounce
// This file adds live search functionality without full page reloads

// Debounce timer variable - stores the timeout ID
let debounceTimer;

// Wait for the DOM to be fully loaded before attaching event listeners
document.addEventListener('DOMContentLoaded', function() {
    
    // Get references to the search input elements
    const searchInput = document.getElementById('searchName');
    const categorySelect = document.getElementById('category');
    const dateInput = document.getElementById('date');
    
    // Add event listener to search input with debounce
    // Debounce delays the search until user stops typing for 500ms
    if (searchInput) {
        searchInput.addEventListener('input', function() {
            // Clear any existing timer
            clearTimeout(debounceTimer);
            
            // Set a new timer - only executes if user stops typing for 500ms
            debounceTimer = setTimeout(function() {
                performSearch();
            }, 500); // 500ms delay
        });
    }
    
    // Add instant search on category change (no debounce needed)
    if (categorySelect) {
        categorySelect.addEventListener('change', function() {
            performSearch();
        });
    }
    
    // Add instant search on date change (no debounce needed)
    if (dateInput) {
        dateInput.addEventListener('change', function() {
            performSearch();
        });
    }
});

/**
 * Performs the search by submitting the form via AJAX
 * This prevents full page reload and provides a smoother user experience
 */
function performSearch() {
    // Get the current values from the search form
    const searchName = document.getElementById('searchName').value;
    const category = document.getElementById('category').value;
    const date = document.getElementById('date').value;
    
    // Build the query string with only non-empty values
    const params = new URLSearchParams();
    if (searchName) params.append('searchName', searchName);
    if (category) params.append('category', category);
    if (date) params.append('date', date);
    
    // Build the URL with query parameters
    const url = `/LocalEvents/Index?${params.toString()}`;
    
    // Option 1: Simple page reload with new parameters (works immediately)
    // window.location.href = url;
    
    // Option 2: AJAX fetch (uncomment to use - requires additional setup)
    fetch(url, {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest' // Indicates AJAX request
        }
    })
    .then(response => response.text())
    .then(html => {
        // Parse the HTML and update only the events section
        const parser = new DOMParser();
        const doc = parser.parseFromString(html, 'text/html');
        
        // Replace the events list with the new filtered results
        const newEventsList = doc.querySelector('.events-list');
        const currentEventsList = document.querySelector('.events-list');
        
        if (newEventsList && currentEventsList) {
            currentEventsList.innerHTML = newEventsList.innerHTML;
        }
        
        // Update the URL without reloading the page
        window.history.pushState({}, '', url);
    })
    .catch(error => {
        console.error('Search error:', error);
    });
    
}

/**
 * Clear all filters and reload the page
 */
function clearFilters() {
    window.location.href = '/LocalEvents/Index';
}

