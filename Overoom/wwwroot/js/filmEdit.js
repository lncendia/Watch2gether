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
    let el1 = $("[copy-target='.copyVoiceVideoCdn\\[" + counter + "\\]']")
    el1.click(CopyInput)
    console.log(el1)
    return false
}