function onConnectReceive(room, event) {
    if (room.Users.some(x => x.Id === event.Id)) {
        if(room.CurrentId === event.Id) showNotify("#813232", "Просмотр с двух вкладок может приветси к ошибкам");
        return;
    }
    let user = new FilmUser(event.Id, event.Username, event.Avatar, event.Time, true, false, event.Beep, event.Scream, event.Change, event.Season, event.Series)
    room.Users.push(user)
    updateOnline(room)
    showUser(room, user)
    initActions(room, user)
    showNotify("#8eb969", event.Username + " подключился к комнате");
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

function onSeriesChangedReceive(room, event) {
    if(event.Id === room.CurrentId) return
    let user = room.Users.find(x => x.Id === event.Id);
    if (user === null) return;
    user.Season = event.Season
    user.Series = event.Series
    if (user.Id === room.OwnerId) Sync(user.Second, user.Pause, user.Season + '_' + user.Series)
}