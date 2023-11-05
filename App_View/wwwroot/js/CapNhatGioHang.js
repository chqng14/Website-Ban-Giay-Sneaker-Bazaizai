//$(document).ready(function () {
//    $('#save-button').on('click', function (e) {
//        // Ngăn chặn sự kiện mặc định của form để tránh submit trang
//        e.preventDefault();

//        // Các dòng code xử lý dữ liệu và gửi Ajax ở đây
//        var selectedCity = $('#city option:selected').text();
//        var selectedDistrict = $('#district option:selected').text();
//        var selectedWard = $('#wards option:selected').text();
//        var inputAddress = $('#address-street').val();
//        var idNguoiDung = $('#idNguoiDung').val();

//        var DiaChiFull = inputAddress + ',' + selectedWard + ',' + selectedDistrict + ',' + selectedCity;
//        var TenNguoiNhan = $('#address-fname').val();
//        var SDT = $('#address-phone').val();

//        // Sử dụng Ajax để gửi dữ liệu đến action "CreateThongTin"
//        $.ajax({
//            url: '/ThongTinGH/CreateThongTin',
//            method: 'POST',
//            data: { idNguoiDung: idNguoiDung, TenNguoiNhan: TenNguoiNhan, SDT: SDT, DiaChiFull: DiaChiFull },
//            success: function (result) {
//                // Xử lý kết quả từ controller nếu cần
//                console.log(result);
//                // Sau khi thành công, có thể thực hiện các bước tiếp theo, ví dụ như đóng modal hoặc chuyển hướng
//            },
//            error: function (error) {
//                console.error(error);
//            }
//        });
//    });
//});


// Hàm xử lý khi ấn vào mũi tên


