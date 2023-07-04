let $data = new URLSearchParams();
let inverseOrder = true;
let data = $('#dataForm')
GetFormData();

let nextHandler = async function (pageIndex) {
    try {
        let data = await GetList(pageIndex + 1);
        return ParseData(data);
    } catch (e) {
        console.log(e);
        return false;
    }
}

let scroller = new InfiniteAjaxScroll('.films', {
    item: '.film', next: nextHandler, spinner: '.spinner', delay: 600
});

async function GetList(page) {
    $data.set('page', page)
    $data.set('inverseOrder', inverseOrder);
    let data = await fetch('/Film/FilmsList?' + $data.toString());
    if (data.status === 200) return await data.text();
    throw new Error(data.statusText);
}

let films = $('.films')

function ParseData(json) {
    let data = JSON.parse(json);
    data.films.forEach(el => {
        let html = '<div class="film col-xxl-3 col-lg-4 col-md-6 col-12"><div class="card filmCard"><div class="card-header filmCardTop"><div class="float-start">Рейтинг: ' + el.rating + '</div>';
        if (data.isAdmin) html += '<div class="float-end"><a href="#" data-title="' + el.name + '" class="nav-link d-inline" onclick="update(this)"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-clockwise" viewBox="0 0 16 16"><path fill-rule="evenodd" d="M8 3a5 5 0 1 0 4.546 2.914.5.5 0 0 1 .908-.417A6 6 0 1 1 8 2v1z"></path><path d="M8 4.466V.534a.25.25 0 0 1 .41-.192l2.36 1.966c.12.1.12.284 0 .384L8.41 4.658A.25.25 0 0 1 8 4.466z"></path></svg></a> <a href="#" data-id="' + el.id + '" class="nav-link d-inline" onclick="del(this);return false;"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-circle" viewBox="0 0 16 16"><path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/><path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/></svg></a></div>'
        html += '</div><a class="filmLink" href="/Film/Film?id=' + el.id + '"><img src="img/Posters/' + el.posterUri + '" class="card-img-top poster" alt="..."><div class="card-body"><h5 class="card-title">' + el.name + ' (' + el.year + ')' + '</h5><h6 class="card-subtitle mb-2 filmDescriptionList">' + el.description + '</h6><p class="card-text">' + el.genres + '</p></div></a><div class="card-footer filmCardBottom">' + el.type + '</div></div></div>';
        films.append(html);
    })
    return true;
}

function GetFormData() {
    let d = new FormData(data[0]);
    $data = new URLSearchParams();
    d.forEach((value, key) => {
        $data.set(key, value);
    })
}

data.on('change', Select);

async function Select() {
    scroller.pause();
    films.empty();

    GetFormData();
    scroller.pageIndex = -1;
    scroller.resume();
    await scroller.next();
}

$('#clearFilter').click(function () {
    let form = data[0];
    form.reset()
    form.dispatchEvent(new Event('change'))
    return false;
});


$("#sortOrder").click(function () {
    if (inverseOrder) {
        inverseOrder = false;
        $(this).children("svg:last").remove();
        $(this).append('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-up" viewBox="0 0 16 16"><path fill-rule="evenodd" d="M8 15a.5.5 0 0 0 .5-.5V2.707l3.146 3.147a.5.5 0 0 0 .708-.708l-4-4a.5.5 0 0 0-.708 0l-4 4a.5.5 0 1 0 .708.708L7.5 2.707V14.5a.5.5 0 0 0 .5.5z"/></svg>');
    } else {
        inverseOrder = true;
        $(this).children("svg:last").remove();
        $(this).append('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-down" viewBox="0 0 16 16"><path fill-rule="evenodd" d="M8 1a.5.5 0 0 1 .5.5v11.793l3.146-3.147a.5.5 0 0 1 .708.708l-4 4a.5.5 0 0 1-.708 0l-4-4a.5.5 0 0 1 .708-.708L7.5 13.293V1.5A.5.5 0 0 1 8 1z"/></svg>');
    }
    data[0].dispatchEvent(new Event('change'))
    return false;
});

async function del(el) {
    let id = $(el).attr('data-id');
    let res = await fetch('/Film/Delete?id=' + id, {method: 'DELETE'});
    if (res.ok) $(el).parent().parent().parent().parent().remove(); else alert('Ошибка при удалении');
}

function update(el) {
    let title = $(el).attr('data-title');
    window.open('/FilmDownloader?title=' + title, '_blank');
    return false;
}