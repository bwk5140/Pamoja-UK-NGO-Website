// wwwroot/js/outsideClick.js
window.registerOutsideClick = (elementId, dotnetHelper) => {
    document.addEventListener('mousedown', (e) => {
        const el = document.getElementById(elementId);
        if (el && !el.contains(e.target)) {
            dotnetHelper.invokeMethodAsync('CloseDropdown');
        }
    });
};