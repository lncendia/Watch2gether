function onLoadUser(room) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    initPlayer(room, user)
    initTimer(room)
    initMessage(room)
    for (let i = 0; i < room.Users.length; i++) {
        initActions(room, room.Users[i])
        initYoutubeActions(room, room.Users[i])
    }
}

function onSyncUser(room) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    let owner = room.Users.find(x=>x.Id === room.OwnerId)
    if (owner == null) return
    Sync(user.Second, user.Pause)
    showNotify("#606baf", 'Вы синхронизированы');
}