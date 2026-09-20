  <!-- Loader Script -->
 
      function showLoader() {
          var overlay = document.getElementById("loadingOverlay");
          if (overlay) overlay.style.display = "block";
      }

      function hideLoader() {
          var overlay = document.getElementById("loadingOverlay");
          if (overlay) overlay.style.display = "none";
      }
