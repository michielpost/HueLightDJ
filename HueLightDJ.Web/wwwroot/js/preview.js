const previewConnection = new signalR.HubConnectionBuilder()
  .withUrl("/previewhub")
  .build();

previewConnection.start()
  .catch(err => console.error(err.toString()));

function renderPreviewGrid(size) {

  var WIDTH = size;
  var HEIGHT = WIDTH;
  var lights = [];
  var cellSize = WIDTH / 20;

  var app = new PIXI.Application(WIDTH, HEIGHT, { backgroundColor: 0x191D21 });
  document.getElementById("pixiPreview").appendChild(app.view);

  //create the stage instead of container
  app.stage = new PIXI.display.Stage();

  var lightContainer = new PIXI.Container();
  app.stage.addChild(lightContainer);

  //placeLight(1, 0, 0, "0F0000", 0.5);
  //placeLight(2, 0.1, 0.1, "0FF00", 1);

  previewConnection.on("preview", (preview) => {
    for (var i = 0; i < preview.length; i++) {
      var light = preview[i];
      placeLight(light.id, light.x, light.y, light.hex, light.bri)
    }
  });


  function placeLight(id, x, y, hex, bri) {

    var current = lights[id];
    var rad = 30 * bri;
    var xPos = xyToPosition(x);
    var yPos = xyToPosition(y * -1)

    if (current === undefined || current == null) {

      lights[id] = {
        glow: createGlowRing(xPos, yPos),
        label: createLightLabel(xPos, yPos, id),
      };
      updateGlowRing(id, hex, bri);
      addLightToContainer(lights[id]);
    }
    else {
      updateGlowRing(id, hex, bri);
    }
  }


  function addLightToContainer(light) {
    lightContainer.addChild(light.glow);
    lightContainer.addChild(light.label);
  }

  function updateGlowRing(id, hex, bri) {
    lights[id].glow.tint = "0x" + hex;
    lights[id].glow.height = cellSize * 5 * bri;
    lights[id].glow.width = cellSize * 5 * bri;
  }

  function createGlowRing(x, y) {
    var glowRing = PIXI.Sprite.fromImage("images/glow.png");
    glowRing.anchor.set(0.5, 0.5);
    glowRing.height = cellSize * 5;
    glowRing.width = cellSize * 5;
    glowRing.position.x = x;
    glowRing.position.y = y;
    glowRing.blendMode = PIXI.BLEND_MODES.SCREEN;
    return glowRing;
  }

  function createLightLabel(x, y, id) {
    var label = new PIXI.Text(String(id), { font: "12px", fill: 0xE9ECEF });
    label.anchor.set(0.5, 0.5);
    label.position.x = x;
    label.position.y = y;
    return label;
  }

  function xyToPosition(x) {
    return (WIDTH / 2 + x * WIDTH / 2)
  }

}
