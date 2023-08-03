function leave(room, user) {
    room.Users.splice(room.Users.indexOf(user), 1);
    $("#" + user.Id).remove();
    $("#countViewers").html("В сети: " + room.Users.length);
}


function getTimeString(time) {
    let hours = Math.floor(time / 3600);
    let minutes = Math.floor((time - (hours * 3600)) / 60);
    let seconds = Math.floor(time - (hours * 3600) - (minutes * 60));
    if (hours < 10) hours = "0" + hours;
    if (minutes < 10) minutes = "0" + minutes;
    if (seconds < 10) seconds = "0" + seconds;
    return hours + ":" + minutes + ":" + seconds;
}


let div = $(".chat-scroll");

function showMessage(user, owner, message) {
    let html;
    if (owner) html = '<div class="d-flex flex-row justify-content-end mb-4"><div class="message message-me"><p style="font-size: 12px;" class="text-end additional-text">' + user.Name + '</p><p class="small text-break mb-0">' + message + '</p><p style="font-size: 10px" class="additional-text">' + new Date().toLocaleTimeString() + '</p></div><img src="/' + user.Avatar + '" class="message-avatar" alt=""></div>';
    else html = '<div class="d-flex flex-row justify-content-start mb-4"><img src="/' + user.Avatar + '" class="message-avatar" alt=""><div class="message message-other"><p style="font-size: 12px" class="additional-text">' + user.Name + '</p><p class="small text-break mb-0">' + message + '</p><p style="font-size: 10px;" class="text-end additional-text">' + new Date().toLocaleTimeString() + '</p></div></div>';
    div.append(html);
    div.scrollTop(div.prop('scrollHeight'));
}

function showNotify(color, text) {
    let id = Math.random().toString(36).substring(7);
    div.append('<div id="' + id + '" class="text-center mb-4"><div style="background-color: ' + color + '" class="border notify"><p class="mb-0">' + text + '</p></div></div>');
    div.scrollTop(div.prop('scrollHeight'));
    setTimeout(function () {
        $('#' + id).hide('slow', function () {
            $(this).remove();
        });
    }, 5000);
}

function showType(user) {
    let type = $("#" + user.Id).find('.type')
    type.removeClass('d-none');
    setTimeout(() => {
        type.addClass('opacity-75')
    }, 500)
    setTimeout(() => {
        type.removeClass('opacity-75')
        type.addClass('opacity-50')
    }, 1000)
    setTimeout(() => {
        type.removeClass('opacity-50')
        type.addClass('opacity-75')
    }, 1500)
    setTimeout(() => {
        type.removeClass('opacity-75')
    }, 2000)
    setTimeout(() => {
        type.addClass('opacity-75')
    }, 2500)
    setTimeout(() => {
        type.removeClass('opacity-75')
        type.addClass('opacity-50')
    }, 3000)
    setTimeout(() => {
        type.removeClass('opacity-50')
        type.addClass('opacity-75')
    }, 3500)
    setTimeout(() => {
        type.removeClass('opacity-75')
        type.addClass('d-none');
    }, 4000)
}

function initTimer(room) {
    let start = Date.now()
    setInterval(function () {
        let delta = Date.now() - start
        start = Date.now()
        room.Users.forEach((user) => {
            if (user.Pause === false) {
                user.Second = (user.Second + (delta / 1000))
                $("#" + user.Id).find('.time-block').html(getTimeString(user.Second))
            }
        })
    }, 1000)
}

function messageKeyUp(room, event) {
    if (event.keyCode === 13 && !event.shiftKey) {
        let messageEl = $('#message');
        let message = messageEl.val()
        messageEl.val('')
        room.ProcessUserEvent(new MessageUserEvent(message.substring(0, message.length - 1)))
    } else {
        room.ProcessUserEvent(new TypeUserEvent())
    }
}


function initActions(room, user) {
    $('.beep-button[action-target="' + user.Id + '"]').click(e => {
        let target = parseInt($(e.currentTarget).attr('action-target'))
        room.ProcessUserEvent(new BeepUserEvent(target))
        return false;
    })
    $('.scream-button[action-target="' + user.Id + '"]').click(e => {
        let target = parseInt($(e.currentTarget).attr('action-target'))
        room.ProcessUserEvent(new ScreamUserEvent(target))
        return false;
    })
    $('.kick-button[action-target="' + user.Id + '"]').click(e => {
        let target = parseInt($(e.currentTarget).attr('action-target'))
        room.ProcessUserEvent(new KickUserEvent(target))
        return false;
    })
    $('.change-button[action-target="' + user.Id + '"]').click(e => {
        let target = parseInt($(e.currentTarget).attr('action-target'))
        $('.change-block[change-target="' + target + '"]').toggleClass('d-none')
        return false;
    })
    if (room.CurrentId === user.Id)
        $('.sync-button').click(() => {
            room.ProcessUserEvent(new SyncUserEvent())
            return false;
        })
}


function withName(html, room, user) {
    html +=
        '<div class="name-block viewer-block">' +
        '    ' + user.Name +
        '</div>'
    return html;
}

function withTime(html, room, user) {
    html +=
        '<div class="time-block viewer-block">' +
        '    ' + getTimeString(user.Second) +
        '</div>'
    return html;
}

function withInfo(html, room, user) {
    html +=
        '<div class="info-block viewer-block">'

    if (user.Id === room.OwnerId) html +=
        '   <div class="d-inline-block">' +
        '       <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-patch-check-fill" viewBox="0 0 16 16">' +
        '           <path d="M10.067.87a2.89 2.89 0 0 0-4.134 0l-.622.638-.89-.011a2.89 2.89 0 0 0-2.924 2.924l.01.89-.636.622a2.89 2.89 0 0 0 0 4.134l.637.622-.011.89a2.89 2.89 0 0 0 2.924 2.924l.89-.01.622.636a2.89 2.89 0 0 0 4.134 0l.622-.637.89.011a2.89 2.89 0 0 0 2.924-2.924l-.01-.89.636-.622a2.89 2.89 0 0 0 0-4.134l-.637-.622.011-.89a2.89 2.89 0 0 0-2.924-2.924l-.89.01-.622-.636zm.287 5.984-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 1 1 .708-.708L7 8.793l2.646-2.647a.5.5 0 0 1 .708.708z"/>' +
        '       </svg>' +
        '   </div>'
    html +=
        '    <div class="d-inline-block d-none type">' +
        '        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-chat-dots-fill" viewBox="0 0 16 16">' +
        '            <path d="M16 8c0 3.866-3.582 7-8 7a9.06 9.06 0 0 1-2.347-.306c-.584.296-1.925.864-4.181 1.234-.2.032-.352-.176-.273-.362.354-.836.674-1.95.77-2.966C.744 11.37 0 9.76 0 8c0-3.866 3.582-7 8-7s8 3.134 8 7zM5 8a1 1 0 1 0-2 0 1 1 0 0 0 2 0zm4 0a1 1 0 1 0-2 0 1 1 0 0 0 2 0zm3 1a1 1 0 1 0 0-2 1 1 0 0 0 0 2z"/>' +
        '        </svg>' +
        '    </div>' +
        '    <div class="d-inline-block pause">' +
        '        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pause-circle-fill" viewBox="0 0 16 16">' +
        '            <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM6.25 5C5.56 5 5 5.56 5 6.25v3.5a1.25 1.25 0 1 0 2.5 0v-3.5C7.5 5.56 6.94 5 6.25 5zm3.5 0c-.69 0-1.25.56-1.25 1.25v3.5a1.25 1.25 0 1 0 2.5 0v-3.5C11 5.56 10.44 5 9.75 5z"/>' +
        '        </svg>' +
        '    </div>' +
        '    <div class="d-inline-block fullscreen-off">' +
        '        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-fullscreen-exit" viewBox="0 0 16 16">' +
        '            <path d="M5.5 0a.5.5 0 0 1 .5.5v4A1.5 1.5 0 0 1 4.5 6h-4a.5.5 0 0 1 0-1h4a.5.5 0 0 0 .5-.5v-4a.5.5 0 0 1 .5-.5zm5 0a.5.5 0 0 1 .5.5v4a.5.5 0 0 0 .5.5h4a.5.5 0 0 1 0 1h-4A1.5 1.5 0 0 1 10 4.5v-4a.5.5 0 0 1 .5-.5zM0 10.5a.5.5 0 0 1 .5-.5h4A1.5 1.5 0 0 1 6 11.5v4a.5.5 0 0 1-1 0v-4a.5.5 0 0 0-.5-.5h-4a.5.5 0 0 1-.5-.5zm10 1a1.5 1.5 0 0 1 1.5-1.5h4a.5.5 0 0 1 0 1h-4a.5.5 0 0 0-.5.5v4a.5.5 0 0 1-1 0v-4z"/>' +
        '        </svg>' +
        '    </div>' +
        '    <div class="d-inline-block fullscreen-on d-none">' +
        '        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-fullscreen" viewBox="0 0 16 16">' +
        '            <path d="M1.5 1a.5.5 0 0 0-.5.5v4a.5.5 0 0 1-1 0v-4A1.5 1.5 0 0 1 1.5 0h4a.5.5 0 0 1 0 1h-4zM10 .5a.5.5 0 0 1 .5-.5h4A1.5 1.5 0 0 1 16 1.5v4a.5.5 0 0 1-1 0v-4a.5.5 0 0 0-.5-.5h-4a.5.5 0 0 1-.5-.5zM.5 10a.5.5 0 0 1 .5.5v4a.5.5 0 0 0 .5.5h4a.5.5 0 0 1 0 1h-4A1.5 1.5 0 0 1 0 14.5v-4a.5.5 0 0 1 .5-.5zm15 0a.5.5 0 0 1 .5.5v4a1.5 1.5 0 0 1-1.5 1.5h-4a.5.5 0 0 1 0-1h4a.5.5 0 0 0 .5-.5v-4a.5.5 0 0 1 .5-.5z"/>' +
        '        </svg>' +
        '    </div>' +
        '</div>'
    return html;
}


function withActions(html, room, user) {
    if (room.CurrentId !== user.Id) {
        html +=
            '<div class="actions-block viewer-block">'
        if (room.CurrentId === room.OwnerId) html +=
            '    <a href="#" action-target="' + user.Id + '" class="kick-button">' +
            '        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-person-fill-dash" viewBox="0 0 16 16">' +
            '            <path d="M12.5 16a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7ZM11 12h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1 0-1Zm0-7a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"/>' +
            '            <path d="M2 13c0 1 1 1 1 1h5.256A4.493 4.493 0 0 1 8 12.5a4.49 4.49 0 0 1 1.544-3.393C9.077 9.038 8.564 9 8 9c-5 0-6 3-6 4Z"/>' +
            '        </svg>' +
            '    </a>'
        if (user.AllowChange) html +=
            '    <a href="#" action-target="' + user.Id + '" class="change-button">' +
            '        <div class="d-inline-block">' +
            '            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-tag-fill" viewBox="0 0 16 16">' +
            '                <path d="M2 1a1 1 0 0 0-1 1v4.586a1 1 0 0 0 .293.707l7 7a1 1 0 0 0 1.414 0l4.586-4.586a1 1 0 0 0 0-1.414l-7-7A1 1 0 0 0 6.586 1H2zm4 3.5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0z"/>' +
            '            </svg>' +
            '        </div>' +
            '    </a>'
        if (user.AllowBeep) html +=
            '    <a href="#" action-target="' + user.Id + '" class="beep-button">' +
            '        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-music-note-beamed" viewBox="0 0 16 16">' +
            '            <path d="M6 13c0 1.105-1.12 2-2.5 2S1 14.105 1 13c0-1.104 1.12-2 2.5-2s2.5.896 2.5 2zm9-2c0 1.105-1.12 2-2.5 2s-2.5-.895-2.5-2 1.12-2 2.5-2 2.5.895 2.5 2z"/>' +
            '            <path fill-rule="evenodd" d="M14 11V2h1v9h-1zM6 3v10H5V3h1z"/>' +
            '            <path d="M5 2.905a1 1 0 0 1 .9-.995l8-.8a1 1 0 0 1 1.1.995V3L5 4V2.905z"/>' +
            '        </svg>' +
            '    </a>'
        if (user.AllowScream) html +=
            '    <a href="#" action-target="' + user.Id + '" class="scream-button">' +
            '        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-speaker-fill" viewBox="0 0 16 16">' +
            '            <path d="M9 4a1 1 0 1 1-2 0 1 1 0 0 1 2 0zm-2.5 6.5a1.5 1.5 0 1 1 3 0 1.5 1.5 0 0 1-3 0z"/>' +
            '            <path d="M4 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H4zm6 4a2 2 0 1 1-4 0 2 2 0 0 1 4 0zM8 7a3.5 3.5 0 1 1 0 7 3.5 3.5 0 0 1 0-7z"/>' +
            '        </svg>' +
            '    </a>'
        html += '</div>'
    } else if (room.CurrentId !== room.OwnerId) {
        html +=
            '<div class="actions-block viewer-block">' +
            '     <a href="#" class="sync-button">' +
            '         <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-repeat" viewBox="0 0 16 16">' +
            '             <path d="M11.534 7h3.932a.25.25 0 0 1 .192.41l-1.966 2.36a.25.25 0 0 1-.384 0l-1.966-2.36a.25.25 0 0 1 .192-.41zm-11 2h3.932a.25.25 0 0 0 .192-.41L2.692 6.23a.25.25 0 0 0-.384 0L.342 8.59A.25.25 0 0 0 .534 9z"/>' +
            '             <path fill-rule="evenodd" d="M8 3c-1.552 0-2.94.707-3.857 1.818a.5.5 0 1 1-.771-.636A6.002 6.002 0 0 1 13.917 7H12.9A5.002 5.002 0 0 0 8 3zM3.1 9a5.002 5.002 0 0 0 8.757 2.182.5.5 0 1 1 .771.636A6.002 6.002 0 0 1 2.083 9H3.1z"/>' +
            '         </svg>' +
            '     </a>' +
            '</div>'
    }

    return html;
}