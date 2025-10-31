(function () {
  "use strict";

  AOS.init({
    duration: 800,
    easing: "slide",
    once: true,
  });

  var preloader = function () {
    var loader = document.querySelector(".loader");
    var overlay = document.getElementById("overlayer");

    function fadeOut(el) {
      el.style.opacity = 1;
      (function fade() {
        if ((el.style.opacity -= 0.1) < 0) {
          el.style.display = "none";
        } else {
          requestAnimationFrame(fade);
        }
      })();
    }

    setTimeout(function () {
      fadeOut(loader);
      fadeOut(overlay);
    }, 200);
  };
  preloader();

  var tinySdlier = function () {
    var heroSlider = document.querySelectorAll(".hero-slide");
    var propertySlider = document.querySelectorAll(".property-slider");
    var imgPropertySlider = document.querySelectorAll(".img-property-slide");
    var testimonialSlider = document.querySelectorAll(".testimonial-slider");

    if (heroSlider.length > 0) {
      var tnsHeroSlider = tns({
        container: ".hero-slide",
        mode: "carousel",
        speed: 700,
        autoplay: true,
        controls: false,
        nav: false,
        autoplayButtonOutput: false,
        controlsContainer: "#hero-nav",
      });
    }

    if (imgPropertySlider.length > 0) {
      var tnsPropertyImageSlider = tns({
        container: ".img-property-slide",
        mode: "carousel",
        speed: 700,
        items: 1,
        gutter: 30,
        autoplay: true,
        controls: false,
        nav: true,
        autoplayButtonOutput: false,
      });
    }

    if (propertySlider.length > 0) {
      var tnsSlider = tns({
        container: ".property-slider",
        mode: "carousel",
        speed: 700,
        gutter: 30,
        items: 3,
        autoplay: true,
        autoplayButtonOutput: false,
        controlsContainer: "#property-nav",
        responsive: {
          0: {
            items: 1,
          },
          700: {
            items: 2,
          },
          900: {
            items: 3,
          },
        },
      });
    }

    if (testimonialSlider.length > 0) {
      var tnsSlider = tns({
        container: ".testimonial-slider",
        mode: "carousel",
        speed: 700,
        items: 3,
        gutter: 50,
        autoplay: true,
        autoplayButtonOutput: false,
        controlsContainer: "#testimonial-nav",
        responsive: {
          0: {
            items: 1,
          },
          700: {
            items: 2,
          },
          900: {
            items: 3,
          },
        },
      });
    }
  };
  tinySdlier();

  // Modal scroll disable functionality
  var modalScrollDisable = function () {
    var modal = document.getElementById("exampleModal");
    var body = document.body;
    var scrollPosition = 0;

    // Function to disable scrolling
    function disableScroll() {
      // Store current scroll position
      scrollPosition = window.pageYOffset || document.documentElement.scrollTop;

      // Apply styles to prevent scrolling
      body.style.position = "fixed";
      body.style.top = "-" + scrollPosition + "px";
      body.style.width = "100%";
      body.style.overflow = "hidden";
    }

    // Function to enable scrolling
    function enableScroll() {
      // Remove the styles that prevent scrolling
      body.style.position = "";
      body.style.top = "";
      body.style.width = "";
      body.style.overflow = "";

      // Restore scroll position
      window.scrollTo(0, scrollPosition);
    }

    // Listen for modal show event
    if (modal) {
      modal.addEventListener("show.bs.modal", function () {
        disableScroll();
      });

      // Listen for modal hide event
      modal.addEventListener("hide.bs.modal", function () {
        enableScroll();
      });

      // Also handle when modal is hidden by clicking outside or pressing escape
      modal.addEventListener("hidden.bs.modal", function () {
        enableScroll();
      });
    }
  };

  // Initialize modal scroll disable functionality
  modalScrollDisable();
})();

