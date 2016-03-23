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
                $query.html('<span class="raquo">&raquo;</span> You searched for: <b>' + searchCollection.serialize() + '</b>');
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
    if(onSearchView){
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
                    console.log(data);
                },
                error: function (ex) {
                    console.log(ex.responseText)
                }
            });
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
}
