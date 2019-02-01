const connection = new signalR.HubConnectionBuilder()
    .withUrl("/statushub")
    .build();

connection.on("StatusMsg", (message) => {
  addLogMsg(message);
});
connection.on("StartingEffect", (message, log) => {
  var color = log.rgbColor === null ? null : '\'' + log.rgbColor + '\'';
  var replayLink = '<span onclick="connection.invoke(\'StartEffect\', \'' + log.name + '\', ' + color + ');">replay</span>';
  if (log.effectType === "group") {
    replayLink = '<span onclick="connection.invoke(\'StartGroupEffect\', \'' + log.name + '\', ' + color + ', \'' + log.group + '\', \'' + log.iteratorMode + '\', \'' + log.secondaryIteratorMode + '\');">replay</span>';
  }
  addLogMsg(replayLink + " | " + message);
});

function addLogMsg(msg) {
  document.getElementById("messagesList").innerHTML = js_yyyy_mm_dd_hh_mm_ss() + ' | ' + msg + '\r\n<br>' + document.getElementById("messagesList").innerHTML;
}

function js_yyyy_mm_dd_hh_mm_ss() {
  now = new Date();
  year = "" + now.getFullYear();
  month = "" + (now.getMonth() + 1); if (month.length === 1) { month = "0" + month; }
  day = "" + now.getDate(); if (day.length === 1) { day = "0" + day; }
  hour = "" + now.getHours(); if (hour.length === 1) { hour = "0" + hour; }
  minute = "" + now.getMinutes(); if (minute.length === 1) { minute = "0" + minute; }
  second = "" + now.getSeconds(); if (second.length === 1) { second = "0" + second; }
  return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
};

connection.on("Status", (status) => {
  vuedj.fillStatus(status);
});
connection.on("Bri", (value) => {
  document.getElementById("briRange").value = 100 - (value * 100);
});


const vuedj = new Vue({
  el: '#vuedj',
  data: {
    effects: {
      baseEffects: [],
      shortEffects: [],
      groupEffects: [],
      groups: null,
      iteratorModes: [],
      secondaryIteratorModes: [],
    },
    status: {
      isAutoMode: false,
      showDisconnect: false,
      groupNames: null
    },
    groupPicked: "",
    iteratorPicked: "",
    secondaryIteratorPicked: ""
  },
  computed: {
    // a computed getter
    composerFilled: function () {
      // `this` points to the vm instance
      return this.groupPicked.length > 1 && this.iteratorPicked.length > 1 && this.secondaryIteratorPicked.length > 1;
    }
  },
  methods: {
    start(effect) {
      var color = effect.isRandom ? null: effect.color;
      connection.invoke("StartEffect", effect.typeName, color).catch(err => console.error(err.toString()));
    },
    startGroup(effect, groupPicked, iteratorPicked, secondaryIteratorPicked) {
      var color = effect.isRandom ? null : effect.color;
      connection.invoke("StartGroupEffect", effect.typeName, color, groupPicked, iteratorPicked, secondaryIteratorPicked).catch(err => console.error(err.toString()));
    },
    increaseBPM(inc) {
      connection.invoke("IncreaseBPM", inc).catch(err => console.error(err.toString()));
    },
    setBPM(v) {
      connection.invoke("SetBPM", v).catch(err => console.error(err.toString()));
    },
    startRandom() {
      connection.invoke("StartRandom").catch(err => console.error(err.toString()));
    },
    startAutoMode() {
      connection.invoke("StartAutoMode").catch(err => console.error(err.toString()));
    },
    stopAutoMode() {
      connection.invoke("StopAutoMode").catch(err => console.error(err.toString()));
    },
    toggleAutoRandomMode() {
      connection.invoke("ToggleAutoRandomMode").catch(err => console.error(err.toString()));
    },
    stopEffects() {
      connection.invoke("StopEffects").catch(err => console.error(err.toString()));
    },
    setColors() {
      //API to set colors using 2d array
      connection.invoke("SetColors", [["FF0000", "FFFFFF"], ["00FF00", "0000FF"]]).catch(err => console.error(err.toString()));
    },
    beat() {
      connection.invoke("Beat", 1).catch(err => console.error(err.toString()));
    },
    connect(name) {
      connection.invoke('Connect', name);
    },
    disconnect() {
      connection.invoke("Disconnect").catch(err => console.error(err.toString()));
    },
    fill(effectvm) {
      this.effects = effectvm;
    },
    fillStatus(status) {
      this.status = status;
    }
  }
})

connection.on("effects", (effectvm) => {
  vuedj.fill(effectvm);
});

connection.start()
    .then(() => connection.invoke("GetStatus"))
    .then(() => connection.invoke("GetEffects", false))
    .catch(err => console.error(err.toString()));

function setBri(value) {
  connection.invoke("SetBri", value).catch(err => console.error(err.toString()));
}

function startShortEffect(key) {
  var index = key - 1;
  var item = vuedj.effects.shortEffects[index];
  if (item !== null)
    vuedj.start(item);
}
function startLongEffect(key) {
  var index = key - 1;
  var item = vuedj.effects.baseEffects[index];
  if (item !== null)
    vuedj.start(item);
}

window.onhelp = function () { return false };
Mousetrap.bindGlobal(['f1', 'f2', 'f3', 'f4', 'f5', 'f6', 'f7', 'f8', 'f9', 'f10', 'f11', 'f12'], (e, key) => {
  console.log(key);
  if(e.preventDefault) {
    e.preventDefault();
  } else {
    // internet explorer
    e.returnValue = false;
  }
  console.log(key.substr(1));
  startLongEffect(key.substr(1));
});

Mousetrap.bindGlobal(['1', '2', '3', '4', '5', '6', '7', '8', '9'], function (e, key) { startShortEffect(key); });
Mousetrap.bindGlobal('w', function () { document.getElementById('briRange').value = 100; setBri(0) });
Mousetrap.bindGlobal('s', function () { document.getElementById('briRange').value = 50; setBri(0.5) });
Mousetrap.bindGlobal('x', function () { document.getElementById('briRange').value = 0; setBri(1) });
Mousetrap.bindGlobal('r', function () { vuedj.startRandom(); });
Mousetrap.bindGlobal('esc', function () { vuedj.stopEffects(); });
Mousetrap.bindGlobal('b', function () { vuedj.beat(); });
