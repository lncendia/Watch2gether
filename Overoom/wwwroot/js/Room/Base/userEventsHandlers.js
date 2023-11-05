function onSyncUser(room) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    let owner = room.Users.find(x => x.Id === room.OwnerId)
    if (owner == null) return
    user.Pause = owner.Pause;
    user.Second = owner.Second;
    Sync(owner.Second, owner.Pause)
    updateTime(user)
    showNotify("#606baf", 'Вы синхронизированы');
}

function onSeekUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    user.Second = event.Second
    updateTime(user)
    hubConnection.invoke('Seek', event.Second).then()
}

function onPauseUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    user.Pause = event.Pause
    user.Second = event.Second
    updateTime(user)
    updateInfo(user)
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

function onChangeUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null || user.Id !== room.OwnerId) return
    let target = room.Users.find(x => x.Id === event.Target)
    if (target == null || !target.AllowChange || user.Id === target.Id) return
    showNotify("#606baf", user.Name + " изменил имя " + target.Name);
    target.Name = event.Username
    updateName(target)
    hubConnection.invoke('Change', event.Target, event.Username).then()
}


function onFullScreenUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    user.FullScreen = event.FullScreen
    updateInfo(user)
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