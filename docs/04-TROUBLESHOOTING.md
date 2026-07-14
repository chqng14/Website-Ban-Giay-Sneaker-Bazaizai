# Xử lý sự cố

## Bước chẩn đoán chung

```text
docker compose ps
docker compose logs --tail 200 sqlserver
docker compose logs --tail 200 app-api
docker compose logs --tail 200 app-view
docker compose config --quiet
```

Không xóa volume chỉ để thử sửa lỗi.

## SQL Server không healthy

Kiểm tra:

- `SA_PASSWORD` có đủ mạnh không.
- Cổng `1433` có bị ứng dụng khác chiếm không.
- Máy còn đủ RAM và dung lượng ổ đĩa không.
- Volume database có quyền truy cập và không bị hỏng không.

Lệnh:

```text
docker compose logs --tail 300 sqlserver
docker compose restart sqlserver
```

Nếu đổi `SA_PASSWORD` trong `.env` nhưng dùng lại volume database cũ, mật khẩu
trong SQL Server không tự đổi theo. Hãy dùng lại mật khẩu cũ hoặc đổi mật khẩu
bên trong SQL Server theo quy trình quản trị thích hợp.

## API unhealthy hoặc trả 500

```text
docker compose logs --tail 300 app-api
curl -i http://localhost/api/Health
```

Nguyên nhân thường gặp:

- Database chưa healthy.
- Connection string sai.
- Migration database lỗi.
- `INTERNAL_API_KEY` thiếu hoặc API/View dùng hai giá trị khác nhau.

Sau khi sửa `.env`:

```text
docker compose up -d --force-recreate app-api app-view
```

## API trả 401

Toàn bộ API được bảo vệ mặc định bằng header `X-Internal-Api-Key`; chỉ
`GET /api/Health` cho phép gọi ẩn danh. `GET /api/Health/detailed` và các endpoint nghiệp
vụ phải trả `401` nếu thiếu/sai key.

Kiểm tra key hợp lệ bằng PowerShell (không ghi key trực tiếp vào lịch sử lệnh):

```powershell
$key = (Get-Content .env | Where-Object { $_ -match '^INTERNAL_API_KEY=' }) -replace '^INTERNAL_API_KEY=', ''
Invoke-WebRequest http://localhost/api/Health/detailed -Headers @{ 'X-Internal-Api-Key' = $key }
```

Nếu website gọi API bị `401`, kiểm tra `INTERNAL_API_KEY` đã vào cả `app-api` và
`app-view`, rồi recreate hai container. Không tạo endpoint bỏ qua xác thực để chữa tạm.

## Website trả 500

```text
docker compose logs --tail 300 app-view
```

Kiểm tra API trước. Nếu log có `Permission denied` tại `/app/keys` hoặc
`/app/storage`, đảm bảo đang dùng Dockerfile và image mới, sau đó build/recreate:

```text
docker compose build app-view
docker compose up -d --force-recreate app-view
```

Không chạy container bằng root chỉ để che lỗi quyền.

## Upload ảnh lỗi

| HTTP | Ý nghĩa thường gặp |
|---|---|
| `400` | File rỗng, quá lớn, sai đuôi hoặc nội dung không phải ảnh |
| `401` | Thiếu/sai `INTERNAL_API_KEY` |
| `409` | Tên file đã tồn tại |
| `500` | Lỗi ghi volume hoặc lỗi database |

Định dạng được chấp nhận: JPG, JPEG, PNG và WEBP. Giới hạn mặc định mỗi ảnh là
5 MB. Kiểm tra volume và quyền:

```text
docker exec app-api id
docker exec app-api ls -ld /app/storage /app/storage/AnhSanPham
docker exec app-view id
docker exec app-view ls -ld /app/storage /app/keys
```

API và View bình thường chạy bằng UID không-root `1654`.

## Ảnh mất sau restart

Kiểm tra hai container có cùng mount `uploads_data:/app/storage`:

```text
docker inspect app-api
docker inspect app-view
docker volume ls --filter name=uploads_data
```

Ảnh sẽ mất nếu trước đó đã chạy `docker compose down -v`, xóa volume thủ công,
hoặc deploy sang máy mới mà không restore volume ảnh.

## Không gửi được email

Kiểm tra:

- `MAIL_ADDRESS` đúng email SMTP.
- `MAIL_PASSWORD` là App Password, không phải mật khẩu Gmail thường.
- Tài khoản Google đã bật xác minh hai bước.
- Máy chủ cho phép kết nối ra `smtp.gmail.com:587`.
- Container đã được recreate sau khi sửa `.env`.

```text
docker compose up -d --force-recreate app-view
docker compose logs --tail 300 app-view
docker exec app-view ls -la /app/mailssave
```

Nếu có `.eml` trong `mailssave`, đó là thư fallback do gửi SMTP thất bại.

## Đăng nhập cũ bị mất sau recreate

Kiểm tra volume `keys_data` vẫn được mount tại `/app/keys`. Nếu volume khóa bị
xóa, cookie cũ không giải mã được và người dùng phải đăng nhập lại. Đây không
phải mất tài khoản trong database.

## Cổng bị chiếm

Đổi cổng host trong `.env`, ví dụ:

```env
VIEW_PORT=8081
API_PORT=8082
DB_PORT=1434
```

Sau đó:

```text
docker compose up -d --force-recreate
```

Không đổi cổng nội bộ `8080` trong `API_URL` nếu không sửa Compose/Dockerfile.
