function ParseData(elements, model) {
    model.forEach(el => {
        let html = '<div class="element col-lg-2 col-md-3 col-4 text-center"><a href="/Film?id=' + el.id + '" class="film-link"><img class="poster" src="/' + el.poster + '" alt=""><span>' + el.name + '</span></a><div>' + el.score + ' ★</div></div>'
        elements.append(html);
    })
}

let scroller = new Scroller('.ratings', '#filter', ParseData)
scroller.Start();