var CartController = function () {
    this.initialize = function () {
        loadData();

        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            updateCart(id, quantity);
        });

        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            updateCart(id, quantity);
        });

        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateCart(id, 0);
        });
    }

    function updateCart(id, quantity) {
        $.ajax({
            type: "POST",
            url: '/Cart/UpdateCart',
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                $('#lbl_number_items_header').text(res.length);
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function loadData() {
        $.ajax({
            type: "GET",
            url: '/Cart/GetListItems',
            success: function (res) {
                var html = '';
                var total = 0;

                $.each(res, function (i, item) {
                    var amount = item.price * item.quantity;
                    html += "<div class=\"cart__list\">"
                        + "<div class=\"cart__img\">"
                        + "<img src=\"" + $('#hidBaseAddress').val() + item.image + "\" alt=\"\">"
                        + "</div>"
                        + "<div class=\"cart__info\">"
                        + "<div class=\"item__info\">"
                        + "<p class=\"item__info-name\">" + item.name + "</p>"
                        + "<p class=\"item__info-option\">" + item.color + "/" + item.memory + "/" + item.ram + "</p>"
                        + "</div>"
                        + "<button class=\"btn btn-minus\" data-id=\"" + item.productId + "\" type =\"button\"><i class=\"fa-solid fa-circle-minus\"></i></button>"
                        + "<div class=\"item__quan\">"
                        + "<input id=\"txt_quantity_" + item.productId + "\" value=\"" + item.quantity + "\"  type=\"text\">"
                        + "</div>"
                        + "<button class=\"btn btn-plus\" data-id=\"" + item.productId + "\" type=\"button\"><i class=\"fa-solid fa-circle-plus\"></i></button>"
                        + "<button class=\"btn btn-remove\"data-id=\"" + item.productId + "\" type=\"button\"><i class=\"far fa-times-circle\"></i></button>"
                        + "<div class=\"item__price\">" + numberWithCommas(item.price) + " </div>"
                        + "<div class=\"item__price\">" + numberWithCommas(amount) + "</div>"
                        + "</div>"
                        + "</div>"
                    total += amount;
                });
                $('.cart__listItem').html(html);
                $('#lbl_number_of_items').text(res.length);
                $('#lbl_total').text(numberWithCommas(total));
            }
        });
    }
}