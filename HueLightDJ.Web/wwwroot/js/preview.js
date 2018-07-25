var WIDTH = 400, HEIGHT = 400, PAD = 80;

var app = new PIXI.Application(WIDTH, HEIGHT, { backgroundColor: 0xFFFFFF });
document.getElementById("pixiPreview").appendChild(app.view);

//create the stage instead of container
app.stage = new PIXI.display.Stage();

var bunnyWorld = new PIXI.Container();
app.stage.addChild(bunnyWorld);

var lighting = new PIXI.display.Layer();
lighting.on('display', function (element) {
  element.blendMode = PIXI.BLEND_MODES.ADD;
});
lighting.useRenderTexture = true;
//lighting.clearColor = [0.5, 0.5, 0.5, 1]; // ambient gray

app.stage.addChild(lighting);

var lightingSprite = new PIXI.Sprite(lighting.getRenderTexture());
lightingSprite.blendMode = PIXI.BLEND_MODES.MULTIPLY;

app.stage.addChild(lightingSprite);

function placeLight(id, x, y, r, g, b, bri) {

  var basicText = bunnyWorld.children.find((x) => x.lightId === id)

  if (basicText == null) {
    basicText = new PIXI.Text(id);

    basicText.position.set(xyToPosition(x), xyToPosition(y));
    //bunny.anchor.set(0.5, 0.5);

    var lightbulb = new PIXI.Graphics();
    var rad = 50 * bri;
    lightbulb.beginFill((r << 16) + (g << 8) + b, 1.0, 1);
    lightbulb.drawCircle(0, 0, rad);
    lightbulb.endFill();
    lightbulb.parentLayer = lighting;

    basicText.addChild(lightbulb);
    basicText.lightId = id;

    bunnyWorld.addChild(basicText);
  }
  else {
    basicText.children.splice(0, basicText.children.length);
    var lightbulb = new PIXI.Graphics();
    var rad = 50 * bri;
    lightbulb.beginFill((r << 16) + (g << 8) + b, 1.0, 1);
    lightbulb.drawCircle(0, 0, rad);
    lightbulb.endFill();
    lightbulb.parentLayer = lighting;

    basicText.addChild(lightbulb);
  }
}

function xyToPosition(x) {
  return (WIDTH / 2 + x * WIDTH / 2)
}

//placeLight(1, 0, 0, "#0F0000", 0.5);
//placeLight(2, 0.1, 0.1, "#0FF00", 1);

