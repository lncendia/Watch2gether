class YoutubeUser extends User{
    constructor(id, name, avatar, time, onPause, videoId) {super(id, name, avatar,time, onPause);
        this.videoId = videoId
    }
    updateVideo(videoId) {
        this.videoId = videoId;
        this.content.children(".video").html("[" + this.videoId + "]");
    }
}

videos = [];

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/youtubeRoom", null)
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

hubConnection.on("RemoveVideo", function (id, videoId) {
    if (id === data.ownerId) syncMe(0, false, season + '_' + number)
});
hubConnection.on("AddVideo", function (id, videoId) {
    if (id === data.ownerId) syncMe(0, false, season + '_' + number)
});

hubConnection.on("Change", function (id, videoId) {
    if (id === data.ownerId) syncMe(0, false, season + '_' + number)
});

hubConnection.on("Leave", onDisconnect);

hubConnection.on("Connect", onConnect);

hubConnection.start().then();





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
            hubConnection.invoke("ChangeSeries", parseInt(data[0]), parseInt(data[1])).then();
        }
    })
};


function syncMe(time, pause, videoId) {
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