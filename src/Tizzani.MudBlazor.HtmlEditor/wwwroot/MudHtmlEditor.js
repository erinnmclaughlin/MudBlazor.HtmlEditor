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

function createQuillInstance(dotNetRef, editorRef, toolbarRef, placeholder) {
    return new QuillInstance(dotNetRef, editorRef, toolbarRef, placeholder);
}

class QuillInstance {

    constructor(dotNetRef, editorRef, toolbarRef, placeholder) {
        this.quill = new Quill(editorRef, {
            modules: {
                toolbar: {
                    container: toolbarRef,
                    handlers: {
                        'image': () => this.imageHandler(dotNetRef, toolbarRef),
                        'hr': () => this.dividerHandler()
                    }
                },
                blotFormatter: {}
            },
            placeholder: placeholder,
            theme: 'snow'
        });

        this.quill.on('text-change', (delta, oldDelta, source) => {
            dotNetRef.invokeMethodAsync('NotifyHtmlChanged', this.getHtml());
        });
    }

    insertImage(src) {
        this.quill.insertEmbed(this.quill.getSelection().index, 'image', src);
    }

    getHtml() {
        return this.quill.root.innerHTML;
    }

    setHtml(html) {
        this.quill.root.innerHTML = html;
    }

    dividerHandler = () => {
        this.quill.insertEmbed(this.quill.getSelection().index, 'hr', 'null');
    }

    imageHandler = (dotNetRef, toolbarRef) => {
        let fileInput = toolbarRef.querySelector('input.ql-image[type=file]');
        if (fileInput == null) {
            fileInput = document.createElement('input');
            fileInput.setAttribute('type', 'file');
            fileInput.setAttribute('accept', 'image/png, image/gif, image/jpeg, image/bmp, image/x-icon');
            fileInput.classList.add('ql-image');
            fileInput.addEventListener('change', () => {

                if (fileInput.files != null && fileInput.files[0] != null) {

                    let file = fileInput.files[0];

                    let reader = new FileReader();

                    reader.onload = (e) => {
                        dotNetRef.invokeMethodAsync('HandleFileUpload', file.name, file.type, DotNet.createJSStreamReference(e.target.result));
                    };

                    reader.readAsArrayBuffer(file);
                }
            });
            toolbarRef.appendChild(fileInput);
        }
        fileInput.click();
    }
}
