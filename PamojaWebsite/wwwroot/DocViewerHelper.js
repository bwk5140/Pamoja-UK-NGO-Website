window.createObjectURL = (byteArray) => {
    // If byteArray is coming in as a base64 string or unexpected format:
    const uint8Array = new Uint8Array(byteArray);
    const blob = new Blob([uint8Array], { type: 'application/pdf' });
    return URL.createObjectURL(blob);
};
// Get selected text from inside an iframe
window.getSelectedTextFromIframe = function (iframeId) {
    var iframe = document.getElementById(iframeId);
    if (!iframe || !iframe.contentWindow) return "";

    var iframeDoc = iframe.contentWindow.document;
    var sel = iframeDoc.getSelection ? iframeDoc.getSelection() : null;
    if (sel && sel.rangeCount > 0) {
        return sel.toString();
    }
    return "";
};

// Wrap selection inside an iframe
window.wrapSelectionInIframe = function (iframeId, actionType, extraText) {
    var iframe = document.getElementById(iframeId);
    if (!iframe || !iframe.contentWindow) return "";

    var iframeDoc = iframe.contentWindow.document;
    var sel = iframeDoc.getSelection ? iframeDoc.getSelection() : null;
    if (!sel || sel.rangeCount === 0) return "";

    var range = sel.getRangeAt(0);
    var selectedText = sel.toString();

    var span = iframeDoc.createElement("span");

    if (actionType === "highlight") {
        span.className = "bg-yellow-200";
    } else if (actionType === "comment") {
        span.className = "bg-blue-100 relative";
        var commentBox = iframeDoc.createElement("div");
        commentBox.className = "absolute bg-white border p-2 mt-1 text-sm";
        commentBox.innerText = extraText || "Comment";
        span.appendChild(commentBox);
    } else if (actionType === "bookmark") {
        var id = "bookmark_" + Date.now();
        span.id = id;
        span.className = "bg-green-100";
    }

    range.surroundContents(span);

    // Return updated HTML of the iframe’s body so you can persist it
    return iframeDoc.body.innerHTML;
};
