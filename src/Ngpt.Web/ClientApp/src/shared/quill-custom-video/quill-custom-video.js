//import Quill from "quill";

//console.log(Quill);

function registerQuillCustomVideo(Quill) {
    const BlockEmbed = Quill.import("blots/block/embed");

    class VideoBlot extends BlockEmbed {
        static create(url) {
            let node = super.create(url);
            let iframe = document.createElement('iframe');
            // Set styles for wrapper
            node.setAttribute('class', 'responsive-video');
            // Set styles for iframe
            iframe.setAttribute('frameborder', '0');
            iframe.setAttribute('allowfullscreen', 'true');
            iframe.setAttribute('src', url);
            // Append iframe as child to wrapper
            node.appendChild(iframe);
            return node;
        }

        static value(domNode) {
            return domNode.firstChild.getAttribute('src');
        }
    }
    VideoBlot.blotName = 'video';
    VideoBlot.tagName = 'div';

    Quill.register(VideoBlot, true);
}
