export class MudHtmlEditor {

    /**
     * Creates a new instance of the MudHtmlEditor
     * @param {any} dotNetRef
     * @param {Element} editorRef
     * @param {Element} toobarRef
     * @param {string} placeholderText
     * @param {string} theme
     * @returns
     */
    static createInstance(dotNetRef, editorRef, toolbarRef, placeholderText, theme) {
        return new MudHtmlEditor(dotNetRef, new Quill(editorRef, {
            modules: {
                toolbar: {
                    container: toolbarRef
                }
            },
            placeholder: placeholderText,
            theme: theme || 'snow'
        }));
    }

    /**
     * Creates a new instance of MudHtmlEditor
     * @param {any} dotNetRef
     * @param {Quill} quill
     */
    constructor(dotNetRef, quill) {
        //quill.addEventListener("paste", this.pasteHandler, false);
        quill.on("text-change", this.textChangedHandler);

        const toolbar = quill.getModule('toolbar');
        toolbar.addHandler("image", this.insertImageHandler);

        this.dotNetRef = dotNetRef;
        this.quill = quill;
        this.toolbar = toolbar;
    }

    /**
     * Gets the HTML content of the editor.
     * @returns {string}
     */
    getHtml = () => {
        return this.quill.getSemanticHTML();
    }

    /**
     * Sets the HTML content of the editor.
     * @param {string} html
     */
    setHtml = (html) => {
        console.log('setting html!');
        this.quill.root.innerHTML = html;
    }

    insertImageHandler = () => {
        let fileInput = this.toolbar.container.querySelector('input.ql-image[type=file]');
        if (fileInput === null) {
            fileInput = document.createElement('input');
            Object.assign(fileInput, {
                type: 'file',
                accept: 'image/png, image/gif, image/jpeg, image/bmp, image/x-icon'
            });
            fileInput.classList.add('ql-image');
            fileInput.addEventListener('change', () => {
                if (fileInput.files != null && fileInput.files[0] != null) {
                    this.uploadFile(fileInput.files[0]);
                }
            });
            this.toolbar.container.appendChild(fileInput);
        }
        fileInput.click();
    }

    /**
     * 
     * @param {Delta} delta
     * @param {Delta} oldDelta
     * @param {any} source
     */
    textChangedHandler = (delta, oldDelta, source) => {
        this.dotNetRef.invokeMethodAsync('NotifyHtmlChanged', this.getHtml());
    }

    pasteHandler = (e) => {
        let clipboard = e.clipboardData || window.clipboardData;

        if (clipboard && clipboard.items) {
            let items = clipboard.items;
            const IMAGE_MIME_REGEX = /^image\/(png|jpg|jpeg|gif|bmp)$/i;

            for (let i = 0; i < items.length; i++) {
                if (IMAGE_MIME_REGEX.test(items[i].type)) {
                    let file = items[i].getAsFile();

                    if (file) {
                        this.quill.focus();
                        this.range = this.quill.getSelection();
                        e.preventDefault();
                        setTimeout(() => {
                            this.quill.focus();
                            this.quill.getSelection();
                            this.uploadFile(file);
                        }, 0);
                    }
                }
            }
        }
    }

    uploadFile = (file, range) => {
        const reader = new FileReader();

        reader.onload = async (e) => {
            const url = await this.dotNetRef.invokeMethodAsync('HandleFileUpload', file.name, DotNet.createJSStreamReference(e.target.result));

            if (url != '') {
                const index = range ? range.index : this.quill.getSelection().index;
                this.quill.insertEmbed(index, 'image', url);
            }
        };

        reader.readAsArrayBuffer(file);
    }
}