# Website bán giày Sneaker Bazaizai

Ứng dụng ASP.NET Core gồm ba thành phần chạy bằng Docker:

- `app-view`: website MVC, cổng mặc định `8080`.
- `app-api`: API nội bộ, cổng mặc định `80` trên máy host.
- `sqlserver`: SQL Server, cổng mặc định `1433`.

## Khởi động nhanh

```powershell
Copy-Item .env.example .env
# Sửa SA_PASSWORD và INTERNAL_API_KEY trong .env
.\deploy-docker.ps1
```

Trên Linux/macOS có PowerShell 7: `pwsh ./deploy-docker.ps1`. Script tự kiểm tra cấu hình,
build, chờ healthcheck và kiểm thử bảo vệ API. Có thể deploy thủ công bằng
`docker compose up -d --build --wait` nếu cần.

Mở website tại `http://localhost:8080` và kiểm tra API tại
`http://localhost/api/Health`.

Không commit file `.env` lên Git và không dùng `docker compose down -v` nếu
chưa sao lưu dữ liệu.

## Tài liệu

- [Chỉ mục tài liệu](docs/README.md)
- [Deploy Docker trên máy khác](docs/01-DEPLOY-DOCKER.md)
- [Đổi email hệ thống](docs/02-CHANGE-SYSTEM-EMAIL.md)
- [Vận hành, cập nhật và sao lưu](docs/03-OPERATIONS-BACKUP-UPDATE.md)
- [Xử lý sự cố](docs/04-TROUBLESHOOTING.md)
- [Checklist bảo mật trước khi public](docs/05-SECURITY-CHECKLIST.md)

Tài liệu `DOCKER.md` cũ được giữ lại như đường dẫn tương thích và trỏ tới hướng
dẫn mới.
