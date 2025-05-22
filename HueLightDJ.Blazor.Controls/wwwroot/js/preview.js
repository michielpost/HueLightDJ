//const previewConnection = new signalR.HubConnectionBuilder()
//    .withUrl("/previewhub")
//    .withAutomaticReconnect()
//    .build();

//previewConnection.start()
//  .catch(err => console.error(err.toString()));

// Namespace for shared variables
window.huePreview = {};

var lights = []; // Still used as a cache for individual light objects (glow, icon, label)
// var WIDTH = 0; // Removed, will use app.screen.width
var cellSize = 0; // Will be based on app.screen.width or fixed
var allowEdit = false;
var lightContainer = new PIXI.Container();

// Bounds for light positions, initialized before first use in showPreview
var minX, minY, maxX, maxY;
var previewPadding = 50; // Pixels of padding around the lights


function placeLight(bridgeIp, lightId, x, y, hex, bri, groupId, positionIndex, name) {
  var bridgeArray = lights[bridgeIp];
  // var rad = 30 * bri; // rad seems unused

  // x and y are from Hue (-1 to +1 range typically). Convert to initial pixel positions.
  // These positions will be relative to a conceptual canvas before scaling.
  // If your original xyToPosition was mapping to a fixed size (e.g., 500x500),
  // we can keep that behavior for initial placement, and scaling will adjust.
  // For simplicity, let's assume x, y from Hue are already in a suitable range
  // that can be directly used or scaled by a fixed factor if they are too small (e.g. -1 to 1).
  // Let's use a hypothetical fixed conceptual canvas size for initial mapping.
  const conceptualWidth = 500; // Or use app.screen.width if xyToPosition is updated
  const conceptualHeight = 500; // Or use app.screen.height

  var xPos = (conceptualWidth / 2) + (x * conceptualWidth / 2);
  var yPos = (conceptualHeight / 2) + (y * -1 * conceptualHeight / 2); // y is inverted

  // Track bounds based on these initial pixel positions
  minX = Math.min(minX, xPos);
  maxX = Math.max(maxX, xPos);
  minY = Math.min(minY, yPos);
  maxY = Math.max(maxY, yPos);

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
      icon: createLampIcon(xPos, yPos), // Create and store icon
      label: createLightLabel(xPos, yPos, name, id, bridgeIp), // Pass name to label
      groupId: groupId,
      lightId: lightId,
      positionIndex: positionIndex
    };
    updateGlowRing(bridgeIp, id, hex, bri);
    addLightToContainer(lights[bridgeIp][id]);
  }
  else {
    updateGlowRing(bridgeIp, id, hex, bri);
    // If label text could change (e.g. name updated), update it here too
    // lights[bridgeIp][id].label.text = String(name || 'ID: ' + id);
  }
}

// This function's role changes. It now maps a coordinate from the conceptual canvas (e.g., 0-500)
// to the -1 to +1 range if needed for saving, or it might not be needed if xPos/yPos are stored directly.
// For now, let's assume the main use of xyToPosition was for initial pixel mapping from -1 to 1.
// The `placeLight` function now handles this initial mapping.
// The `getXYPosition` function for saving might need adjustment if it relied on this.
function xyToPosition(coord, totalSize) { // totalSize would be app.screen.width or height
  return (totalSize / 2 + coord * totalSize / 2);
}


function renderPreviewGrid(size, allowEditParam) { // size parameter might be deprecated or used differently
  allowEdit = allowEditParam;
  var container = document.getElementById("pixiPreview");
  if (!container) {
      console.error("pixiPreview container not found!");
      return;
  }
  
  var appWidth = container.clientWidth;
  var appHeight = container.clientHeight;

  cellSize = appWidth / 20; // Or a fixed value like 10

  var app = new PIXI.Application({ width: appWidth, height: appHeight, backgroundColor: 0x191D21, resolution: window.devicePixelRatio || 1, autoDensity: true });
  container.appendChild(app.view);
  
  window.huePreview.app = app; // Store app instance

  app.stage = new PIXI.display.Stage();
  app.stage.addChild(lightContainer);
  
  // addGridLines might need to use appWidth and appHeight now
  addGridLines(app.stage, allowEdit, appWidth, appHeight);


  if (allowEdit) {
    // Ensure saveButton exists if we are in edit mode
    var saveButton = document.getElementById("saveButton");
    if (saveButton) {
        saveButton.onclick = saveLocations;
    }
    // Existing commented out SignalR code for newLocations
  }
  else {
    app.renderer.plugins.interaction.on('pointerdown', touch);
    // Existing commented out SignalR code for preview
  }
  
  updatePreviewScale(app, container); // Initial scale setup
  
  // Resize listener for responsiveness
  window.addEventListener('resize', () => {
      var newAppWidth = container.clientWidth;
      var newAppHeight = container.clientHeight;
      app.renderer.resize(newAppWidth, newAppHeight);
      // addGridLines might need to be redrawn if it's not part of lightContainer and using old WIDTH
      // For simplicity, grid lines are not redrawn here, but in a full app, you might.
      updatePreviewScale(app, container);
  });

  function addGridLines(stage, drawGrid, currentWidth, currentHeight) {
    // This function might need to be cleared and redrawn on resize if it's not dynamic
    // For now, it draws based on initial dimensions.
    // If grid is static relative to stage, it will scale with stage.
    // If it needs to be fixed to screen, it shouldn't be added to app.stage directly or needs updates.
    var graphics = new PIXI.Graphics();
    var stepSize = cellSize * 2; // cellSize is based on initial appWidth

    graphics.lineStyle(1, 0xffffff, 0.1); // Make grid lines less prominent

    // Vertical lines
    for (let x = 0; x <= currentWidth; x += stepSize) {
        graphics.moveTo(x, 0);
        graphics.lineTo(x, currentHeight);
    }
    // Horizontal lines
    for (let y = 0; y <= currentHeight; y += stepSize) {
        graphics.moveTo(0, y);
        graphics.lineTo(currentWidth, y);
    }
    
    // Center cross
    graphics.lineStyle(1, 0xffffff, 0.2);
    graphics.moveTo(currentWidth / 2, 0);
    graphics.lineTo(currentWidth / 2, currentHeight);
    graphics.moveTo(0, currentHeight / 2);
    graphics.lineTo(currentWidth, currentHeight / 2);

    stage.addChildAt(graphics, 0); // Add grid behind lights
  }

  function saveLocations() {
    var result = [];
    var app = window.huePreview.app;
    if (!app) return;

    for (const [key, values] of Object.entries(lights)) { // lights cache
      for (const idInCache of Object.keys(lights[key])) {
        var l = lights[key][idInCache];
        if (l && l.label) { // Ensure label exists for position
          // Convert current PIXI coordinates (which are absolute on the conceptual canvas)
          // back to the -1 to +1 range.
          // This requires knowing the conceptual canvas dimensions used in placeLight.
          const conceptualWidth = 500; 
          const conceptualHeight = 500;
          
          var currentXPos = l.label.x; // Assuming label is center after icon adjustment
          if(l.icon) currentXPos = (l.label.x + l.icon.x) / 2; // More accurate center

          var currentYPos = l.label.y;

          result.push({
            Id: l.lightId, // Original light ID without positionIndex suffix
            Bridge: key,
            GroupId: l.groupId,
            PositionIndex: l.positionIndex,
            X: (currentXPos - conceptualWidth / 2) / (conceptualWidth / 2),
            Y: ((currentYPos - conceptualHeight / 2) / (conceptualHeight / 2)) * -1 // Invert Y
          });
        }
      }
    }
    console.log("Saving locations:", result);
    // previewConnection.invoke("SetLocations", result).catch(err => console.error(err.toString()));
  }

  function touch(event) {
    var app = window.huePreview.app;
    if (!app) return;
    var pos = event.data.getLocalPosition(app.stage);
    
    // Convert screen tap coordinates to the original -1 to +1 range for the backend
    // This is the reverse of the initial mapping in placeLight, considering current scale and offset.
    // For now, this part is complex and might not be perfectly accurate without full inverse transform.
    // The example below is a placeholder.
    const conceptualWidth = 500; 
    const conceptualHeight = 500;
    var originalX = (pos.x - conceptualWidth / 2) / (conceptualWidth / 2);
    var originalY = ((pos.y - conceptualHeight / 2) / (conceptualHeight / 2)) * -1;

    // previewConnection.invoke("touch", originalX, originalY).catch(err => console.error(err.toString()));
    if (window.console) {
      console.log('Touch transformed to approx original coords: ' + originalX + ',' + originalY);
    }
  }
}

// getXYPosition and positionToXY are problematic with scaling if not handled carefully.
// getXYPosition was used in saveLocations. saveLocations now calculates -1 to 1 directly.
// positionToXY was used in touch and getXYPosition. touch now calculates -1 to 1 directly.
// These might be deprecated or need careful review if used elsewhere.


function addLightToContainer(light) {
  if (light.glow) lightContainer.addChild(light.glow);
  if (light.icon) lightContainer.addChild(light.icon);
  if (light.label) lightContainer.addChild(light.label);

  if (allowEdit && light.label) {
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
  // cellSize might need to be adjusted if it's too large/small after scaling
  // Or, keep it based on initial conceptual size, and let the whole thing scale.
  var initialGlowSize = (window.huePreview.app ? window.huePreview.app.screen.width / 20 : 25) * 5; // Example adjustment
  glowRing.height = initialGlowSize; // cellSize * 5;
  glowRing.width = initialGlowSize;  // cellSize * 5;
  glowRing.position.x = x;
  glowRing.position.y = y;
  glowRing.blendMode = PIXI.BLEND_MODES.SCREEN;
  return glowRing;
}

function createLightLabel(x, y, name, id, bridgeIp) {
  var textColor = 0xE9ECEF;
  if (allowEdit)
    textColor = 0xFF0000;

  var labelText = String(name || 'ID: ' + id);
  var label = new PIXI.Text(labelText, { font: "12px Arial", fill: textColor, align: "center" });
  label.anchor.set(0, 0.5); // Anchor left-middle for positioning right of icon
  label.position.x = x + 10; // Position label to the right of the icon's (new) center
  label.position.y = y;
  label.lightId = id;
  label.bridgeIp = bridgeIp;

  return label;
}

function createLampIcon(x, y) {
    var iconSprite = PIXI.Sprite.fromImage("../images/lamp_icon.png");
    iconSprite.anchor.set(0.5, 0.5);
    iconSprite.width = 16;
    iconSprite.height = 16;
    iconSprite.position.x = x - 5; // Position icon slightly to the left of the logical light center
    iconSprite.position.y = y;
    return iconSprite;
}


// Main function called from C# to update the preview
window.huePreview.showPreview = function(previewData) {
    var app = window.huePreview.app;
    var container = document.getElementById("pixiPreview");

    if (!app || !container) {
        console.error("Preview app or container not initialized.");
        return;
    }

    // Clear previous lights and reset bounds
    if (lightContainer) {
        lightContainer.removeChildren();
    }
    minX = Infinity; minY = Infinity; maxX = -Infinity; maxY = -Infinity;
    lights = []; // Reset the cache

    if (previewData && previewData.length > 0) {
        previewData.forEach(light => {
            placeLight(light.bridge, light.id, light.x, light.y, light.hex, light.bri, light.groupId, light.positionIndex, light.name);
        });
    }
    
    updatePreviewScale(app, container);
};

function updatePreviewScale(app, containerElement) {
    if (!app || !app.stage) return;

    var containerWidth = containerElement.clientWidth;
    var containerHeight = containerElement.clientHeight;
    
    // If renderer hasn't caught up with container size, use renderer dimensions
    if (app.renderer.width !== containerWidth || app.renderer.height !== containerHeight) {
        app.renderer.resize(containerWidth, containerHeight);
    }

    if (minX === Infinity || maxX === -Infinity || minY === Infinity || maxY === -Infinity ) { // No lights or bounds not updated
        app.stage.scale.set(1);
        app.stage.position.set(containerWidth / 2, containerHeight / 2); // Center if no content
        return;
    }

    var contentWidth = maxX - minX;
    var contentHeight = maxY - minY;

    if (contentWidth <= 0) contentWidth = 100; // Default small width if single point or no width
    if (contentHeight <= 0) contentHeight = 100; // Default small height

    var scaleX = (containerWidth - previewPadding * 2) / contentWidth;
    var scaleY = (containerHeight - previewPadding * 2) / contentHeight;
    var scaleFactor = Math.min(scaleX, scaleY);

    if (scaleFactor <= 0 || !isFinite(scaleFactor)) {
         scaleFactor = Math.min(containerWidth / 200, containerHeight / 200); // Fallback scale
         if(scaleFactor <= 0 || !isFinite(scaleFactor)) scaleFactor = 1;
    }
    
    app.stage.scale.set(scaleFactor);

    var scaledContentWidth = contentWidth * scaleFactor;
    var scaledContentHeight = contentHeight * scaleFactor;
    
    // Center the scaled content
    var offsetX = (containerWidth - scaledContentWidth) / 2 - (minX * scaleFactor);
    var offsetY = (containerHeight - scaledContentHeight) / 2 - (minY * scaleFactor);

    app.stage.position.set(offsetX, offsetY);
}


function onDragStart(event) {
  // previewConnection.invoke("Locate", { id: this.lightId, bridge: this.bridgeIp }).catch(err => console.error(err.toString()));

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

