# Vận hành, cập nhật và sao lưu

## 1. Lệnh vận hành thường dùng

```text
docker compose ps
docker compose logs --tail 200
docker compose logs -f app-view
docker compose restart app-view
docker compose restart app-api
docker compose stop
docker compose start
```

`docker compose down` xóa container/network nhưng giữ named volume. Tuy nhiên,
không dùng tùy tiện trong giờ hoạt động. Tuyệt đối không thêm `-v` khi chưa có
backup.

## 2. Các volume phải sao lưu

```text
docker volume ls
```

Ba volume quan trọng:

- `sqlserver_data`: database.
- `uploads_data`: ảnh sản phẩm, avatar, ảnh sale và QR.
- `keys_data`: khóa cookie/phiên đăng nhập.

Tên thật có thể có prefix theo tên thư mục project. Xem bằng:

```text
docker volume ls --filter name=sqlserver_data
docker volume ls --filter name=uploads_data
docker volume ls --filter name=keys_data
```

## 3. Backup SQL Server

Tạo thư mục backup trong container:

```text
docker exec sqlserver mkdir -p /var/opt/mssql/backup
```

Thực hiện backup. Thay `<SA_PASSWORD>` bằng giá trị thật trong môi trường an toàn:

```text
docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<SA_PASSWORD>" -C -Q "BACKUP DATABASE [DuAnTotNghiep_BazaizaiStore] TO DISK = N'/var/opt/mssql/backup/bazaizai.bak' WITH INIT, CHECKSUM"
```

Chép file ra host:

```text
docker cp sqlserver:/var/opt/mssql/backup/bazaizai.bak ./bazaizai.bak
```

Kiểm tra file backup có kích thước lớn hơn 0 và chép thêm một bản sang máy/ổ
lưu trữ khác. Không commit `.bak` lên Git.

## 4. Backup ảnh và khóa phiên

Có thể tạo archive trực tiếp từ volume bằng container tạm. Trước hết lấy đúng
tên volume từ `docker volume ls`, sau đó chạy:

```text
docker run --rm -v <UPLOADS_VOLUME>:/data:ro -v <THU_MUC_BACKUP_TUYET_DOI>:/backup alpine tar czf /backup/uploads.tar.gz -C /data .
docker run --rm -v <KEYS_VOLUME>:/data:ro -v <THU_MUC_BACKUP_TUYET_DOI>:/backup alpine tar czf /backup/keys.tar.gz -C /data .
```

Thư mục backup phải là đường dẫn tuyệt đối và đã tồn tại. File `keys.tar.gz` là
dữ liệu nhạy cảm, cần mã hóa và giới hạn quyền truy cập.

## 5. Quy trình cập nhật an toàn

1. Thông báo thời gian bảo trì nếu hệ thống đang có người dùng.
2. Backup database, ảnh và keys.
3. Lấy source mới bằng Git hoặc chép phiên bản mới.
4. So sánh `.env.example` và bổ sung biến mới vào `.env`; không ghi đè bí mật.
5. Kiểm tra cấu hình.
6. Build image.
7. Tạo lại ứng dụng, giữ nguyên volume.
8. Kiểm tra health, trang chủ, đăng nhập và ảnh.

Lệnh tham khảo:

```text
docker compose config --quiet
docker compose build app-api app-view
docker compose up -d --force-recreate app-api app-view
docker compose ps
docker compose logs --since 5m app-api app-view
```

Không cần tạo lại SQL Server khi chỉ đổi code API/View.

## 6. Kiểm tra định kỳ

Hằng ngày:

- Container đều `healthy`.
- Không có chuỗi `fail:` hoặc `Unhandled exception` trong log.
- Ổ đĩa chưa đầy.
- Website và `/api/Health` trả `200`.

Hằng tuần:

- Tạo backup và thử đọc danh sách file backup.
- Kiểm tra dung lượng volume ảnh.
- Kiểm tra log đăng nhập thất bại và lỗi gửi mail.

Hằng tháng:

- Cập nhật bản vá Docker host và các image/phụ thuộc sau khi kiểm thử staging.
- Thử restore backup trên máy/database thử nghiệm.
- Xoay vòng bí mật nếu có dấu hiệu lộ lọt.
