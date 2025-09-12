// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

import * as preview from './js/threejs/threejspreview.js';

export function initPreview() {
  preview.renderPreviewGrid();
}

export function showLights(list) {
  preview.scheduleLightUpdate(list);
}
