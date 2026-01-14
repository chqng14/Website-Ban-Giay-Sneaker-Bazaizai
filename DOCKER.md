# Hướng Dẫn Triển Khai Ứng Dụng Với Docker

Tài liệu này hướng dẫn chi tiết cách triển khai hệ thống Website Bán Giày Sneaker Bazaizai bao gồm **API**, **View** (giao diện người dùng) và **Database** (SQL Server) bằng Docker.

## Mục Lục

1. [Yêu Cầu Hệ Thống](#1-yêu-cầu-hệ-thống)
2. [Cấu Trúc Dự Án](#2-cấu-trúc-dự-án)
3. [Cấu Hình Môi Trường](#3-cấu-hình-môi-trường)
4. [Khởi Chạy Ứng Dụng](#4-khởi-chạy-ứng-dụng)
5. [Truy Cập Ứng Dụng](#5-truy-cập-ứng-dụng)
6. [Các Lệnh Docker Thường Dùng](#6-các-lệnh-docker-thường-dùng)
7. [Xử Lý Sự Cố](#7-xử-lý-sự-cố)
8. [Triển Khai Production](#8-triển-khai-production)

---

## 1. Yêu Cầu Hệ Thống

### Phần mềm cần cài đặt

- **Docker**: Phiên bản 20.10 trở lên
- **Docker Compose**: Phiên bản 2.0 trở lên (thường đi kèm với Docker Desktop)
- **Git**: Để clone repository

### Kiểm tra phiên bản Docker

```bash
# Kiểm tra Docker version
docker --version
# Kết quả mẫu: Docker version 24.0.7, build afdd53b

# Kiểm tra Docker Compose version
docker compose version
# Kết quả mẫu: Docker Compose version v2.21.0
```

### Cài đặt Docker

#### Windows / macOS
- Tải và cài đặt [Docker Desktop](https://www.docker.com/products/docker-desktop/)

#### Linux (Ubuntu/Debian)
```bash
# Cập nhật hệ thống
sudo apt-get update

# Cài đặt Docker
sudo apt-get install docker.io docker-compose-plugin

# Khởi động Docker service
sudo systemctl start docker
sudo systemctl enable docker

# Thêm user hiện tại vào group docker (không cần sudo)
sudo usermod -aG docker $USER
```

---

## 2. Cấu Trúc Dự Án

```
Website-Ban-Giay-Sneaker-Bazaizai/
├── App_Api/                 # API Backend
│   ├── Dockerfile          # Docker build file cho API
│   ├── Controllers/        # Các API endpoints
│   └── ...
├── App_View/               # Frontend MVC
│   ├── Dockerfile         # Docker build file cho View
│   ├── Views/             # Các trang giao diện
│   └── ...
├── AppData/               # Data Layer (Entity Framework)
│   ├── DbContext/        # Database context
│   ├── Models/           # Database models
│   └── Migrations/       # Database migrations
├── docker-compose.yml    # Docker Compose configuration
├── .env.example         # File cấu hình môi trường mẫu
└── .dockerignore        # Các file bỏ qua khi build Docker
```

### Kiến trúc hệ thống

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   App_View  │────▶│   App_Api   │────▶│  SQL Server │
│  (Port 8080)│     │  (Port 80)  │     │ (Port 1433) │
│   Frontend  │     │   Backend   │     │  Database   │
└─────────────┘     └─────────────┘     └─────────────┘
```

---

## 3. Cấu Hình Môi Trường

### Bước 1: Clone Repository

```bash
git clone https://github.com/chqng14/Website-Ban-Giay-Sneaker-Bazaizai.git
cd Website-Ban-Giay-Sneaker-Bazaizai
```

### Bước 2: Tạo file .env

Tạo file `.env` từ file mẫu `.env.example`:

```bash
# Linux/macOS
cp .env.example .env

# Windows (Command Prompt)
copy .env.example .env

# Windows (PowerShell)
Copy-Item .env.example .env
```

### Bước 3: Cấu hình biến môi trường

Mở file `.env` và chỉnh sửa các giá trị theo nhu cầu:

```env
# Database Configuration - Cấu hình cơ sở dữ liệu
SA_PASSWORD=<Thay_bằng_mật_khẩu_mạnh>  # Mật khẩu SQL Server (xem yêu cầu bên dưới)
DB_NAME=DuAnTotNghiep_BazaizaiStore    # Tên database
DB_SERVER=sqlserver                    # Tên container SQL Server

# Database URL (connection string đầy đủ - tùy chọn, sẽ ghi đè các cài đặt ở trên)
# DATABASE_URL=Server=sqlserver;Database=DuAnTotNghiep_BazaizaiStore;User Id=sa;Password=<Thay_bằng_mật_khẩu>;TrustServerCertificate=True

# API Configuration - Cấu hình API
API_URL=http://app-api:80/          # URL nội bộ của API (trong Docker network)

# Application Settings - Cài đặt ứng dụng
ASPNETCORE_ENVIRONMENT=Production   # Môi trường: Development, Staging, Production
```

#### Lưu ý quan trọng về mật khẩu SQL Server

Mật khẩu SQL Server (`SA_PASSWORD`) phải đáp ứng các yêu cầu sau:
- Ít nhất **8 ký tự**
- Bao gồm **chữ hoa** (A-Z)
- Bao gồm **chữ thường** (a-z)
- Bao gồm **số** (0-9)
- Bao gồm **ký tự đặc biệt** (!@#$%^&*...)

**Tạo mật khẩu ngẫu nhiên mạnh (khuyến nghị):**
```bash
# Linux/macOS
openssl rand -base64 16 | tr -d '=' | head -c 16 && echo '@1Aa'

# Hoặc sử dụng công cụ tạo mật khẩu online có uy tín
```

---

## 4. Khởi Chạy Ứng Dụng

### Khởi chạy toàn bộ hệ thống

```bash
# Build và chạy tất cả services
docker compose up -d --build

# Xem logs của tất cả services
docker compose logs -f
```

### Khởi chạy từng service riêng lẻ

```bash
# Chỉ khởi chạy SQL Server
docker compose up -d sqlserver

# Chỉ khởi chạy API (sẽ tự động chạy SQL Server vì có dependency)
docker compose up -d app-api

# Chỉ khởi chạy View (sẽ tự động chạy SQL Server và API)
docker compose up -d app-view
```

### Theo dõi quá trình khởi động

```bash
# Xem logs của SQL Server
docker compose logs -f sqlserver

# Xem logs của API
docker compose logs -f app-api

# Xem logs của View
docker compose logs -f app-view

# Xem logs của tất cả services
docker compose logs -f
```

### Kiểm tra trạng thái các containers

```bash
# Liệt kê tất cả containers đang chạy
docker compose ps

# Kết quả mẫu:
# NAME         IMAGE                                       STATUS                   PORTS
# sqlserver    mcr.microsoft.com/mssql/server:2019-latest  Up 2 minutes (healthy)   0.0.0.0:1433->1433/tcp
# app-api      website-ban-giay-sneaker-bazaizai-app-api   Up 2 minutes             0.0.0.0:80->80/tcp
# app-view     website-ban-giay-sneaker-bazaizai-app-view  Up 1 minute              0.0.0.0:8080->80/tcp
```

---

## 5. Truy Cập Ứng Dụng

Sau khi các containers đã khởi động thành công, bạn có thể truy cập:

| Service | URL | Mô tả |
|---------|-----|-------|
| **Website (View)** | http://localhost:8080 | Giao diện người dùng |
| **API** | http://localhost:80 | Backend API |
| **SQL Server** | localhost:1433 | Database server |

### Kết nối SQL Server từ công cụ quản lý

Sử dụng các công cụ như **SQL Server Management Studio (SSMS)**, **Azure Data Studio** hoặc **DBeaver**:

- **Server**: `localhost,1433`
- **Authentication**: SQL Server Authentication
- **Username**: `sa`
- **Password**: Giá trị `SA_PASSWORD` trong file `.env`
- **Database**: `DuAnTotNghiep_BazaizaiStore`

---

## 6. Các Lệnh Docker Thường Dùng

### Quản lý containers

```bash
# Dừng tất cả services
docker compose stop

# Khởi động lại services (không rebuild)
docker compose start

# Khởi động lại services (với rebuild nếu có thay đổi)
docker compose up -d

# Dừng và xóa containers, networks
docker compose down

# Dừng, xóa containers, networks VÀ volumes (xóa database!)
docker compose down -v
```

### Rebuild và cập nhật

```bash
# Rebuild một service cụ thể
docker compose build app-api

# Rebuild và khởi động lại
docker compose up -d --build app-api

# Rebuild tất cả services
docker compose build

# Xóa cache và rebuild hoàn toàn
docker compose build --no-cache
```

### Xem logs và debug

```bash
# Xem logs realtime
docker compose logs -f

# Xem logs của một service
docker compose logs -f app-api

# Xem 100 dòng log cuối cùng
docker compose logs --tail=100 app-api

# Truy cập vào container (bash shell)
docker exec -it app-api /bin/bash

# Truy cập vào SQL Server container (sử dụng biến môi trường từ .env)
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD"
```

### Quản lý images và volumes

```bash
# Liệt kê tất cả images
docker images

# Xóa images không sử dụng
docker image prune

# Liệt kê volumes
docker volume ls

# Xóa volumes không sử dụng
docker volume prune
```

---

## 7. Xử Lý Sự Cố

### Lỗi 1: Container SQL Server không khởi động

**Triệu chứng**: Container `sqlserver` restart liên tục hoặc không healthy.

**Nguyên nhân**: Mật khẩu không đủ phức tạp.

**Giải pháp**:
```bash
# Kiểm tra logs
docker compose logs sqlserver

# Đặt mật khẩu đủ mạnh trong file .env
# SA_PASSWORD phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt
```

### Lỗi 2: API không kết nối được database

**Triệu chứng**: API báo lỗi connection refused hoặc cannot connect to database.

**Giải pháp**:
```bash
# Đợi SQL Server khởi động hoàn toàn (khoảng 30-60 giây)
docker compose logs -f sqlserver

# Kiểm tra SQL Server đã healthy chưa
docker compose ps

# Restart API service
docker compose restart app-api
```

### Lỗi 3: Port đã được sử dụng

**Triệu chứng**: Error response from daemon: port is already allocated.

**Giải pháp**:
```bash
# Tìm process đang sử dụng port
# Linux/macOS
lsof -i :80
lsof -i :8080
lsof -i :1433

# Windows
netstat -ano | findstr :80
netstat -ano | findstr :8080
netstat -ano | findstr :1433

# Dừng process hoặc thay đổi port trong docker-compose.yml
```

### Lỗi 4: Không đủ dung lượng ổ đĩa

**Triệu chứng**: no space left on device.

**Giải pháp**:
```bash
# Dọn dẹp Docker resources không sử dụng
docker system prune -a --volumes

# Hoặc chỉ dọn images không sử dụng
docker image prune -a
```

### Lỗi 5: Build thất bại

**Triệu chứng**: Lỗi khi build Docker image.

**Giải pháp**:
```bash
# Xóa cache và rebuild
docker compose build --no-cache

# Kiểm tra Dockerfile syntax
docker build -t test-app-api -f App_Api/Dockerfile .
```

---

## 8. Triển Khai Production

### Cấu hình bảo mật

1. **Sử dụng mật khẩu mạnh, ngẫu nhiên**
   ```bash
   # Tạo mật khẩu ngẫu nhiên và lưu vào .env
   echo "SA_PASSWORD=$(openssl rand -base64 16 | tr -d '=' | head -c 16)@1Aa" >> .env
   ```

2. **Sử dụng HTTPS** - Cấu hình reverse proxy (Nginx, Traefik, Caddy)

3. **Giới hạn port expose** - Chỉ expose port cần thiết ra ngoài

### Cấu hình docker-compose cho Production

Tạo file `docker-compose.prod.yml`:

```yaml
version: '3.8'

services:
  app-api:
    restart: always
    deploy:
      resources:
        limits:
          memory: 512M
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"

  app-view:
    restart: always
    deploy:
      resources:
        limits:
          memory: 512M
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"

  sqlserver:
    restart: always
    deploy:
      resources:
        limits:
          memory: 2G
```

Chạy với cấu hình production:
```bash
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

### Backup Database

```bash
# Load biến môi trường từ file .env
source .env

# Tạo thư mục backup trong container (nếu chưa có)
docker exec sqlserver mkdir -p /var/opt/mssql/backup

# Tạo backup (sử dụng biến môi trường SA_PASSWORD)
docker exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" \
  -Q "BACKUP DATABASE [DuAnTotNghiep_BazaizaiStore] TO DISK = N'/var/opt/mssql/backup/backup.bak' WITH NOFORMAT, NOINIT, NAME = 'Full Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10"

# Copy backup ra host
docker cp sqlserver:/var/opt/mssql/backup/backup.bak ./backup.bak
```

### Restore Database

```bash
# Load biến môi trường từ file .env
source .env

# Copy backup vào container
docker cp ./backup.bak sqlserver:/var/opt/mssql/backup/backup.bak

# Restore (sử dụng biến môi trường SA_PASSWORD)
docker exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" \
  -Q "RESTORE DATABASE [DuAnTotNghiep_BazaizaiStore] FROM DISK = N'/var/opt/mssql/backup/backup.bak' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 5"
```

---

## Liên Hệ và Hỗ Trợ

Nếu gặp vấn đề khi triển khai, vui lòng tạo Issue trên GitHub repository hoặc liên hệ team phát triển.

**Repository**: https://github.com/chqng14/Website-Ban-Giay-Sneaker-Bazaizai
