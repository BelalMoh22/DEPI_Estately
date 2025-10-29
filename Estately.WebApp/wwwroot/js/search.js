// Search Interface JavaScript
document.addEventListener("DOMContentLoaded", function () {
  // Tab switching functionality
  const searchTabs = document.querySelectorAll(".search-tab");
  searchTabs.forEach((tab) => {
    tab.addEventListener("click", function () {
      // Remove active class from all tabs
      searchTabs.forEach((t) => t.classList.remove("active"));
      // Add active class to clicked tab
      this.classList.add("active");
    });
  });

  // Property type selection
  const propertyTypeBtns = document.querySelectorAll(".property-type-btn");
  propertyTypeBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
      this.classList.toggle("selected");
    });
  });

  // Amenity selection
  const amenityBtns = document.querySelectorAll(".amenity-btn");
  amenityBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
      this.classList.toggle("selected");
    });
  });

  // Bedroom/Bathroom selection
  const optionBtns = document.querySelectorAll(".option-btn");
  optionBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
      // Remove selected class from siblings in the same section
      const section = this.closest(".filter-section");
      const siblings = section.querySelectorAll(".option-btn");
      siblings.forEach((sibling) => sibling.classList.remove("selected"));

      // Add selected class to clicked button
      this.classList.add("selected");
    });
  });

  // Range slider functionality
  const priceMinSlider = document.getElementById("priceMin");
  const priceMaxSlider = document.getElementById("priceMax");
  const priceMinInput = document.getElementById("priceMinInput");
  const priceMaxInput = document.getElementById("priceMaxInput");

  if (priceMinSlider && priceMaxSlider && priceMinInput && priceMaxInput) {
    // Update input values when sliders change
    priceMinSlider.addEventListener("input", function () {
      const value = parseInt(this.value).toLocaleString();
      priceMinInput.value = value;
    });

    priceMaxSlider.addEventListener("input", function () {
      const value = parseInt(this.value).toLocaleString();
      priceMaxInput.value = value;
    });

    // Update slider values when inputs change
    priceMinInput.addEventListener("input", function () {
      const value = parseInt(this.value.replace(/,/g, ""));
      if (!isNaN(value)) {
        priceMinSlider.value = value;
      }
    });

    priceMaxInput.addEventListener("input", function () {
      const value = parseInt(this.value.replace(/,/g, ""));
      if (!isNaN(value)) {
        priceMaxSlider.value = value;
      }
    });
  }

  // Area range sliders
  const areaMinSlider = document.getElementById("areaMin");
  const areaMaxSlider = document.getElementById("areaMax");
  const areaMinInput = document.getElementById("areaMinInput");
  const areaMaxInput = document.getElementById("areaMaxInput");

  if (areaMinSlider && areaMaxSlider && areaMinInput && areaMaxInput) {
    // Update input values when sliders change
    areaMinSlider.addEventListener("input", function () {
      areaMinInput.value = this.value;
    });

    areaMaxSlider.addEventListener("input", function () {
      areaMaxInput.value = this.value;
    });

    // Update slider values when inputs change
    areaMinInput.addEventListener("input", function () {
      const value = parseInt(this.value);
      if (!isNaN(value)) {
        areaMinSlider.value = value;
      }
    });

    areaMaxInput.addEventListener("input", function () {
      const value = parseInt(this.value);
      if (!isNaN(value)) {
        areaMaxSlider.value = value;
      }
    });
  }

  // Reset functionality
  const resetBtns = document.querySelectorAll(".reset-btn");
  resetBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
      const section = this.closest(".filter-section");

      // Reset property type buttons
      const propertyBtns = section.querySelectorAll(".property-type-btn");
      propertyBtns.forEach((btn) => btn.classList.remove("selected"));

      // Reset amenity buttons
      const amenityBtns = section.querySelectorAll(".amenity-btn");
      amenityBtns.forEach((btn) => btn.classList.remove("selected"));

      // Reset option buttons (bedrooms/bathrooms)
      const optionBtns = section.querySelectorAll(".option-btn");
      optionBtns.forEach((btn) => btn.classList.remove("selected"));

      // Reset range sliders
      const sliders = section.querySelectorAll(".range-slider");
      sliders.forEach((slider) => {
        if (slider.id === "priceMin") slider.value = 500000;
        if (slider.id === "priceMax") slider.value = 25000000;
        if (slider.id === "areaMin") slider.value = 50;
        if (slider.id === "areaMax") slider.value = 400;
      });

      // Reset range inputs
      const inputs = section.querySelectorAll('input[type="number"]');
      inputs.forEach((input) => {
        if (input.id === "priceMinInput") input.value = "500,000";
        if (input.id === "priceMaxInput") input.value = "25,000,000";
        if (input.id === "areaMinInput") input.value = "50";
        if (input.id === "areaMaxInput") input.value = "400";
      });

      // Reset text inputs
      const textInputs = section.querySelectorAll('input[type="text"]');
      textInputs.forEach((input) => (input.value = ""));
    });
  });

  // Search functionality
  const searchButtons = document.querySelectorAll(
    ".search-button, .search-btn"
  );
  searchButtons.forEach((btn) => {
    btn.addEventListener("click", function () {
      // Collect all filter data
      const filterData = {
        searchQuery:
          document.querySelector(".main-search-input, .mobile-search-input")
            ?.value || "",
        propertyTypes: Array.from(
          document.querySelectorAll(".property-type-btn.selected")
        ).map((btn) => btn.textContent),
        amenities: Array.from(
          document.querySelectorAll(".amenity-btn.selected")
        ).map((btn) => btn.textContent),
        bedrooms:
          document.querySelector(
            ".bedroom-bathroom-options .option-btn.selected"
          )?.textContent || "",
        bathrooms:
          document.querySelectorAll(
            ".bedroom-bathroom-options .option-btn.selected"
          )[1]?.textContent || "",
        priceRange: {
          min: document.getElementById("priceMinInput")?.value || "",
          max: document.getElementById("priceMaxInput")?.value || "",
        },
        areaRange: {
          min: document.getElementById("areaMinInput")?.value || "",
          max: document.getElementById("areaMaxInput")?.value || "",
        },
      };

      console.log("Search filters:", filterData);

      // Here you would typically send the data to your backend
      // For now, we'll just show an alert
      alert(
        "Search functionality would be implemented here with the collected filter data."
      );
    });
  });

  // Modal close functionality
  const filterModal = document.getElementById("filterModal");
  if (filterModal) {
    filterModal.addEventListener("hidden.bs.modal", function () {
      // Optional: Reset filters when modal is closed
      // This can be uncommented if you want to reset filters on close
      // resetAllFilters();
    });
  }
});

// Function to reset all filters (can be called from anywhere)
function resetAllFilters() {
  // Reset all property type buttons
  document
    .querySelectorAll(".property-type-btn")
    .forEach((btn) => btn.classList.remove("selected"));

  // Reset all amenity buttons
  document
    .querySelectorAll(".amenity-btn")
    .forEach((btn) => btn.classList.remove("selected"));

  // Reset all option buttons
  document
    .querySelectorAll(".option-btn")
    .forEach((btn) => btn.classList.remove("selected"));

  // Reset range sliders
  const priceMinSlider = document.getElementById("priceMin");
  const priceMaxSlider = document.getElementById("priceMax");
  const areaMinSlider = document.getElementById("areaMin");
  const areaMaxSlider = document.getElementById("areaMax");

  if (priceMinSlider) priceMinSlider.value = 500000;
  if (priceMaxSlider) priceMaxSlider.value = 25000000;
  if (areaMinSlider) areaMinSlider.value = 50;
  if (areaMaxSlider) areaMaxSlider.value = 400;

  // Reset range inputs
  const priceMinInput = document.getElementById("priceMinInput");
  const priceMaxInput = document.getElementById("priceMaxInput");
  const areaMinInput = document.getElementById("areaMinInput");
  const areaMaxInput = document.getElementById("areaMaxInput");

  if (priceMinInput) priceMinInput.value = "500,000";
  if (priceMaxInput) priceMaxInput.value = "25,000,000";
  if (areaMinInput) areaMinInput.value = "50";
  if (areaMaxInput) areaMaxInput.value = "400";

  // Reset text inputs
  document
    .querySelectorAll('input[type="text"]')
    .forEach((input) => (input.value = ""));
}
