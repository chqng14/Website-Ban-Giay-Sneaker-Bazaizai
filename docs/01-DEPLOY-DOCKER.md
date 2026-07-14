# Deploy bằng Docker trên máy khác

Tài liệu này áp dụng cho một máy mới chạy Windows, Linux hoặc macOS. Production
Linux được khuyến nghị.

## 1. Yêu cầu

- Docker Engine/Desktop có Docker Compose v2.
- Git nếu lấy source từ repository.
- Tối thiểu 4 GB RAM trống; nên có 8 GB RAM cho SQL Server và quá trình build.
- Tối thiểu 15 GB ổ đĩa trống, chưa tính dữ liệu và ảnh phát sinh.
- Cổng mặc định: website `8080`, API `80`, SQL Server `1433`.

Kiểm tra:

```text
docker --version
docker compose version
```

## 2. Chuyển source sang máy mới

Cách dùng Git:

```text
git clone <URL_REPOSITORY>
cd Website-Ban-Giay-Sneaker-Bazaizai
```

Nếu chép file thủ công, không chép các thư mục `bin`, `obj`, `.vs` và không gửi
file `.env` qua kênh không an toàn.

## 3. Tạo file cấu hình

Linux/macOS:

```text
cp .env.example .env
```

Windows PowerShell:

```powershell
Copy-Item .env.example .env
```

Mở `.env` và thay ít nhất hai giá trị bắt buộc:

```env
SA_PASSWORD=ThayBangMatKhauSQLManh
INTERNAL_API_KEY=ThayBangChuoiNgauNhienDaiItNhat32KyTu
```

`SA_PASSWORD` nên dài ít nhất 12 ký tự, có chữ hoa, chữ thường, số và ký tự đặc
biệt. Hai giá trị trên phải khác nhau.

Các giá trị cơ bản:

```env
DB_NAME=DuAnTotNghiep_BazaizaiStore
DB_SERVER=sqlserver
DB_PORT=1433
API_PORT=80
VIEW_PORT=8080
ASPNETCORE_ENVIRONMENT=Production
API_URL=http://app-api:8080/
```

Không đổi `DB_SERVER=sqlserver` hoặc `API_URL=http://app-api:8080/` khi các
container vẫn chạy trong cùng file Compose.

### Tạo admin lần đầu

Chỉ điền các biến dưới đây khi database mới chưa có admin:

```env
ADMIN_EMAIL=admin@tenmien-cua-ban.vn
ADMIN_USERNAME=Admin
ADMIN_PASSWORD=MatKhauAdminManh
ADMIN_ENSURE_PASSWORD=true
```

`ADMIN_ENSURE_PASSWORD=true` dùng cho lần đầu tạo tài khoản hoặc khi cần chủ động đặt lại
mật khẩu của tài khoản seed đã tồn tại. Hàm seed đồng bộ tên đăng nhập, xác nhận email,
mở khóa tài khoản, tắt 2FA và bảo đảm role `Admin`.

Script `deploy-docker.ps1` tự đổi cờ này về `false` và recreate `app-view` sau khi seed thành
công. Sau đó nên xóa giá trị `ADMIN_PASSWORD` khỏi `.env` nếu quy trình vận hành không cần
seed lại mật khẩu.

### Cấu hình mail

Nếu cần xác nhận tài khoản/quên mật khẩu, điền:

```env
MAIL_ADDRESS=mail-cua-hang@gmail.com
MAIL_PASSWORD=app-password-cua-google
```

Xem quy trình đầy đủ trong [hướng dẫn đổi email](02-CHANGE-SYSTEM-EMAIL.md).

## 4. Tự kiểm tra và deploy bằng một lệnh

Khuyến nghị dùng PowerShell 7 (`pwsh`) trên Windows, Linux hoặc macOS:

```powershell
pwsh ./deploy-docker.ps1
```

Trên Windows PowerShell:

```powershell
.\deploy-docker.ps1
```

Script tự thực hiện các việc sau:

1. Kiểm tra Docker Engine và Docker Compose v2.
2. Kiểm tra `.env`, độ mạnh của mật khẩu SQL, internal API key, cổng và cấu hình admin.
3. Xác thực file Compose, build và khởi động toàn bộ container.
4. Chờ các container healthy.
5. Kiểm tra website, API health, HTTP `401` khi thiếu key và HTTP `200` khi key hợp lệ.
6. Hoàn tất seed admin một lần rồi tự tắt `ADMIN_ENSURE_PASSWORD`.

Tùy chọn:

```powershell
pwsh ./deploy-docker.ps1 -Pull             # tải base image mới trước khi deploy
pwsh ./deploy-docker.ps1 -NoBuild          # chỉ recreate/chạy, không build lại source
pwsh ./deploy-docker.ps1 -TimeoutSeconds 600
```

Nếu `.env` chưa tồn tại, script tạo từ `.env.example` và dừng lại để người vận hành điền
secret thật. Script không tự sinh hoặc in secret ra màn hình.

### Cách thủ công

```text
docker compose config --quiet
docker compose up -d --build --wait
docker compose ps
```

Lần đầu có thể mất vài phút vì Docker phải tải SQL Server, .NET runtime và build
hai ứng dụng. Chỉ tiếp tục khi cả ba container hiển thị `healthy`.

Xem log nếu cần:

```text
docker compose logs --tail 200 sqlserver
docker compose logs --tail 200 app-api
docker compose logs --tail 200 app-view
```

## 5. Kiểm tra sau deploy

Trên máy deploy:

- Website: `http://localhost:8080`
- API health: `http://localhost/api/Health`

Linux/macOS:

```text
curl -f http://localhost/api/Health
curl -f http://localhost:8080/
```

Windows PowerShell:

```powershell
Invoke-WebRequest -UseBasicParsing http://localhost/api/Health
Invoke-WebRequest -UseBasicParsing http://localhost:8080/
```

Sau đó thử thủ công:

1. Mở trang chủ và trang sản phẩm.
2. Đăng nhập admin.
3. Tải một ảnh sản phẩm thử.
4. Tạo lại container bằng `docker compose up -d --force-recreate app-api app-view`.
5. Xác nhận ảnh vẫn xem được và phiên đăng nhập hoạt động.

## 6. Mở website cho máy khác truy cập

Trên firewall chỉ nên mở cổng website/reverse proxy. Không mở `1433` ra Internet.
API cũng nên được chặn từ bên ngoài vì website gọi API qua mạng Docker nội bộ.

Nếu chưa có reverse proxy, truy cập tạm bằng:

```text
http://IP_MAY_CHU:8080
```

Production nên dùng tên miền và HTTPS:

```text
Internet -> HTTPS 443 -> Caddy/Nginx -> app-view:8080
                                |
                                +-> app-api:8080 (mạng nội bộ)
```

## 7. Không làm những việc sau

- Không chạy `docker compose down -v` nếu muốn giữ database và ảnh.
- Không sửa dữ liệu trực tiếp trong thư mục quản lý volume của Docker.
- Không commit `.env`.
- Không dùng mật khẩu mẫu từ `.env.example`.
- Không công khai cổng SQL Server `1433`.

Quy trình update và backup nằm trong
[03-OPERATIONS-BACKUP-UPDATE.md](03-OPERATIONS-BACKUP-UPDATE.md).
