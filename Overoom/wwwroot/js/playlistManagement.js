function ParseData(elements, model) {
    model.forEach(el => {
        let html = '<div class="element col-lg-2 col-md-3 col-sm-4 col-6 text-center"><a href="PlaylistManagement/Edit?id=' + el.id + '" class="content-link"><img class="poster" src="/' + el.posterUri + '" alt=""><span>'+el.name+'</span></a></div>'
        elements.append(html);
    })
}

let scroller = new Scroller('.films', '#filter', ParseData)
scroller.Start();