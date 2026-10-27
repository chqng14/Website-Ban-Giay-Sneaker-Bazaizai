# Đổi `bazaizaistore@gmail.com` sang email khác

Trong project hiện tại, email cửa hàng có ba vai trò khác nhau. Cần xử lý đủ cả
ba để tránh tình trạng gửi mail bằng địa chỉ mới nhưng giao diện hoặc nghiệp vụ
voucher vẫn tìm địa chỉ cũ.

## 1. Đổi tài khoản gửi email SMTP

### Với Gmail

1. Đăng nhập tài khoản Google mới.
2. Bật xác minh hai bước.
3. Tạo App Password dành riêng cho ứng dụng.
4. Không dùng mật khẩu đăng nhập Google thông thường.
5. Điền vào `.env` trên máy deploy:

```env
MAIL_ADDRESS=mail-moi@gmail.com
MAIL_PASSWORD=app-password-moi
```

Ứng dụng hiện dùng:

- SMTP host: `smtp.gmail.com`
- Port: `587`
- Bảo mật: STARTTLS
- Display name: `Bazaizai`

Các giá trị host/port/display name nằm trong
`App_View/appsettings.json`. Nếu chuyển sang nhà cung cấp khác, sửa mục
`MailSettings` tương ứng rồi build lại image `app-view`.

Áp dụng cấu hình:

```text
docker compose up -d --force-recreate app-view
docker compose logs --tail 100 app-view
```

Không cần build lại nếu chỉ đổi `MAIL_ADDRESS` hoặc `MAIL_PASSWORD` trong `.env`.

### Kiểm tra gửi mail

Thử chức năng đăng ký, xác nhận email hoặc quên mật khẩu. Sau đó kiểm tra:

```text
docker compose logs --tail 200 app-view
```

Nếu SMTP thất bại, `SendMailService` lưu một file `.eml` vào thư mục
`mailssave` bên trong container và ghi lỗi vào log. File fallback này không được
coi là bằng chứng email đã gửi thành công.

## 2. Đổi email của tài khoản admin hiện có

`ADMIN_EMAIL` chỉ dùng để tạo admin lần đầu trên database mới. Sửa biến này
không tự đổi tài khoản đã tồn tại.

Với database đang sử dụng:

1. Đăng nhập bằng admin hiện tại.
2. Vào trang quản lý hồ sơ/tài khoản.
3. Đổi email sang địa chỉ mới và hoàn tất xác nhận nếu hệ thống yêu cầu.
4. Đăng xuất, đăng nhập lại và kiểm tra quyền Admin.

Nếu không đăng nhập được, dùng chức năng quên mật khẩu sau khi SMTP mới hoạt
động. Không sửa trực tiếp `PasswordHash` trong SQL Server.

Với database hoàn toàn mới, cấu hình:

```env
ADMIN_EMAIL=mail-moi@gmail.com
ADMIN_USERNAME=Admin
ADMIN_PASSWORD=mat-khau-admin-manh
```

Sau lần seed thành công, xóa `ADMIN_PASSWORD` khỏi `.env`.

## 3. Thay các tham chiếu email cửa hàng còn ghi cứng

Các vị trí đang dùng trực tiếp `bazaizaistore@gmail.com`:

| File | Mục đích |
|---|---|
| `App_View/Views/Shared/_Layout.cshtml` | Link mail và email ở giao diện |
| `App_View/Views/Shared/_LayoutUser.cshtml` | Email hiển thị ở giao diện user |
| `App_View/Areas/Admin/Controllers/VouchersController.cs` | Tìm admin sở hữu voucher tại quầy |
| `App_View/Areas/Admin/Pages/User/UserDetail.cshtml` | Kiểm tra tài khoản admin chủ |
| `App_View/Areas/Admin/Pages/User/DanhSachQuanLy.cshtml` | Kiểm tra tài khoản admin chủ |

Thay đúng chuỗi email cũ bằng email admin/cửa hàng mới trong các file này, sau
đó build và tạo lại `app-view`:

```text
docker compose build app-view
docker compose up -d --force-recreate app-view
```

Kiểm tra không còn tham chiếu active code:

Windows PowerShell:

```powershell
rg -n "bazaizaistore@gmail\.com" App_View -g "*.cs" -g "*.cshtml"
```

Linux/macOS cũng dùng cùng lệnh nếu đã cài `ripgrep`.

Thư mục `App_View/mailssave` có thể chứa các email `.eml` lịch sử. Không cần sửa
những file này để hệ thống hoạt động; nên xóa chúng khỏi source và không commit
email thực tế vào Git.

## 4. Phân biệt các biến dễ nhầm

| Biến | Công dụng |
|---|---|
| `MAIL_ADDRESS` | Tài khoản SMTP gửi mail |
| `MAIL_PASSWORD` | App Password SMTP |
| `ADMIN_EMAIL` | Chỉ seed admin khi database mới |
| Email trong database | Email đăng nhập của admin đang tồn tại |
| Email ghi trong Razor/controller | Hiển thị UI và logic admin/voucher cũ |

## 5. Checklist hoàn tất

- [ ] SMTP gửi được email thật.
- [ ] Email admin trong database đã đổi và xác nhận.
- [ ] Admin mới đăng nhập được và vẫn có role `Admin`.
- [ ] In/lọc voucher tại quầy không báo lỗi.
- [ ] Footer/header hiển thị email mới.
- [ ] Không còn email cũ trong file `.cs`/`.cshtml`.
- [ ] Không commit `.env` hoặc App Password.
