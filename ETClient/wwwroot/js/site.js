
function scrollToEnd(element) {
    element.scrollTop = element.scrollHeight;
    element.animate({ scrollTop: element.scrollHeight });
}

function PlaySound(elementName) {
    document.getElementById(elementName).play();
}