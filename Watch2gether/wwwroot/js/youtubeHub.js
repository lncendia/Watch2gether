const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/youtubeRoom", null)
    .build();

hubConnection.onclose((x) => setTimeout(async () => await x.start(), 5000));

hubConnection.on("ReceiveMessage", onError);

hubConnection.on("Send", (username, id, avatar, message) => showMessage(username, avatar, message, id === data.currentUserId));

hubConnection.on("Pause", function (seconds, id) {
    if (id === data.ownerId) syncMe(seconds, true, null);
    let user = users.find(u => u.id === id);
    user.updateInfo(true, seconds);
});

hubConnection.on("Play", function (seconds, id) {
    if (id === data.ownerId) syncMe(seconds, false, null);
    let user = users.find(u => u.id === id);
    user.updateInfo(false, seconds);
});

hubConnection.on("Change", function (id, season, number) {
    if (id === data.ownerId) syncMe(0, false, season + '_' + number)
});

hubConnection.on("Leave", onDisconnect);

hubConnection.on("Connect", onConnect);

hubConnection.start().then();

let player = new YT.Player('player', {
    height: '390',
    width: '640',
    videoId: data.videoId,
    events: {
        'onStateChange': onPlayerStateChange
    }
});

frame.onload = function () {
    window.addEventListener("message", function (event) {
        if (event.data.event === "play" && event.data.time != null) {
            let time = parseInt(event.data.time);
            hubConnection.invoke("Play", time).then();
            let user = users.find(u => u.id === data.currentUserId);
            user.updateInfo(false, parseInt(event.data.time));
        } else if (event.data.event === "pause" && event.data.time != null) {
            let time = parseInt(event.data.time);
            hubConnection.invoke("Pause", time).then();
            let user = users.find(u => u.id === data.currentUserId);
            user.updateInfo(true, time);
        } else if (event.data.event === "seek" && event.data.time != null) {
            let user = users.find(u => u.id === data.currentUserId);
            if (!user.onPause) return;
            let time = parseInt(event.data.time);
            hubConnection.invoke(user.onPause ? "Pause" : "Play", time).then();
            user.updateInfo(user.onPause, time);
        } else if (event.data.event === "buffered" && event.data.time != null) {
            let user = users.find(u => u.id === data.currentUserId);
            if (user.onPause) return;
            let time = parseInt(event.data.time);
            hubConnection.invoke(user.onPause ? "Pause" : "Play", time).then();
            user.updateInfo(user.onPause, time);
        } else if (event.data.event === "new" && event.data.id != null) {
            let data = event.data.id.split('_')
            hubConnection.invoke("ChangeSeries", parseInt(data[0]), parseInt(data[1])).then();
        }
    })
};


function syncMe(time, pause, fileId) {
    if (fileId !== null) frame.contentWindow.postMessage({"api": "find", "set": fileId}, "*");
    let message = pause ? "pause" : "play";
    frame.contentWindow.postMessage({"api": message}, "*");
    frame.contentWindow.postMessage({"api": "seek", "set": time}, "*");
}