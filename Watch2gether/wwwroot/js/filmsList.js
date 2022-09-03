let $data = new URLSearchParams();
let inverseOrder = false;
let data = $("#dataForm")
GetFormData();

let nextHandler = async function (pageIndex) {
    let data = await GetList(pageIndex + 1);
    if (!data.succeeded) return false;
    ShowList(data.text);
    return true;
}

let scroller = new InfiniteAjaxScroll('.films', {
    item: '.film', next: nextHandler, spinner: '.spinner', delay: 600
});

async function GetList(page) {
    $data.set("page", page)
    $data.set("inverseOrder", inverseOrder);
    try {
        let data = await fetch('Film/FilmsList?' + $data.toString());
        if (data.status === 200) {
            return {text: await data.text(), succeeded: true};
        }
        return {succeeded: false, text: null};
    } catch {
        return {succeeded: false, text: null};
    }
}

let films = $(".films")

function ShowList(json) {
    let data = JSON.parse(json);
    data.forEach(el => {
        films.append("<div class=\"film col-xxl-3 col-lg-4 col-md-6 col-12\"><div class=\"card filmCard\"><a class=\"filmLink\" href=\"/Film/Film?id=" + el.id + "\"><div class=\"card-header filmCardHeader\">Рейтинг: " + el.rating + "</div> <img src=\"img/Posters/" + el.posterFileName + "\" class=\"card-img-top poster\" alt=\"...\"><div class=\"card-body\"><h5 class=\"card-title\">" + el.name + "</h5><h6 class=\"card-subtitle mb-2 additionalText\">" + el.description + "</h6><p class=\"card-text\">" + el.genres + "</p></div><div class=\"card-footer filmCardFooter\">" + el.type + "</div></a></div></div>");
    })
}

function GetFormData() {
    let d = new FormData(data[0]);
    $data = new URLSearchParams();
    d.forEach((value, key) => {
        $data.set(key, value);
    })
}

data.on("change", Select);

async function Select() {
    scroller.pause();
    films.empty();

    GetFormData();
    scroller.pageIndex = -1;
    scroller.resume();
    await scroller.next();
}

$("#clearFilter").click(function () {
    let form = data[0];
    form.reset()
    form.dispatchEvent(new Event("change"))
    return false;
});


$("#sortOrder").click(function () {
    if (inverseOrder) {
        inverseOrder = false;
        $(this).children("svg:last").remove();
        $(this).append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-arrow-up\" viewBox=\"0 0 16 16\"><path fill-rule=\"evenodd\" d=\"M8 15a.5.5 0 0 0 .5-.5V2.707l3.146 3.147a.5.5 0 0 0 .708-.708l-4-4a.5.5 0 0 0-.708 0l-4 4a.5.5 0 1 0 .708.708L7.5 2.707V14.5a.5.5 0 0 0 .5.5z\"/></svg>");
    } else {
        inverseOrder = true;
        $(this).children("svg:last").remove();
        $(this).append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-arrow-down\" viewBox=\"0 0 16 16\"><path fill-rule=\"evenodd\" d=\"M8 1a.5.5 0 0 1 .5.5v11.793l3.146-3.147a.5.5 0 0 1 .708.708l-4 4a.5.5 0 0 1-.708 0l-4-4a.5.5 0 0 1 .708-.708L7.5 13.293V1.5A.5.5 0 0 1 8 1z\"/></svg>");
    }
    data[0].dispatchEvent(new Event("change"))
    return false;
});