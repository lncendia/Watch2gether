let filmSearchForm = $('#films-search-form');
let filmSearchInput = $('#films-search-form :input')
let searchBlock = $('.films-search-block')
let anyFilms = false
let thread = null;

filmSearchForm.on('propertychange input', e => {
    clearTimeout(thread);
    let val = filmSearchInput.val()
    if (val === '') {
        anyFilms = false
        searchBlock.slideUp(500)
        return
    }
    thread = setTimeout(async () => {
        let url = filmSearchForm.attr('action')
        let data = await fetch(url + '?' + 'Query=' + val);
        if (data.status === 200) {
            anyFilms = true
            searchBlock.slideDown(500)
            Parse(await data.json())
        } else {
            anyFilms = false
            searchBlock.slideUp(500)
        }
    }, 500)
})

filmSearchInput.on('blur', e => {
    searchBlock.slideUp(500)
})

filmSearchInput.on('focus', e => {
    if (anyFilms) searchBlock.slideDown(500)
})

filmSearchForm.submit(e => e.preventDefault())

function Parse(films) {
    searchBlock.empty()
    films.forEach(el => {
        let html = '<a class="film-search-element" href="/Film?id=' + el.id + '"><img src="/' + el.posterUri + '" alt=""><div class="film-search-text"><div>' + el.name + '</div><div class="film-search-description">' + el.description + '</div></div></a>'
        searchBlock.append(html);
    })
}