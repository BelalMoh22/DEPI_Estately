// Search Interface JavaScript
document.addEventListener("DOMContentLoaded", function () {
  console.log("🔵 search.js loaded successfully");

  // Desktop filter state
  let desktopSelectedPropertyTypes = [];
  let desktopSelectedBedrooms = "";
  let desktopSelectedBathrooms = "";
  let desktopMinPrice = "";
  let desktopMaxPrice = "";
  let desktopSelectedAreas = [];
  let desktopSelectedZones = [];
  let desktopSelectedDevelopers = [];
  let desktopSelectedAmenities = [];
  let selectedCityId = "";

  // Tab switching functionality
  const searchTabs = document.querySelectorAll(".search-tab");
  searchTabs.forEach((tab) => {
    tab.addEventListener("click", function () {
      searchTabs.forEach((t) => t.classList.remove("active"));
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
      const section = this.closest(".filter-section");
      const siblings = section.querySelectorAll(".option-btn");
      siblings.forEach((sibling) => sibling.classList.remove("selected"));
      this.classList.add("selected");
    });
  });

  // ============================================
  // PRICE FILTER FUNCTIONALITY
  // ============================================
  const priceMinSlider = document.getElementById("priceMin");
  const priceMaxSlider = document.getElementById("priceMax");
  const priceMinInput = document.getElementById("priceMinInput");
  const priceMaxInput = document.getElementById("priceMaxInput");

  if (priceMinSlider && priceMaxSlider && priceMinInput && priceMaxInput) {
    // Initialize values - ensure inputs match sliders
    const minSliderVal = parseInt(priceMinSlider.value) || 0;
    const maxSliderVal = parseInt(priceMaxSlider.value) || 100000000;
    priceMinInput.value = minSliderVal;
    priceMaxInput.value = maxSliderVal;
    // Also ensure sliders are set correctly
    priceMinSlider.value = minSliderVal;
    priceMaxSlider.value = maxSliderVal;

    // Update input when slider changes
    priceMinSlider.addEventListener("input", function () {
      const value = parseInt(this.value);
      priceMinInput.value = value;
      
      // Ensure min doesn't exceed max
      if (value > parseInt(priceMaxSlider.value)) {
        priceMaxSlider.value = value;
        priceMaxInput.value = value;
      }
    });

    priceMaxSlider.addEventListener("input", function () {
      const value = parseInt(this.value);
      priceMaxInput.value = value;
      
      // Ensure max doesn't go below min
      if (value < parseInt(priceMinSlider.value)) {
        priceMinSlider.value = value;
        priceMinInput.value = value;
      }
    });

    // Update slider when input changes
    priceMinInput.addEventListener("input", function () {
      let value = this.value.toString().replace(/[^\d]/g, "");
      value = parseInt(value) || 0;
      
      // Clamp value to valid range
      const min = parseInt(priceMinSlider.min) || 0;
      const max = parseInt(priceMinSlider.max) || 100000000;
      value = Math.max(min, Math.min(max, value));
      
      // Ensure min doesn't exceed max
      const maxValue = parseInt(priceMaxInput.value) || max;
      if (value > maxValue) {
        value = maxValue;
      }
      
      priceMinSlider.value = value;
      this.value = value;
    });

    priceMaxInput.addEventListener("input", function () {
      let value = this.value.toString().replace(/[^\d]/g, "");
      value = parseInt(value) || 0;
      
      // Clamp value to valid range
      const min = parseInt(priceMaxSlider.min) || 0;
      const max = parseInt(priceMaxSlider.max) || 100000000;
      value = Math.max(min, Math.min(max, value));
      
      // Ensure max doesn't go below min
      const minValue = parseInt(priceMinInput.value) || min;
      if (value < minValue) {
        value = minValue;
      }
      
      priceMaxSlider.value = value;
      this.value = value;
    });

    // Handle blur to ensure valid values
    priceMinInput.addEventListener("blur", function () {
      if (!this.value || this.value === "0") {
        this.value = priceMinSlider.min || "0";
        priceMinSlider.value = this.value;
      }
    });

    priceMaxInput.addEventListener("blur", function () {
      if (!this.value || this.value === "0") {
        this.value = priceMaxSlider.max || "100000000";
        priceMaxSlider.value = this.value;
      }
    });
  }

  // Area range sliders
  const areaMinSlider = document.getElementById("areaMin");
  const areaMaxSlider = document.getElementById("areaMax");
  const areaMinInput = document.getElementById("areaMinInput");
  const areaMaxInput = document.getElementById("areaMaxInput");

  if (areaMinSlider && areaMaxSlider && areaMinInput && areaMaxInput) {
    areaMinSlider.addEventListener("input", function () {
      areaMinInput.value = this.value;
    });

    areaMaxSlider.addEventListener("input", function () {
      areaMaxInput.value = this.value;
    });

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

      const propertyBtns = section.querySelectorAll(".property-type-btn");
      propertyBtns.forEach((btn) => btn.classList.remove("selected"));

      const amenityBtns = section.querySelectorAll(".amenity-btn");
      amenityBtns.forEach((btn) => btn.classList.remove("selected"));

      const optionBtns = section.querySelectorAll(".option-btn");
      optionBtns.forEach((btn) => btn.classList.remove("selected"));

      const sliders = section.querySelectorAll(".range-slider");
      sliders.forEach((slider) => {
        if (slider.id === "priceMin") slider.value = slider.min || "0";
        if (slider.id === "priceMax") slider.value = slider.max || "100000000";
        if (slider.id === "areaMin") slider.value = 50;
        if (slider.id === "areaMax") slider.value = 400;
      });

      const inputs = section.querySelectorAll('input[type="number"]');
      inputs.forEach((input) => {
        if (input.id === "priceMinInput") {
          const priceMinSlider = section.querySelector("#priceMin");
          input.value = priceMinSlider?.min || "0";
        }
        if (input.id === "priceMaxInput") {
          const priceMaxSlider = section.querySelector("#priceMax");
          input.value = priceMaxSlider?.max || "100000000";
        }
        if (input.id === "areaMinInput") input.value = "50";
        if (input.id === "areaMaxInput") input.value = "400";
      });

      const textInputs = section.querySelectorAll('input[type="text"]');
      textInputs.forEach((input) => (input.value = ""));

      // Reset zones
      const zonesDropdownMenu = section.querySelector("#zonesDropdownMenu");
      selectedZones = [];
      if (zonesDropdownMenu) {
        zonesDropdownMenu.style.display = "none";
      }
      // Refresh zones list to remove selected state
      if (zonesList && allZones.length > 0) {
        renderZonesList();
      }
      
      // Reset price filters
      const priceMinSlider = section.querySelector("#priceMin");
      const priceMaxSlider = section.querySelector("#priceMax");
      const priceMinInput = section.querySelector("#priceMinInput");
      const priceMaxInput = section.querySelector("#priceMaxInput");
      if (priceMinSlider && priceMinInput) {
        priceMinSlider.value = priceMinSlider.min || "0";
        priceMinInput.value = priceMinSlider.min || "0";
      }
      if (priceMaxSlider && priceMaxInput) {
        priceMaxSlider.value = priceMaxSlider.max || "100000000";
        priceMaxInput.value = priceMaxSlider.max || "100000000";
      }

      // Clear selected filters display
      const selectedFilters = section.querySelectorAll(".selected-filters");
      selectedFilters.forEach((container) => (container.innerHTML = ""));
    });
  });

  // ============================================
  // MOBILE SEARCH AUTocomplete
  // ============================================
  const mobileSearchInput = document.getElementById("mobileSearchInput");
  const searchCategorySelect = document.getElementById("searchCategorySelect");
  const searchSuggestions = document.getElementById("searchSuggestions");
  let autocompleteTimeout = null;
  let selectedZones = [];
  let selectedDevelopers = [];

  // Autocomplete functionality
  if (mobileSearchInput && searchCategorySelect && searchSuggestions) {
    mobileSearchInput.addEventListener("input", function () {
      const term = this.value.trim();
      const category = searchCategorySelect.value;

      clearTimeout(autocompleteTimeout);

      if (term.length < 2) {
        searchSuggestions.innerHTML = "";
        searchSuggestions.classList.remove("show");
        return;
      }

      autocompleteTimeout = setTimeout(() => {
        fetchSuggestions(category, term);
      }, 300);
    });

    // Hide suggestions when clicking outside
    document.addEventListener("click", function (e) {
      if (!mobileSearchInput.contains(e.target) && !searchSuggestions.contains(e.target)) {
        searchSuggestions.classList.remove("show");
      }
    });

    // Handle suggestion selection
    searchSuggestions.addEventListener("click", function (e) {
      const suggestionItem = e.target.closest(".suggestion-item");
      if (suggestionItem) {
        const value = suggestionItem.dataset.value;
        const text = suggestionItem.textContent.trim();
        mobileSearchInput.value = text;
        searchSuggestions.classList.remove("show");

        // Add to selected filters based on category
        const category = searchCategorySelect.value;
        if (category === "zones" && !selectedZones.includes(value)) {
          selectedZones.push(value);
          addSelectedFilter("selectedZones", text, value, "zones");
        } else if (category === "developers" && !selectedDevelopers.includes(value)) {
          selectedDevelopers.push(value);
          addSelectedFilter("selectedDevelopers", text, value, "developers");
        }
      }
    });
  }

  // Fetch autocomplete suggestions
  function fetchSuggestions(category, term) {
    if (!category || !term) {
      searchSuggestions.innerHTML = "";
      searchSuggestions.classList.remove("show");
      return;
    }

    const url = `/TblProperties/Suggestions?category=${encodeURIComponent(category)}&term=${encodeURIComponent(term)}`;
    
    fetch(url)
      .then((response) => response.json())
      .then((data) => {
        if (data && data.length > 0) {
          displaySuggestions(data);
        } else {
          searchSuggestions.innerHTML = '<div class="suggestion-item no-results">No results found</div>';
          searchSuggestions.classList.add("show");
        }
      })
      .catch((error) => {
        console.error("Error fetching suggestions:", error);
        searchSuggestions.classList.remove("show");
      });
  }

  // Display suggestions
  function displaySuggestions(suggestions) {
    searchSuggestions.innerHTML = "";
    suggestions.forEach((item) => {
      const div = document.createElement("div");
      div.className = "suggestion-item";
      // Prefer DeveloperTitle for developers, but keep fallbacks for other entities
      div.textContent =
        item.developerTitle ||
        item.DeveloperTitle ||
        item.name ||
        item.text;
      div.dataset.value = item.id || item.value;
      searchSuggestions.appendChild(div);
    });
    searchSuggestions.classList.add("show");
  }

  // Add selected filter chip
  function addSelectedFilter(containerId, text, value, type) {
    const container = document.getElementById(containerId);
    if (!container) return;

    const chip = document.createElement("div");
    chip.className = "filter-chip";
    chip.innerHTML = `
      <span>${text}</span>
      <button type="button" class="remove-filter" data-value="${value}" data-type="${type}">
        <i class="fa-solid fa-times"></i>
      </button>
    `;

    container.appendChild(chip);

    // Remove filter handler
    chip.querySelector(".remove-filter").addEventListener("click", function () {
      const val = this.dataset.value;
      const filterType = this.dataset.type;
      
      if (filterType === "zones") {
        selectedZones = selectedZones.filter((z) => z !== val);
        // Update zone list item visual state
        const zoneItem = zonesList?.querySelector(`.zone-list-item[data-zone-id="${val}"]`);
        if (zoneItem) {
          zoneItem.classList.remove("selected");
        }
      } else if (filterType === "developers") {
        selectedDevelopers = selectedDevelopers.filter((d) => d !== val);
      }
      
      chip.remove();
    });
  }

  // ============================================
  // ZONES CLICK-TO-ADD FUNCTIONALITY
  // ============================================
  const addZonesBtn = document.getElementById("addZonesBtn");
  const zonesDropdownMenu = document.getElementById("zonesDropdownMenu");
  const zonesList = document.getElementById("zonesList");

  // Static zones data mirroring the database, shaped for the frontend ({ id, cityId, name })
  const zonesData = [
    // Cairo (CityID: 1)
    { id: 1, cityId: 1, name: "Nasr City" },
    { id: 2, cityId: 1, name: "Heliopolis" },
    { id: 3, cityId: 1, name: "Maadi" },
    { id: 4, cityId: 1, name: "New Cairo" },
    { id: 5, cityId: 1, name: "Zamalek" },

    // Giza (CityID: 2)
    { id: 6, cityId: 2, name: "Dokki" },
    { id: 7, cityId: 2, name: "Mohandessin" },
    { id: 8, cityId: 2, name: "Haram" },
    { id: 9, cityId: 2, name: "October City" },
    { id: 10, cityId: 2, name: "Sheikh Zayed" },

    // Alexandria (CityID: 3)
    { id: 11, cityId: 3, name: "Stanley" },
    { id: 12, cityId: 3, name: "Smouha" },
    { id: 13, cityId: 3, name: "Glim" },
    { id: 14, cityId: 3, name: "Miami" },
    { id: 15, cityId: 3, name: "Sidi Gaber" },

    // Mansoura (CityID: 4)
    { id: 16, cityId: 4, name: "Al Gomhoria" },
    { id: 17, cityId: 4, name: "Al Mashaya" },
    { id: 18, cityId: 4, name: "Talkha" },
    { id: 19, cityId: 4, name: "Al Mahatta" },

    // Tanta (CityID: 5)
    { id: 20, cityId: 5, name: "El Gish Street" },
    { id: 21, cityId: 5, name: "Saad Street" },
    { id: 22, cityId: 5, name: "Stanley" },
    { id: 23, cityId: 5, name: "El Bahr Street" },

    // Aswan (CityID: 6)
    { id: 24, cityId: 6, name: "Aswan (Aswa)" },
    { id: 25, cityId: 6, name: "El Shallal" },
    { id: 26, cityId: 6, name: "El Sadat" },
    { id: 27, cityId: 6, name: "Ferasan" },

    // Luxor (CityID: 7)
    { id: 28, cityId: 7, name: "East Bank" },
    { id: 29, cityId: 7, name: "West Bank" },
    { id: 30, cityId: 7, name: "Karnak" },
    { id: 31, cityId: 7, name: "El Tod" },

    // Hurghada (CityID: 8)
    { id: 32, cityId: 8, name: "El Dahar" },
    { id: 33, cityId: 8, name: "Sakkala" },
    { id: 34, cityId: 8, name: "El Kawther" },
    { id: 35, cityId: 8, name: "Makadi Bay" },

    // Port Said (CityID: 10)
    { id: 41, cityId: 10, name: "Al Manakh" },
    { id: 42, cityId: 10, name: "Port Fouad" },
    { id: 43, cityId: 10, name: "Al Sharp" }
  ];

  // Start with all zones loaded from the static mapping
  let allZones = zonesData.slice();

  // Helper: filter zones based on currently selected city (if any)
  function getFilteredZonesForCurrentCity(sourceZones) {
    if (!sourceZones || sourceZones.length === 0) return [];

    // If no city is selected, return all zones
    if (!selectedCityId || selectedCityId.toString().trim() === "") {
      return sourceZones;
    }

    const cityIdStr = selectedCityId.toString();

    // Filter strictly by the known cityId field from zonesData
    return sourceZones.filter((zone) => {
      return zone.cityId && zone.cityId.toString() === cityIdStr;
    });
  }

  // Load zones on page load
  if (zonesList) {
    loadZonesList();
  }

  // Toggle dropdown menu on plus button click
  if (addZonesBtn && zonesDropdownMenu) {
    addZonesBtn.addEventListener("click", function (e) {
      e.preventDefault();
      e.stopPropagation();
      
      // Toggle dropdown visibility
      const isVisible = zonesDropdownMenu.style.display !== "none";
      zonesDropdownMenu.style.display = isVisible ? "none" : "block";
      
      // Refresh the list to show which zones are already selected
      if (!isVisible && allZones.length > 0) {
        renderZonesList();
      }
    });
  }

  // Close dropdown when clicking outside
  document.addEventListener("click", function (e) {
    if (
      zonesDropdownMenu &&
      !zonesDropdownMenu.contains(e.target) &&
      (!addZonesBtn || !addZonesBtn.contains(e.target)) &&
      zonesDropdownMenu.style.display === "block"
    ) {
      zonesDropdownMenu.style.display = "none";
    }
  });

  function loadZonesList() {
    if (!zonesList) return;
    
    // Use static zonesData to avoid relying on API city linkage
    allZones = zonesData.slice();
    renderZonesList();
  }

  // Render zones as clickable list items
  function renderZonesList() {
    if (!zonesList) return;
    
    zonesList.innerHTML = "";
    
    if (allZones.length === 0) {
      zonesList.innerHTML = '<div class="zones-loading">No zones available</div>';
      return;
    }
    
    const filteredZones = getFilteredZonesForCurrentCity(allZones);
    filteredZones.forEach((zone) => {
      const zoneItem = document.createElement("div");
      zoneItem.className = "zone-list-item";
      zoneItem.dataset.zoneId = zone.id;
      zoneItem.textContent = zone.name;
      
      // Mark as selected if already in selectedZones
      if (selectedZones.includes(zone.id.toString())) {
        zoneItem.classList.add("selected");
      }
      
      // Click handler to add/remove zone
      zoneItem.addEventListener("click", function () {
        const zoneId = this.dataset.zoneId;
        
        if (selectedZones.includes(zoneId)) {
          // Remove zone
          selectedZones = selectedZones.filter(z => z !== zoneId);
          this.classList.remove("selected");
          removeZoneFromDisplay(zoneId);
        } else {
          // Add zone
          selectedZones.push(zoneId);
          this.classList.add("selected");
          addSelectedFilter("selectedZones", zone.name, zoneId, "zones");
        }
      });
      
      zonesList.appendChild(zoneItem);
    });
  }

  // Remove zone from display when unselected
  function removeZoneFromDisplay(zoneId) {
    const container = document.getElementById("selectedZones");
    if (!container) return;
    
    const chip = container.querySelector(`.filter-chip .remove-filter[data-value="${zoneId}"]`);
    if (chip && chip.closest(".filter-chip")) {
      chip.closest(".filter-chip").remove();
    }
  }

  // Update selected zones display (called when removing via X button)
  function updateSelectedZonesDisplay() {
    const container = document.getElementById("selectedZones");
    if (!container) return;
    
    container.innerHTML = "";
    
    // Get zone names for selected IDs
    if (allZones.length > 0 && selectedZones.length > 0) {
      selectedZones.forEach((zoneId) => {
        const zone = allZones.find(z => z.id.toString() === zoneId);
        if (zone) {
          addSelectedFilter("selectedZones", zone.name, zoneId, "zones");
        }
      });
    }
  }

  // Zones and Developers input handlers
  const developersInput = document.getElementById("developersInput");

  if (developersInput) {
    developersInput.addEventListener("keypress", function (e) {
      if (e.key === "Enter") {
        e.preventDefault();
        const value = this.value.trim();
        if (value && !selectedDevelopers.includes(value)) {
          selectedDevelopers.push(value);
          addSelectedFilter("selectedDevelopers", value, value, "developers");
          this.value = "";
        }
      }
    });
  }
  // Remove any old event listeners and attach new ones
  const searchButtons = document.querySelectorAll(".search-button, .search-btn");
  searchButtons.forEach((btn) => {
    // Clone button to remove all existing event listeners
    const newBtn = btn.cloneNode(true);
    if (btn.parentNode) {
      btn.parentNode.replaceChild(newBtn, btn);
    }
    
    // Add the new event listener with capture phase to ensure it runs first
    newBtn.addEventListener("click", function (e) {
      e.preventDefault();
      e.stopPropagation();
      e.stopImmediatePropagation();
      
      // Close modal if it's open (for mobile filter modal)
      const filterModalEl = document.getElementById("filterModal");
      if (filterModalEl) {
        try {
          const filterModal = bootstrap.Modal.getInstance(filterModalEl);
          if (filterModal) {
            filterModal.hide();
          }
        } catch (err) {
          // If Bootstrap modal API fails, just hide it manually
          filterModalEl.classList.remove("show");
          filterModalEl.style.display = "none";
          document.body.classList.remove("modal-open");
          const backdrop = document.querySelector(".modal-backdrop");
          if (backdrop) backdrop.remove();
        }
      }
      
      // Small delay to allow modal to close before redirect
      setTimeout(() => {
        performSearch(e);
      }, 150);
    }, true); // Use capture phase
  });

  // Also handle Enter key on mobile search input
  if (mobileSearchInput) {
    mobileSearchInput.addEventListener("keypress", function (e) {
      if (e.key === "Enter") {
        e.preventDefault();
        performSearch();
      }
    });
  }

  function performSearch(e) {
    // Prevent any default behavior if event is provided
    if (e) {
      e.preventDefault();
      e.stopPropagation();
    }
    
    // Collect all filter data
    const searchQuery = mobileSearchInput?.value || document.querySelector(".main-search-input")?.value || "";
    
    // Get selected property types (from mobile modal or desktop modal)
    // Exclude desktop multi-select buttons used for areas/zones/developers
    let propertyTypes = Array.from(
      document.querySelectorAll(
        ".property-type-btn.selected:not(.desktop-property-type-btn):not(.desktop-multiselect-btn)"
      )
    ).map((btn) => btn.textContent.trim());
    
    // Add desktop property types if any
    if (desktopSelectedPropertyTypes && desktopSelectedPropertyTypes.length > 0) {
      propertyTypes = [...propertyTypes, ...desktopSelectedPropertyTypes];
      propertyTypes = [...new Set(propertyTypes)]; // Remove duplicates
    }

    // Get selected amenities (from mobile modal)
    let amenities = Array.from(
      document.querySelectorAll(".amenity-btn.selected")
    ).map((btn) => btn.textContent.trim());
    
    // Add desktop amenities if any
    if (desktopSelectedAmenities && desktopSelectedAmenities.length > 0) {
      // Get amenity names from IDs
      const desktopAmenityNames = desktopSelectedAmenities
        .map(id => {
          const item = desktopDropdownItems.amenities.find(a => a.id.toString() === id);
          return item ? item.name : null;
        })
        .filter(name => name !== null);
      amenities = [...amenities, ...desktopAmenityNames];
      amenities = [...new Set(amenities)]; // Remove duplicates
    }

    // Get bedrooms and bathrooms (from mobile modal or desktop modal)
    let bedrooms = "";
    let bathrooms = "";
    
    // Check desktop selections first
    if (desktopSelectedBedrooms) {
      bedrooms = desktopSelectedBedrooms;
    } else {
      // Fallback to mobile modal
      const allSections = Array.from(document.querySelectorAll(".filter-section"));
      const bedroomSection = allSections.find(
        (section) => section.querySelector(".filter-title")?.textContent.trim() === "Bedrooms"
      );
      bedrooms = bedroomSection?.querySelector(".bedroom-bathroom-options .option-btn.selected")?.textContent.trim() || "";
    }
    
    if (desktopSelectedBathrooms) {
      bathrooms = desktopSelectedBathrooms;
    } else {
      // Fallback to mobile modal
      const allSections = Array.from(document.querySelectorAll(".filter-section"));
      const bathroomSection = allSections.find(
        (section) => section.querySelector(".filter-title")?.textContent.trim() === "Bathrooms"
      );
      bathrooms = bathroomSection?.querySelector(".bedroom-bathroom-options .option-btn.selected")?.textContent.trim() || "";
    }

    // Get price range - collect from desktop modal first, then mobile modal
    let minPrice = "";
    let maxPrice = "";
    
    // Check desktop price selections first
    if (desktopMinPrice) {
      minPrice = desktopMinPrice;
    }
    if (desktopMaxPrice) {
      maxPrice = desktopMaxPrice;
    }
    
    // Fallback to mobile modal if desktop not set
    if (!minPrice && !maxPrice) {
      const priceMinInput = document.getElementById("priceMinInput");
      const priceMaxInput = document.getElementById("priceMaxInput");
      
      if (priceMinInput && priceMaxInput) {
        // Get current values from inputs (they're synced with sliders)
        const minVal = parseFloat(priceMinInput.value);
        const maxVal = parseFloat(priceMaxInput.value);
        
      // Default values (no filter applied)
      const defaultMin = 0;
      const defaultMax = 10000000;
        
        // Only include minPrice if it's valid and greater than default (user has set a minimum)
        if (!isNaN(minVal) && minVal > defaultMin && minVal > 0) {
          minPrice = minVal.toString();
        }
        
        // Only include maxPrice if it's valid and less than default (user has set a maximum)
        if (!isNaN(maxVal) && maxVal < defaultMax && maxVal > 0) {
          maxPrice = maxVal.toString();
        }
        
        // Validate: minPrice should be less than maxPrice if both are set
        if (minPrice && maxPrice) {
          const min = parseFloat(minPrice);
          const max = parseFloat(maxPrice);
          if (min >= max) {
            // If min >= max, only use minPrice (filter by minimum only)
            maxPrice = "";
          }
        }
      }
    }

    // Get area range
    const minArea = document.getElementById("areaMinInput")?.value || "";
    const maxArea = document.getElementById("areaMaxInput")?.value || "";

    // Get zones and developers from selected filters (mobile or desktop)
    let zones = selectedZones;
    let developers = selectedDevelopers;
    let areas = [];
    
    // Add desktop selections
    if (desktopSelectedZones && desktopSelectedZones.length > 0) {
      zones = [...zones, ...desktopSelectedZones];
      zones = [...new Set(zones)]; // Remove duplicates
    }
    
    if (desktopSelectedDevelopers && desktopSelectedDevelopers.length > 0) {
      developers = [...developers, ...desktopSelectedDevelopers];
      developers = [...new Set(developers)]; // Remove duplicates
    }
    
    if (desktopSelectedAreas && desktopSelectedAreas.length > 0) {
      areas = desktopSelectedAreas;
    }
    
    if (desktopSelectedAmenities && desktopSelectedAmenities.length > 0) {
      // Merge desktop amenity IDs into the existing amenities list
      amenities = [...amenities, ...desktopSelectedAmenities];
      amenities = [...new Set(amenities)];
    }

    // Build query string - only include non-empty filters
    const params = new URLSearchParams();
    
    // Only add parameters that have actual values
    if (searchQuery && searchQuery.trim()) {
      params.append("search", searchQuery.trim());
    }

    // City filter - send selected city ID if available
    const cityInput = document.getElementById("selectedCityId");
    const cityValue = (cityInput && cityInput.value) || selectedCityId || "";
    if (cityValue && cityValue.toString().trim()) {
      params.append("city", cityValue.toString().trim());
    }
    
    // Merge areas and zones into a single zones parameter
    const allZones = [
      ...(areas || []),
      ...(zones || [])
    ].filter((item) => item && item.trim());
    if (allZones.length > 0) {
      params.append("zones", allZones.join(","));
    }
    
    if (developers && developers.length > 0 && developers.some(d => d && d.trim())) {
      params.append("developers", developers.filter(d => d && d.trim()).join(","));
    }
    
    if (amenities && amenities.length > 0 && amenities.some(a => a && a.trim())) {
      params.append("amenities", amenities.filter(a => a && a.trim()).join(","));
    }
    
    if (propertyTypes && propertyTypes.length > 0 && propertyTypes.some(pt => pt && pt.trim())) {
      params.append("propertyTypes", propertyTypes.filter(pt => pt && pt.trim()).join(","));
    }
    
    // Add price filters - only if they have valid values
    if (minPrice && minPrice.trim()) {
      const minP = parseFloat(minPrice.trim());
      if (!isNaN(minP) && minP > 0) {
        params.append("minPrice", minPrice.trim());
      }
    }
    
    if (maxPrice && maxPrice.trim()) {
      const maxP = parseFloat(maxPrice.trim());
      if (!isNaN(maxP) && maxP > 0) {
        params.append("maxPrice", maxPrice.trim());
      }
    }
    
    if (minArea && minArea.trim() && !isNaN(parseFloat(minArea))) {
      params.append("minArea", minArea.trim());
    }
    
    if (maxArea && maxArea.trim() && !isNaN(parseFloat(maxArea))) {
      params.append("maxArea", maxArea.trim());
    }
    
    if (bedrooms && bedrooms.trim()) {
      params.append("bedrooms", bedrooms.trim());
    }
    
    if (bathrooms && bathrooms.trim()) {
      params.append("bathrooms", bathrooms.trim());
    }
    
    if (amenities && amenities.length > 0 && amenities.some(a => a && a.trim())) {
      params.append("amenities", amenities.filter(a => a && a.trim()).join(","));
    }

    // Always include page and pageSize for pagination
    params.append("page", "1");
    params.append("pageSize", "9");

    // Redirect to Properties page
    const url = `/App/Properties?${params.toString()}`;
    window.location.href = url;
  }

  // Handle desktop search button click
  const desktopSearchBtn = document.querySelector(".desktop-search-btn");
  if (desktopSearchBtn) {
    desktopSearchBtn.addEventListener("click", function (e) {
      e.preventDefault();
      performSearch(e);
    });
  }
  
  // Store items globally for badge display and desktop modals
  const desktopDropdownItems = {
    areas: [],
    zones: [],
    developers: [],
    amenities: [],
    cities: []
  };

  // ============================================
  // CITY SELECTION (DESKTOP) - CONSISTENT MAPPING
  // ============================================

  console.log("🔵 search.js loaded - city selection fix");

  const selectedCityInput = document.getElementById("selectedCityId");
  const selectedCityTextEl = document.getElementById("selectedCityText");
  const mobileSelectedCityTextEl = document.getElementById("mobileSelectedCityText");
  const mobileZonesTextEl = document.getElementById("mobileZonesText");
  const mobileDevelopersTextEl = document.getElementById("mobileDevelopersText");
  const mobileAmenitiesTextEl = document.getElementById("mobileAmenitiesText");
  const mobilePropertyTypesTextEl = document.getElementById("mobilePropertyTypesText");
  const mobileBedsBathsTextEl = document.getElementById("mobileBedsBathsText");
  const mobilePriceRangeTextEl = document.getElementById("mobilePriceRangeText");
  const applyCityBtn = document.getElementById("applyCityBtn");

  let citiesData = []; // unified { id, name } list for mapping

  function applyCityStyles() {
    if (document.getElementById("cityStyles")) return;

    const style = document.createElement("style");
    style.id = "cityStyles";
    style.textContent = `
        .city-btn {
            display: inline-block;
            margin: 5px;
            padding: 10px 20px;
            background: #ffffff;
            border: 2px solid #e0e0e0;
            border-radius: 25px;
            cursor: pointer;
            font-size: 14px;
            color: #333;
            transition: all 0.3s ease;
            text-decoration: none;
            text-align: center;
        }
        
        .city-btn:hover {
            background: #007bff;
            color: white;
            border-color: #007bff;
            transform: translateY(-2px);
        }
        
        .city-btn.active {
            background: #007bff;
            color: white;
            border-color: #007bff;
        }
        
        #citiesListContainer {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            padding: 15px;
            justify-content: center;
        }
    `;
    document.head.appendChild(style);
  }

  function loadCities() {
    console.log("🔵 Loading cities with proper mapping...");

    const container = document.getElementById("citiesListContainer");
    if (!container) {
      console.error("❌ Cities container not found");
      return;
    }

    applyCityStyles();

    loadCitiesFromAPI()
      .then((apiCities) => {
        if (apiCities && apiCities.length > 0) {
          // Normalize API data to { id, name }
          citiesData = apiCities.map((c) => ({ id: c.id, name: c.name }));
          console.log("✅ Using API cities data:", citiesData);
          displayCities(citiesData);
        } else {
          throw new Error("No API data");
        }
      })
      .catch((error) => {
        console.warn("⚠️ Using fallback cities data:", error);
        citiesData = [
          { id: 1, name: "Cairo" },
          { id: 2, name: "Alexandria" },
          { id: 3, name: "Giza" },
          { id: 4, name: "Mansoura" },
          { id: 5, name: "Aswan" },
          { id: 6, name: "Luxor" },
          { id: 7, name: "Hurghada" },
          { id: 8, name: "Sharm El Sheikh" }
        ];
        displayCities(citiesData);
      });
  }

  function loadCitiesFromAPI() {
    return fetch("/TblProperties/GetAllCities")
      .then((response) => {
        if (!response.ok) {
          throw new Error(`API response: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        console.log("🌐 API Cities response:", data);
        return data;
      });
  }

  function displayCities(cities) {
    const container = document.getElementById("citiesListContainer");
    if (!container) return;

    container.innerHTML = "";

    cities.forEach((city) => {
      const button = document.createElement("button");
      button.type = "button";
      button.className = "city-btn";
      button.textContent = city.name;

      // store both id and name
      button.setAttribute("data-city-id", city.id);
      button.setAttribute("data-city-name", city.name);

      button.addEventListener("click", function () {
        handleCitySelection(
          this.getAttribute("data-city-id"),
          this.getAttribute("data-city-name")
        );
      });

      container.appendChild(button);
    });

    console.log("✅ Cities displayed with verified mapping");
  }

  function handleCitySelection(cityId, cityName) {
    console.log(`📍 City Selected - ID: ${cityId}, Name: ${cityName}`);

    if (!cityId || !cityName) {
      console.error("❌ Invalid city selection data");
      return;
    }

    const selectedCity = citiesData.find(
      (city) => city.id == cityId && city.name === cityName
    );

    if (!selectedCity) {
      console.error("❌ City selection mismatch:", { cityId, cityName, citiesData });
      alert("Error: City selection mismatch. Please try again.");
      return;
    }

    updateCitySelectionUI(cityId, cityName);
    updateCityFormFields(cityId, cityName);

    const modal = document.getElementById("cityModal");
    if (modal) {
      const bootstrapModal = bootstrap.Modal.getInstance(modal);
      if (bootstrapModal) {
        bootstrapModal.hide();
      }
    }

    console.log("✅ City selection completed successfully");
  }

  function updateCitySelectionUI(cityId, cityName) {
    // use existing city button label
    if (selectedCityTextEl) {
      selectedCityTextEl.textContent = cityName;
    }

    if (mobileSelectedCityTextEl) {
      mobileSelectedCityTextEl.textContent = cityName;
    }

    // If zones have already been loaded for mobile, re-render them with the new city filter
    if (typeof renderZonesList === "function" && Array.isArray(allZones) && allZones.length > 0) {
      try {
        renderZonesList();
      } catch (err) {
        console.warn("Zones re-render after city change failed:", err);
      }
    }

    // highlight active city button
    const container = document.getElementById("citiesListContainer");
    if (container) {
      const buttons = container.querySelectorAll(".city-btn");
      buttons.forEach((btn) => {
        btn.classList.toggle(
          "active",
          btn.getAttribute("data-city-id") === String(cityId)
        );
      });
    }
  }

  function updateCityFormFields(cityId, cityName) {
    if (selectedCityInput) selectedCityInput.value = cityId;
    if (typeof selectedCityId !== "undefined") {
      selectedCityId = cityId; // keep JS state in sync for performSearch
    }
    console.log("📝 Form fields updated:", { cityId, cityName });
  }

  // Event delegation as a safety net for dynamically created buttons
  document.addEventListener("click", function (e) {
    const target = e.target;
    if (target && target.classList.contains("city-btn")) {
      const cityId = target.getAttribute("data-city-id");
      const cityName = target.getAttribute("data-city-name");
      if (cityId && cityName) {
        handleCitySelection(cityId, cityName);
      }
    }
  });

  // Initialize city list shortly after DOM is ready and also when modal opens
  console.log("🔵 Initializing city selection...");
  setTimeout(loadCities, 1000);

  const cityBtn = document.getElementById("cityBtn");
  if (cityBtn) {
    cityBtn.addEventListener("click", function () {
      setTimeout(loadCities, 300);
    });
  }

  const cityModalEl = document.getElementById("cityModal");
  if (cityModalEl) {
    cityModalEl.addEventListener("shown.bs.modal", function () {
      setTimeout(loadCities, 200);
    });
  }

  function renderDesktopMultiSelect(gridEl, items, selectedArray, itemNameSelector) {
    if (!gridEl) return;
    gridEl.innerHTML = "";
    if (!items || items.length === 0) return;

    items.forEach((item) => {
      const id = item.id?.toString();
      const name = item[itemNameSelector] || item.displayName || item.name || item.text || `Item ${id}`;
      const btn = document.createElement("button");
      btn.type = "button";
      btn.className = "property-type-btn desktop-multiselect-btn";
      btn.dataset.id = id;
      btn.textContent = name;
      if (selectedArray.includes(id)) {
        btn.classList.add("selected");
      }
      gridEl.appendChild(btn);
    });
  }

  // Areas modal
  const desktopAreaModal = document.getElementById("desktopAreaModal");
  const desktopAreasGrid = document.getElementById("desktopAreasGrid");
  const desktopAreasConfirm = document.getElementById("desktopAreasConfirm");

  if (desktopAreaModal && desktopAreasGrid && desktopAreasConfirm) {
    desktopAreaModal.addEventListener("show.bs.modal", function () {
      if (desktopDropdownItems.areas.length === 0) {
        fetch("/TblProperties/GetAllAreas")
          .then((r) => r.json())
          .then((data) => {
            desktopDropdownItems.areas = data || [];
            renderDesktopMultiSelect(desktopAreasGrid, desktopDropdownItems.areas, desktopSelectedAreas, "displayName");
          })
          .catch((err) => console.error("Error loading areas:", err));
      } else {
        renderDesktopMultiSelect(desktopAreasGrid, desktopDropdownItems.areas, desktopSelectedAreas, "displayName");
      }
    });

    desktopAreasGrid.addEventListener("click", function (e) {
      const btn = e.target.closest(".desktop-multiselect-btn");
      if (!btn) return;
      const id = btn.dataset.id;
      btn.classList.toggle("selected");
      if (btn.classList.contains("selected")) {
        if (!desktopSelectedAreas.includes(id)) desktopSelectedAreas.push(id);
      } else {
        desktopSelectedAreas = desktopSelectedAreas.filter((a) => a !== id);
      }
      updateDesktopAreaDisplay();
    });

    desktopAreasConfirm.addEventListener("click", function () {
      const selectedBtns = desktopAreasGrid.querySelectorAll(".desktop-multiselect-btn.selected");
      desktopSelectedAreas = Array.from(selectedBtns).map((b) => b.dataset.id);
      updateDesktopAreaDisplay();
      const modal = bootstrap.Modal.getInstance(desktopAreaModal);
      if (modal) modal.hide();
    });
  }

  // Zones modal
  const desktopZonesModal = document.getElementById("desktopZonesModal");
  const desktopZonesGrid = document.getElementById("desktopZonesGrid");
  const desktopZonesConfirm = document.getElementById("desktopZonesConfirm");

  if (desktopZonesModal && desktopZonesGrid && desktopZonesConfirm) {
    desktopZonesModal.addEventListener("show.bs.modal", function () {
      const renderForCurrentCity = () => {
        const sourceZones = desktopDropdownItems.zones || [];
        const filtered = getFilteredZonesForCurrentCity(sourceZones);
        renderDesktopMultiSelect(desktopZonesGrid, filtered, desktopSelectedZones, "name");
      };

      if (desktopDropdownItems.zones.length === 0) {
        // Use the same static zonesData mapping as mobile to ensure city/zone linkage
        desktopDropdownItems.zones = zonesData.slice();
        renderForCurrentCity();
      } else {
        renderForCurrentCity();
      }
    });

    desktopZonesGrid.addEventListener("click", function (e) {
      const btn = e.target.closest(".desktop-multiselect-btn");
      if (!btn) return;
      const id = btn.dataset.id;
      btn.classList.toggle("selected");
      if (btn.classList.contains("selected")) {
        if (!desktopSelectedZones.includes(id)) desktopSelectedZones.push(id);
      } else {
        desktopSelectedZones = desktopSelectedZones.filter((z) => z !== id);
      }
      updateDesktopZonesDisplay();
    });

    desktopZonesConfirm.addEventListener("click", function () {
      const selectedBtns = desktopZonesGrid.querySelectorAll(".desktop-multiselect-btn.selected");
      desktopSelectedZones = Array.from(selectedBtns).map((b) => b.dataset.id);
      updateDesktopZonesDisplay();
      const modal = bootstrap.Modal.getInstance(desktopZonesModal);
      if (modal) modal.hide();
    });
  }

  // Developers modal
  const desktopDevelopersModal = document.getElementById("desktopDevelopersModal");
  const desktopDevelopersGrid = document.getElementById("desktopDevelopersGrid");
  const desktopDevelopersConfirm = document.getElementById("desktopDevelopersConfirm");

  if (desktopDevelopersModal && desktopDevelopersGrid && desktopDevelopersConfirm) {
    desktopDevelopersModal.addEventListener("show.bs.modal", function () {
      if (desktopDropdownItems.developers.length === 0) {
        fetch("/TblProperties/GetAllDevelopers")
          .then((r) => r.json())
          .then((data) => {
            desktopDropdownItems.developers = data || [];
            // Use DeveloperTitle where available
            renderDesktopMultiSelect(
              desktopDevelopersGrid,
              desktopDropdownItems.developers,
              desktopSelectedDevelopers,
              "developerTitle"
            );
          })
          .catch((err) => console.error("Error loading developers:", err));
      } else {
        renderDesktopMultiSelect(
          desktopDevelopersGrid,
          desktopDropdownItems.developers,
          desktopSelectedDevelopers,
          "developerTitle"
        );
      }
    });

    desktopDevelopersGrid.addEventListener("click", function (e) {
      const btn = e.target.closest(".desktop-multiselect-btn");
      if (!btn) return;
      const id = btn.dataset.id;
      btn.classList.toggle("selected");
      if (btn.classList.contains("selected")) {
        if (!desktopSelectedDevelopers.includes(id)) desktopSelectedDevelopers.push(id);
      } else {
        desktopSelectedDevelopers = desktopSelectedDevelopers.filter((d) => d !== id);
      }
      updateDesktopDevelopersDisplay();
    });

    desktopDevelopersConfirm.addEventListener("click", function () {
      const selectedBtns = desktopDevelopersGrid.querySelectorAll(".desktop-multiselect-btn.selected");
      desktopSelectedDevelopers = Array.from(selectedBtns).map((b) => b.dataset.id);
      updateDesktopDevelopersDisplay();
      const modal = bootstrap.Modal.getInstance(desktopDevelopersModal);
      if (modal) modal.hide();
    });
  }

  // Amenities modal
  const desktopAmenitiesModal = document.getElementById("desktopAmenitiesModal");
  const desktopAmenitiesGrid = document.getElementById("desktopAmenitiesGrid");
  const desktopAmenitiesConfirm = document.getElementById("desktopAmenitiesConfirm");

  if (desktopAmenitiesModal && desktopAmenitiesGrid && desktopAmenitiesConfirm) {
    desktopAmenitiesModal.addEventListener("show.bs.modal", function () {
      if (desktopDropdownItems.amenities.length === 0) {
        fetch("/TblProperties/GetAllAmenities")
          .then((r) => r.json())
          .then((data) => {
            desktopDropdownItems.amenities = data || [];
            renderDesktopMultiSelect(desktopAmenitiesGrid, desktopDropdownItems.amenities, desktopSelectedAmenities, "name");
          })
          .catch((err) => console.error("Error loading amenities:", err));
      } else {
        renderDesktopMultiSelect(desktopAmenitiesGrid, desktopDropdownItems.amenities, desktopSelectedAmenities, "name");
      }
    });

    desktopAmenitiesGrid.addEventListener("click", function (e) {
      const btn = e.target.closest(".desktop-multiselect-btn");
      if (!btn) return;
      const id = btn.dataset.id;
      btn.classList.toggle("selected");
      if (btn.classList.contains("selected")) {
        if (!desktopSelectedAmenities.includes(id)) desktopSelectedAmenities.push(id);
      } else {
        desktopSelectedAmenities = desktopSelectedAmenities.filter((a) => a !== id);
      }
      updateDesktopAmenitiesDisplay();
    });

    desktopAmenitiesConfirm.addEventListener("click", function () {
      const selectedBtns = desktopAmenitiesGrid.querySelectorAll(".desktop-multiselect-btn.selected");
      desktopSelectedAmenities = Array.from(selectedBtns).map((b) => b.dataset.id);
      updateDesktopAmenitiesDisplay();
      const modal = bootstrap.Modal.getInstance(desktopAmenitiesModal);
      if (modal) modal.hide();
    });
  }
  
  // Desktop Property Types Modal - Use event delegation for dynamic content
  const desktopPropertyTypesGrid = document.getElementById("desktopPropertyTypesGrid");
  if (desktopPropertyTypesGrid) {
    desktopPropertyTypesGrid.addEventListener("click", function (e) {
      const btn = e.target.closest(".desktop-property-type-btn");
      if (btn) {
        e.preventDefault();
        e.stopPropagation();
        btn.classList.toggle("selected");
      }
    });
  }
  
  // Also attach listeners directly when modal opens (backup)
  const desktopPropertyTypesModal = document.getElementById("desktopPropertyTypesModal");
  if (desktopPropertyTypesModal) {
    desktopPropertyTypesModal.addEventListener("shown.bs.modal", function () {
      const desktopPropertyTypeBtns = document.querySelectorAll(".desktop-property-type-btn");
      desktopPropertyTypeBtns.forEach((btn) => {
        // Remove any existing listeners to avoid duplicates
        const newBtn = btn.cloneNode(true);
        btn.parentNode.replaceChild(newBtn, btn);
        
        newBtn.addEventListener("click", function (e) {
          e.preventDefault();
          e.stopPropagation();
          this.classList.toggle("selected");
        });
      });
    });
  }

  // Apply Property Types
  const desktopPropertyTypesConfirm = document.getElementById("desktopPropertyTypesConfirm");
  if (desktopPropertyTypesConfirm) {
    desktopPropertyTypesConfirm.addEventListener("click", function () {
      desktopSelectedPropertyTypes = Array.from(
        document.querySelectorAll(".desktop-property-type-btn.selected")
      ).map((btn) => btn.dataset.type);
      
      updateDesktopPropertyTypesDisplay();
      
      const modal = bootstrap.Modal.getInstance(document.getElementById("desktopPropertyTypesModal"));
      if (modal) modal.hide();
    });
  }

  // Desktop Beds and Baths Modal
  const desktopBedroomBtns = document.querySelectorAll(".desktop-bedroom-btn");
  const desktopBathroomBtns = document.querySelectorAll(".desktop-bathroom-btn");
  
  desktopBedroomBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
      desktopBedroomBtns.forEach((b) => b.classList.remove("selected"));
      this.classList.add("selected");
    });
  });

  desktopBathroomBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
      desktopBathroomBtns.forEach((b) => b.classList.remove("selected"));
      this.classList.add("selected");
    });
  });

  // Apply Beds and Baths
  const desktopBedsBathsConfirm = document.getElementById("desktopBedsBathsConfirm");
  if (desktopBedsBathsConfirm) {
    desktopBedsBathsConfirm.addEventListener("click", function () {
      const selectedBedroom = document.querySelector(".desktop-bedroom-btn.selected");
      const selectedBathroom = document.querySelector(".desktop-bathroom-btn.selected");
      
      desktopSelectedBedrooms = selectedBedroom ? selectedBedroom.dataset.value : "";
      desktopSelectedBathrooms = selectedBathroom ? selectedBathroom.dataset.value : "";

      updateDesktopBedsBathsDisplay();

      const modal = bootstrap.Modal.getInstance(document.getElementById("desktopBedsBathsModal"));
      if (modal) modal.hide();
    });
  }

  // Desktop Price Range Modal - Initialize sliders and inputs
  const desktopPriceMinSlider = document.getElementById("desktopPriceMin");
  const desktopPriceMaxSlider = document.getElementById("desktopPriceMax");
  const desktopPriceMinInput = document.getElementById("desktopPriceMinInput");
  const desktopPriceMaxInput = document.getElementById("desktopPriceMaxInput");

  if (desktopPriceMinSlider && desktopPriceMaxSlider && desktopPriceMinInput && desktopPriceMaxInput) {
    // Initialize values (max is 50 million)
    const minSliderVal = parseInt(desktopPriceMinSlider.value) || 0;
    const maxSliderVal = parseInt(desktopPriceMaxSlider.value) || 10000000;
    desktopPriceMinInput.value = minSliderVal;
    desktopPriceMaxInput.value = maxSliderVal;
    desktopPriceMinSlider.value = minSliderVal;
    desktopPriceMaxSlider.value = maxSliderVal;

    // Update input when slider changes
    desktopPriceMinSlider.addEventListener("input", function () {
      const value = parseInt(this.value);
      desktopPriceMinInput.value = value;
      if (value > parseInt(desktopPriceMaxSlider.value)) {
        desktopPriceMaxSlider.value = value;
        desktopPriceMaxInput.value = value;
      }
      // Persist desktop min price for performSearch
      desktopMinPrice = value.toString();
    });

    desktopPriceMaxSlider.addEventListener("input", function () {
      const value = parseInt(this.value);
      desktopPriceMaxInput.value = value;
      if (value < parseInt(desktopPriceMinSlider.value)) {
        desktopPriceMinSlider.value = value;
        desktopPriceMinInput.value = value;
      }
      // Persist desktop max price for performSearch
      desktopMaxPrice = value.toString();
    });

    // Update slider when input changes
    desktopPriceMinInput.addEventListener("input", function () {
      let value = this.value.toString().replace(/[^\d]/g, "");
      value = parseInt(value) || 0;
      const min = parseInt(desktopPriceMinSlider.min) || 0;
        const max = parseInt(desktopPriceMinSlider.max) || 10000000;
      value = Math.max(min, Math.min(max, value));
      const maxValue = parseInt(desktopPriceMaxInput.value) || max;
      if (value > maxValue) value = maxValue;
      desktopPriceMinSlider.value = value;
      this.value = value;
      // Persist desktop min price for performSearch
      desktopMinPrice = value.toString();
    });

    desktopPriceMaxInput.addEventListener("input", function () {
      let value = this.value.toString().replace(/[^\d]/g, "");
      value = parseInt(value) || 0;
      const min = parseInt(desktopPriceMaxSlider.min) || 0;
      const max = parseInt(desktopPriceMaxSlider.max) || 10000000;
      value = Math.max(min, Math.min(max, value));
      const minValue = parseInt(desktopPriceMinInput.value) || min;
      if (value < minValue) value = minValue;
      desktopPriceMaxSlider.value = value;
      this.value = value;
      // Persist desktop max price for performSearch
      desktopMaxPrice = value.toString();
    });
  }

  // Simple desktop display updater functions (also update mobile labels)
  function updateDesktopAreaDisplay() {
    // Areas currently reflected only in query params; no label text change required
  }

  function updateDesktopZonesDisplay() {
    const desktopZonesTextEl = document.getElementById("desktopZonesText");
    const count = desktopSelectedZones ? desktopSelectedZones.length : 0;
    const label = count > 0 ? `Zones (${count})` : "Zones";
    if (desktopZonesTextEl) desktopZonesTextEl.textContent = label;
    if (mobileZonesTextEl) mobileZonesTextEl.textContent = label;
  }

  function updateDesktopDevelopersDisplay() {
    const desktopDevelopersTextEl = document.getElementById("desktopDevelopersText");
    const count = desktopSelectedDevelopers ? desktopSelectedDevelopers.length : 0;
    const label = count > 0 ? `Developers (${count})` : "Developers";
    if (desktopDevelopersTextEl) desktopDevelopersTextEl.textContent = label;
    if (mobileDevelopersTextEl) mobileDevelopersTextEl.textContent = label;
  }

  function updateDesktopAmenitiesDisplay() {
    const desktopAmenitiesTextEl = document.getElementById("desktopAmenitiesText");
    const count = desktopSelectedAmenities ? desktopSelectedAmenities.length : 0;
    const label = count > 0 ? `Amenities (${count})` : "Amenities";
    if (desktopAmenitiesTextEl) desktopAmenitiesTextEl.textContent = label;
    if (mobileAmenitiesTextEl) mobileAmenitiesTextEl.textContent = label;
  }

  function updateDesktopPropertyTypesDisplay() {
    const desktopPropertyTypesTextEl = document.getElementById("desktopPropertyTypesText");
    const count = desktopSelectedPropertyTypes ? desktopSelectedPropertyTypes.length : 0;
    const label = count > 0 ? `Property Types (${count})` : "Property Types";
    if (desktopPropertyTypesTextEl) desktopPropertyTypesTextEl.textContent = label;
    if (mobilePropertyTypesTextEl) mobilePropertyTypesTextEl.textContent = label;
  }

  function updateDesktopBedsBathsDisplay() {
    const desktopBedsBathsTextEl = document.getElementById("desktopBedsBathsText");
    let label = "Beds and Baths";
    if (desktopSelectedBedrooms || desktopSelectedBathrooms) {
      const beds = desktopSelectedBedrooms || "Any";
      const baths = desktopSelectedBathrooms || "Any";
      label = `${beds} Beds, ${baths} Baths`;
    }
    if (desktopBedsBathsTextEl) desktopBedsBathsTextEl.textContent = label;
    if (mobileBedsBathsTextEl) mobileBedsBathsTextEl.textContent = label;
  }

  function updateDesktopPriceRangeDisplay() {
    const desktopPriceRangeTextEl = document.getElementById("desktopPriceRangeText");
    let label = "Price Range";
    if (desktopMinPrice || desktopMaxPrice) {
      const min = desktopMinPrice || "0";
      const max = desktopMaxPrice || "10M";
      label = `${min} - ${max}`;
    }
    if (desktopPriceRangeTextEl) desktopPriceRangeTextEl.textContent = label;
    if (mobilePriceRangeTextEl) mobilePriceRangeTextEl.textContent = label;
  }

  const desktopBedsBathsModal = document.getElementById("desktopBedsBathsModal");
  if (desktopBedsBathsModal) {
    desktopBedsBathsModal.addEventListener("show.bs.modal", function () {
      desktopBedroomBtns.forEach((btn) => {
        if (btn.dataset.value === desktopSelectedBedrooms) {
          btn.classList.add("selected");
        } else {
          btn.classList.remove("selected");
        }
      });
      desktopBathroomBtns.forEach((btn) => {
        if (btn.dataset.value === desktopSelectedBathrooms) {
          btn.classList.add("selected");
        } else {
          btn.classList.remove("selected");
        }
      });
    });
  }

  const desktopPriceRangeModal = document.getElementById("desktopPriceRangeModal");
  if (desktopPriceRangeModal) {
    desktopPriceRangeModal.addEventListener("show.bs.modal", function () {
      if (desktopPriceMinInput && desktopPriceMinSlider) {
        const minVal = desktopMinPrice ? parseFloat(desktopMinPrice) : 0;
        desktopPriceMinInput.value = minVal;
        desktopPriceMinSlider.value = minVal;
      }
      if (desktopPriceMaxInput && desktopPriceMaxSlider) {
          const maxVal = desktopMaxPrice ? parseFloat(desktopMaxPrice) : 10000000;
        desktopPriceMaxInput.value = maxVal;
        desktopPriceMaxSlider.value = maxVal;
      }
    });
  }

  // Modal close functionality
  const filterModal = document.getElementById("filterModal");
  if (filterModal) {
    filterModal.addEventListener("hidden.bs.modal", function () {
      // Keep filter state - don't reset on close
    });
  }
});

// Function to reset all filters (can be called from anywhere)
function resetAllFilters() {
  document
    .querySelectorAll(".property-type-btn")
    .forEach((btn) => btn.classList.remove("selected"));

  document
    .querySelectorAll(".amenity-btn")
    .forEach((btn) => btn.classList.remove("selected"));

  document
    .querySelectorAll(".option-btn")
    .forEach((btn) => btn.classList.remove("selected"));

  const priceMinSlider = document.getElementById("priceMin");
  const priceMaxSlider = document.getElementById("priceMax");
  const areaMinSlider = document.getElementById("areaMin");
  const areaMaxSlider = document.getElementById("areaMax");

  if (priceMinSlider) priceMinSlider.value = priceMinSlider.min || "0";
    if (priceMaxSlider) priceMaxSlider.value = priceMaxSlider.max || "10000000";
  if (areaMinSlider) areaMinSlider.value = 50;
  if (areaMaxSlider) areaMaxSlider.value = 400;

  const priceMinInput = document.getElementById("priceMinInput");
  const priceMaxInput = document.getElementById("priceMaxInput");
  const areaMinInput = document.getElementById("areaMinInput");
  const areaMaxInput = document.getElementById("areaMaxInput");

  if (priceMinInput) priceMinInput.value = priceMinSlider?.min || "0";
    if (priceMaxInput) priceMaxInput.value = priceMaxSlider?.max || "10000000";
  if (areaMinInput) areaMinInput.value = "50";
  if (areaMaxInput) areaMaxInput.value = "400";

  document
    .querySelectorAll('input[type="text"]')
    .forEach((input) => (input.value = ""));
}
