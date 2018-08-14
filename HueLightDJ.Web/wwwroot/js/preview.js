const previewConnection = new signalR.HubConnectionBuilder()
  .withUrl("/previewhub")
  .build();

previewConnection.start()
  .catch(err => console.error(err.toString()));

function renderPreviewGrid(size, allowEdit) {

  var WIDTH = size;
  var HEIGHT = WIDTH;
  var lights = [];
  var cellSize = WIDTH / 20;

  var app = new PIXI.Application(WIDTH, HEIGHT, { backgroundColor: 0x191D21 });
  document.getElementById("pixiPreview").appendChild(app.view);

  //create the stage instead of container
  app.stage = new PIXI.display.Stage();

  addGridLines(app.stage, allowEdit);

  var lightContainer = new PIXI.Container();
  app.stage.addChild(lightContainer);

  if (allowEdit) {
    document.getElementById("saveButton").onclick = saveLocations;
    previewConnection.on("newLocations", (preview) => {
      for (var i = 0; i < preview.length; i++) {
        var light = preview[i];
        placeLight(light.bridge, light.id, light.x, light.y, light.hex, light.bri, light.groupId)
      }
    });
  }
  else {
    app.renderer.plugins.interaction.on('pointerdown', touch);

    //placeLight(1, 0, 0, "0F0000", 0.5);
    //placeLight(2, 0.1, 0.1, "0FF00", 1);

    previewConnection.on("preview", (preview) => {
      for (var i = 0; i < preview.length; i++) {
        var light = preview[i];
        placeLight(light.bridge, light.id, light.x, light.y, light.hex, light.bri)
      }
    });
  }

  function addGridLines(stage, drawGrid) {

    var graphics = new PIXI.Graphics();
    var stepSize = cellSize * 2;

    // set a fill and line style
    graphics.lineStyle(1, 0xffffff, 1);

    // draw a shape
    graphics.moveTo(1, 0);
    graphics.lineTo(0, WIDTH);

    graphics.moveTo(0, 1);
    graphics.lineTo(WIDTH, 0);

    if (drawGrid) {
      for (var i = stepSize; i <= WIDTH; i = i + stepSize) {
        graphics.moveTo(i, 0);
        graphics.lineTo(i, WIDTH);

        graphics.moveTo(0, i);
        graphics.lineTo(WIDTH, i);
      }
    }

    graphics.moveTo(WIDTH-1, 0);
    graphics.lineTo(WIDTH - 1, WIDTH);

    graphics.moveTo(0, WIDTH - 1);
    graphics.lineTo(WIDTH, WIDTH - 1);

    app.stage.addChild(graphics);
  }

  function saveLocations() {
    var result = [];
    for (const [key, value] of Object.entries(lights)) {
      for (var i = 0; i < value.length; i++) {
        var l = value[i];
        if (l != undefined && l != null) {
          var pos = getXYPosition(l.label);
          result.push({
            Id: i,
            Bridge: key,
            GroupId: l.groupId,
            X: pos.x,
            Y: pos.y
          });
        }
      }
    }
  

    previewConnection.invoke("SetLocations", result).catch(err => console.error(err.toString()));

  }

  function touch(event) {
    var pos = event.data.getLocalPosition(app.stage);
    previewConnection.invoke("touch", positionToXY(pos.x), positionToXY(WIDTH - pos.y)).catch(err => console.error(err.toString()));
  }

  function getXYPosition(obj) {
    var xypos = {};
    xypos.x = positionToXY(obj.x);
    xypos.y = positionToXY(WIDTH - obj.y);

    return xypos;
  }

  function placeLight(bridgeIp, id, x, y, hex, bri, groupId) {
    var bridgeArray = lights[bridgeIp];
    var rad = 30 * bri;
    var xPos = xyToPosition(x);
    var yPos = xyToPosition(y * -1)

    if (bridgeArray === undefined || bridgeArray == null) {
      lights[bridgeIp] = [];
    }

    var current = lights[bridgeIp][id];

    if (current === undefined || current == null) {

      lights[bridgeIp][id] = {
        glow: createGlowRing(xPos, yPos),
        label: createLightLabel(xPos, yPos, id),
        groupId: groupId
      };
      updateGlowRing(bridgeIp, id, hex, bri);
      addLightToContainer(lights[bridgeIp][id]);
    }
    else {
      updateGlowRing(bridgeIp, id, hex, bri);
    }
  }


  function addLightToContainer(light) {
    lightContainer.addChild(light.glow);
    lightContainer.addChild(light.label);

    if (allowEdit) {
      light.label.interactive = true;
      light.label.buttonMode = true;
      light.label.on('pointerdown', onDragStart)
        .on('pointerup', onDragEnd)
        .on('pointerupoutside', onDragEnd)
        .on('pointermove', onDragMove);
    }
  }

  function updateGlowRing(bridgeIp, id, hex, bri) {
    lights[bridgeIp][id].glow.tint = "0x" + hex;
    lights[bridgeIp][id].glow.height = cellSize * 5 * bri;
    lights[bridgeIp][id].glow.width = cellSize * 5 * bri;
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

  function positionToXY(x) {
    return (x / (WIDTH / 2) - 1)
  }

  function onDragStart(event) {
    console.log(this);
    //previewConnection.invoke("Locate", result).catch(err => console.error(err.toString()));

    // store a reference to the data
    // the reason for this is because of multitouch
    // we want to track the movement of this particular touch
    this.data = event.data;
    this.alpha = 0.5;
    this.dragging = true;
  }

  function onDragEnd() {
    this.alpha = 1;
    this.dragging = false;
    // set the interaction data to null
    this.data = null;
  }

  function onDragMove() {
    if (this.dragging) {
      var newPosition = this.data.getLocalPosition(this.parent);
      this.x = newPosition.x;
      this.y = newPosition.y;
    }
  }
}
