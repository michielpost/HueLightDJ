//const previewConnection = new signalR.HubConnectionBuilder()
//    .withUrl("/previewhub")
//    .withAutomaticReconnect()
//    .build();

//previewConnection.start()
//  .catch(err => console.error(err.toString()));

var lights = [];
var WIDTH = 0;
var cellSize = 0;
var allowEdit = false;
var lightContainer = new PIXI.Container();

function placeLight(bridgeIp, lightId, x, y, hex, bri, groupId, positionIndex) {
  var bridgeArray = lights[bridgeIp];
  var rad = 30 * bri;
  var xPos = xyToPosition(x);
  var yPos = xyToPosition(y * -1);

  if (bridgeArray === undefined || bridgeArray == null) {
    lights[bridgeIp] = [];
  }

  var id = lightId + '_' + positionIndex;
  if (positionIndex === undefined || positionIndex == null) {
    id = lightId;
  }
  var current = lights[bridgeIp][id];

  if (current === undefined || current == null) {

    lights[bridgeIp][id] = {
      glow: createGlowRing(xPos, yPos),
      label: createLightLabel(xPos, yPos, id, bridgeIp),
      groupId: groupId,
      lightId: lightId,
      positionIndex: positionIndex
    };
    updateGlowRing(bridgeIp, id, hex, bri);
    addLightToContainer(lights[bridgeIp][id]);
  }
  else {
    updateGlowRing(bridgeIp, id, hex, bri);
  }
}

function xyToPosition(x) {
  return (WIDTH / 2 + x * WIDTH / 2)
}


function renderPreviewGrid(size, allowEditParam) {
  allowEdit = allowEditParam;
  WIDTH = size;
  var HEIGHT = WIDTH;
  cellSize = WIDTH / 20;

  var app = new PIXI.Application(WIDTH, HEIGHT, { backgroundColor: 0x191D21 });
  document.getElementById("pixiPreview").appendChild(app.view);

  //create the stage instead of container
  app.stage = new PIXI.display.Stage();

  addGridLines(app.stage, allowEdit);

  //var lightContainer = new PIXI.Container();
  app.stage.addChild(lightContainer);

  if (allowEdit) {
    document.getElementById("saveButton").onclick = saveLocations;
    //previewConnection.on("newLocations", (preview) => {
    //  for (var i = 0; i < preview.length; i++) {
    //    var light = preview[i];
    //    placeLight(light.bridge, light.id, light.x, light.y, light.hex, light.bri, light.groupId, light.positionIndex)
    //  }
    //});
  }
  else {
    app.renderer.plugins.interaction.on('pointerdown', touch);

    //placeLight("a", 1, 0.3, 0.3, "0FF00", 0.5);
    //placeLight("b", 2, 0.1, 0.1, "0FF00", 1);

    //previewConnection.on("preview", (preview) => {
    //  for (var i = 0; i < preview.length; i++) {
    //    var light = preview[i];
    //    placeLight(light.bridge, light.id, light.x, light.y, light.hex, light.bri)
    //  }
    //});
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
      var middle = WIDTH / 2;

      graphics.moveTo(0, 0);
      graphics.lineTo(WIDTH, WIDTH);
      graphics.moveTo(0, WIDTH);
      graphics.lineTo(WIDTH, 0);

      for (var i = stepSize; i <= WIDTH; i = i + stepSize) {
        graphics.moveTo(i, 0);
        graphics.lineTo(i, WIDTH);

        graphics.moveTo(middle, middle);
        graphics.lineTo(i, WIDTH);
        graphics.moveTo(middle, middle);
        graphics.lineTo(i, 0);

        graphics.moveTo(0, i);
        graphics.lineTo(WIDTH, i);

        graphics.moveTo(middle, middle);
        graphics.lineTo(WIDTH, i);
        graphics.moveTo(middle, middle);
        graphics.lineTo(0, i);
      }
    }

    graphics.moveTo(WIDTH-1, 0);
    graphics.lineTo(WIDTH, WIDTH);

    graphics.moveTo(0, WIDTH - 1);
    graphics.lineTo(WIDTH, WIDTH - 1);

    app.stage.addChild(graphics);
  }

  function saveLocations() {
    var result = [];
    for (const [key, values] of Object.entries(lights)) {
      for (var i = 0; i < Object.keys(lights[key]).length; i++) {
        var prop = Object.keys(lights[key])[i];
        var l = lights[key][prop];
        if (l != undefined && l != null) {
          var pos = getXYPosition(l.label);
          result.push({
            Id: l.lightId,
            Bridge: key,
            GroupId: l.groupId,
            PositionIndex: l.positionIndex,
            X: pos.x,
            Y: pos.y
          });
        }
      }
    }
  

    //previewConnection.invoke("SetLocations", result).catch(err => console.error(err.toString()));

  }

  function touch(event) {
    var pos = event.data.getLocalPosition(app.stage);
    //previewConnection.invoke("touch", positionToXY(pos.x), positionToXY(WIDTH - pos.y)).catch(err => console.error(err.toString()));

    if (window.console) {
      console.log('Pointer at: ' + positionToXY(pos.x) + ',' + positionToXY(WIDTH - pos.y));
    }
  }

  function getXYPosition(obj) {
    var xypos = {};
    xypos.x = positionToXY(obj.x);
    xypos.y = positionToXY(WIDTH - obj.y);

    return xypos;
  }

  function positionToXY(x) {
    return (x / (WIDTH / 2) - 1)
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
  var glowRing = PIXI.Sprite.fromImage("../images/glow.png");
  glowRing.anchor.set(0.5, 0.5);
  glowRing.height = cellSize * 5;
  glowRing.width = cellSize * 5;
  glowRing.position.x = x;
  glowRing.position.y = y;
  glowRing.blendMode = PIXI.BLEND_MODES.SCREEN;
  return glowRing;
}

function createLightLabel(x, y, id, bridgeIp) {
  var textColor = 0xE9ECEF;
  if (allowEdit)
    textColor = 0xFF0000;

  var label = new PIXI.Text(String(id), { font: "12px", fill: textColor });
  label.anchor.set(0.5, 0.5);
  label.position.x = x;
  label.position.y = y;
  label.lightId = id;
  label.bridgeIp = bridgeIp;

  return label;
}

function onDragStart(event) {
  //previewConnection.invoke("Locate", { id: this.lightId, bridge: this.bridgeIp }).catch(err => console.error(err.toString()));

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

