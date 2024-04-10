var Embed = Quill.import('blots/block/embed');

class Divider extends Embed {
    static create(value) {
        let node = super.create(value);
        node.setAttribute('style', "height: 0px; margin-top: 0.5em 0; border-width; 1px; border-style: solid none none none;");
        return node;
    }
}

Divider.blotName = 'hr';
Divider.tagName = 'hr';
Quill.register(Divider, true);
Quill.register('modules/blotFormatter', QuillBlotFormatter.default);

function initializeQuill(dotNetRef, editorRef, toolbarRef, placeholder) {
    var quill = new Quill(editorRef, {
        modules: {
            toolbar: {
                container: toolbarRef,
                handlers: {
                    'hr': () => {
                        var range = quill.getSelection();

                        if (range) {
                            quill.insertEmbed(range.index, "hr", "null");
                        }
                    }
                }
            },
            blotFormatter: { }
        },
        placeholder: placeholder,
        theme: 'snow'
    });

    quill.on('text-change', (delta, oldDelta, source) => {
        dotNetRef.invokeMethodAsync('NotifyHtmlChanged', getQuillHtml(editorRef));
    });
}

function getQuillHtml(editorRef) {
    return editorRef.__quill.root.innerHTML;
}

function setQuillHtml(editorRef, html) {
    return editorRef.__quill.root.innerHTML = html;
}
