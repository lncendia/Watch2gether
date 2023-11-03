let playlistFilmSearchForm = $('#playlist-films-search-form');
let playlistFilmSearchInput = $('#playlist-films-search-form :input')
let playlistSearchBlock = $('.playlist-films-search-block')
let playlistFilmsBlock = $('#films')
let threadId = null;

playlistFilmSearchForm.on('propertychange input', e => {
    clearTimeout(threadId);
    let val = playlistFilmSearchInput.val()
    if (val === '') {
        playlistSearchBlock.empty()
        return
    }
    threadId = setTimeout(async () => {
        let url = playlistFilmSearchForm.attr('action')
        let data = await fetch(url + '?' + 'Query=' + val);
        if (data.status === 200) {
            Parse(await data.json())
        } else {
        }
    }, 500)
})

playlistFilmSearchForm.submit(e => e.preventDefault())

function Parse(films) {
    playlistSearchBlock.empty()
    films.forEach(el => {
        let a = jQuery('<a>', {
            class: 'playlist-film-search-element',
            href: '#'
        })
        let html = '<img src="/' + el.posterUri + '" alt=""><div class="playlist-film-search-text"><div>' + el.name + '</div><div class="playlist-film-search-description">' + el.description + '</div></div>'
        a.html(html)
        playlistSearchBlock.append(a);
        a.click(ev => {
            AddToPlaylist(el)
            ev.preventDefault()
            ev.currentTarget.remove()
        })
    })
}

function AddToPlaylist(film) {

    let counter = parseInt(playlistFilmsBlock.attr('counter')) + 1;
    playlistFilmsBlock.attr('counter', counter)
    let html = '         <div class="col-12">' +
        '<div class="playlist-film-element">' +
        '<img src="/'+ film.posterUri + '" alt="">' +
        '<div class="playlist-film-text">' +
        '<div>' + film.name + '</div>' +
        '<div class="playlist-film-description">' + film.description + '</div>' +
        '</div>' +
        '</div>' +
        '<input type="hidden" name="Films[' + counter + '].Id" value="' + film.id + '"/>' +
        '<input type="hidden" name="Films[' + counter + '].Name" value="' + film.name + '"/>' +
        '<input type="hidden" name="Films[' + counter + '].Description" value="' + film.description + '"/>' +
        '<input type="hidden" name="Films[' + counter + '].Uri" value="' + film.posterUri + '"/>' +
        '</div>'
    playlistFilmsBlock.append(html);
}