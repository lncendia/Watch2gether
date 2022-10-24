let div = $("#chatScroll");
div.ready(() => div.scrollTop(div.prop('scrollHeight')));

class User {
    constructor(id, name, avatar, time, onPause) {
        this.name = name;
        this.onPause = onPause;
        this.time = time;
        this.id = id;
        this.avatar = avatar;
    }

    updateTime(time) {
        this.time = time;
        $("#" + this.id).children(".time").html(getTimeString(this.time));
    }

    setPause(pause) {
        this.onPause = pause;
        $("#" + this.id).children(".pause").toggleClass("d-none", !this.onPause);
    }
    
    init(isAdmin){
        
    }
}

class Data {
    constructor(id, ownerId) {
        this.id = id;
        this.ownerId = ownerId;
    }
}

let users = [];
let data;

function onConnect(user, addUser) {
    if (users.some(x => x.id === user.id)) return;
    console.log(user);
    users.push(user);
    addUser(user);
    updateUsersCount();
    showNotify("#8eb969", user.name + " подключился к комнате");
}

function onDisconnect(id) {
    let user = users.find(x => x.id === id);
    if (user == null) return;
    users.splice(users.indexOf(user), 1);
    removeUserFromList(user);
    updateUsersCount();
    showNotify("#813232", user.name + " покинул комнату");
}

function onPauseToggled(id, pause, time) {
    let user = users.find(x => x.id === id);
    if (user == null) return;
    user.setPause(pause);
    user.updateTime(time);
}

function onTimeUpdated(id, time) {
    let user = users.find(x => x.id === id);
    if (user == null) return;
    user.updateTime(time);
}

function onMessageReceived(id, message) {
    let user = users.find(x => x.id === id);
    if (user == null) return;
    showMessage(user, message);
}

function onError(error) {
    showNotify("#ce2e2e", "Ошибка: " + error);
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
        if (user.onPause === false) user.updateTime(user.time + (delta / 1000));
    });
}, 1000);

function updateUsersCount() {
    $("#countViewers").html("В сети: " + users.length);
}

function removeUserFromList(user) {
    $("#" + user.id).remove();
}

function showMessage(user, message) {
    let html;
    if (user.id === data.id) html = '<div class="d-flex flex-row justify-content-end mb-4"><div class="message messageMe"><p style="font-size: 12px;" class="text-end additionalText">' + user.name + '</p><p class="small mb-0">' + message + '</p><p style="font-size: 10px" class="additionalText">' + new Date().toLocaleTimeString() + '</p></div><img src="/img/Avatars/' + user.avatar + '" class="messageAvatar" alt=""></div>';
    else html = '<div class="d-flex flex-row justify-content-start mb-4"><img src="/img/Avatars/' + user.avatar + '" class="messageAvatar" alt=""><div class="message messageOther"><p style="font-size: 12px" class="additionalText">' + user.name + '</p><p class="small mb-0">' + message + '</p><p style="font-size: 10px;" class="text-end additionalText">' + new Date().toLocaleTimeString() + '</p></div></div>';
    div.append(html);
    div.scrollTop(div.prop('scrollHeight'));
}

function showNotify(color, text) {
    let id = Math.random().toString(36).substring(7);
    div.append('<div id="' + id + '" class="text-center mb-4"><div style="background-color: ' + color + '" class="border notify"><p class="mb-0">' + text + '</p></div></div>');
    div.scrollTop(div.prop('scrollHeight'));
    setTimeout(function () {
        $('#' + id).hide('slow', function () {
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
    el.html('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-clipboard-check" viewBox="0 0 16 16">\n  <path fill-rule="evenodd" d="M10.854 7.146a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 1 1 .708-.708L7.5 9.793l2.646-2.647a.5.5 0 0 1 .708 0z"/>\n  <path d="M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z"/>\n  <path d="M9.5 1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5h3zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z"/></svg> Ссылка скопирована');
    setTimeout(function () {
        el.html('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-clipboard" viewBox="0 0 16 16">\n  <path d="M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z"/>\n  <path d="M9.5 1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5h3zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z"/></svg> Ссылка для подключения');
    }, 3000)
}