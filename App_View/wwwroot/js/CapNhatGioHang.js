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

//function shipping() {
//    //var tongtien = "";
//    //var ship = "";
//    //var ship1 = "";
//    //var uudai = "";
//    //var SUM = "";

//    $(document).ready(function () {
//        const apiUrl = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/province';
//        const token = '1194852d-fde8-11ed-8a8c-6e4795e6d902';

//        axios.get(apiUrl, {
//            headers: {
//                'Token': token
//            }
//        })
//            .then(response => {
//                var selectElement = $('#city');
//                selectElement.empty();

//                selectElement.append($('<option>').val('').text('Chọn thành phố'));

//                $.each(response.data.data, function (index, item) {
//                    selectElement.append($('<option>').val(item.ProvinceID).text(item.ProvinceName));
//                });

//                selectElement.val(selectElement.val()).trigger('change');
//            })
//            .catch(error => {
//                console.error(error);
//            });
//    });

//    $(document).ready(function () {
//        $('#city').on('change', function () {
//            if ($('#city').val() === null) {
//                var selectElement = $('#district');
//                selectElement.empty();
//                selectElement.val('').trigger('change');
//                selectElement.append($('<option>').val('').text('Chọn huyện'));
//            }
//            const apiUrl = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/district';
//            const token = '1194852d-fde8-11ed-8a8c-6e4795e6d902';
//            axios.get(apiUrl, {
//                headers: {
//                    'Token': token
//                },
//                params: {
//                    province_id: $('#city').val()
//                }
//            })
//                .then(response => {
//                    console.log(response.data.data);
//                    var selectElement = $('#district');
//                    selectElement.empty();
//                    selectElement.val('').trigger('change');
//                    selectElement.append($('<option>').val('').text('Chọn huyện'));
//                    $.each(response.data.data, function (index, item) {
//                        selectElement.append($('<option>').val(item.DistrictID).text(item.DistrictName));
//                    });
//                    selectElement.val(selectElement.val()).trigger('change');
//                })
//                .catch(error => {
//                    console.error(error);
//                });
//        });
//    });

//    $(document).ready(function () {
//        $('#district').on('change', function () {
//            if ($('#district').val() === null) {
//                var selectElement = $('#wards');
//                selectElement.empty();
//                selectElement.val('').trigger('change');
//                selectElement.append($('<option>').val('').text('Chọn phường/xã'));
//            }
//            const apiUrl = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/ward';
//            const token = '1194852d-fde8-11ed-8a8c-6e4795e6d902';
//            axios.get(apiUrl, {
//                headers: {
//                    'Token': token
//                },
//                params: {
//                    district_id: $('#district').val()
//                }
//            })
//                .then(response => {
//                    console.log(response.data.data);
//                    var selectElement = $('#wards');
//                    selectElement.empty();
//                    selectElement.val('').trigger('change');
//                    selectElement.append($('<option>').val('').text('Chọn phường/xã'));
//                    $.each(response.data.data, function (index, item) {
//                        selectElement.append($('<option>').val(item.WardCode).text(item.WardName));
//                    });
//                    selectElement.val(selectElement.val()).trigger('change');
//                })
//                .catch(error => {
//                    console.error(error);
//                });
//        });
//    });
//}

// Trong script.js

