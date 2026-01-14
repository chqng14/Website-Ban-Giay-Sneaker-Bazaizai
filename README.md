# Website Bán Giày Sneaker Bazaizai

## Giới thiệu

Website này là một nền tảng bán giày sneaker trực tuyến, được xây dựng để cung cấp trải nghiệm mua sắm tốt nhất cho khách hàng.

## Các công nghệ sử dụng

- **JavaScript**: Sử dụng cho các tương tác phía client.
- **HTML**: Ngôn ngữ đánh dấu để xây dựng cấu trúc trang web.
- **CSS**: Định kiểu và thiết kế giao diện trang web.
- **SCSS**: Một phiên bản nâng cao của CSS.
- **C#**: Sử dụng cho phía server với ASP.NET.
- **Docker**: Container hóa ứng dụng để dễ dàng triển khai.

## Thư viện và Frameworks

- **Bootstrap**: Framework front-end để phát triển các dự án web responsive và mobile-first.
- **jQuery**: Thư viện JavaScript nhanh và nhỏ gọn để thao tác với DOM.
- **Perfect Scrollbar**: Thư viện giúp thêm scrollbar tùy chỉnh vào các phần tử HTML.
- **Popper.js**: Thư viện để quản lý các phần tử popover, tooltip và các vị trí gắn kèm.

## Cài đặt

### Clone repo

```bash
git clone https://github.com/chqng14/Website-Ban-Giay-Sneaker-Bazaizai.git
cd Website-Ban-Giay-Sneaker-Bazaizai
```

### Triển khai với Docker (Khuyến nghị)

📖 **[Xem hướng dẫn chi tiết triển khai với Docker tại DOCKER.md](./DOCKER.md)**

**Khởi chạy nhanh:**

```bash
# 1. Tạo file cấu hình môi trường
cp .env.example .env

# 2. Khởi chạy tất cả services
docker compose up -d --build

# 3. Truy cập ứng dụng
# Website: http://localhost:8080
# API: http://localhost:80
