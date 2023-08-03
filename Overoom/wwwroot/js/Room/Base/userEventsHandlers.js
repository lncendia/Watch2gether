function onSyncUser(room) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    let owner = room.Users.find(x=>x.Id === room.OwnerId)
    if (owner == null) return
    user.Pause = owner.Pause;
    user.Second = owner.Second;
    Sync(user.Second, user.Pause)
    $("#" + user.Id).find('.time-block').html(getTimeString(user.Second))
    showNotify("#606baf", 'Вы синхронизированы');
}

function onSeekUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    user.Second = event.Second
    $("#" + user.Id).find('.time-block').html(getTimeString(user.Second))
    hubConnection.invoke('Seek', event.Second).then()
}

function onPauseUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    user.Pause = event.Pause
    user.Second = event.Second
    $("#" + user.Id).find(".pause").toggleClass("d-none", !user.Pause)
    hubConnection.invoke('Pause', event.Pause, event.Second).then()
}


function onBeepUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    let target = room.Users.find(x => x.Id === event.Target)
    if (target == null || !target.AllowBeep || user.Id === target.Id) return
    showNotify("#606baf", user.Name + " разбудил " + target.Name);
    hubConnection.invoke('Beep', event.Target).then()
}

function onScreamUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    let target = room.Users.find(x => x.Id === event.Target)
    if (target == null || !target.AllowScream || user.Id === target.Id) return
    showNotify("#606baf", user.Name + " напугал " + target.Name);
    hubConnection.invoke('Scream', event.Target).then()
}


function onKickUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null || user.Id !== room.OwnerId) return
    let target = room.Users.find(x => x.Id === event.Target)
    if (target == null || user.Id === target.Id) return
    showNotify("#606baf", user.Name + " выгнал " + target.Name);
    leave(room, user)
    hubConnection.invoke('Kick', event.Target).then()
}


function onFullScreenUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
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
    hubConnection.invoke('FullScreen', event.FullScreen).then()
}

function onMessageUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    showMessage(user, true, event.Text)
    hubConnection.invoke('Send', event.Text).then()
}

let canType = true

function onTypeUser(room) {
    if (!canType) return
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    canType = false
    hubConnection.invoke('Type').then()
    showType(user)
    setTimeout(() => {
        canType = true
    }, 4000)
}