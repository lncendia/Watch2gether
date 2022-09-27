let $data = new URLSearchParams();
$data.append("id", $('#id').val());

let nextHandler = async function (pageIndex) {
    try {
        let data = await GetList(pageIndex + 1);
        return ParseData(data);
    } catch (e) {
        console.log(e);
        return false;
    }
}

let scroller = new InfiniteAjaxScroll('.comments', {
    item: '.comment', next: nextHandler, spinner: '.spinner', delay: 600
});

//scroller.pageIndex = -1;

async function GetList(page) {
    $data.set("page", page)
    let data = await fetch('/Film/Comments?' + $data.toString());
    if (data.status === 200) return await data.text();
    throw new Error(data.statusText);

}

function ParseData(json) {
    let data = JSON.parse(json);
    data.forEach(el => {
        ShowComment(el);
    })
    return true;
}

let form = $('#commentForm');
let input = $('#comment');
let validation = $("span[data-valmsg-for='comment']");

function clearForm() {
    input.addClass("valid");
    input.val(null);
    input.removeClass("input-validation-error");
    validation.addClass("field-validation-valid");
    validation.removeClass("field-validation-error");
    validation.text("");
}

function failForm(error) {
    input.removeClass("valid");
    validation.removeClass("field-validation-valid");
    validation.text("Ошибка отправки комментария: " + error);
    validation.addClass("field-validation-error");
    input.addClass("input-validation-error");
}

$("#commentButton").click(async () => {
    try {
        await AddComment();
        clearForm();
    } catch (e) {
        failForm(e);
    }
    return false;
});


async function AddComment() {
    let data = new FormData(form[0]);
    let response = await fetch('/Film/AddComment', {
        method: 'POST', body: data
    });
    if (response.status !== 200) throw new Error(response.statusText);
    ShowComment(JSON.parse(await response.text()), true);
}

let comments = $('.comments');

function ShowComment(el, prepend = false) {
    let text = "<div class=\"comment\"><div class=\"card bg-dark mb-3\"><div class=\"card-header\"><span class=\"float-start\">" + el.username + "</span><span class=\"float-end\">" + el.createdAt + "</span></div><div class=\"card-body d-flex\"><img src=\"/img/Avatars/" + el.avatarFileName + "\" alt=\"\" class=\"commentAvatar\"><p class=\"card-text\">" + el.text + "</p></div></div></div>"
    if (prepend) comments.prepend(text);
    else comments.append(text);
}