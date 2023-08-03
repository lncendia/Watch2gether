function showUser(room, user) {
    let html = '<div id="' + user.Id + '" class="viewer d-flex justify-content-center flex-wrap">'
    html = withName(html, room, user)
    html = withTime(html, room, user)
    html = withInfo(html, room, user)
    html = withActions(html, room, user)
    html = withSerial(html, room, user)
    html += '</div>'
    $("#viewers").append(html);
}

function withSerial(html, room, user) {
    if (room.Type === 'Serial') html +=
        '<div class="serial-block viewer-block">' +
        '    Сезон ' + user.Season + ', серия ' + user.Series +
        '</div>'
    return html;
}


function Sync(time, pause, fileId) {
    let frame = $('#frame')[0];
    if (fileId !== null) frame.contentWindow.postMessage({'api': 'find', 'set': fileId}, '*');
    let message = pause ? 'pause' : 'play';
    frame.contentWindow.postMessage({'api': message}, '*');
    frame.contentWindow.postMessage({'api': 'seek', 'set': time}, '*');
}

function initPlayer(room, user) {
    let player = $("#player")
    let url = room.Url
    if (room.Type === 'Serial') url += '?episode=' + user.Series + '&season=' + user.Season
    player.html('<iframe id="frame" src="' + url + '" allowfullscreen></iframe>')
    $("#frame").ready(() => eventHandler(room))
}


function eventHandler(room) {
    window.addEventListener('message', function (event) {
        // let types = ['resumed', 'pause', 'paused', 'seek', 'play', 'buffered', 'buffering']
        // for (let i = 0; i < types.length; i++) {
        //     if (types[i] === event.data.event) console.log(event.data.event)
        // }
        if ((event.data.event === 'play' || event.data.event === 'buffered') && event.data.time != null) {
            let time = parseInt(event.data.time);
            room.ProcessUserEvent(new PauseUserEvent(false, time))
        } else if ((event.data.event === 'pause' || event.data.event === 'buffering') && event.data.time != null) {
            let time = parseInt(event.data.time);
            room.ProcessUserEvent(new PauseUserEvent(true, time))
        } else if (event.data.event === 'seek' && event.data.time != null) {
            let time = parseInt(event.data.time);
            room.ProcessUserEvent(new SeekUserEvent(time))
        } else if (event.data.event === 'new' && event.data.id != null) {
            let data = event.data.id.split('_')
            let season = parseInt(data[0]);
            let series = parseInt(data[1]);
            room.ProcessUserEvent(new ChangeSeriesUserEvent(season, series))
        } else if (event.data.event === 'fullscreen') {
            room.ProcessUserEvent(new FullScreenUserEvent(true))
        } else if (event.data.event === 'exitfullscreen') {
            room.ProcessUserEvent(new FullScreenUserEvent(false))
        }
    })
}