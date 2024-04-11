const Embed = Quill.import('blots/block/embed');

export function createQuill(dotNetRef, editorRef, toolbarRef, placeholder) {
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

    return new MudQuillEditor(dotNetRef, editorRef, toolbarRef, quill);
}

export class MudQuillEditor {
    constructor(dotNetRef, editorRef, toolbarRef, quill) {

        const toolbar = quill.getModule('toolbar');
        toolbar.addHandler('hr', this.onClickInsertDivider);
        toolbar.addHandler('image', this.onClickInsertImage);

        quill.on('text-change', (delta, oldDelta, source) => {
            dotNetRef.invokeMethodAsync('NotifyHtmlChanged', this.getQuillHtml());
        });

        this.dotNetRef = dotNetRef;
        this.editorRef = editorRef;
        this.toolbarRef = toolbarRef;
        this.quill = quill;
        this.quill.root.addEventListener("paste", this.onPaste, false);
    }
    
    setQuillHtml = (html) => this.quill.root.innerHTML = html;
    getQuillHtml = () => this.quill.root.innerHTML;

    onClickInsertDivider = () => this.quill.insertEmbed(this.quill.getSelection().index, 'hr', 'null');
    onClickInsertImage = () => {
        let fileInput = this.toolbarRef.querySelector('input.ql-image[type=file]');
        if (fileInput == null) {
            fileInput = document.createElement('input');
            fileInput.setAttribute('type', 'file');
            fileInput.setAttribute('accept', 'image/png, image/gif, image/jpeg, image/bmp, image/x-icon');
            fileInput.classList.add('ql-image');
            fileInput.addEventListener('change', () => {

                if (fileInput.files != null && fileInput.files[0] != null) {
                    this.uploadFile(fileInput.files[0]);
                }
            });
            this.toolbarRef.appendChild(fileInput);
        }
        fileInput.click();
    };

    onPaste = (e) => {
        let clipboard = e.clipboardData || window.clipboardData;

        if (clipboard && clipboard.items) {
            let items = clipboard.items;
            const IMAGE_MIME_REGEX = /^image\/(jpe?g|gif|png|svg|webp)$/i;

            for (let i = 0; i < items.length; i++) {
                if (IMAGE_MIME_REGEX.test(items[i].type)) {
                    let file = items[i].getAsFile ? items[i].getAsFile() : items[i];

                    if (file) {
                        this.quill.focus();
                        this.range = this.quill.getSelection();
                        e.preventDefault();
                        setTimeout(() => {
                            this.quill.focus();
                            this.range = this.quill.getSelection();
                            this.uploadFile(file);
                        }, 0);
                    }
                }
            }
        }
    }

    uploadFile(file, range) {
        const reader = new FileReader();

        reader.onload = async (e) => {
            const url = await this.dotNetRef.invokeMethodAsync('HandleFileUpload', file.name, file.type, DotNet.createJSStreamReference(e.target.result));

            if (url != '') {
                const index = range ? range.index : this.quill.getSelection().index;
                this.quill.insertEmbed(index, 'image', url);
            }
        };

        reader.readAsArrayBuffer(file);
    }
}

export class Divider extends Embed {
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