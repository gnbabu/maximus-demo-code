function showModal(options) {
    const {
        title = "",
        titleClass = "",
        message = "",
        buttons = []
    } = options;

    // Elements
    const modalEl = document.getElementById("customModal");
    const titleEl = document.getElementById("customModalTitle");
    const messageEl = document.getElementById("customModalMessage");
    const buttonsEl = document.getElementById("customModalButtons");

    // Reset
    titleEl.className = "modal-title";
    buttonsEl.innerHTML = "";

    // Apply content
    titleEl.textContent = title;
    if (titleClass) titleEl.classList.add(titleClass);

    messageEl.innerHTML = message;

    // Build buttons
    buttons.forEach(btn => {
        const b = document.createElement("button");
        b.textContent = btn.label;
        b.className = `btn ${btn.class || "btn-secondary"}`;

        if (typeof btn.callback === "function") {
            b.addEventListener("click", () => {
                btn.callback();
                modal.hide();
            });
        } else {
            b.setAttribute("data-bs-dismiss", "modal");
        }

        buttonsEl.appendChild(b);
    });

    // Show modal
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();
}


$(function () {
    
    //we append after page load. normally i would use a partial view for this but since we want to keep it as a plugin we will just inject the html into the page
    
    const modalHTML = `

    <!-- General Purpose Modal -->
    <div class="modal" id="customModal" tabindex="-1" style="z-index:6000 !important;">
      <div class="modal-dialog">
        <div class="modal-content">

          <div class="modal-header">
            <h5 class="modal-title" id="customModalTitle"></h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>

          <div class="modal-body" id="customModalMessage"></div>

          <div class="modal-footer" id="customModalButtons"></div>

        </div>
      </div>
    </div>
  `;

        document.body.insertAdjacentHTML("beforeend", modalHTML);
    

});