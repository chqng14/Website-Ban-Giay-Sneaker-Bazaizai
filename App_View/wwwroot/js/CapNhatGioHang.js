$(document).on("input click", ".input-counter__text, .input-counter__plus, .input-counter__minus", function () {
    console.log($(this));
    var SoLuong = $(this).data('value');
    var IdSanPhamChiTiet = $(this).data('product-id');
    var IdGioHangChiTiet = $(this).data('cart-id');
    console.log(SoLuong);
    console.log(IdSanPhamChiTiet);
    console.log(IdGioHangChiTiet);
    if ($(this).hasClass('input-counter__plus')) {
        SoLuong++;
    } else if ($(this).hasClass('input-counter__minus') && SoLuong > 1) {
        SoLuong--;
    }
    $.ajax({
        url: '/GioHangChiTiets/CapNhatSoLuongGioHang',
        method: 'POST',
        data: { IdSanPhamChiTiet: IdSanPhamChiTiet, SoLuong: SoLuong, IdGioHangChiTiet: IdGioHangChiTiet },
        success: function (response) {
            var inputs = document.getElementsByClassName('input-counter__text');
            var sum = 0;

            for (var i = 0; i < inputs.length; i++) {
                var inputValue = parseInt(inputs[i].value);
                if (!isNaN(inputValue)) {
                    sum += inputValue;
                }
            }
            console.log(sum);
        }
    });
});
function shipping() {
    //var tongtien = "";
    //var ship = "";
    //var ship1 = "";
    //var uudai = "";
    //var SUM = "";

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
                selectElement.append($('<option>').val('').text('Chọn xã'));
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
}

