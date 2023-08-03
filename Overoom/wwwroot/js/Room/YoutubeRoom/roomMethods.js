function showUser(room, user) {
    let html = '<div id="' + user.Id + '" class="viewer d-flex justify-content-center flex-wrap">'
    html = withName(html, room, user)
    html = withTime(html, room, user)
    html = withInfo(html, room, user)
    html = withActions(html, room, user)
    html += '</div>'
    $("#viewers").append(html);
}

let player;

function Sync(time, pause, videoId) {
    pause ? player.pauseVideo() : player.playVideo();
    player.seekTo(time, true);
}


function initPlayer(room, user) {
    console.log(room.Ids)
    let ids =''
    for (let i = 0; i < room.Ids.length; i++) {
        ids += room.Ids[i] + ','
    }
    console.log(ids)
    window.YT.ready(() => {
        player = new window.YT.Player('player', {
            height: '100%',
            width: '100%',
            playerVars: {
                playlist: ids,
                loop: 1,
            },
            events: {
                onStateChange: (el) => {
                    console.log(el)
                    if (el.data === 1) {
                        room.ProcessUserEvent(new PauseUserEvent(false, player.getCurrentTime()))
                    } else if (el.data === 2) {
                        room.ProcessUserEvent(new PauseUserEvent(true, player.getCurrentTime()))
                    } else if (el.data === 3) {
                        room.ProcessUserEvent(new SeekUserEvent(player.getCurrentTime()))
                    }
                }
            }
        });
    })
}