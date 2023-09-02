function initYoutubeActions(room, user) {

    if (room.CurrentId === user.Id && (room.Access || room.OwnerId === user.Id)) {

        let block = $('.add-block')
        let input = block.find('input')
        let span = block.find('span')
        let button = block.find('button')

        input.on('input', () => {
            let val = input.val()
            if (/(.)*youtu(?:be\.com\/watch\?v=|\.be\/)([\w\-_]*)(&(amp;)?U+200B[\w?U+200B=]*)?/.test(val) || val.length === 0) {
                span.addClass('d-none')
                input.removeClass('input-validation-error')
            } else {
                span.removeClass('d-none')
                input.addClass('input-validation-error')
            }
        })
        button.click(() => {
            let val = input.val()
            if (!/(.)*youtu(?:be\.com\/watch\?v=|\.be\/)([\w\-_]*)(&(amp;)?U+200B[\w?U+200B=]*)?/.test(val)) return;
            room.ProcessUserEvent(new AddVideoUserEvent(val))
            input.val('')
            block.addClass('d-none')
        })

        $('.add-button').click(e => {
            block.toggleClass('d-none')
            return false;
        })

    }
}

function addVideo() {
    
}

function showUser(room, user) {
    let html = '<div id="' + user.Id + '" class="viewer d-flex justify-content-center flex-wrap">'
    html = withName(html, room, user)
    html = withActions(html, room, user)
    html = withTime(html, room, user)
    html = withInfo(html, room, user)
    html += '</div>'
    $("#viewers").append(html);
}

let player

function Sync(time, pause, videoId) {
    pause ? player.pauseVideo() : player.playVideo();
    player.seekTo(time, true);
}


function initPlayer(room) {
    let ids = ''
    for (let i = 0; i < room.Ids.length; i++) {
        ids += room.Ids[i] + ','
    }
    window.YT.ready(() => {
        player = new window.YT.Player('player', {
            height: '100%',
            width: '100%',
            playerVars: {
                playlist: ids,
                loop: 1,
            },
            events: {
                onStateChange: (event) => onStateChange(room, event),
                onReady: (event) => onReady(room, event)
            }
        });
    })
}


function getTime(){
    return Math.round(player.getCurrentTime())
}

function onStateChange(room, event) {
    if (event.data === 1) {
        room.ProcessUserEvent(new PauseUserEvent(false, getTime()))
    } else if (event.data === 2) {
        room.ProcessUserEvent(new PauseUserEvent(true, getTime()))
    } else if (event.data === 3) {
        room.ProcessUserEvent(new SeekUserEvent(getTime()))
    }
}

function onReady(room, event) {
    event.target.addEventListener('onFullscreenChange', function (event) {
        room.ProcessUserEvent(new FullScreenUserEvent(event.data.fullscreen))
    });
}