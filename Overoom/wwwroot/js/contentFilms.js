function ParseData(elements, model) {
    model.forEach(el => {
        let html = '<div class="element col-xl-6 col-sm-12 col-6"><div class="card film-card"><div class="card-header"><div class="float-start">Рейтинг: ' + el.rating + '</div></div><a class="film-link" href="/Film?id=' + el.id + '"><div class="row g-0"><div class="col-md-4"><img src="/' + el.posterUri + '" class="card-img-top poster" alt="..."></div><div class="col-md-8"><div class="card-body"><h5 class="card-title">' + el.name + '</h5><p class="card-text mb-2 film-description">' + el.description + '</p><p class="card-text">' + el.genres + '</p><p class="card-text position-absolute bottom-0 pb-2"><small class="text-body-secondary">' + el.type + '</small></p></div></div></div></a></div></div>'

        elements.append(html);
    })
}

let scroller = new Scroller( '.films', '#filter', ParseData)
scroller.Start();

$('#clearFilter').click(function () {
    scroller.ResetData()
    return false;
});