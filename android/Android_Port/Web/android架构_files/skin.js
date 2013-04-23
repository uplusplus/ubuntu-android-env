window.__blog = {};
window.__$ = function(id) {
    return document.getElementById(id);
}

window.__blog.sidebar = {};
window.__blog.sidebar.mainElements = ["side_ing_block", "side-recent-posts", "side-recent-comments"];
window.__blog.sidebar.leftElements = ["side-calendar","side-CustomLink", "side-categories", 'side-group-manange,', 'side-group-attend', 'side-team', 'side-top-posts-custom', 'side-top-posts'];
window.__blog.sidebar.rightElements = ["side-blogroll", "side-archives"];

window.__blog.sidebar.__setElements = function(elements, elementIds, container) {
    var len = elementIds.length;
    for (var i = 0; i < len; i++) {
        var id = elementIds[i];
        var e = elements[id];
        if (e) {
            container.appendChild(e);
            delete elements[id];
        }
    }
}

window.__blog.sidebar.__balanceElements = function(elements, left, right) {
    var length = elements.length;
    for (var i = 0; i < length; i++) {
        var container = left.clientHeight < right.clientHeight ? left : right;
        container.appendChild(elements[i]);
    }
}

window.__blog.sidebar.__layout = function() {
    var elements = {};
    var noneIdElements = [];

    var container = document.getElementById("sideContainer");
    var children = container.childNodes;
    for (var i = 0, len = children.length; i < len; i++) {
        var e = children[i];
        var id = e.id;
        if (id) {
            elements[id] = e;
        } else {
            noneIdElements.push(e);
        }
    }

    this.__setElements(
		elements,
		window.__blog.sidebar.mainElements,
		document.getElementById("sideMain"));
    this.__setElements(
		elements,
		window.__blog.sidebar.leftElements,
		document.getElementById("sideLeft"));
    this.__setElements(
		elements,
		window.__blog.sidebar.rightElements,
		document.getElementById("sideRight"));

    for (var m in elements) {
        noneIdElements.push(elements[m]);
    }

    if (noneIdElements.length > 0) {
        this.__balanceElements(
			noneIdElements,
			document.getElementById("sideLeft"),
			document.getElementById("sideRight"));
    }
}