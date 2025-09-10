// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function initPreview() {
  //var size = Math.min(document.documentElement.clientHeight, document.documentElement.clientWidth) - 6;
  renderPreviewGrid(false);

  //placeLight("a", 1, 0.3, 0.3, "0FF00", 0.5);
  //placeLight("b", 2, 0.8, 0.8, "0FF00", 1);
}

export function showLights(list) {
  for (var i = 0; i < list.length; i++) {
    var light = list[i];
    placeLight(light.bridge, light.id, light.x, light.y, light.hex, light.bri)
  }
}
