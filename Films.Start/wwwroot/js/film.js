$('.watch').click(async e => {
    e.preventDefault()
    let link = $(e.target).attr('href')
    let res = await fetch(link)
    if (res.ok) {
        let block = $('#watchBlock')
        block.removeClass('d-none')
        let json = await res.json();
        block.html('<iframe src="' + json.uri + '" allowfullscreen></iframe>')
    }
})


$('.watchlist-button').click(async e => {
    e.preventDefault()
    let form = $('#watchlist');
    let data = new FormData(form[0]);
    let res = await fetch('/Film/Watchlist', {
        method: 'POST', body: data
    });
    if (res.ok) {
        let target = $('.watchlist-button')
        let inWatchlist = target.attr('in-watchlist').toLowerCase() === 'true'
        if (inWatchlist) {
            target.html('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-clock" viewBox="0 0 16 16"><path d="M8 3.5a.5.5 0 0 0-1 0V9a.5.5 0 0 0 .252.434l3.5 2a.5.5 0 0 0 .496-.868L8 8.71V3.5z"/><path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm7-8A7 7 0 1 1 1 8a7 7 0 0 1 14 0z"/></svg>')
        } else {
            target.html('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-clock-fill" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8 3.5a.5.5 0 0 0-1 0V9a.5.5 0 0 0 .252.434l3.5 2a.5.5 0 0 0 .496-.868L8 8.71V3.5z"/></svg>')
        }
        target.attr('in-watchlist', !inWatchlist)
    }
})

function ParseData(elements, model) {
    model.forEach(el => {
        ShowComment(el);
    })
}

let scroller = new Scroller('.comments', '#com', ParseData)
scroller.Start();

let addForm = $('#commentForm');
let deleteForm = $('#deleteComment');
addForm.submit(async (e) => {
    e.preventDefault()
    try {
        await AddComment();
        $("[name='Text']").val('')
    } catch (e) {
        console.log(e)
    }
    return false;
});


async function AddComment() {
    let data = new FormData(addForm[0]);
    let response = await fetch('/Film/AddComment', {
        method: 'POST', body: data
    });
    if (!response.ok) throw new Error(response.statusText);
    ShowComment(await response.json());
}

async function DeleteComment(e) {
    e.preventDefault()
    let a = $(e.currentTarget)
    let id = a.attr('data-id');
    let data = new FormData(deleteForm[0]);
    data.append('id', id)
    let response = await fetch('/Film/DeleteComment', {
        method: 'POST', body: data
    });
    if (!response.ok) throw new Error(response.statusText);
    a.parent().parent().parent().remove()
}

function ShowComment(el) {
    let text = '<div class="element"><div class="card comment-card mb-3"><div class="card-header"><span class="float-start">' + el.username + '</span><span class="float-end">' + el.createdAt + '</span></div><div class="card-body d-flex"><img src="/' + el.avatarUri + '" alt="" class="comment-avatar">' +
        '<p class="card-text">' + el.text + '</p>'

    if (el.isUserComment) {
        text += '<a href="/Film/DeleteComment" data-id="' + el.id + '">' +
            '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">\n' +
            '  <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>\n' +
            '</svg></a>'
    }
    text += '</div></div></div>';
    if (el.isUserComment) $('.comments').prepend(text); else $('.comments').append(text);
    $("[data-id=" + el.id + "]").click(DeleteComment)
}