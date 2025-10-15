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

try {
    Quill.register('modules/blotFormatter', QuillBlotFormatter.default);
} catch { }    

export function createQuillInterop(dotNetRef, editorRef, toolbarRef, placeholder, options) {
    var quill = new Quill(editorRef, {
        modules: {
            toolbar: {
                container: toolbarRef
            },
            blotFormatter: {}
        },
        placeholder: placeholder,
        theme: 'snow'
    });
    return new MudQuillInterop(dotNetRef, quill, editorRef, toolbarRef, options);
}

export class MudQuillInterop {
    
    constructor(dotNetRef, quill, editorRef, toolbarRef, options) {
        quill.getModule('toolbar').addHandler('hr', this.insertDividerHandler);
        quill.on('text-change', this.textChangedHandler);
        this.dotNetRef = dotNetRef;
        this.quill = quill;
        this.editorRef = editorRef;
        this.toolbarRef = toolbarRef;
        this.options = options;
    }

    getText = () => {
        return this.quill.getText();
    };

    getHtml = () => {
        const html = this.quill.root.innerHTML;
        return this.options.sanitizeHtml ? this.getSanitizedHtml(html) : html;
    };
    
    getSanitizedHtml = (html) => {

        // Parse the HTML into a DOM
        const parser = new DOMParser();
        const doc = parser.parseFromString(html, 'text/html');

        // Find all <ol> elements
        doc.querySelectorAll('ol').forEach(ol => {

            const hasBullets = Array.from(ol.querySelectorAll('li')).some(
                li => li.getAttribute('data-list') === 'bullet'
            );

            if (hasBullets) {
                // Create a new <ul> element
                const ul = doc.createElement('ul');

                // Move children from <ol> to <ul>
                while (ol.firstChild) {
                    ul.appendChild(ol.firstChild);
                }

                // Replace <ol> with <ul>
                ol.replaceWith(ul);
            }
        });

        return doc.body.innerHTML;
    };

    getQuillHtml = (html) => {

        // Parse the HTML into a DOM
        const parser = new DOMParser();
        const doc = parser.parseFromString(html, 'text/html');

        // Find all <ul> elements
        doc.querySelectorAll('ul').forEach(ul => {

            const hasBullets = Array.from(ul.querySelectorAll('li')).some(
                li => li.getAttribute('data-list') === 'bullet'
            );
            
            if (hasBullets) {
                // Create a new <ol> element
                const ol = doc.createElement('ol');

                // Move children from <ul> to <ol>
                while (ul.firstChild) {
                    ol.appendChild(ul.firstChild);
                }

                // Replace <ul> with <ol>
                ul.replaceWith(ol);
            }
        });

        return doc.body.innerHTML;
    };

    insertDividerHandler = () => {
        const range = this.quill.getSelection();

        if (range) {
            this.quill.insertEmbed(range.index, "hr", "null");
        }
    };

    setHtml = (html) => {
        this.quill.root.innerHTML = this.options.sanitizeHtml ? this.getQuillHtml(html) : html;
    }

    textChangedHandler = (delta, oldDelta, source) => {
        this.dotNetRef.invokeMethodAsync('HandleHtmlContentChanged', this.getHtml());
        this.dotNetRef.invokeMethodAsync('HandleTextContentChanged', this.getText());
    };
}
