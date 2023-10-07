//$(document).on("input", ".input-counter__text", function () {
//    updateCart(this);
//});

//$(document).on("click", ".input-counter__plus, .input-counter__minus", function () {
//    var inputElement = $(this).siblings(".input-counter__text")[0];
//    updateCart(inputElement);
//});

//function updateCart(inputElement) {
//    var soLuong = parseInt($(inputElement).val());
//    var idSanPhamChiTiet = $(inputElement).data('product-id');
//    var idGioHangChiTiet = $(inputElement).data('cart-id');

//    if ($(inputElement).hasClass('input-counter__plus')) {
//        soLuong++;
//    } else if ($(inputElement).hasClass('input-counter__minus') && soLuong > 1) {
//        soLuong--;
//    }

//    $.ajax({
//        url: '/GioHangChiTiets/CapNhatSoLuongGioHang',
//        method: 'POST',
//        data: { IdSanPhamChiTiet: idSanPhamChiTiet, SoLuong: soLuong, IdGioHangChiTiet: idGioHangChiTiet },
//        success: function (response) {
//            var inputs = document.getElementsByClassName('input-counter__text');
//            var sum = 0;

//            for (var i = 0; i < inputs.length; i++) {
//                var inputValue = parseInt(inputs[i].value);
//                if (!isNaN(inputValue)) {
//                    sum += inputValue;
//                }
//            }
//            console.log(sum);
//        }
//    });
//}

$(document).ready(function () {
    const apiUrl = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/province';
    const token = '1194852d-fde8-11ed-8a8c-6e4795e6d902';

    axios.get(apiUrl, {
        headers: {
            'Token': token
        }
    })
        .then(response => {
            var selectElement = $('#city');
            selectElement.empty();

            selectElement.append($('<option>').val('').text('Chọn thành phố'));

            $.each(response.data.data, function (index, item) {
                selectElement.append($('<option>').val(item.ProvinceID).text(item.ProvinceName));
            });

            selectElement.val(selectElement.val()).trigger('change');
        })
        .catch(error => {
            console.error(error);
        });
});

$(document).ready(function () {
    $('#city').on('change', function () {
        if ($('#city').val() === null) {
            var selectElement = $('#district');
            selectElement.empty();
            selectElement.val('').trigger('change');
            selectElement.append($('<option>').val('').text('Chọn huyện'));
        }
        const apiUrl = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/district';
        const token = '1194852d-fde8-11ed-8a8c-6e4795e6d902';
        axios.get(apiUrl, {
            headers: {
                'Token': token
            },
            params: {
                province_id: $('#city').val()
            }
        })
            .then(response => {
                console.log(response.data.data);
                var selectElement = $('#district');
                selectElement.empty();
                selectElement.val('').trigger('change');
                selectElement.append($('<option>').val('').text('Chọn huyện'));
                $.each(response.data.data, function (index, item) {
                    selectElement.append($('<option>').val(item.DistrictID).text(item.DistrictName));
                });
                selectElement.val(selectElement.val()).trigger('change');
            })
            .catch(error => {
                console.error(error);
            });
    });
});

$(document).ready(function () {
    $('#district').on('change', function () {
        if ($('#district').val() === null) {
            var selectElement = $('#wards');
            selectElement.empty();
            selectElement.val('').trigger('change');
            selectElement.append($('<option>').val('').text('Chọn phường/xã'));
        }
        const apiUrl = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/ward';
        const token = '1194852d-fde8-11ed-8a8c-6e4795e6d902';
        axios.get(apiUrl, {
            headers: {
                'Token': token
            },
            params: {
                district_id: $('#district').val()
            }
        })
            .then(response => {
                console.log(response.data.data);
                var selectElement = $('#wards');
                selectElement.empty();
                selectElement.val('').trigger('change');
                selectElement.append($('<option>').val('').text('Chọn phường/xã'));
                $.each(response.data.data, function (index, item) {
                    selectElement.append($('<option>').val(item.WardCode).text(item.WardName));
                });
                selectElement.val(selectElement.val()).trigger('change');
            })
            .catch(error => {
                console.error(error);
            });
    });
});

$(document).ready(function () {
    $('#wards').on('change', function () {
        var inputs = document.getElementsByClassName('input-counter__text');
        var sum = 0;
        for (var i = 0; i < inputs.length - 1; i++) {
            var inputValue = parseInt(inputs[i].value);
            if (!isNaN(inputValue)) {
                sum += inputValue;
            }
        }

        var soLuongDonHang = sum;
        var length = 20; // Giá trị mặc định cho chiều dài
        var width = 15; // Giá trị mặc định cho chiều rộng
        var height = 20;
        var weight = parseInt((length * width * height) / 5000);
        if ($('#wards').val() != null) {
            const apiUrl = 'https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee';
            const token = '1194852d-fde8-11ed-8a8c-6e4795e6d902';
            const shop_id = 705899;
            axios.get(apiUrl, {
                headers: {
                    'Token': token,
                    'shop_id': shop_id
                },
                params: {
                    service_type_id: 2,
                    insurance_value: $('#tamtinh').text().replace(/\D/g, ''),
                    coupon: null,
                    to_ward_code: $('#wards').val(),
                    to_district_id: $('#district').val(),
                    from_district_id: 3440,
                    weight: weight,
                    length: length,
                    width: width,
                    height: height
                }
            })
                .then(response => {

                    //lưu tiền ship vào localstorage
                    localStorage.setItem('phiship', response.data.data.service_fee);
                    //gán tiền ship vào id phiship
                    $('#phiship').text(parseInt(response.data.data.service_fee).toLocaleString("vi-VN").replace(/\./g, ',') + "đ");
                    // $('#divShippingFee1').val(response.data.data.service_fee);

                    // lấy ra tổng tiền của sản phẩm có trong giỏ hàng
                    var tongtienSanPham = parseInt(document.getElementById("tamtinh").innerText.replace("đ", "").replace(/\,/g, "").replace(/\./g, "").trim());

                    var totalAmountElement = document.getElementById("tongtien");

                    var tongTien = tongtienSanPham + response.data.data.service_fee;

                    // Hiển thị tổng tiền
                    totalAmountElement.innerText = tongTien.toLocaleString("vi-VN").replace(/\./g, ',') + "đ";
                    totalAmountElement.value = tongTien.toString();
                })
                .catch(error => {
                    console.error(error);
                });
        }
    });
});

$(window).on('beforeunload', function () {

    // Xoá giá trị từ localStorage
    localStorage.removeItem('phiship');
});
