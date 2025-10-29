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

// Function to set active navigation link based on current page
function setActiveNavLink() {
  const currentPage = window.location.pathname.split("/").pop() || "index.html";

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

  allNavLinks.forEach((link) => {
    const linkHref = link.getAttribute("href");

    // Check if the link matches the current page
    if (linkHref === currentPage) {
      link.classList.add("active");
    } else {
      link.classList.remove("active");
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
