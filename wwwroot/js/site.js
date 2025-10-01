document.addEventListener("DOMContentLoaded", () => {
    const menuToggle = document.getElementById("menu-toggle");
    const menuClose = document.getElementById("menu-toggle-active");
    const sidebarMenu = document.getElementById("sidebar-menu");
    const sidebarOverlay = document.getElementById("sidebarOverlay");
    const navbar = document.querySelector(".navbar-top");

    let navbarHovered = false; // 👈 track hover state

    const updateNavbarScrolledState = () => {
        const sidebarIsActive = sidebarMenu?.classList.contains("active");
        const isScrolled = window.scrollY > 50;

        if (sidebarIsActive || isScrolled || navbarHovered) {
            navbar.classList.add("scrolled");
        } else {
            navbar.classList.remove("scrolled");
        }
    };

    const openSidebar = () => {
        sidebarMenu.classList.add("active");
        sidebarOverlay.classList.add("active");
        document.body.classList.add("no-scroll");
        updateNavbarScrolledState();
    };

    const closeSidebar = () => {
        sidebarMenu.classList.remove("active");
        sidebarOverlay.classList.remove("active");
        document.body.classList.remove("no-scroll");
        updateNavbarScrolledState();
    };

    if (menuToggle && menuClose && sidebarMenu && sidebarOverlay && navbar) {
        menuToggle.addEventListener("click", (event) => {
            event.stopPropagation();
            openSidebar();
        });

        menuClose.addEventListener("click", (event) => {
            event.stopPropagation();
            closeSidebar();
        });

        sidebarMenu.addEventListener("click", (event) => {
            event.stopPropagation(); // clicking inside sidebar does nothing
        });

        sidebarOverlay.addEventListener("click", () => {
            closeSidebar();
        });

        window.addEventListener("scroll", () => {
            updateNavbarScrolledState();
        });

        navbar.addEventListener("mouseenter", () => {
            navbarHovered = true;
            updateNavbarScrolledState();
        });

        navbar.addEventListener("mouseleave", () => {
            navbarHovered = false;
            updateNavbarScrolledState();
        });
    }
});

// =============================
// Search Overlay
// =============================
document.addEventListener("DOMContentLoaded", () => {
    const searchToggle = document.querySelector(".search-toggle");
    const searchContainer = document.querySelector(".navbar-search");
    const searchInput = document.getElementById("navbarSearchInput");
    const suggestionsContainer = document.getElementById("navbarSearchSuggestions");

    if (searchToggle && searchContainer && searchInput) {
        searchToggle.addEventListener("click", (e) => {
            e.stopPropagation();
            searchContainer.classList.add("active");
            searchInput.focus();
        });

        document.addEventListener("click", (e) => {
            if (!searchContainer.contains(e.target) && !searchToggle.contains(e.target)) {
                searchContainer.classList.remove("active");
                suggestionsContainer.innerHTML = "";
            }
        });

        searchInput.addEventListener("input", async () => {
            const query = searchInput.value;
            if (query.length < 2) {
                suggestionsContainer.innerHTML = "";
                return;
            }

            const res = await fetch(`/Search/Suggestions?term=${encodeURIComponent(query)}`);
            const data = await res.json();

            suggestionsContainer.innerHTML = data
                .map(d => `<div onclick="location.href='/Search/Index?q=${encodeURIComponent(d)}'">${d}</div>`)
                .join("");
        });
    }
});
