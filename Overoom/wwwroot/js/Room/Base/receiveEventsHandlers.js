function onLeaveReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id);
    if (user == null) return;
    leave(room, user)
    showNotify("#813232", user.Name + " покинул комнату");
}

function onFullScreenReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id);
    if (user == null) return
    user.FullScreen = event.FullScreen
    let userEl = $("#" + user.Id)
    if (event.FullScreen) {
        userEl.find(".fullscreen-off").addClass('d-none')
        userEl.find(".fullscreen-on").removeClass('d-none')
    } else {
        userEl.find(".fullscreen-off").removeClass('d-none')
        userEl.find(".fullscreen-on").addClass('d-none')
    }
}

function onMessageReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id);
    if (user == null) return;
    showMessage(user, false, event.Text);
}

function onTypeReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id);
    if (user == null) return;
    showType(user)
}

function onBeepReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id)
    if (user == null) return
    let target = room.Users.find(x => x.Id === event.Target)
    if (target == null) return
    showNotify("#606baf", user.Name + " разбудил " + target.Name);
    if (room.CurrentId === event.Target) {
        let beep = $('#beep')[0];
        beep.currentTime = 0
        beep.volume = 0.1;
        beep.play()
    }
}

function onScreamReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id)
    if (user == null) return
    let target = room.Users.find(x => x.Id === event.Target)
    if (target == null) return
    showNotify("#606baf", user.Name + " напугал " + target.Name);
    let container = $('.container')
    let screamer = $('#scream')
    
    container.addClass('d-none')
    screamer.removeClass('d-none')
    screamer[0].volume = 0.3;
    screamer[0].play();
    setTimeout(() => {
        screamer.addClass('d-none')
        container.removeClass('d-none')
    }, 2000)
}

function onKickReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id)
    if (user == null) return
    let target = room.Users.find(x => x.Id === event.Target)
    if (target == null) return
    if (room.CurrentId === event.Target) {
        window.location.href = '/FilmRoom/Leave'
    } else {
        leave(room, target)
        showNotify("#606baf", user.Name + " выгнал " + target.Name);
    }
}

function onErrorReceive(room, event) {
    showNotify("#ce2e2e", "Ошибка: " + event.Text);
}

// $("#copyButton").click(function () {
//     copyButton($(this)).then();
//     return false;
// })
//
// async function copyButton(el) {
//     let copyText = el.attr("data-clipboard-text");
//     await navigator.clipboard.writeText(copyText);
//     el.html('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-clipboard-check" viewBox="0 0 16 16">\n  <path fill-rule="evenodd" d="M10.854 7.146a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 1 1 .708-.708L7.5 9.793l2.646-2.647a.5.5 0 0 1 .708 0z"/>\n  <path d="M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z"/>\n  <path d="M9.5 1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5h3zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z"/></svg> Ссылка скопирована');
//     setTimeout(function () {
//         el.html('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-clipboard" viewBox="0 0 16 16">\n  <path d="M4 1.5H3a2 2 0 0 0-2 2V14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V3.5a2 2 0 0 0-2-2h-1v1h1a1 1 0 0 1 1 1V14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V3.5a1 1 0 0 1 1-1h1v-1z"/>\n  <path d="M9.5 1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-3a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5h3zm-3-1A1.5 1.5 0 0 0 5 1.5v1A1.5 1.5 0 0 0 6.5 4h3A1.5 1.5 0 0 0 11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3z"/></svg> Ссылка для подключения');
//     }, 3000)
// }