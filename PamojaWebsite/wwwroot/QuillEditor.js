window.initializeQuill = function (editorId) {
    var quill = new Quill('#' + editorId, {
        theme: 'snow',
        modules: {
            toolbar: [
                ['bold', 'italic', 'underline'],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                ['link', 'blockquote']
            ]
        }
    });
    window[editorId + "_quill"] = quill;
};

window.getQuillContent = function (editorId) {
    var quill = window[editorId + "_quill"];
    return quill.root.innerHTML; // returns HTML
};