const connection = new signalR.HubConnectionBuilder()
    .withUrl("/statushub")
    .build();

connection.on("StatusMsg", (message) => {
    document.getElementById("messagesList").innerHTML = message;
});

connection.on("Status", (status) => {
    document.getElementById("bpm").innerHTML = status.bpm;
});

connection.on("effects", (effectvm) => {
    var example1 = new Vue({
        el: '#effects',
        data: {
          baseEffects: effectvm.baseEffects,
          shortEffects: effectvm.shortEffects,
          groupEffects: effectvm.groupEffects,
          groups: effectvm.groups,
          iteratorModes: effectvm.iteratorModes,
          secondaryIteratorModes: effectvm.secondaryIteratorModes,
          groupPicked: "",
          iteratorPicked: "",
          secondaryIteratorPicked: ""
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
          }
        }
    })
});

connection.start()
    .then(() => connection.invoke("GetStatus"))
    .catch(err => console.error(err.toString()));

document.getElementById("connectButton").addEventListener("click", event => {
    connection.invoke("Connect").catch(err => console.error(err.toString()));
    event.preventDefault();
});
document.getElementById("disconnectButton").addEventListener("click", event => {
  connection.invoke("Disconnect").catch(err => console.error(err.toString()));
  event.preventDefault();
});

document.getElementById("bpmPlusButton").addEventListener("click", event => {
    connection.invoke("IncreaseBPM", 1).catch(err => console.error(err.toString()));
    event.preventDefault();
});
document.getElementById("bpmPlusPlusButton").addEventListener("click", event => {
    connection.invoke("IncreaseBPM", 5).catch(err => console.error(err.toString()));
    event.preventDefault();
});
document.getElementById("bpmMinButton").addEventListener("click", event => {
    connection.invoke("IncreaseBPM", -1).catch(err => console.error(err.toString()));
    event.preventDefault();
});
document.getElementById("bpmMinMinButton").addEventListener("click", event => {
    connection.invoke("IncreaseBPM", -5).catch(err => console.error(err.toString()));
    event.preventDefault();
});
