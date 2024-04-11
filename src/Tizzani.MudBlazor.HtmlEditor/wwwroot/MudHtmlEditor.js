var Embed = Quill.import('blots/block/embed');
var Delta = Quill.import('delta');

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
                        'image': () => {
                            let fileInput = toolbarRef.querySelector('input.ql-image[type=file]');
                            if (fileInput == null) {
                                fileInput = document.createElement('input');
                                fileInput.setAttribute('type', 'file');
                                fileInput.setAttribute('accept', 'image/png, image/gif, image/jpeg, image/bmp, image/x-icon');
                                fileInput.classList.add('ql-image');
                                fileInput.addEventListener('change', () => {

                                    if (fileInput.files != null && fileInput.files[0] != null) {

                                        let reader = new FileReader();

                                        reader.onload = (e) => {
                                            dotNetRef.invokeMethodAsync('HandleFileUpload', DotNet.createJSStreamReference(e.target.result));
                                        };

                                        reader.readAsArrayBuffer(fileInput.files[0]);
                                    }
                                });
                                toolbarRef.appendChild(fileInput);
                            }
                            fileInput.click();
                        },
                        'hr': () => {
                            var range = quill.getSelection();

                            if (range) {
                                quill.insertEmbed(range.index, "hr", "null");
                            }
                        }
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

    getHtml() {
        return this.quill.root.innerHTML;
    }

    setHtml(html) {
        this.quill.root.innerHTML = html;
    }
}