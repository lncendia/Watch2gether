class Scroller {
    constructor(elements, form, parseFunc) {
        this.elements = $(elements);
        this.data = $(form);
        this.uri = this.data.attr('action');
        this.parseFunc = parseFunc;
        this.data.submit(ev => ev.preventDefault())
        this.data.change(async () => (await this.ChangeData()));
        this.query = GetFormData(this.data);
    }

    Start() {
        this.scroller = new InfiniteAjaxScroll(this.elements, {
            item: '.element', next: index => this.Handler(index), spinner: '.spinner', delay: 600
        });
    }

    async ChangeData() {
        this.scroller.pause();
        this.elements.empty();
        this.query = GetFormData(this.data);
        this.scroller.pageIndex = -1;
        this.scroller.resume();
        await this.scroller.next();
    }

    async Handler(pageIndex) {
        try {
            this.query.set('page', pageIndex + 1)
            let data = await GetList(this.uri, this.query);
            this.parseFunc(this.elements, data)
            return true;
        } catch (e) {
            console.log(e.message);
            return false;
        }
    }

    ResetData() {
        let form = this.data[0];
        form.reset()
        form.dispatchEvent(new Event('change'))
    }
}

async function GetList(uri, query) {
    let data = await fetch(uri + '?' + query.toString());
    if (data.status === 200) return await data.json();
    throw new Error(await data.text());
}

function GetFormData(data) {
    let url = new URLSearchParams();
    let formData = new FormData(data[0])
    formData.forEach((value, key) => {
        url.set(key, value);
    })
    return url;
}