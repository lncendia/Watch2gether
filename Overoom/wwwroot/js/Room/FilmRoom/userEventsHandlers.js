function onLoadUser(room) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user == null) return
    initPlayer(room, user)
    initTimer(room)
    for (let i = 0; i < room.Users.length; i++) {
        initActions(room, room.Users[i])
    }
    $('#message').keyup((event) => messageKeyUp(room, event))
    let div = $(".chat-scroll")
    div.ready(() => div.scrollTop(div.prop('scrollHeight')))
}

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

function onSeriesChangedUser(room, event) {
    let user = room.Users.find(x => x.Id === room.CurrentId)
    if (user === null) return
    user.Season = event.Season
    user.Series = event.Series
    hubConnection.invoke('ChangeSeries', event.Season, event.Series).then()
}