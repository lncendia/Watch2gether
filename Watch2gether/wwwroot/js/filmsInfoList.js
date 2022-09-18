let $data = new URLSearchParams();
let data = $("#dataForm")
let films = $("#tableBody")
let startPage = $("#startPage")
let title = $("#title")

let count = 1, startPageVal = 0;

let nextHandler = async function (pageIndex) {
    try {
        let data = await GetList(pageIndex + 1);
        return ParseData(data);
    }
    catch (e) {
        console.log(e);
        return false;
    }
}

let scroller = new InfiniteAjaxScroll('.films', {
    item: '.film', next: nextHandler, spinner: '.spinner', delay: 600
});

async function GetList(page) {
    $data.set("page", page + startPageVal)
    let data = await fetch('/FilmDownloader/FilmsList?' + $data.toString());
    if (data.status === 200) return await data.text();
    throw new Error(data.statusText);
}

function ParseData(json) {
    let data = JSON.parse(json);
    data.films.forEach(el => {
        films.append("<tr class='film'><th scope=\"row\">" + count++ + "</th><td>" + el.name + "</td><td class=\"text-center\">" + el.year + "</td><td class=\"text-center\">" + el.type + "</td><td class=\"text-center\"><a class=\"addBtn nav-link\" id=\"" + el.id + "\" href=\"#\"><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-plus-square\" viewBox=\"0 0 16 16\"><path d=\"M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z\"/><path d=\"M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z\"/></svg></a></td></tr>");
        $("#" + el.id).click(function () {
            AddFilm($(this)).then();
            return false;
        });
    })
    return data.moreAvailable;
}

data.on("change", Select);

async function Select() {
    console.log(title.val());
    scroller.pause();
    films.empty();
    $data = new URLSearchParams();
    let val = title.val(), page = startPage.val();
    if (val !== "") $data.set("title", title.val());
    startPageVal = page !== "" ? parseInt(startPage.val()) : 0;
    scroller.pageIndex = -1;
    count = 1;
    scroller.resume();
    await scroller.next();
}

async function AddFilm(el) {
    try {
        let data = new FormData();
        data.append("id", el.attr("id"));

        let response = await fetch('FilmDownloader/AddFilm', {method: 'POST', body: data});
        if (response.status === 200) {
            el.empty();
            el.append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-check-square\" viewBox=\"0 0 16 16\"><path d=\"M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z\"/><path d=\"M10.97 4.97a.75.75 0 0 1 1.071 1.05l-3.992 4.99a.75.75 0 0 1-1.08.02L4.324 8.384a.75.75 0 1 1 1.06-1.06l2.094 2.093 3.473-4.425a.235.235 0 0 1 .02-.022z\"/>\n" + "</svg>");
            el.addClass('disabled');
        } else alert("Ошибка добавления фильма: " + response.status);

    } catch (e) {
        alert("Ошибка добавления фильма: " + e);
    }
}