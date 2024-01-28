function onConnectReceive(room, event) {
    if (room.Users.some(x => x.Id === event.Id)) {
        if(room.CurrentId === event.Id) showNotify("#813232", "Просмотр с двух вкладок может приветси к ошибкам");
    }
    else {
        let user = new YoutubeUser(event.Id, event.Username, event.Avatar, event.Time, true, false, event.Beep, event.Scream, event.Change, event.VideoId)
        room.Users.push(user)
        updateOnline(room)
        showUser(room, user)
        initActions(room, user)
        initYoutubeActions(room, user)
        showNotify("#8eb969", event.Username + " подключился к комнате");
    }
    if (room.CurrentId === event.Id) room.ProcessUserEvent(new LoadUserEvent())
}

function onPauseReceive(room, event) {
    if(event.Id === room.CurrentId) return
    let user = room.Users.find(x => x.Id === event.Id);
    if (user == null) return;
    user.Pause = event.Pause;
    user.Second = event.Second;
    if (user.Id === room.OwnerId) Sync(user.Second, user.Pause)
    updateInfo(user)
    updateTime(user)
}

function onSeekReceive(room, event) {
    if(event.Id === room.CurrentId) return
    let user = room.Users.find(x => x.Id === event.Id);
    if (user == null) return;
    user.Second = event.Second;
    if (user.Id === room.OwnerId) Sync(user.Second, user.Pause)
    updateTime(user)
}