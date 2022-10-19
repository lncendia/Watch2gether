class FilmUser extends User {
    constructor(id, name, avatar, time, onPause, season, series) {super(id, name, avatar, time, onPause);
        this.season = season;
        this.series = series;

    }
    updateSeries(season, series) {
        this.season = season;
        this.series = series;
        this.content.children(".episode").html("[" + this.season + ":" + this.series + "]");
    }
}

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/filmRoom", null)
    .build();

hubConnection.onclose((x) => setTimeout(async () => await x.start(), 5000));

hubConnection.on("ReceiveMessage", onError);

hubConnection.on("Send", onMessageReceived);

hubConnection.on("Pause", function (seconds, id) {
    if (id === data.ownerId) syncMe(seconds, true, null);
    onPauseToggled(id, true, seconds);
});

hubConnection.on("Play", function (seconds, id) {
    if (id === data.ownerId) syncMe(seconds, false, null);
    onPauseToggled(id, false, seconds);
});

hubConnection.on("Change", function (id, season, number) {
    if (id === data.ownerId) syncMe(0, false, season + '_' + number)
    onSeriesChanged(id, season, number);
});

hubConnection.on("Leave", onDisconnect);

hubConnection.on("Connect", function (json) {
    let data = JSON.parse(json);
    let user = new FilmUser(data.id, data.username, data.avatar, data.time, true, data.season, data.series);
    onConnect(user);
});

hubConnection.start().then();

let frame = $("#player")[0];

frame.onload = function () {
    window.addEventListener("message", function (event) {
        if (event.data.event === "play" && event.data.time != null) {
            let time = parseInt(event.data.time);
            hubConnection.invoke("Play", time).then();
            onPauseToggled(data.id,false, time)
        } else if (event.data.event === "pause" && event.data.time != null) {
            let time = parseInt(event.data.time);
            hubConnection.invoke("Pause", time).then();
            onPauseToggled(true, time)
        } else if (event.data.event === "seek" && event.data.time != null) {
            let user = users.find(u => u.id === data.id);
            if (!user.onPause) return;
            let time = parseInt(event.data.time);
            hubConnection.invoke(user.onPause ? "Pause" : "Play", time).then();
            onTimeUpdated(data.id, time);
        } else if (event.data.event === "buffered" && event.data.time != null) {
            let user = users.find(u => u.id === data.id);
            if (user.onPause) return;
            let time = parseInt(event.data.time);
            hubConnection.invoke(user.onPause ? "Pause" : "Play", time).then();
            onTimeUpdated(data.id, time);
        } else if (event.data.event === "new" && event.data.id != null) {
            let data = event.data.id.split('_')
            let season = parseInt(data[0]);
            let series = parseInt(data[1]);
            hubConnection.invoke("ChangeSeries", season, series).then();
            onSeriesChanged(data.id, season, series);
        }
    })
};


function syncMe(time, pause, fileId) {
    if (fileId !== null) frame.contentWindow.postMessage({"api": "find", "set": fileId}, "*");
    let message = pause ? "pause" : "play";
    frame.contentWindow.postMessage({"api": message}, "*");
    frame.contentWindow.postMessage({"api": "seek", "set": time}, "*");
}

$("#message").keyup(function (event) {
    if (event.keyCode === 13 && !event.shiftKey) {
        let messageEl = $(this);
        let message = messageEl.val();
        messageEl.val("");
        hubConnection.invoke("Send", message).then();
    }
});

function onSeriesChanged(id, season, number) {
    let user = users.find(u => u.id === id);
    if (user === undefined) return;
    user.updateSeries(season, number);
}