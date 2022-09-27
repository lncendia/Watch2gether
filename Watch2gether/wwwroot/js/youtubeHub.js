//import {signalR} from "../lib/signalr/dist/signalr.min";

let div = $("#chatScroll");
div.ready(() => div.scrollTop(div.prop('scrollHeight')));

let current = $("#currentViewer");
let data = {
    currentUserId: current.attr("data-id"),
    currentUsername: current.attr("data-username"),
    currentUserAvatar: current.attr("data-avatar"),
    ownerId: current.attr("data-owner"),
}


class User {

    constructor(id, name, time, onPause) {
        this.name = name;
        this.onPause = onPause;
        this.time = time;
        this.id = id;
        this.content = $("#" + this.id);
    }

    updateInfo(pause, time) {
        this.onPause = pause;
        this.time = time;
        this.content.children(".time").html(getTimeString(this.time));
        this.content.children(".pause").toggleClass("d-none", !this.onPause);
    }
}

let users = getUsers();


const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/youtubeRoom", null)
    .build();

hubConnection.onclose((x) => setTimeout(async () => await x.start(), 5000));

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

hubConnection.on("Leave", function (vId) {
    let user = users.find(x => x.id === vId);
    if (user == null) return;
    users.splice(users.indexOf(user), 1);
    removeUserFromList(user);
    updateUsersCount();
    showNotify("#813232", user.name + " покинул комнату");
});

hubConnection.on("Connect", function (name, vId) {
    let user = users.find(x => x.id === vId);
    if (user != null) return;
    user = new User(vId, name, 0, true);
    users.push(user);
    addUserToList(user);
    updateUsersCount();
    showNotify("#8eb969", name + " подключился к комнате");
});

hubConnection.start().then();

let frame = $("#player")[0];


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

function getTimeString(time) {
    let hours = Math.floor(time / 3600);
    let minutes = Math.floor((time - (hours * 3600)) / 60);
    let seconds = Math.floor(time - (hours * 3600) - (minutes * 60));
    if (hours < 10) hours = "0" + hours;
    if (minutes < 10) minutes = "0" + minutes;
    if (seconds < 10) seconds = "0" + seconds;
    return hours + ":" + minutes + ":" + seconds;
}


let start = Date.now();
setInterval(function () {
    let delta = Date.now() - start;
    start = Date.now();
    users.forEach((user) => {
        if (user.onPause === false) user.updateInfo(user.onPause, user.time + (delta / 1000));
    });
}, 1000);

function getUsers() {
    let users = [];
    $(".viewer").each(function () {
        let name = $(this).children(".username").text();
        let id = $(this).attr("id");
        let timeArray = $(this).children(".time").text().split(":");
        let time = parseInt(timeArray[0]) * 3600 + parseInt(timeArray[1]) * 60 + parseInt(timeArray[2]);
        let onPause = !$(this).children(".isPause").hasClass("d-none");
        users.push(new User(id, name, time, onPause));
    });
    return users;
}

function updateUsersCount() {
    $("#countViewers").html("В сети: " + users.length);
}

function addUserToList(user) {
    let html = "<li id=\"" + user.id + "\" class=\"list-group-item py-0 pb-1 viewer\"><div class=\"d-inline-block pause " + (user.onPause ? "" : "d-none") + "\"><svg xmlns=\"http://www.w3.org/2000/svg/\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-pause-circle\" viewBox=\"0 0 16 16\"><path d=\"M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z\"/><path d=\"M5 6.25a1.25 1.25 0 1 1 2.5 0v3.5a1.25 1.25 0 1 1-2.5 0v-3.5zm3.5 0a1.25 1.25 0 1 1 2.5 0v3.5a1.25 1.25 0 1 1-2.5 0v-3.5z\"/></svg></div> ";
    if (user.id === data.ownerId) html += "<div class=\"d-inline-block\"><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-flag\" viewBox=\"0 0 16 16\"><path d=\"M14.778.085A.5.5 0 0 1 15 .5V8a.5.5 0 0 1-.314.464L14.5 8l.186.464-.003.001-.006.003-.023.009a12.435 12.435 0 0 1-.397.15c-.264.095-.631.223-1.047.35-.816.252-1.879.523-2.71.523-.847 0-1.548-.28-2.158-.525l-.028-.01C7.68 8.71 7.14 8.5 6.5 8.5c-.7 0-1.638.23-2.437.477A19.626 19.626 0 0 0 3 9.342V15.5a.5.5 0 0 1-1 0V.5a.5.5 0 0 1 1 0v.282c.226-.079.496-.17.79-.26C4.606.272 5.67 0 6.5 0c.84 0 1.524.277 2.121.519l.043.018C9.286.788 9.828 1 10.5 1c.7 0 1.638-.23 2.437-.477a19.587 19.587 0 0 0 1.349-.476l.019-.007.004-.002h.001M14 1.221c-.22.078-.48.167-.766.255-.81.252-1.872.523-2.734.523-.886 0-1.592-.286-2.203-.534l-.008-.003C7.662 1.21 7.139 1 6.5 1c-.669 0-1.606.229-2.415.478A21.294 21.294 0 0 0 3 1.845v6.433c.22-.078.48-.167.766-.255C4.576 7.77 5.638 7.5 6.5 7.5c.847 0 1.548.28 2.158.525l.028.01C9.32 8.29 9.86 8.5 10.5 8.5c.668 0 1.606-.229 2.415-.478A21.317 21.317 0 0 0 14 7.655V1.222z\"/></svg></div> ";
    html += "<div class=\"d-inline-block username\">" + user.name + "</div> <div class=\"d-inline-block time\">" + "00:00:00" + "</div></li>";
    $("#viewers").append(html);
}

function removeUserFromList(user) {
    $("#" + user.id).remove();
}

function showMessage(username, avatar, message, isCurrentViewer) {
    let html;
    if (isCurrentViewer) html = "<div class=\"d-flex flex-row justify-content-end mb-4\"><div class=\"message messageMe\"><p style=\"font-size: 12px;\" class=\"text-end additionalText\">" + username + "</p><p class=\"small mb-0\">" + message + "</p><p style=\"font-size: 10px\" class=\"additionalText\">" + new Date().toLocaleTimeString() + "</p></div><img src=\"/img/Avatars/" + avatar + "\" class=\"messageAvatar\" alt=\"\"></div>";
    else html = "<div class=\"d-flex flex-row justify-content-start mb-4\"><img src=\"/img/Avatars/" + avatar + "\" class=\"messageAvatar\" alt=\"\"><div class=\"message messageOther\"><p style=\"font-size: 12px\" class=\"additionalText\">" + username + "</p><p class=\"small mb-0\">" + message + "</p><p style=\"font-size: 10px;\" class=\"text-end additionalText\">" + new Date().toLocaleTimeString() + "</p></div></div>"
    div.append(html);
    div.scrollTop(div.prop('scrollHeight'));
}

function showNotify(color, text) {
    let id = Math.random().toString(36).substring(7);
    div.append("<div id=\"" + id + "\" class=\"text-center mb-4\"><div style=\"background-color: " + color + "\" class=\"border notify\"><p class=\"mb-0\">" + text + "</p></div></div>");
    div.scrollTop(div.prop('scrollHeight'));
    setTimeout(function () {
        $("#" + id).hide('slow', function () {
            $(this).remove();
        });
    }, 5000);
}


$("#copyButton").click(function () {
    copyButton($(this)).then();
    return false;
})

async function copyButton(el) {
    let copyText = el.attr("data-clipboard-text");
    await navigator.clipboard.writeText(copyText);
    el.html("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-clipboard-check\" viewBox=\"0 0 16 16\">\n  <path fill-rule=\"evenodd\" d=\"M10.854 7.146a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 1 1 .708-.708L7.5 9.793l2.646-2.647a.5.5 0 0 1 .708 0z\"/>\n  <path d=\"M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z\"/>\n  <path d=\"M9.5 1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5h3zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z\"/>\n</svg> Ссылка скопирована")
    setTimeout(function () {
        el.html("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-clipboard\" viewBox=\"0 0 16 16\">\n  <path d=\"M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z\"/>\n  <path d=\"M9.5 1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5h3zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z\"/>\n  </svg>\n Ссылка для подключения")
    }, 3000)
}

$("#message").keyup(function (event) {
    if (event.keyCode === 13 && !event.shiftKey) {
        let messageEl = $(this);
        let message = messageEl.val();
        messageEl.val("");
        hubConnection.invoke("Send", message).then();
    }
});