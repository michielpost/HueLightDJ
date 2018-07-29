const connection = new signalR.HubConnectionBuilder()
    .withUrl("/statushub")
    .build();

connection.on("StatusMsg", (message) => {
  document.getElementById("messagesList").value = message + '\r\n' + document.getElementById("messagesList").value;
});

connection.on("Status", (status) => {
    document.getElementById("bpm").innerHTML = status.bpm;
});


const example1 = new Vue({
  el: '#effects',
  data: {
    baseEffects: [],
    shortEffects: [],
    groupEffects: [],
    groups: null,
    iteratorModes: [],
    secondaryIteratorModes: [],
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
      if (effect.isRandom) {
        effect.color = null;
      }
      connection.invoke("StartEffect", effect.typeName, effect.color).catch(err => console.error(err.toString()));
    },
    startGroup(effect, groupPicked, iteratorPicked, secondaryIteratorPicked) {
      if (effect.isRandom) {
        effect.color = null;
      }
      connection.invoke("StartGroupEffect", effect.typeName, effect.color, groupPicked, iteratorPicked, secondaryIteratorPicked).catch(err => console.error(err.toString()));
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
    fill(effectvm) {
      this.baseEffects = effectvm.baseEffects;
      this.shortEffects = effectvm.shortEffects;
      this.groupEffects = effectvm.groupEffects;
      this.groups = effectvm.groups;
      this.iteratorModes = effectvm.iteratorModes;
      this.secondaryIteratorModes = effectvm.secondaryIteratorModes;
      this.groupPicked = "";
      this.iteratorPicked = "";
      this.secondaryIteratorPicked = "";
    }
  }
})

connection.on("effects", (effectvm) => {
  example1.fill(effectvm);
});

connection.start()
    .then(() => connection.invoke("GetStatus"))
    .catch(err => console.error(err.toString()));

document.getElementById("connectButton").addEventListener("click", event => {
    connection.invoke("Connect").catch(err => console.error(err.toString()));
    event.preventDefault();
});
document.getElementById("connectDemoButton").addEventListener("click", event => {
  connection.invoke("Connect", true).catch(err => console.error(err.toString()));
  event.preventDefault();
});
document.getElementById("disconnectButton").addEventListener("click", event => {
  connection.invoke("Disconnect").catch(err => console.error(err.toString()));
  event.preventDefault();
});

function setBri(value) {
  connection.invoke("SetBri", value).catch(err => console.error(err.toString()));
}
