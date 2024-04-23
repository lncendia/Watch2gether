let rating = $('.rating-area')

rating.change(async e => {
    e.preventDefault()
    try {
        await AddRating();
    } catch (e) {
        console.log(e)
    }
})

async function AddRating() {
    let data = new FormData(rating[0])
    let response = await fetch('/Film/AddRating', {
        method: 'POST', body: data
    })
    if (!response.ok) throw new Error(response.statusText)
    UpdateRating(await response.json())
}

function UpdateRating(score) {
    if (!rating.hasClass('rating-area-selected')) rating.addClass('rating-area-selected')
    let ratingValue = $('.rating');
    let ratingCount = $('.rating-count');
    
    ratingValue.html(score.rating)
    ratingCount.html(score.count + ' оценок')
}