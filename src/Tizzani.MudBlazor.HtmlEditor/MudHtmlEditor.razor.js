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

if (typeof QuillBlotFormatter !== 'undefined') {
    Quill.register('modules/blotFormatter', QuillBlotFormatter.default);
}

export function createQuillInterop(dotNetRef, editorRef, toolbarRef, placeholder) {
    if (!editorRef || !editorRef.isConnected || !toolbarRef || !toolbarRef.isConnected) {
        throw new Error('DOM elements disconnected.');
    }

    const modulesConfig = {
        toolbar: {
                container: toolbarRef
            }
    };
    
    if (typeof QuillBlotFormatter !== 'undefined') {
    modulesConfig.blotFormatter = {};
    }

    var quill = new Quill(editorRef, {
        modules: modulesConfig,
        placeholder: placeholder,
        theme: 'snow'
    });
    return new MudQuillInterop(dotNetRef, quill, editorRef, toolbarRef);
}

export class MudQuillInterop {
    /**
     * @param {Quill} quill
     * @param {Element} editorRef
     * @param {Element} toolbarRef
     */
    constructor(dotNetRef, quill, editorRef, toolbarRef) {
        quill.getModule('toolbar').addHandler('hr', this.insertDividerHandler);
        quill.on('text-change', this.textChangedHandler);
        this.dotNetRef = dotNetRef;
        this.quill = quill;
        this.editorRef = editorRef;
        this.toolbarRef = toolbarRef;
    }

    getText = () => {
        return this.quill.getText();
    };

    getHtml = () => {
        return this.quill.root.innerHTML;
    };

    setHtml = (html) => {
        this.quill.root.innerHTML = html;
    }

    insertDividerHandler = () => {
        const range = this.quill.getSelection();

        if (range) {
            this.quill.insertEmbed(range.index, "hr", "null");
        }
    };

    /**
     * 
     * @param {Delta} delta
     * @param {Delta} oldDelta
     * @param {any} source
     */
    textChangedHandler = (delta, oldDelta, source) => {
        this.dotNetRef.invokeMethodAsync('HandleHtmlContentChanged', this.getHtml());
        this.dotNetRef.invokeMethodAsync('HandleTextContentChanged', this.getText());
    };
}
