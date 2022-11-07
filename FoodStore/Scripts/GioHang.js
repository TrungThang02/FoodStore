var giohang = {
    init: function () {
        giohang.registerEvent();
    },
    registerEvent: function () {
        $(".quantity").on('change', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data("id"));
            var quantity = parseInt($(this)[0].value)
            var productName = $(".productName")[0].outerText;
            console.log($(".quantity"))
            var total = 0;
            for (var i = 0; i < $(".quantity").length; i++) {
                total += parseInt($(".quantity")[i].value)
            }
            const that = $(this);
            $.ajax({
                url: "/GioHang/CapNhatGioHang/",
                data: { id: id, quantity: quantity },
                dataType: 'json',
                type: 'GET',
                contentType: 'application/json;charset=utf-8',
                success: function (response) {
                    console.log(response)

                    response.item.productQuantity = $(that)[0].value
                    response.item.totalPrice = parseInt(response.item.productQuantity * response.item.productPrice);
                    console.log(response.item.totalPrice)
                    $(that)[0].value = quantity;
                    $(".qty")[0].innerHTML = total;
                    //console.log(response.item.totalPrice.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }))

                    that[0].value = response.item.productQuantity;
                    $("#product" + id)[0].innerHTML = response.item.totalPrice.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });

                }
            })
        })
    }
}

giohang.init();