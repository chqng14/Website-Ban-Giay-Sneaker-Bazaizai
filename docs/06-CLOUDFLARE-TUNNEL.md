# Cloudflare Tunnel cho môi trường local

## Thông tin hiện tại

- Tunnel: `bazaizai-local`
- Hostname: `https://store.hkladoi.tech`
- Origin: `http://localhost:8080`
- Tunnel ID: `e63a3fb8-83c2-486e-9927-41471909ec78`
- Cấu hình local: `.cloudflared/config.yml` (đã được Git bỏ qua)
- Windows service: `cloudflared`, trạng thái khởi động `Automatic`, chạy dưới `LocalSystem`

Tunnel chỉ công khai website. Cổng API host và SQL Server không được đưa vào ingress.

## Điều kiện hoạt động

Ba container Docker phải đang healthy và `app-view` phải trả `200` tại
`http://localhost:8080/`. Windows service `cloudflared` tự khởi động khi Windows boot. Nếu
service khởi động trước Docker Desktop, tunnel vẫn kết nối Cloudflare và sẽ tự kết nối lại
origin sau khi website local sẵn sàng.

Kiểm tra:

```powershell
docker compose ps
Get-Process cloudflared
Get-Service cloudflared
Invoke-WebRequest -UseBasicParsing https://store.hkladoi.tech/
```

## Khởi động hoặc restart tunnel

Mở PowerShell bằng quyền Administrator:

```powershell
Start-Service cloudflared
Restart-Service cloudflared
```

Kiểm tra trạng thái connector:

```powershell
& 'C:\Program Files (x86)\cloudflared\cloudflared.exe' tunnel info bazaizai-local
```

## Dừng tunnel

```powershell
Stop-Service cloudflared
```

Tắt tự khởi động nếu không còn muốn public website sau reboot:

```powershell
Stop-Service cloudflared
Set-Service cloudflared -StartupType Disabled
```

Khi service dừng, DNS vẫn tồn tại nhưng Cloudflare sẽ không kết nối được tới website local.

## Lưu ý bảo mật

- Không commit `.cloudflared`, `cert.pem` hoặc file credentials JSON.
- URL hiện là public Internet; trang quản trị vẫn được bảo vệ bằng đăng nhập ứng dụng.
- Nếu chỉ cho một nhóm người truy cập, cấu hình thêm Cloudflare Access.
- Không tạo ingress cho SQL Server `1433` hoặc API host `80` nếu không có nhu cầu rõ ràng.
- Không xóa tunnel trên Cloudflare trước khi đã gỡ DNS liên quan.
- Windows service hiện được bật tự khởi động; dừng/disable service trước khi không còn muốn
  website local được public.

Tài liệu Cloudflare chính thức:

- [Tạo locally-managed tunnel](https://developers.cloudflare.com/tunnel/advanced/local-management/create-local-tunnel/)
- [Cấu hình ingress](https://developers.cloudflare.com/tunnel/advanced/local-management/configuration-file/)
- [Chạy tunnel dưới dạng Windows service](https://developers.cloudflare.com/tunnel/advanced/local-management/as-a-service/windows/)
