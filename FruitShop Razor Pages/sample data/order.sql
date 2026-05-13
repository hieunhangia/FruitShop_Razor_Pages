INSERT INTO "CartItems"("Quantity", "CustomerId", "ProductId", "IsSelected")
VALUES (4, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), true),
       (5, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'), false),
       (3, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'), true),
       (6, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'), false),
       (4, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài tượng'), true),
       (3, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), false),
       (5, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'), true),
       (6, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam vàng'), false),
       (4, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'), true),
       (3, (SELECT "Id" from "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối tiêu'), false);


-- ==============================================================================
-- 2. THÊM DỮ LIỆU THANH TOÁN QR CODE MẪU
-- ==============================================================================
INSERT INTO "OrderQrCodePaymentData" ("Id", "PaymentLink", "ExpirationDate", "PaymentDate")
VALUES (1000, 'https://pay.app.com/qr/1001', '2026-05-20 10:00:00+00', '2026-05-13 09:15:00+00'),
       (1001, 'https://pay.app.com/qr/1002', '2026-05-20 11:00:00+00', '2026-05-13 10:20:00+00'),
       (1002, 'https://pay.app.com/qr/1003', '2026-05-21 12:00:00+00', NULL), -- Chưa thanh toán
       (1003, 'https://pay.app.com/qr/1004', '2026-05-21 15:00:00+00', NULL);

-- ==============================================================================
-- 3. THÊM 30 ĐƠN HÀNG (ORDERS)
-- OrderStatus: 0(PendingConf), 1(PendingPay), 2(Processing), 3(Shipping), 4(Delivered), 5(Cancelled)
-- PaymentMethod: 0(COD), 1(QRCode)
-- ==============================================================================
INSERT INTO "Orders" ("Id", "OrderDate", "OrderStatus", "PaymentMethod", "TotalAmount", "ShippingAddressSnapshot",
                      "CustomerId", "ShipperId", "QrCodePaymentDataId")
VALUES
-- Trạng thái: Delivered (4) - Đã giao thành công
(1001, '2026-05-01 08:00:00+00', 4, 1, 200000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com'), 1000),
(1002, '2026-05-02 09:30:00+00', 4, 0, 150000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "45 Ngõ B",
  "CommuneName": "Phường Ngọc Hà",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com'), NULL),
(1003, '2026-05-03 14:20:00+00', 4, 1, 350000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Chung cư C",
  "CommuneName": "Phường Giảng Võ",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com'), 1001),
(1004, '2026-05-04 10:15:00+00', 4, 0, 80000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0911222333",
  "SpecificAddress": "Khu phố 1",
  "CommuneName": "Phường Hoàn Kiếm",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com'), NULL),
(1005, '2026-05-05 16:45:00+00', 4, 0, 420000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Số 10",
  "CommuneName": "Phường Phú Thượng",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper5@app.com'), NULL),

-- Trạng thái: Shipping (3) - Đang giao hàng
(1006, '2026-05-10 08:00:00+00', 3, 0, 115000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Khu đô thị D",
  "CommuneName": "Phường Cửa Nam",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com'), NULL),
(1007, '2026-05-11 09:00:00+00', 3, 0, 260000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Số 12",
  "CommuneName": "Phường Tây Hồ",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com'), NULL),
(1008, '2026-05-11 11:30:00+00', 3, 0, 180000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com'), NULL),
(1009, '2026-05-12 14:00:00+00', 3, 0, 310000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Tòa nhà F",
  "CommuneName": "Phường Cầu Giấy",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com'), NULL),
(1010, '2026-05-12 16:00:00+00', 3, 0, 95000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Ngõ 99",
  "CommuneName": "Phường Yên Hòa",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper5@app.com'), NULL),

-- Trạng thái: Processing (2) - Đang xử lý (Chưa có shipper)
(1011, '2026-05-12 18:00:00+00', 2, 0, 150000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1012, '2026-05-12 19:30:00+00', 2, 0, 220000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1013, '2026-05-13 07:00:00+00', 2, 0, 135000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1014, '2026-05-13 08:15:00+00', 2, 0, 480000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1015, '2026-05-13 08:45:00+00', 2, 0, 90000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Trạng thái: PendingPayment (1) - Chờ thanh toán (Thanh toán QR)
(1016, '2026-05-13 09:00:00+00', 1, 1, 550000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, 1002),
(1017, '2026-05-13 09:10:00+00', 1, 1, 210000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, 1003),

-- Trạng thái: PendingConfirmation (0) - Chờ xác nhận (Vừa đặt xong)
(1018, '2026-05-13 09:20:00+00', 0, 0, 75000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1019, '2026-05-13 09:25:00+00', 0, 0, 320000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1020, '2026-05-13 09:30:00+00', 0, 0, 145000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- 10 Đơn hàng ngẫu nhiên khác (Trộn lẫn các trạng thái)
(1021, '2026-04-15 10:00:00+00', 4, 0, 160000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com'), NULL),
(1022, '2026-04-20 11:00:00+00', 5, 0, 230000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL), -- Cancelled
(1023, '2026-04-25 12:00:00+00', 4, 0, 410000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com'), NULL),
(1024, '2026-04-28 13:00:00+00', 5, 0, 85000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL), -- Cancelled
(1025, '2026-05-08 14:00:00+00', 3, 0, 195000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com'), NULL),
(1026, '2026-05-09 15:00:00+00', 4, 0, 315000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com'), NULL),
(1027, '2026-05-10 16:00:00+00', 2, 0, 275000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1028, '2026-05-11 17:00:00+00', 5, 0, 110000, '{
  "RecipientName": "Tay Trừ Tà",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL), -- Cancelled
(1029, '2026-05-12 18:00:00+00', 0, 0, 500000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1030, '2026-05-13 08:30:00+00', 0, 0, 65000, '{
  "RecipientName": "Hóa Thanh Sư",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL);

-- ==============================================================================
-- 4. THÊM CHI TIẾT ĐƠN HÀNG (ORDER ITEMS)
-- ==============================================================================
INSERT INTO "OrderItem" ("OrderId", "ProductId", "Quantity", "ProductSnapshot")
VALUES (1001, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 2, '{
  "Name": "Táo đỏ",
  "ImageUrl": "http://localhost:9000/public/images/products/1.jpg",
  "ProductUnitName": "Kg",
  "UnitPrice": 50000
}'::jsonb),
       (1001, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageUrl": "http://localhost:9000/public/images/products/24.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),

       (1002, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), 1, '{
         "Name": "Cherry",
         "ImageUrl": "http://localhost:9000/public/images/products/27.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 150000
       }'::jsonb),

       (1003, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'), 5, '{
         "Name": "Dâu tây",
         "ImageUrl": "http://localhost:9000/public/images/products/6.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 70000
       }'::jsonb),

       (1004, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'), 1, '{
         "Name": "Cam sành",
         "ImageUrl": "http://localhost:9000/public/images/products/3.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 35000
       }'::jsonb),
       (1004, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 1, '{
         "Name": "Nho tím",
         "ImageUrl": "http://localhost:9000/public/images/products/11.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),

       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 2, '{
         "Name": "Nho xanh",
         "ImageUrl": "http://localhost:9000/public/images/products/9.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),
       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'), 2, '{
         "Name": "Dưa lưới",
         "ImageUrl": "http://localhost:9000/public/images/products/8.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),
       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'), 2, '{
         "Name": "Bưởi da xanh",
         "ImageUrl": "http://localhost:9000/public/images/products/17.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 60000
       }'::jsonb),

       (1006, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài cát'), 1, '{
         "Name": "Xoài cát",
         "ImageUrl": "http://localhost:9000/public/images/products/12.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1006, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài tượng'), 1, '{
         "Name": "Xoài tượng",
         "ImageUrl": "http://localhost:9000/public/images/products/13.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),

       (1007, (SELECT "Id" FROM "Products" WHERE "Name" = 'Kiwi'), 2, '{
         "Name": "Kiwi",
         "ImageUrl": "http://localhost:9000/public/images/products/21.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 90000
       }'::jsonb),
       (1007, (SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'), 1, '{
         "Name": "Vải thiều",
         "ImageUrl": "http://localhost:9000/public/images/products/30.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 80000
       }'::jsonb),

       (1008, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho đỏ'), 2, '{
         "Name": "Nho đỏ",
         "ImageUrl": "http://localhost:9000/public/images/products/10.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 90000
       }'::jsonb),

       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Thanh long đỏ'), 3, '{
         "Name": "Thanh long đỏ",
         "ImageUrl": "http://localhost:9000/public/images/products/19.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 40000
       }'::jsonb),
       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 1, '{
         "Name": "Nho tím",
         "ImageUrl": "http://localhost:9000/public/images/products/11.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),
       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'), 1, '{
         "Name": "Nhãn",
         "ImageUrl": "http://localhost:9000/public/images/products/31.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),

       (1010, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'), 1, '{
         "Name": "Mít",
         "ImageUrl": "http://localhost:9000/public/images/products/20.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1010, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'), 1, '{
         "Name": "Dưa hấu",
         "ImageUrl": "http://localhost:9000/public/images/products/7.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),

       (1011, (SELECT "Id" FROM "Products" WHERE "Name" = 'Hồng giòn'), 2, '{
         "Name": "Hồng giòn",
         "ImageUrl": "http://localhost:9000/public/images/products/25.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 70000
       }'::jsonb),

       (1012, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mơ'), 2, '{
         "Name": "Mơ",
         "ImageUrl": "http://localhost:9000/public/images/products/22.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1012, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mãng cầu'), 1, '{
         "Name": "Mãng cầu",
         "ImageUrl": "http://localhost:9000/public/images/products/32.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 70000
       }'::jsonb),

       (1013, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa chuột'), 2, '{
         "Name": "Dưa chuột",
         "ImageUrl": "http://localhost:9000/public/images/products/16.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 12000
       }'::jsonb),
       (1013, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dừa xiêm'), 1, '{
         "Name": "Dừa xiêm",
         "ImageUrl": "http://localhost:9000/public/images/products/15.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 40000
       }'::jsonb),

       (1014, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), 2, '{
         "Name": "Cherry",
         "ImageUrl": "http://localhost:9000/public/images/products/27.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 150000
       }'::jsonb),
       (1014, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 2, '{
         "Name": "Nho xanh",
         "ImageUrl": "http://localhost:9000/public/images/products/9.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),

       (1015, (SELECT "Id" FROM "Products" WHERE "Name" = 'Ổi'), 3, '{
         "Name": "Ổi",
         "ImageUrl": "http://localhost:9000/public/images/products/23.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),

       (1016, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 11, '{
         "Name": "Táo đỏ",
         "ImageUrl": "http://localhost:9000/public/images/products/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1017, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'), 3, '{
         "Name": "Dâu tây",
         "ImageUrl": "http://localhost:9000/public/images/products/6.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 70000
       }'::jsonb),

       (1018, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'), 3, '{
         "Name": "Dưa hấu",
         "ImageUrl": "http://localhost:9000/public/images/products/7.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),

       (1019, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 4, '{
         "Name": "Nho tím",
         "ImageUrl": "http://localhost:9000/public/images/products/11.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 80000
       }'::jsonb),

       (1020, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam vàng'), 2, '{
         "Name": "Cam vàng",
         "ImageUrl": "http://localhost:9000/public/images/products/4.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 40000
       }'::jsonb),
       (1020, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi hồng'), 1, '{
         "Name": "Bưởi hồng",
         "ImageUrl": "http://localhost:9000/public/images/products/29.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 65000
       }'::jsonb),

       (1021, (SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'), 2, '{
         "Name": "Vải thiều",
         "ImageUrl": "http://localhost:9000/public/images/products/30.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 80000
       }'::jsonb),

       (1022, (SELECT "Id" FROM "Products" WHERE "Name" = 'Chôm chôm'), 2, '{
         "Name": "Chôm chôm",
         "ImageUrl": "http://localhost:9000/public/images/products/18.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1022, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'), 1, '{
         "Name": "Bưởi da xanh",
         "ImageUrl": "http://localhost:9000/public/images/products/17.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 60000
       }'::jsonb),

       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Kiwi'), 3, '{
         "Name": "Kiwi",
         "ImageUrl": "http://localhost:9000/public/images/products/21.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 90000
       }'::jsonb),
       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'), 1, '{
         "Name": "Táo xanh",
         "ImageUrl": "http://localhost:9000/public/images/products/28.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),
       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 1, '{
         "Name": "Nho xanh",
         "ImageUrl": "http://localhost:9000/public/images/products/9.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),

       (1024, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài keo'), 1, '{
         "Name": "Xoài keo",
         "ImageUrl": "http://localhost:9000/public/images/products/14.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1024, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'), 1, '{
         "Name": "Cam sành",
         "ImageUrl": "http://localhost:9000/public/images/products/3.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 35000
       }'::jsonb),

       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageUrl": "http://localhost:9000/public/images/products/24.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),
       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'), 1, '{
         "Name": "Dưa lưới",
         "ImageUrl": "http://localhost:9000/public/images/products/8.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),
       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'), 1, '{
         "Name": "Mít",
         "ImageUrl": "http://localhost:9000/public/images/products/20.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'), 2, '{
         "Name": "Nhãn",
         "ImageUrl": "http://localhost:9000/public/images/products/31.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Đu đủ'), 1, '{
         "Name": "Đu đủ",
         "ImageUrl": "http://localhost:9000/public/images/products/26.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),
       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageUrl": "http://localhost:9000/public/images/products/24.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),

       (1027, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo vàng'), 2, '{
         "Name": "Táo vàng",
         "ImageUrl": "http://localhost:9000/public/images/products/2.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1027, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'), 1, '{
         "Name": "Táo xanh",
         "ImageUrl": "http://localhost:9000/public/images/products/28.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),

       (1028, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 2, '{
         "Name": "Nho tím",
         "ImageUrl": "http://localhost:9000/public/images/products/11.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1028, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa chuột'), 1, '{
         "Name": "Dưa chuột",
         "ImageUrl": "http://localhost:9000/public/images/products/16.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 12000
       }'::jsonb),

       (1029, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), 3, '{
         "Name": "Cherry",
         "ImageUrl": "http://localhost:9000/public/images/products/27.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 150000
       }'::jsonb),
       (1029, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "http://localhost:9000/public/images/products/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1030, (SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối tiêu'), 2, '{
         "Name": "Chuối tiêu",
         "ImageUrl": "http://localhost:9000/public/images/products/5.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),
       (1030, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 1, '{
         "Name": "Nho tím",
         "ImageUrl": "http://localhost:9000/public/images/products/11.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 40000
       }'::jsonb);

-- ==============================================================================
-- 5. THÊM LỊCH SỬ VẬN CHUYỂN (ORDER SHIPPINGS)
-- Dành cho những đơn hàng đang giao (3) và đã giao (4).
-- ShippingStatus: 0(PickingUp), 1(PickedUp), 2(Shipping), 3(Delivered)
-- ==============================================================================
INSERT INTO "OrderShipping" ("OrderId", "ShippingStatus", "OccurredAt")
VALUES
-- Đơn hàng 1001 (Đã giao)
(1001, 0, '2026-05-01 08:30:00+00'),
(1001, 1, '2026-05-01 10:00:00+00'),
(1001, 2, '2026-05-01 13:00:00+00'),
(1001, 3, '2026-05-01 15:30:00+00'),

-- Đơn hàng 1002 (Đã giao)
(1002, 0, '2026-05-02 10:00:00+00'),
(1002, 1, '2026-05-02 11:30:00+00'),
(1002, 2, '2026-05-02 14:00:00+00'),
(1002, 3, '2026-05-02 17:00:00+00'),

-- Đơn hàng 1003 (Đã giao)
(1003, 0, '2026-05-03 15:00:00+00'),
(1003, 1, '2026-05-03 16:30:00+00'),
(1003, 2, '2026-05-04 08:00:00+00'),
(1003, 3, '2026-05-04 10:30:00+00'),

-- Đơn hàng 1004 (Đã giao)
(1004, 0, '2026-05-04 11:00:00+00'),
(1004, 1, '2026-05-04 14:00:00+00'),
(1004, 2, '2026-05-04 16:30:00+00'),
(1004, 3, '2026-05-05 09:00:00+00'),

-- Đơn hàng 1005 (Đã giao)
(1005, 0, '2026-05-05 17:15:00+00'),
(1005, 1, '2026-05-06 08:30:00+00'),
(1005, 2, '2026-05-06 10:00:00+00'),
(1005, 3, '2026-05-06 14:30:00+00'),

-- Đơn hàng 1021 (Đã giao)
(1021, 0, '2026-04-15 11:00:00+00'),
(1021, 1, '2026-04-15 13:00:00+00'),
(1021, 2, '2026-04-15 15:00:00+00'),
(1021, 3, '2026-04-16 09:00:00+00'),

-- Đơn hàng 1023 (Đã giao)
(1023, 0, '2026-04-25 13:00:00+00'),
(1023, 1, '2026-04-25 15:30:00+00'),
(1023, 2, '2026-04-26 08:00:00+00'),
(1023, 3, '2026-04-26 11:00:00+00'),

-- Đơn hàng 1026 (Đã giao)
(1026, 0, '2026-05-09 16:00:00+00'),
(1026, 1, '2026-05-10 08:30:00+00'),
(1026, 2, '2026-05-10 10:00:00+00'),
(1026, 3, '2026-05-10 14:00:00+00'),

-- Đơn hàng 1006 (Đang giao hàng - Shipping)
(1006, 0, '2026-05-10 09:00:00+00'),
(1006, 1, '2026-05-10 11:00:00+00'),
(1006, 2, '2026-05-10 14:00:00+00'),

-- Đơn hàng 1007 (Đang giao hàng - Shipping)
(1007, 0, '2026-05-11 10:00:00+00'),
(1007, 1, '2026-05-11 13:30:00+00'),
(1007, 2, '2026-05-11 16:00:00+00'),

-- Đơn hàng 1008 (Đang giao hàng - Shipping)
(1008, 0, '2026-05-11 13:00:00+00'),
(1008, 1, '2026-05-11 15:30:00+00'),
(1008, 2, '2026-05-12 08:30:00+00'),

-- Đơn hàng 1009 (Đang giao hàng - Shipping)
(1009, 0, '2026-05-12 15:00:00+00'),
(1009, 1, '2026-05-12 17:30:00+00'),
(1009, 2, '2026-05-13 08:00:00+00'),

-- Đơn hàng 1010 (Đang giao hàng - Shipping)
(1010, 0, '2026-05-12 17:00:00+00'),
(1010, 1, '2026-05-13 08:30:00+00'),
(1010, 2, '2026-05-13 10:30:00+00'),

-- Đơn hàng 1025 (Đang giao hàng - Shipping)
(1025, 0, '2026-05-08 15:00:00+00'),
(1025, 1, '2026-05-09 08:30:00+00'),
(1025, 2, '2026-05-09 10:30:00+00');