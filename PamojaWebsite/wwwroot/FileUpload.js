window.triggerFileInput = function (elementId) {
    const input = document.getElementById(elementId);
    if (input) {
        input.click();
    } else {
        console.warn("File input element not found:", elementId);
    }
};