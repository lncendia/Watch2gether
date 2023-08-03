function onConnectReceive(room, event) {
    if (room.Users.some(x => x.Id === event.Id)) return;
    let user = new FilmUser(event.Id, event.Username, event.Avatar, event.Time, true, false, event.Beep, event.Scream, event.Change, event.Season, event.Series)
    room.Users.push(user)
    $("#countViewers").html("В сети: " + room.Users.length);
    showUser(room, user)
    initActions(room, user)
    showNotify("#8eb969", event.Username + " подключился к комнате");
    if (room.CurrentId === event.Id) room.ProcessUserEvent(new LoadUserEvent())
}

function onPauseReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id);
    if (user == null) return;
    user.Pause = event.Pause;
    user.Second = event.Second;
    if (user.Id === room.OwnerId) Sync(user.Second, user.Pause)
    let userEl = $("#" + user.Id);
    userEl.find(".pause").toggleClass("d-none", !user.Pause);
    userEl.find('.time-block').html(getTimeString(user.Second))
}

function onSeekReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id);
    if (user == null) return;
    user.Second = event.Second;
    if (user.Id === room.OwnerId) Sync(user.Second, user.Pause)
    $("#" + user.Id).find('.time-block').html(getTimeString(user.Second))
}

function onSeriesChangedReceive(room, event) {
    let user = room.Users.find(x => x.Id === event.Id);
    if (user === null) return;
    user.Season = event.Season
    user.Series = event.Series
    if (user.Id === room.OwnerId) Sync(user.Second, user.Pause, user.Season + '_' + user.Series)
}