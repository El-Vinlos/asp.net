document.addEventListener("DOMContentLoaded", () => {
    const menuToggle = document.getElementById("menu-toggle");
    const menuClose = document.getElementById("menu-toggle-active");
    const sidebarMenu = document.getElementById("sidebar-menu");
    const sidebarOverlay = document.getElementById("sidebarOverlay");
    const navbar = document.querySelector(".navbar-top");
    const isInnerPage = document.body.classList.contains("inner-page");

    let navbarHovered = false;
    console.log("Page class:", document.body.className, "isInnerPage:", isInnerPage);
    const updateNavbarScrolledState = () => {
    const sidebarIsActive = sidebarMenu?.classList.contains("active");
    const isScrolled = window.scrollY > 50;

    if (isInnerPage) {
        navbar.classList.add("scrolled");
        return;
    }

    if (sidebarIsActive || isScrolled || navbarHovered) {
        navbar.classList.add("scrolled");
    } else {
        navbar.classList.remove("scrolled");
    }
};

    // Sidebar open/close
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
        menuToggle.addEventListener("click", (e) => { e.stopPropagation(); openSidebar(); });
        menuClose.addEventListener("click", (e) => { e.stopPropagation(); closeSidebar(); });
        sidebarMenu.addEventListener("click", (e) => e.stopPropagation());
        sidebarOverlay.addEventListener("click", closeSidebar);
        window.addEventListener("scroll", updateNavbarScrolledState);
        navbar.addEventListener("mouseenter", () => { navbarHovered = true; updateNavbarScrolledState(); });
        navbar.addEventListener("mouseleave", () => { navbarHovered = false; updateNavbarScrolledState(); });
        updateNavbarScrolledState();
    }

    // =============================
    // Search Overlay
    // =============================
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
            const query = searchInput.value.trim();
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

    // =============================
    // Cart Dropdown + Refresh
    // =============================
    const bagToggle = document.getElementById("bagToggle");
    const bagMenu = document.getElementById("bagMenu");
    const bagCount = document.getElementById("bagCount");
    const bagItems = document.getElementById("bagItems");

    if (bagToggle && bagMenu) {
        bagToggle.addEventListener("click", (e) => {
            e.stopPropagation();
            bagMenu.style.display = bagMenu.style.display === "block" ? "none" : "block";
        });
        document.addEventListener("click", () => { bagMenu.style.display = "none"; });
    }

    async function refreshCart() {
        const res = await fetch("/Bag/GetBagDetail");
        if (!res.ok) return;
        const data = await res.json();

        if (data.count >=99 )
            count = "99+";
        else 
            count = data.count;
        
        bagCount.textContent = count ?? 0;

        bagItems.innerHTML = "";

        if (!data.items || data.items.length === 0) {
            bagItems.innerHTML = `<li class="empty">Your bag is empty</li>`;
            return;
        }

        data.items.forEach(item => {
            const li = document.createElement("li");
            li.classList.add("bag-item");
            li.innerHTML = `
                <img src="/${item.image}" alt="${item.image}'s image" class="bag-thumb"/>
                <div class="bag-details">
                    <span class="bag-name">${item.name}</span>
                    <span class="bag-price">$${(item.price * item.quantity).toFixed(2)}</span>
                    <span class="bag-qty">Quantity: ${item.quantity}</span>
                </div>
                <button class="remove-btn" data-id="${item.id}"><i class="fa-solid fa-xmark"></i></button>
            `;
            bagItems.appendChild(li);
        });

        document.querySelectorAll(".remove-btn").forEach(btn => {
            btn.addEventListener("click", async (e) => {
                e.stopPropagation();
                await fetch(`/Bag/RemoveFromCart?id=${btn.dataset.id}`, { method: "POST" });
                refreshCart();
            });
        });
    }

    refreshCart();
});
