function initSearch() {
  const searchInput = document.querySelector(".search-input");
  const searchButton = document.querySelector(".search-button");

  if (searchInput && searchButton) {
    searchButton.addEventListener("click", function () {
      const searchTerm = searchInput.value.trim();
      if (searchTerm) {
        performSearch(searchTerm);
      }
    });
  }
}

function initMobileMenu() {
  const mobileMenuToggle = document.querySelector(".navbar-toggler");
  const mobileMenu = document.querySelector("#mobileMenu");

  if (mobileMenuToggle && mobileMenu) {
    // Initialize offcanvas with proper configuration
    const offcanvasInstance = new bootstrap.Offcanvas(mobileMenu, {
      backdrop: true,
      keyboard: true,
      scroll: false,
    });

    // Close mobile menu when clicking on a link
    const mobileMenuLinks = mobileMenu.querySelectorAll("a");
    mobileMenuLinks.forEach((link) => {
      link.addEventListener("click", () => {
        offcanvasInstance.hide();
      });
    });

    // Handle dropdown toggles within the offcanvas
    const dropdownToggles = mobileMenu.querySelectorAll(
      '[data-bs-toggle="dropdown"]'
    );
    dropdownToggles.forEach((toggle) => {
      toggle.addEventListener("click", (e) => {
        e.preventDefault();
        e.stopPropagation();
        const dropdown = toggle.nextElementSibling;
        if (dropdown) {
          dropdown.classList.toggle("show");
        }
      });
    });
  }
}

// Function to set active navigation link based on current page URL
function setActiveNavLink() {
  const currentPath = window.location.pathname.toLowerCase().trim();
  
  // Get all nav links in desktop menu
  const desktopNavLinks = document.querySelectorAll(
    ".navbar .navbar-nav .nav-link"
  );

  // Get all nav links in mobile menu
  const mobileNavLinks = document.querySelectorAll(
    ".offcanvas .navbar-nav .nav-link"
  );

  // Combine both desktop and mobile nav links
  const allNavLinks = [...desktopNavLinks, ...mobileNavLinks];

  // Extract current route parts
  const currentParts = currentPath.split("/").filter(p => p && p !== "");
  const currentController = currentParts.length >= 1 ? currentParts[0] : "";
  const currentAction = currentParts.length >= 2 ? currentParts[1] : (currentParts.length === 1 && currentParts[0] === "app" ? "index" : currentParts[0] || "index");

  allNavLinks.forEach((link) => {
    const linkHref = link.getAttribute("href")?.toLowerCase().trim() || "";
    
    // Remove active class first
    link.classList.remove("active");
    
    // Extract link route parts
    const hrefParts = linkHref.split("/").filter(p => p && p !== "");
    const linkController = hrefParts.length >= 1 ? hrefParts[0] : "";
    const linkAction = hrefParts.length >= 2 ? hrefParts[1] : (hrefParts.length === 1 && hrefParts[0] === "app" ? "index" : hrefParts[0] || "");
    
    // Check if this link matches the current route
    let isActive = false;
    
    // Controller must match "app"
    if (linkController === "app" && currentController === "app") {
      // Special handling for Index action
      if (linkAction === "index") {
        // Match if current action is "index" or if we're at /App or root
        isActive = currentAction === "index" || currentPath === "/app" || currentPath === "/app/" || currentPath === "/" || (currentParts.length === 1 && currentParts[0] === "app");
      } else {
        // For other actions, exact match required
        isActive = linkAction === currentAction;
      }
    }
    
    if (isActive) {
      link.classList.add("active");
    }
  });
}

document.addEventListener("DOMContentLoaded", function () {
  initSearch();
  initMobileMenu();
  setActiveNavLink();
});

window.addEventListener("scroll", function () {
  const header = document.querySelector(".site-header");
  const secondSection = document.querySelector("#section2");

  if (!header || !secondSection) return;

  const sectionTop = secondSection.offsetTop;
  const scrollY = window.scrollY;

  if (scrollY >= sectionTop - 50) {
    header.classList.add("scrolled");
  } else {
    header.classList.remove("scrolled");
  }
});
