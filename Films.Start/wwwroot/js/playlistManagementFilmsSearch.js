let playlistFilmSearchForm
let playlistFilmSearchInput
let playlistSearchBlock
let playlistFilmsBlock
let threadId = null;

$(document).ready(() => {
    playlistFilmSearchForm = $('#playlist-films-search-form');
    playlistFilmSearchInput = $('#playlist-films-search-form :input')
    playlistSearchBlock = $('.playlist-films-search-block')
    playlistFilmsBlock = $('#films')

    $(".playlist-film-delete").click(deleteFilm)

    playlistFilmSearchForm.on('propertychange input', () => {
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

})

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
            ev.preventDefault()
            try {
                AddToPlaylist(el)
            } catch (Error) {
                a.addClass("invalid")
                return
            }
            ev.currentTarget.remove()
        })
    })
}

function AddToPlaylist(film) {

    let inputs = playlistFilmsBlock.find("input[name^='Films'][name$='.Id']");
    let find
    inputs.each(function() {
        if($(this).attr("value") === film.id){
            find = true
        }
    })
    
    if (find) throw Error("Film already in playlist")

    let counter = parseInt(playlistFilmsBlock.attr('counter')) + 1;
    playlistFilmsBlock.attr('counter', counter)
    
    let divCol = jQuery('<div>', {
        class: 'col-12',
    })
    
    let divEl = jQuery('<div>', {
        class: 'playlist-film-element',
    })
    
    let a = jQuery('<a>', {
        class: 'playlist-film-delete',
        href: "#"
    })

    a.append('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">' +
        '  <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"></path>' +
        '</svg>')
    
    divEl.append('<img src="/'+ film.posterUri + '" alt="">' +
        '<div class="playlist-film-text">' +
        '<div>' + film.name + '</div>' +
        '<div class="playlist-film-description">' + film.description + '</div>' +
        '</div>')
    
    divEl.append(a)
    
    divCol.append(divEl)
    
    divCol.append('<input type="hidden" name="Films[' + counter + '].Id" value="' + film.id + '"/>' +
        '<input type="hidden" name="Films[' + counter + '].Name" value="' + film.name + '"/>' +
        '<input type="hidden" name="Films[' + counter + '].Description" value="' + film.description + '"/>' +
        '<input type="hidden" name="Films[' + counter + '].Uri" value="' + film.posterUri + '"/>')
    
    a.click(deleteFilm)
    
    playlistFilmsBlock.append(divCol);
}

function deleteFilm(ev){
    ev.currentTarget.parentElement.parentElement.remove()
    return false
}