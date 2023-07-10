$('#type').change(e => {
    if ($(e.target).val() === 'Serial') {
        $(".serial").removeClass('d-none')
    } else {
        $(".serial").addClass('d-none')
    }
})

$('.copy-button').click(CopyInput)
$('.copy-cdn').click(CopyCdn)

function CopyInput(e) {
    let el = $(e.target)
    let counter = parseInt(el.attr('counter')) + 1;
    el.attr('counter', counter)
    let attr = el.attr('copy-target').replace('[', '\\[').replace(']', '\\]')
    let name = el.attr('copy-name')
    let copyEl = $(attr)
    let html = copyEl[0].outerHTML
        .replace('id=\"' + attr.slice(1) + '\"', '')
        .replaceAll(name + '[0]', name + '[' + counter + ']')
        .replaceAll(/value="(\\\\.|[^"])+"/gu, 'value=""')
    copyEl.parent().append(html)
    return false
}

function CopyCdn(e) {
    let el = $(e.target)
    let counter = parseInt(el.attr('counter')) + 1;
    el.attr('counter', counter)
    let attr = el.attr('copy-target').replace('[', '\\[').replace(']', '\\]')
    let name = el.attr('copy-name')
    let copyEl = $(attr)
    let html = copyEl[0].outerHTML
        .replace('id=\"' + attr.slice(1) + '\"', '')
        .replaceAll(name + '[0]', name + '[' + counter + ']')
        .replaceAll('copyVoiceVideoCdn[0]', 'copyVoiceVideoCdn' + '[' + counter + ']')
        .replaceAll(/value="(\\\\.|[^"])+"/gu, 'value=""')
    copyEl.parent().append(html)
    let el1 = $("[copy-target='#copyVoiceVideoCdn\\[" + counter + "\\]']")
    el1.click(CopyInput)
    console.log(el1)
    return false
}

$('#titleSearch').click(async e => {
    e.preventDefault()
    let elem = $('#title')
    let name = elem.attr('name')
    let val = elem.val()
    if (val !== '') {
        let res = await fetch('/FilmLoad/GetFromTitle?' + name + '=' + val)
        if (res.ok) Fill(await res.json())
    }
})

$('#kpIdSearch').click(async e => {
    e.preventDefault()
    let elem = $('#kpId')
    let name = elem.attr('name')
    let val = elem.val()
    if (val !== '') {
        let res = await fetch('/FilmLoad/GetFromKpId?' + name + '=' + val)
        if (res.ok) Fill(await res.json())
    }
})

$('#imdbIdSearch').click(async e => {
        e.preventDefault()
        let elem = $('#imdbId')
        let name = elem.attr('name')
        let val = elem.val()
        if (val !== '') {
            let res = await fetch('/FilmLoad/GetFromImdb?' + name + '=' + val)
            if (res.ok) Fill(await res.json())
        }
    }
)

function Fill(model) {

    if (model.type === 'Serial') {
        $('#type').trigger('change')
    }

    $("[name='Name']").val(model.name)
    $("[name='Description']").val(model.description)
    $("[name='ShortDescription']").val(model.shortDescription)
    $("[name='Type']").val(model.type)
    $("[name='CountSeasons']").val(model.countSeasons)
    $("[name='CountEpisodes']").val(model.countEpisodes)
    $("[name='PosterUri']").val(model.posterUri)
    $("[name='Year']").val(model.year)
    $("[name='Rating']").val(model.rating)

    let addGenre = $('#addGenre');
    let addCountry = $('#addCountry');
    let addActor = $('#addActor');
    let addScreenwriter = $('#addScreenwriter');
    let addDirector = $('#addDirector');
    for (let i = parseInt(addGenre.attr('counter')) + 1; i < model.genres.length; i++) addGenre.trigger('click')
    for (let i = parseInt(addCountry.attr('counter')) + 1; i < model.countries.length; i++) addCountry.trigger('click')
    for (let i = parseInt(addActor.attr('counter')) + 1; i < model.actors.length; i++) addActor.trigger('click')
    for (let i = parseInt(addScreenwriter.attr('counter')) + 1; i < model.screenwriters.length; i++) addScreenwriter.trigger('click')
    for (let i = parseInt(addDirector.attr('counter')) + 1; i < model.directors.length; i++) addDirector.trigger('click')

    for (let i = 0; i < model.genres.length; i++) {
        $("[name='Genres[" + i + "].Name']").val(model.genres[i])
    }
    for (let i = 0; i < model.countries.length; i++) {
        $("[name='Countries[" + i + "].Name']").val(model.countries[i])
    }
    for (let i = 0; i < model.actors.length; i++) {
        $("[name='Actors[" + i + "].Name']").val(model.actors[i].name)
        $("[name='Actors[" + i + "].Description']").val(model.actors[i].description)
    }
    for (let i = 0; i < model.screenwriters.length; i++) {
        $("[name='Screenwriters[" + i + "].Name']").val(model.screenwriters[i])
    }
    for (let i = 0; i < model.directors.length; i++) {
        $("[name='Directors[" + i + "].Name']").val(model.directors[i])
    }
}

