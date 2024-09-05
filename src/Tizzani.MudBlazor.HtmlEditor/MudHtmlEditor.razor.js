var Embed = Quill.import('blots/block/embed');
const Delta = Quill.import('delta');

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


export function createQuillInterop(dotNetRef, editorRef, toolbarRef, settings) {
    var interop = new MudQuillInterop(dotNetRef, editorRef, toolbarRef, settings);

    var properties = {
        modules: {
            toolbar: {
                container: toolbarRef
            },
            blotFormatter: {},
            uploader: {
                mimetypes: settings.allowedImageMimeTypes
            }
        },
        placeholder: settings.placeholder,
        theme: 'snow'
    };

    // Use custom handler if specified
    if (settings.blazorImageUpload || settings.imageUploadUrl) {
        properties.modules.uploader.handler = interop.uploadImageHandler;
    }

    var quill = new Quill(editorRef, properties);

    interop.addQuill(quill);

    return interop;
}

export class MudQuillInterop {
    /**
     * @param {Quill} quill
     * @param {Element} editorRef
     * @param {Element} toolbarRef
     * @param {object} toolbarRef
     */
    constructor(dotNetRef, editorRef, toolbarRef, settings) {
        this.dotNetRef = dotNetRef;
        this.quill;
        this.editorRef = editorRef;
        this.toolbarRef = toolbarRef;
        this.blazorImageUpload = settings.blazorImageUpload;
        this.imageBytes = {};
        this.imageUploadUrl = settings.imageUploadUrl;
    }

    addQuill = (quill) => {
        quill.getModule('toolbar').addHandler('hr', this.insertDividerHandler);
        quill.on('text-change', this.textChangedHandler);
        this.quill = quill;
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

    // Get imageBytes by filename
    getImageBytes = (key) => {
        return this.imageBytes[key];
    }

    // Read file and upload it via the blazor or controller method
    upload(file) {
        const fileReader = new FileReader();
        return new Promise((resolve, reject) => {
            fileReader.addEventListener("load", () => {
                // Pass file via DotNetInterop
                if (this.blazorImageUpload) {
                    this.imageBytes[file.name] = new Uint8Array(fileReader.result);
                    this.dotNetRef.invokeMethodAsync("SaveImage", file.name, file.type, file.size).then(url => resolve({name: file.name, url: url }));
                }
                // Upload to API endpoint
                else if (this.imageUploadUrl) {
                    const formData = new FormData();
                    formData.append("files", file);

                    fetch(this.imageUploadUrl, {
                        method: "POST",
                        headers: {},
                        body: formData
                    })
                    .then(response => {
                        if (response.status === 200) {
                            response.text().then(url =>
                                resolve({ name: file.name, url: url })
                            );
                        }
                        else {
                            reject("Error uploading image to " + this.imageUploadUrl);
                        }
                    });
                }
            });

            if (file) {
                fileReader.readAsArrayBuffer(file);
            } else {
                reject("No file selected");
            }
        });
    }

    // Handle when images are copy/pasted or dragged/dropped into the editor
    uploadImageHandler = (range, files) => {
        for (let file of files) {
            this.upload(file).then((data) => {
                let delta = new Delta().retain(range.index).delete(range.length).insert({ image: data.url });
                delete this.imageBytes[data.name]
                this.quill.updateContents(delta, Quill.sources.USER);
                this.quill.setSelection(range.index, Quill.sources.SILENT);
            },
            (error) => {
                console.warn(error);
            });
        }
    }
}
