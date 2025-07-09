document.addEventListener('DOMContentLoaded', function () {
    const sidebarToggle = document.querySelector('.sidebar-toggle');
    const sidebar = document.querySelector('.app-sidebar');
    const appWrapper = document.querySelector('.app-wrapper'); // Used for event listener
    const appContent = document.querySelector('.app-content'); // Used for padding on toggle

    // Function to close sidebar
    function closeSidebar() {
        if (sidebar && sidebar.classList.contains('show')) {
            sidebar.classList.remove('show');
            // Remove overflow hidden from body when sidebar closes
            document.body.style.overflow = '';
        }
    }

    // Toggle sidebar on mobile
    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', function (e) {
            e.stopPropagation(); // Prevent event from bubbling to document
            sidebar.classList.toggle('show');
            // Prevent body scrolling when sidebar is open on mobile
            if (sidebar.classList.contains('show')) {
                document.body.style.overflow = 'hidden';
            } else {
                document.body.style.overflow = '';
            }
        });
    }

    // Close sidebar when clicking outside on mobile
    if (appWrapper && sidebar) {
        appWrapper.addEventListener('click', function (e) {
            // Check if sidebar is currently shown and the click is outside the sidebar
            if (sidebar.classList.contains('show') && !sidebar.contains(e.target) && !sidebarToggle.contains(e.target)) {
                closeSidebar();
            }
        });
    }

    // Highlight active menu item based on controller
    const currentController = window.location.pathname.split('/')[1]; // Gets 'Student', 'Class', etc.
    document.querySelectorAll('.nav-link').forEach(link => {
        const linkHref = link.getAttribute('asp-controller'); // Get the asp-controller attribute

        // Check if the link's asp-controller matches the current controller
        // Also ensure it's not the Home controller unless it's explicitly for Home/Index
        if (linkHref && linkHref.toLowerCase() === currentController.toLowerCase()) {
            link.classList.add('active');
        } else if (currentController === "" || currentController.toLowerCase() === "home") {
            // Special case for Home/Index to highlight its link
            if (link.getAttribute('asp-controller').toLowerCase() === "home") {
                link.classList.add('active');
            }
        }
    });

    // Handle initial state for sidebar on larger screens
    // Ensure sidebar is always visible on desktop and hidden by default on mobile
    function handleSidebarVisibility() {
        if (window.innerWidth >= 768) {
            if (sidebar) {
                sidebar.classList.remove('show'); // Ensure it's not 'show' class which is for mobile toggle
                document.body.style.overflow = ''; // Ensure body scroll is re-enabled
            }
        }
    }

    // Run on load and on resize
    window.addEventListener('resize', handleSidebarVisibility);
    handleSidebarVisibility(); // Initial call
});