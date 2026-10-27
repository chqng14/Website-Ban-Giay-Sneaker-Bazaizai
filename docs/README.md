# Tài liệu vận hành Bazaizai

## Nên đọc theo thứ tự

1. [Deploy Docker trên máy khác](01-DEPLOY-DOCKER.md) — cài đặt mới từ đầu.
2. [Đổi email hệ thống](02-CHANGE-SYSTEM-EMAIL.md) — cấu hình Gmail SMTP,
   email admin và những nơi còn tham chiếu email cửa hàng.
3. [Vận hành, cập nhật và sao lưu](03-OPERATIONS-BACKUP-UPDATE.md) — công việc
   hằng ngày, backup database và cập nhật phiên bản.
4. [Xử lý sự cố](04-TROUBLESHOOTING.md) — container không healthy, lỗi database,
   lỗi upload ảnh hoặc gửi mail.
5. [Checklist bảo mật](05-SECURITY-CHECKLIST.md) — kiểm tra trước khi mở website
   ra Internet.
6. [Cloudflare Tunnel local](06-CLOUDFLARE-TUNNEL.md) — vận hành hostname
   `store.hkladoi.tech` trỏ vào website local.

Deploy tự động sau khi đã tạo `.env`:

```powershell
pwsh ./deploy-docker.ps1
```

## Thành phần và dữ liệu bền vững

| Thành phần | Vai trò | Dữ liệu bền vững |
|---|---|---|
| `sqlserver` | Database | Volume `sqlserver_data` |
| `app-api` | API và xử lý ảnh | Volume `uploads_data` |
| `app-view` | Website MVC | `uploads_data`, `keys_data` |

Volume `uploads_data` chứa ảnh sản phẩm, avatar, ảnh khuyến mãi và QR. Volume
`keys_data` chứa khóa bảo vệ cookie/phiên đăng nhập. Mất một trong các volume này
có thể làm mất ảnh hoặc khiến người dùng phải đăng nhập lại.

## Quy tắc quan trọng

- Bí mật chỉ đặt trong `.env` hoặc secret manager của máy deploy.
- Không commit `.env`, app password, khóa thanh toán hoặc connection string.
- Không chạy `docker compose down -v` trong môi trường có dữ liệu thật.
- Luôn backup database và ảnh trước khi nâng cấp lớn.
- Production cần HTTPS qua reverse proxy như Caddy, Nginx hoặc dịch vụ cloud.
