var arrayStyle = [];
var dataArray = [];
var corpArray = [];
$(document).ready(function () {
    var visualSearch = VS.init({
        container: $('#search_box_container'),
        query: '',
        minLength: 0,
        showFacets: true,
        readOnly: false,
        unquotable: [
                        'text',
                        'account',
                        'filter',
                        'access'
                    ],
        placeholder: "Search for your products...",
        callbacks: {
            search: function (query, searchCollection) {
                var $query = $('#search_query');
                $query.stop().animate({
                    opacity: 1
                }, {
                    duration: 300,
                    queue: false
                });
                clearTimeout(window.queryHideDelay2);
                arrayStyle = [];
                searchCollection.models.forEach(function (elt) {
                    if(elt.attributes.category != "text"){
                            var element = {
                            key: elt.attributes.category,
                            value: elt.attributes.value
                        };
                        arrayStyle.push(element);
                    }
                });
                // ====================== SHOW RESULT HERE ==================== //
                
                 searchAjax(arrayStyle);
                
                window.queryHideDelay2 = setTimeout(function () {
                    $query.animate({
                        opacity: 0
                    }, {
                        duration: 1000,
                        queue: false
                    });
                }, 2000);
            },
            valueMatches: function (category, searchTerm, callback) {
                switch (category) {
                    case 'name':
                        callback(nameArray);
                        break;
                    case 'category':
                        callback(categoryArray);
                        break;
                    case 'Status':
                        callback(['single', 'married',"divorced","widowed","unknown"]);
                        break;
                    default:
                        callback();
                        break;
                }
            },
            facetMatches: function (callback) {
                callback([{
                        label: 'Name',
                        category: 'general'
                                }, {
                        label: 'Category',
                        category: 'general'
                                },{
                        label: 'Price',
                        category: 'general'
                                },
                            ], {
                    preserveOrder: true
                });
            }
        }
    });
});




function search(keyArray) {
    if (keyArray.length != 0) {
        var detail = "";
        keyArray.forEach(function (elt, i){
            if(i != 0){
                detail = detail + "&&"
            }
            if(elt.value != null && elt.value != ""){
                detail = detail + elt.key + "=" + elt.value;
            }
        });

        window.location.href = '/Products/List?' + detail;
    }

}

function searchAjax(keyArray){
    if(onSearchView == true){
        if(keyArray.length != 0){
            var data = "";
            keyArray.forEach(function (elt, i){
                if(elt.value != null && elt.value != ""){
                    data = data + '"' + elt.key + '":"' + elt.value + '"';
                }
                if(i != keyArray.length - 1){
                    data = data + ","
                }
            });
            var s = JSON.parse("{" + data + "}");
            console.log(s);
            console.log(keyArray);
            $.ajax({

                type: 'POST',

                url: '/products/ListUpdate',
                dataType: 'json',
                data: s,
                success: function (data) {
                    deployData(data);
                },
                error: function (ex) {
                    console.log(ex.responseText)
                }
            });
        }
        }else{
            if (keyArray.length != 0) {
                var detail = "";
                keyArray.forEach(function (elt, i){
                    if(i != 0){
                        detail = detail + "&&"
                    }
                    if(elt.value != null && elt.value != ""){
                        detail = detail + elt.key + "=" + elt.value;
                    }
                });

                window.location.href = '/Products/List?' + detail;
            }
    }
}



var deployData = function(data){
    console.log(data);
    var resultPlace = document.getElementById('resultPlace');
    var resultHTML = "";
    
    data.forEach(function (elt, i){
        
        resultHTML = resultHTML + '<div class="col-sm-6 col-md-4">'
        + '<div class="thumbnail" style="margin-bottom: 20px">'
        + '<img src="http://localhost:12862/Uploads/images/' + elt.Image.ImageName 
        + '_medium' + elt.Image.ImageType + '">'
        + '<div class="caption">'
        + '<h3 style="color: #0094ff">' + elt.Name + '</h3>'
        + '<p class="lead">' + elt.Description + '</p>'
        + '<p>'
        + '<span class="btn btn-primary">' + elt.Price + '</span>'
        + '<form action="/Home/Details?returnUrl=%2F" method="post" style="display:inline">'
        + '<span>'
        + '<input id="productId" name="productId" type="hidden" value="' + elt.ID + '">'
        + '<input type="submit" class="btn btn-success" value="Details">'
        + '</span>'
        + '</form>'
        + '<form action="/Carts/checkQuantity?returnUrl=%2F" method="post" style="display:inline">'
        + '<span>'
        + '<input id="productId" name="productId" type="hidden" value="' + elt.ID + '">'
        + '<input type="submit" class="btn btn-default" value="Add to cart">'
        + '</span>'
        + '</form>'
        + '</p>'
        + '</div>'
        + '</div>'
        + '</div>';

    });

    resultPlace.innerHTML = resultHTML;
}