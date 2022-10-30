class YoutubeUser extends User {
    constructor(id, name, avatar, time, onPause, videoId) {
        super(id, name, avatar, time, onPause);
        this.videoId = videoId
    }

    updateVideo(videoId) {
        this.videoId = videoId;
        this.content.children('.video').html('[' + this.videoId + ']');
    }
}

videos = [];

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/youtubeRoom', null)
    .build();

hubConnection.onclose((x) => setTimeout(async () => await x.start(), 5000));

hubConnection.on('ReceiveMessage', onError);

hubConnection.on('Send', onMessageReceived);

hubConnection.on('Pause', function (seconds, id) {
    if (id === data.ownerId) syncMe(seconds, true, null);
    onPauseToggled(id, true, seconds);
});

hubConnection.on('Play', function (seconds, id) {
    if (id === data.ownerId) syncMe(seconds, false, null);
    onPauseToggled(id, false, seconds);
});

hubConnection.on('RemoveVideo', function (id, videoId) {
    if (id === data.ownerId) syncMe(0, false, season + '_' + number)
});
hubConnection.on('AddVideo', function (id, videoId) {
    if (id === data.ownerId) syncMe(0, false, season + '_' + number)
});

hubConnection.on('Change', function (id, videoId) {
    if (id === data.ownerId) syncMe(0, false, season + '_' + number)
});

hubConnection.on('Leave', onDisconnect);

hubConnection.on('Connect', function (json) {
    let data = JSON.parse(json);
    let user = new YoutubeUser(data.id, data.username, data.avatar, data.time, true, data.videoId);
    onConnect(user, addUserToList);
    if (user.id === data.id) init(user);
});

hubConnection.start().then();


let player;

function init(user) {
    console.log(user.videoId)
    player = new YT.Player('player', {
        height: '100%',
        width: '100%',
        videoId: user.videoId,
        events: {
            onStateChange: (el) => console.log(el)
        }
    });
}

// frame.onload = function () {
//     window.addEventListener('message', function (event) {
//         if (event.data.event === 'play' && event.data.time != null) {
//             let time = parseInt(event.data.time);
//             hubConnection.invoke('Play', time).then();
//             onPauseToggled(data.id, false, time)
//         } else if (event.data.event === 'pause' && event.data.time != null) {
//             let time = parseInt(event.data.time);
//             hubConnection.invoke('Pause', time).then();
//             onPauseToggled(true, time)
//         } else if (event.data.event === 'seek' && event.data.time != null) {
//             let user = users.find(u => u.id === data.id);
//             if (!user.onPause) return;
//             let time = parseInt(event.data.time);
//             hubConnection.invoke(user.onPause ? 'Pause' : 'Play', time).then();
//             onTimeUpdated(data.id, time);
//         } else if (event.data.event === 'buffered' && event.data.time != null) {
//             let user = users.find(u => u.id === data.id);
//             if (user.onPause) return;
//             let time = parseInt(event.data.time);
//             hubConnection.invoke(user.onPause ? 'Pause' : 'Play', time).then();
//             onTimeUpdated(data.id, time);
//         } else if (event.data.event === 'new' && event.data.id != null) {
//             let data = event.data.id.split('_')
//             hubConnection.invoke('ChangeSeries', parseInt(data[0]), parseInt(data[1])).then();
//         }
//     })
// };


function syncMe(time, pause, videoId) {
    if (videoId !== null) frame.contentWindow.postMessage({'api': 'find', 'set': fileId}, '*');
    pause ? player.pauseVideo() : player.playVideo();
    player.seekTo(time, true);
}

$('#message').keyup(function (event) {
    if (event.keyCode === 13 && !event.shiftKey) {
        let messageEl = $(this);
        let message = messageEl.val();
        messageEl.val('');
        hubConnection.invoke('Send', message).then();
    }
});


let chat = $('.chatList');
function addUserToList(user) {
    let html = '<div id="' + user.id + '" class="viewer"><div class="d-inline-block pause ' + (user.onPause ? "" : "d-none") + '"><svg xmlns="http://www.w3.org/2000/svg/" width="16" height="16" fill="currentColor" class="bi bi-pause-circle" viewBox="0 0 16 16"><path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/><path d="M5 6.25a1.25 1.25 0 1 1 2.5 0v3.5a1.25 1.25 0 1 1-2.5 0v-3.5zm3.5 0a1.25 1.25 0 1 1 2.5 0v3.5a1.25 1.25 0 1 1-2.5 0v-3.5z"/></svg></div>';
    if (user.id === data.ownerId) html += '<div class="d-inline-block"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-flag" viewBox="0 0 16 16"><path d="M14.778.085A.5.5 0 0 1 15 .5V8a.5.5 0 0 1-.314.464L14.5 8l.186.464-.003.001-.006.003-.023.009a12.435 12.435 0 0 1-.397.15c-.264.095-.631.223-1.047.35-.816.252-1.879.523-2.71.523-.847 0-1.548-.28-2.158-.525l-.028-.01C7.68 8.71 7.14 8.5 6.5 8.5c-.7 0-1.638.23-2.437.477A19.626 19.626 0 0 0 3 9.342V15.5a.5.5 0 0 1-1 0V.5a.5.5 0 0 1 1 0v.282c.226-.079.496-.17.79-.26C4.606.272 5.67 0 6.5 0c.84 0 1.524.277 2.121.519l.043.018C9.286.788 9.828 1 10.5 1c.7 0 1.638-.23 2.437-.477a19.587 19.587 0 0 0 1.349-.476l.019-.007.004-.002h.001M14 1.221c-.22.078-.48.167-.766.255-.81.252-1.872.523-2.734.523-.886 0-1.592-.286-2.203-.534l-.008-.003C7.662 1.21 7.139 1 6.5 1c-.669 0-1.606.229-2.415.478A21.294 21.294 0 0 0 3 1.845v6.433c.22-.078.48-.167.766-.255C4.576 7.77 5.638 7.5 6.5 7.5c.847 0 1.548.28 2.158.525l.028.01C9.32 8.29 9.86 8.5 10.5 8.5c.668 0 1.606-.229 2.415-.478A21.317 21.317 0 0 0 14 7.655V1.222z"/></svg></div>';
    html += '<div class="d-inline-block video">[' + user.videoId + ']</div> <div class="d-inline-block username">' + user.name + '</div> <div class="d-inline-block time">' + getTimeString(user.time) + '</div></div>';
    $("#chat").append(html);
}