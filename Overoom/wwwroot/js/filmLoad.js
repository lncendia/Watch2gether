$('#type').change(e => {
    if ($(e.target).val() === 'Serial') {
        $(".serial").removeClass('d-none')
    } else {
        $(".serial").addClass('d-none')
    }
})

let insertValues = {
    genre: '<div class="col-12"><label>Название</label><input name="Genres[i].Name" class="form-control"/></div>',
    country: '<div class="col-12"><label>Название</label><input name="Countries[i].Name" class="form-control"/></div>',
    actor: '<div class="row g-2 pt-2"><div class="col-6"><label>Имя</label><input name="Actors[i].Name" class="form-control"/></div><div class="col-6"><label>Описание</label><input name="Actors[i].Description" class="form-control"/></div></div>',
    screenwriter: '<div class="col-12"><label>Имя</label><input name="Screenwriters[i].Name" class="form-control"/></div>',
    director: '<div class="col-12"><label>Имя</label><input name="Directors[i].Name" class="form-control"/></div>',
    cdn: '<div class="row g-2 block-2">\<div class="col-lg-6 col-12">\<label>Тип</label>\<select name="Cdns[i].Type" class="form-control"><option selected="selected" value="">Не выбрано</option><option value="Bazon">Bazon</option><option value="VideoCdn">VideoCDN</option></select></div>\<div class="col-lg-6 col-12"><label>Качество</label><input name="Cdns[i].Quality" class="form-control"/></div>\<div class="col-12"><label>Ссылка</label><input name="Cdns[i].Uri" class="form-control"/></div>\<div class="col-12">\<div class="d-flex justify-content-between align-items-center"><label>Озвучки</label><div><a href="#" counter="0" cdn-counter="i" class="btn manage-button btn-outline-primary copy-voice-cdn" add-target="voiceCdn[i]">+</a> <a href="#" class="btn manage-button btn-outline-danger delete-button" delete-target="voiceCdn[i]">-</a></div></div>\<div class="row g-2" id="voiceCdn[i]">\<div class="col-12"><label>Название</label><input name="Cdns[i].Voices[0].Name" class="form-control"/></div></div></div></div>',
    voiceCdn: '<div class="col-12"><label>Название</label><input name="Cdns[i].Voices[j].Name" class="form-control"/></div>'
};

$('.copy-button').click(CopyInput)
$('.copy-cdn').click(CopyCdn)
$('.copy-voice-cdn').click(CopyVoiceCdn)
$('.delete-button').click(Delete)

function CopyInput(e) {
    let el = $(e.target)
    let counter = parseInt(el.attr('counter')) + 1;
    el.attr('counter', counter)
    let target = el.attr('add-target')
    $('#' + target).append(insertValues[target].replaceAll('[i]', '[' + counter + ']'))
    return false
}

function CopyCdn(e) {
    let el = $(e.target)
    let counter = parseInt(el.attr('counter')) + 1;
    el.attr('counter', counter)
    let target = el.attr('add-target')
    let html = insertValues[target].replaceAll('[i]', '[' + counter + ']').replace('cdn-counter="i"', 'cdn-counter="' + counter + '"')
    $('#' + target).append(html)

    $("[add-target=voiceCdn\\[" + counter + "\\]]").click(CopyVoiceCdn)
    $("[delete-target=voiceCdn\\[" + counter + "\\]]").click(Delete)
    return false
}

function CopyVoiceCdn(e) {
    let el = $(e.target)
    let counter = parseInt(el.attr('counter')) + 1;
    el.attr('counter', counter)
    let cdnCounter = parseInt(el.attr('cdn-counter'));
    let target = el.attr('add-target')
    $('#' + target.replace('[', '\\[').replace(']', '\\]')).append(insertValues[target.split('[')[0]].replaceAll('[i]', '[' + cdnCounter + ']').replaceAll('[j]', '[' + counter + ']'))
    return false
}

function Delete(e) {
    let el = $(e.target)
    let target = el.attr('delete-target')
    let addLink = $("[add-target='" + target + "']")
    let counter = parseInt(addLink.attr('counter')) - 1;
    if (counter < 0) return false;
    addLink.attr('counter', counter)
    $('#' + target.replace('[', '\\[').replace(']', '\\]')).children().last().remove()
    return false
}

$('#titleSearch').click(async e => {
    e.preventDefault()
    let elem = $('#title')
    let name = elem.attr('name')
    let val = elem.val()
    if (val !== '') {
        let res = await fetch('/FilmManagement/GetFromTitle?' + name + '=' + val)
        if (res.ok) Fill(await res.json())
    }
})

$('#kpIdSearch').click(async e => {
    e.preventDefault()
    let elem = $('#kpId')
    let name = elem.attr('name')
    let val = elem.val()
    if (val !== '') {
        let res = await fetch('/FilmManagement/GetFromKpId?' + name + '=' + val)
        if (res.ok) Fill(await res.json())
    }
})

$('#imdbIdSearch').click(async e => {
        e.preventDefault()
        let elem = $('#imdbId')
        let name = elem.attr('name')
        let val = elem.val()
        if (val !== '') {
            let res = await fetch('/FilmManagement/GetFromImdb?' + name + '=' + val)
            if (res.ok) Fill(await res.json())
            else alert(await res.text());
        }
    }
)

function Fill(model) {
    let elements = document.getElementsByTagName("input");
    for (let ii = 0; ii < elements.length; ii++) {
        if (elements[ii].type === "text") {
            elements[ii].value = "";
        }
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

    if (model.type === 'Serial') {
        $('#type').trigger('change')
    }

    let addGenre = $("[add-target='genre']")
    let addCountry = $("[add-target='country']")
    let addActor = $("[add-target='actor']")
    let addScreenwriter = $("[add-target='screenwriter']")
    let addDirector = $("[add-target='director']")
    let addCdn = $("[add-target='cdn']")
    for (let i = parseInt(addGenre.attr('counter')) + 1; i < model.genres.length; i++) addGenre.trigger('click')
    for (let i = parseInt(addCountry.attr('counter')) + 1; i < model.countries.length; i++) addCountry.trigger('click')
    for (let i = parseInt(addActor.attr('counter')) + 1; i < model.actors.length; i++) addActor.trigger('click')
    for (let i = parseInt(addScreenwriter.attr('counter')) + 1; i < model.screenwriters.length; i++) addScreenwriter.trigger('click')
    for (let i = parseInt(addDirector.attr('counter')) + 1; i < model.directors.length; i++) addDirector.trigger('click')
    for (let i = parseInt(addCdn.attr('counter')) + 1; i < model.cdn.length; i++) addCdn.trigger('click')

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
    for (let i = 0; i < model.cdn.length; i++) {
        $("[name='Cdns[" + i + "].Type']").val(model.cdn[i].type)
        $("[name='Cdns[" + i + "].Quality']").val(model.cdn[i].quality)
        $("[name='Cdns[" + i + "].Uri']").val(model.cdn[i].uri)
        let el = $("[cdn-counter=" + i + "]")
        for (let j = parseInt(el.attr('counter')) + 1; j < model.cdn[i].voices.length; j++) el.trigger('click')
        for (let j = 0; j < model.cdn[i].voices.length; j++) {
            $("[name='Cdns[" + i + "].Voices[" + j + "].Name']").val(model.cdn[i].voices[j])
        }
    }
}

