function getArticleContent(searchString) {

    $.getJSON(searchString, function (data) {
        $.each(data.query.pages, function (i, item) {
            console.log(item.extract);
            $('#currentArticle').html(item.extract)
        });
    });
}