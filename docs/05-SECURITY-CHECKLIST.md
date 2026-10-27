# Checklist bảo mật trước khi public

## Bí mật và tài khoản

- [ ] `.env` không nằm trong Git và chỉ người vận hành đọc được.
- [ ] `SA_PASSWORD`, `INTERNAL_API_KEY`, mật khẩu admin là ba giá trị khác nhau.
- [ ] Không dùng giá trị mẫu trong `.env.example`.
- [ ] Gmail dùng App Password và bật xác minh hai bước.
- [ ] Đã xoay vòng mọi khóa từng xuất hiện trong source/lịch sử Git.
- [ ] `ADMIN_PASSWORD` được xóa khỏi `.env` sau khi seed admin lần đầu.
- [ ] Không còn tài khoản admin dùng mật khẩu mặc định/yếu.

## Mạng

- [ ] Website chỉ public qua HTTPS 443.
- [ ] SQL Server `1433` bị chặn từ Internet.
- [ ] API không public trực tiếp nếu không thực sự cần.
- [ ] Firewall chỉ mở các cổng cần thiết.
- [ ] Reverse proxy đặt giới hạn request body và timeout hợp lý.

## Ứng dụng

- [ ] `ASPNETCORE_ENVIRONMENT=Production`.
- [ ] Swagger và Hangfire Dashboard không công khai ở Production.
- [ ] Mọi endpoint API ngoài `GET /api/Health` trả `401` nếu thiếu/sai khóa nội bộ.
- [ ] `GET /api/Health/detailed` chỉ trả `200` khi có `X-Internal-Api-Key` hợp lệ.
- [ ] File giả ảnh bị từ chối `400`.
- [ ] Chỉ các thư mục ảnh cần thiết được phục vụ như static file.
- [ ] API và View chạy bằng user không-root.
- [ ] Role Admin/Nhân viên được kiểm tra trên các chức năng quản trị.

API sử dụng authentication handler chuẩn và fallback authorization policy, vì vậy controller
hoặc action mới cũng được bảo vệ mặc định. Chỉ gắn `AllowAnonymous` sau khi đã đánh giá rõ
endpoint đó không tiết lộ dữ liệu hoặc thay đổi trạng thái.

## Dữ liệu và backup

- [ ] Database, ảnh và Data Protection keys đều dùng named volume.
- [ ] Có backup ở một máy/ổ lưu trữ khác.
- [ ] Backup keys được mã hóa.
- [ ] Đã thử restore database trên môi trường thử nghiệm.
- [ ] Không dùng `docker compose down -v` trong quy trình deploy thông thường.

## Giám sát

- [ ] Có health monitoring cho website và `/api/Health`.
- [ ] Docker log có giới hạn dung lượng/rotation.
- [ ] Có cảnh báo khi ổ đĩa gần đầy hoặc container restart liên tục.
- [ ] Kiểm tra log lỗi đăng nhập, gửi mail và thanh toán định kỳ.

## Thanh toán và dịch vụ ngoài

- [ ] Callback payment dùng HTTPS và đúng tên miền production.
- [ ] Khóa MoMo/VNPay/Twilio tách riêng cho production và sandbox.
- [ ] Không ghi secret, access token hoặc payload nhạy cảm vào log.
- [ ] Đã kiểm thử callback thất bại, gọi lặp và timeout.
