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

INSERT INTO "Orders" ("Id", "OrderDate", "OrderStatus", "PaymentMethod", "TotalAmountBeforeDiscount", "TotalAmount",
                      "LoyaltyPointsEarned", "ShippingAddressSnapshot", "CustomerId", "ShipperId",
                      "QrCodePaymentDataId")
VALUES
-- Đơn hàng đã áp dụng mã giảm giá (TotalAmount < TotalAmountBeforeDiscount) 
(1001, '2026-05-01 08:00:00+00', 4, 1, 200000, 170000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com'), 1000),
(1003, '2026-05-03 14:20:00+00', 4, 1, 350000, 300000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Chung cư C",
  "CommuneName": "Phường Giảng Võ",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com'), 1001),
(1005, '2026-05-05 16:45:00+00', 4, 0, 380000, 350000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Số 10",
  "CommuneName": "Phường Phú Thượng",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper5@app.com'), NULL),
(1007, '2026-05-11 09:00:00+00', 3, 0, 260000, 220000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Số 12",
  "CommuneName": "Phường Tây Hồ",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com'), NULL),
(1009, '2026-05-12 14:00:00+00', 3, 0, 310000, 250000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Tòa nhà F",
  "CommuneName": "Phường Cầu Giấy",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com'), NULL),

-- Các đơn hàng không áp dụng mã giảm giá (TotalAmount = TotalAmountBeforeDiscount)
(1002, '2026-05-02 09:30:00+00', 4, 0, 150000, 150000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "45 Ngõ B",
  "CommuneName": "Phường Ngọc Hà",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com'), NULL),
(1004, '2026-05-04 10:15:00+00', 4, 0, 80000, 80000, 136, '{
  "RecipientName": "Người Nhận Thay",
  "RecipientPhoneNumber": "0911222333",
  "SpecificAddress": "Khu phố 1",
  "CommuneName": "Phường Hoàn Kiếm",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com'), NULL),
(1006, '2026-05-10 08:00:00+00', 3, 0, 115000, 115000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Khu đô thị D",
  "CommuneName": "Phường Cửa Nam",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com'), NULL),
(1008, '2026-05-11 11:30:00+00', 3, 0, 180000, 180000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com'), NULL),
(1010, '2026-05-12 16:00:00+00', 3, 0, 95000, 95000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "Ngõ 99",
  "CommuneName": "Phường Yên Hòa",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper5@app.com'), NULL),
(1011, '2026-05-12 18:00:00+00', 2, 0, 150000, 150000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1012, '2026-05-12 19:30:00+00', 2, 0, 220000, 220000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1013, '2026-05-13 07:00:00+00', 2, 0, 135000, 135000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1014, '2026-05-13 08:15:00+00', 2, 0, 480000, 480000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1015, '2026-05-13 08:45:00+00', 2, 0, 90000, 90000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1016, '2026-05-13 09:00:00+00', 1, 1, 550000, 550000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, 1002),
(1017, '2026-05-13 09:10:00+00', 1, 1, 210000, 210000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, 1003),
(1018, '2026-05-13 09:20:00+00', 0, 0, 75000, 75000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1019, '2026-05-13 09:25:00+00', 0, 0, 320000, 320000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1020, '2026-05-13 09:30:00+00', 0, 0, 145000, 145000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1021, '2026-04-15 10:00:00+00', 4, 0, 160000, 160000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com'), NULL),
(1022, '2026-04-20 11:00:00+00', 5, 0, 230000, 230000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1023, '2026-04-25 12:00:00+00', 4, 0, 410000, 410000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com'), NULL),
(1024, '2026-04-28 13:00:00+00', 5, 0, 85000, 85000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1025, '2026-05-08 14:00:00+00', 3, 0, 195000, 195000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com'), NULL),
(1026, '2026-05-09 15:00:00+00', 4, 0, 315000, 315000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com'), NULL),
(1027, '2026-05-10 16:00:00+00', 2, 0, 275000, 275000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1028, '2026-05-11 17:00:00+00', 5, 0, 110000, 110000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1029, '2026-05-12 18:00:00+00', 0, 0, 500000, 500000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),
(1030, '2026-05-13 08:30:00+00', 0, 0, 65000, 65000, 136, '{
  "RecipientName": "Nguyễn Khách 1",
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
  "ImageFilePath": "images/products/1.jpg",
  "ProductUnitName": "Kg",
  "UnitPrice": 50000
}'::jsonb),
       (1001, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageFilePath": "images/products/24.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),

       (1002, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageFilePath": "images/products/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1003, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'), 5, '{
         "Name": "Dâu tây",
         "ImageFilePath": "images/products/6.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 70000
       }'::jsonb),

       (1004, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'), 1, '{
         "Name": "Cam sành",
         "ImageFilePath": "images/products/3.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 35000
       }'::jsonb),
       (1004, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 1, '{
         "Name": "Nho tím",
         "ImageFilePath": "images/products/11.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),

       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 2, '{
         "Name": "Nho xanh",
         "ImageFilePath": "images/products/9.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),
       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'), 2, '{
         "Name": "Dưa lưới",
         "ImageFilePath": "images/products/8.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),
       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'), 2, '{
         "Name": "Bưởi da xanh",
         "ImageFilePath": "images/products/17.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 60000
       }'::jsonb),

       (1006, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài cát'), 1, '{
         "Name": "Xoài cát",
         "ImageFilePath": "images/products/12.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1006, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài tượng'), 1, '{
         "Name": "Xoài tượng",
         "ImageFilePath": "images/products/13.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),

       (1007, (SELECT "Id" FROM "Products" WHERE "Name" = 'Kiwi'), 2, '{
         "Name": "Kiwi",
         "ImageFilePath": "images/products/21.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 90000
       }'::jsonb),
       (1007, (SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'), 1, '{
         "Name": "Vải thiều",
         "ImageFilePath": "images/products/30.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 80000
       }'::jsonb),

       (1008, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho đỏ'), 2, '{
         "Name": "Nho đỏ",
         "ImageFilePath": "images/products/10.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 90000
       }'::jsonb),

       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Thanh long đỏ'), 3, '{
         "Name": "Thanh long đỏ",
         "ImageFilePath": "images/products/19.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 40000
       }'::jsonb),
       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 1, '{
         "Name": "Nho tím",
         "ImageFilePath": "images/products/11.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),
       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'), 1, '{
         "Name": "Nhãn",
         "ImageFilePath": "images/products/31.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),

       (1010, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'), 1, '{
         "Name": "Mít",
         "ImageFilePath": "images/products/20.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1010, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'), 1, '{
         "Name": "Dưa hấu",
         "ImageFilePath": "images/products/7.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),

       (1011, (SELECT "Id" FROM "Products" WHERE "Name" = 'Hồng giòn'), 2, '{
         "Name": "Hồng giòn",
         "ImageFilePath": "images/products/25.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 70000
       }'::jsonb),

       (1012, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mơ'), 2, '{
         "Name": "Mơ",
         "ImageFilePath": "images/products/22.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1012, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mãng cầu'), 1, '{
         "Name": "Mãng cầu",
         "ImageFilePath": "images/products/32.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 70000
       }'::jsonb),

       (1013, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa chuột'), 2, '{
         "Name": "Dưa chuột",
         "ImageFilePath": "images/products/16.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 12000
       }'::jsonb),
       (1013, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dừa xiêm'), 1, '{
         "Name": "Dừa xiêm",
         "ImageFilePath": "images/products/15.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 40000
       }'::jsonb),

       (1014, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), 2, '{
         "Name": "Cherry",
         "ImageFilePath": "images/products/27.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 150000
       }'::jsonb),
       (1014, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 2, '{
         "Name": "Nho xanh",
         "ImageFilePath": "images/products/9.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),

       (1015, (SELECT "Id" FROM "Products" WHERE "Name" = 'Ổi'), 3, '{
         "Name": "Ổi",
         "ImageFilePath": "images/products/23.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),

       (1016, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 11, '{
         "Name": "Táo đỏ",
         "ImageFilePath": "images/products/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1017, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'), 3, '{
         "Name": "Dâu tây",
         "ImageFilePath": "images/products/6.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 70000
       }'::jsonb),

       (1018, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'), 3, '{
         "Name": "Dưa hấu",
         "ImageFilePath": "images/products/7.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),

       (1019, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 4, '{
         "Name": "Nho tím",
         "ImageFilePath": "images/products/11.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 80000
       }'::jsonb),

       (1020, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam vàng'), 2, '{
         "Name": "Cam vàng",
         "ImageFilePath": "images/products/4.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 40000
       }'::jsonb),
       (1020, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi hồng'), 1, '{
         "Name": "Bưởi hồng",
         "ImageFilePath": "images/products/29.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 65000
       }'::jsonb),

       (1021, (SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'), 2, '{
         "Name": "Vải thiều",
         "ImageFilePath": "images/products/30.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 80000
       }'::jsonb),

       (1022, (SELECT "Id" FROM "Products" WHERE "Name" = 'Chôm chôm'), 2, '{
         "Name": "Chôm chôm",
         "ImageFilePath": "images/products/18.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1022, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'), 1, '{
         "Name": "Bưởi da xanh",
         "ImageFilePath": "images/products/17.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 60000
       }'::jsonb),

       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Kiwi'), 3, '{
         "Name": "Kiwi",
         "ImageFilePath": "images/products/21.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 90000
       }'::jsonb),
       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'), 1, '{
         "Name": "Táo xanh",
         "ImageFilePath": "images/products/28.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),
       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 1, '{
         "Name": "Nho xanh",
         "ImageFilePath": "images/products/9.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),

       (1024, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài keo'), 1, '{
         "Name": "Xoài keo",
         "ImageFilePath": "images/products/14.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1024, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'), 1, '{
         "Name": "Cam sành",
         "ImageFilePath": "images/products/3.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 35000
       }'::jsonb),

       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageFilePath": "images/products/24.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),
       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'), 1, '{
         "Name": "Dưa lưới",
         "ImageFilePath": "images/products/8.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),
       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'), 1, '{
         "Name": "Mít",
         "ImageFilePath": "images/products/20.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'), 2, '{
         "Name": "Nhãn",
         "ImageFilePath": "images/products/31.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Đu đủ'), 1, '{
         "Name": "Đu đủ",
         "ImageFilePath": "images/products/26.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),
       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageFilePath": "images/products/24.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),

       (1027, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo vàng'), 2, '{
         "Name": "Táo vàng",
         "ImageFilePath": "images/products/2.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1027, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'), 1, '{
         "Name": "Táo xanh",
         "ImageFilePath": "images/products/28.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),

       (1028, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 2, '{
         "Name": "Nho tím",
         "ImageFilePath": "images/products/11.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1028, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa chuột'), 1, '{
         "Name": "Dưa chuột",
         "ImageFilePath": "images/products/16.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 12000
       }'::jsonb),

       (1029, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), 3, '{
         "Name": "Cherry",
         "ImageFilePath": "images/products/27.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 150000
       }'::jsonb),
       (1029, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageFilePath": "images/products/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1030, (SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối tiêu'), 2, '{
         "Name": "Chuối tiêu",
         "ImageFilePath": "images/products/5.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),
       (1030, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 1, '{
         "Name": "Nho tím",
         "ImageFilePath": "images/products/11.jpg",
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

-- ==============================================================================
-- TẠO ĐƠN HÀNG MỚI (MÃ 1031) DÀNH CHO SHIPPER 1 - CHƯA CÓ LỊCH SỬ VẬN CHUYỂN
-- OrderStatus = 3 (Tương ứng với trạng thái Shipping - Đang đi giao)
-- ShipperId = Tìm theo email 'shipper1@app.com'
-- QrCodePaymentDataId = NULL (Đơn hàng COD thanh toán tiền mặt)
-- ==============================================================================
INSERT INTO "Orders" ("Id", "OrderDate", "OrderStatus", "PaymentMethod", "TotalAmountBeforeDiscount", "TotalAmount",
                      "LoyaltyPointsEarned", "ShippingAddressSnapshot", "CustomerId", "ShipperId",
                      "QrCodePaymentDataId")
VALUES (1031,
        '2026-05-19 12:00:00+00', -- Ngày đặt hàng (Định dạng UTC)
        3, -- OrderStatus = 3 (Shipping) để hiện ra trang danh sách của Shipper
        0, -- PaymentMethod = 0 (CashOnDelivery - Tiền mặt)
        250000, -- Tổng tiền trước giảm giá
        250000, -- Tổng tiền sau giảm giá (Thu hộ 250K),
        136,
        '{
          "RecipientName": "Trần Văn Test Đơn",
          "RecipientPhoneNumber": "0912345678",
          "SpecificAddress": "Khu ký túc xá Alpha",
          "CommuneName": "Xã Thạch Hòa",
          "ProvinceName": "Huyện Thạch Thất, Hà Nội"
        }'::jsonb,
        (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com'),
        NULL -- Đơn COD không cần bảng dữ liệu QR Code
       );

-- ==============================================================================
-- THÊM CHI TIẾT SẢN PHẨM MUA CHO ĐƠN 1031 (Test shipper)
-- ==============================================================================
INSERT INTO "OrderItem" ("OrderId", "ProductId", "Quantity", "ProductSnapshot")
VALUES (1031,
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'),
        2,
        '{
          "Name": "Măng cụt",
          "ImageFilePath": "images/products/24.jpg",
          "ProductUnitName": "Kg",
          "UnitPrice": 100000
        }'::jsonb),
       (1031,
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'),
        1,
        '{
          "Name": "Mít",
          "ImageFilePath": "images/products/20.jpg",
          "ProductUnitName": "Kg",
          "UnitPrice": 50000
        }'::jsonb);



-- ==============================================================================
-- 2. TẠO ĐÁNH GIÁ SẢN PHẨM (PRODUCT REVIEWS) CHO CÁC ĐƠN ĐÃ GIAO
-- CommentClassification: 0 (Unclassified), 1 (Positive), 2 (NegativeNotResolved), 3 (NegativeResolved)
-- ==============================================================================
INSERT INTO "ProductReviews" ("OrderId", "ProductId", "CustomerId", "Rating", "Comment", "CreatedAt",
                              "CommentClassification",
                              "AssignedCustomerSupportId", "ResolutionMessage", "ResolvedAt")
VALUES
-- Đơn 1001 (Táo đỏ, Măng cụt) - Đánh giá tốt
(1001, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 5, 'Táo cực kỳ giòn và ngọt, 10 điểm cho shop!',
 '2026-05-02 08:00:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),
(1001, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 4, 'Măng cụt ngon nhưng quả hơi nhỏ một xíu',
 '2026-05-02 08:05:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),

-- Đơn 1002 (Táo đỏ) - Vừa đánh giá xong, hệ thống chưa kịp phân loại (Unclassified)
(1002, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 5, 'Hàng xịn, ngon, đóng hộp sang trọng',
 '2026-05-03 10:15:00+00', 0, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),

-- Đơn 1003 (Dâu tây) - Đánh giá tốt
(1003, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 5,
 'Dâu mọng nước, hộp gói cẩn thận không bị dập tí nào',
 '2026-05-05 09:00:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),

-- Đơn 1004 (Cam sành, Quýt) - Có khiếu nại đã giải quyết (NegativeResolved) & Khen ngợi (Positive)
(1004, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 2,
 'Cam đợt này bị chua quá shop ơi, không vắt nước uống được', '2026-05-06 14:00:00+00', 3,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 'Đánh giá này đã được chuyển tới bộ phận hỗ trợ vào ngày 05/05/2026', '2026-05-06 15:30:00+00'),
(1004, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 5, 'Nho tím rất ngọt', '2026-05-06 14:05:00+00', 1,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),

-- Đơn 1005 (Nho xanh, Dưa lưới, Bưởi da xanh) - Tổng hợp nhiều trạng thái
(1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 5, 'Nho giòn rụm, ngọt lịm', '2026-05-07 10:00:00+00',
 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),
(1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 3, 'Dưa lưới ruột màu hơi nhạt, ăn chưa được ngọt lắm',
 '2026-05-07 10:05:00+00', 2, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL,
 NULL), -- Negative Not Resolved (Chưa xử lý)
(1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 1,
 'Bưởi bị khô sần, ăn the đắng không như quảng cáo', '2026-05-07 10:10:00+00', 3,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 'Bộ phận bán hàng đã liên hệ hỗ trợ ngày 07/05/2026', '2026-05-08 09:00:00+00'),

-- Đơn 1021 (Vải thiều)
(1021, (SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 5, 'Vải Lục Ngạn chuẩn, hạt lép, ăn rất ưng',
 '2026-04-17 08:20:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),

-- Đơn 1023 (Kiwi, Táo xanh) - Có khiếu nại chưa xử lý
(1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Kiwi'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 2,
 'Giao nhầm size kiwi nhỏ, hơi thất vọng vì đặt size to',
 '2026-04-27 11:00:00+00', 2, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),
(1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 4, 'Táo tươi, chua thanh như mong đợi',
 '2026-04-27 11:05:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),

-- Đơn 1026 (Nhãn, Đu đủ)
(1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 5, 'Nhãn cùi dày, ngọt thanh, ăn rất cuốn',
 '2026-05-11 19:00:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL),
(1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Đu đủ'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), 3, 'Đu đủ hơi chín quá, ăn bị nhũn',
 '2026-05-11 19:05:00+00', 0, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'), NULL, NULL);

-- ==============================================================================
-- 1. TẠO 15 ĐƠN HÀNG MỚI (ORDERS) - Đều ở trạng thái Đã Giao (4)
-- ==============================================================================
INSERT INTO "Orders" ("Id", "OrderDate", "OrderStatus", "PaymentMethod", "TotalAmountBeforeDiscount", "TotalAmount",
                      "LoyaltyPointsEarned", "ShippingAddressSnapshot", "CustomerId", "ShipperId")
VALUES (1035, '2026-05-15 08:00:00+00', 4, 0, 50000, 50000, 369, '{
  "RecipientName": "Nguyễn Khách 1",
  "RecipientPhoneNumber": "0987654321",
  "SpecificAddress": "123 Đường A",
  "CommuneName": "Phường Ba Đình",
  "ProvinceName": "Thành phố Hà Nội"
}'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com')),
       (1036, '2026-05-15 09:30:00+00', 4, 0, 100000, 100000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com')),
       (1037, '2026-05-15 10:45:00+00', 4, 0, 50000, 50000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com')),
       (1038, '2026-05-16 07:15:00+00', 4, 0, 150000, 150000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com')),
       (1039, '2026-05-16 14:20:00+00', 4, 0, 50000, 50000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper5@app.com')),
       (1040, '2026-05-17 08:10:00+00', 4, 0, 100000, 100000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com')),
       (1041, '2026-05-17 11:00:00+00', 4, 0, 50000, 50000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com')),
       (1042, '2026-05-18 09:30:00+00', 4, 0, 200000, 200000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com')),
       (1043, '2026-05-18 15:45:00+00', 4, 0, 50000, 50000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com')),
       (1044, '2026-05-19 10:20:00+00', 4, 0, 50000, 50000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper5@app.com')),
       (1045, '2026-05-19 13:10:00+00', 4, 0, 100000, 100000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper1@app.com')),
       (1046, '2026-05-20 08:50:00+00', 4, 0, 50000, 50000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper2@app.com')),
       (1047, '2026-05-20 16:00:00+00', 4, 0, 150000, 150000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper3@app.com')),
       (1048, '2026-05-21 09:15:00+00', 4, 0, 50000, 50000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper4@app.com')),
       (1049, '2026-05-21 14:40:00+00', 4, 0, 50000, 50000, 369, '{
         "RecipientName": "Nguyễn Khách 1",
         "RecipientPhoneNumber": "0987654321",
         "SpecificAddress": "123 Đường A",
         "CommuneName": "Phường Ba Đình",
         "ProvinceName": "Thành phố Hà Nội"
       }'::jsonb, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
        (SELECT "Id" FROM "Users" WHERE "Email" = 'shipper5@app.com'));


-- ==============================================================================
-- 2. TẠO CHI TIẾT ĐƠN HÀNG (ORDER ITEMS) - Đều là mua "Táo đỏ"
-- ==============================================================================
INSERT INTO "OrderItem" ("OrderId", "ProductId", "Quantity", "ProductSnapshot")
VALUES (1035, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
  "Name": "Táo đỏ",
  "ImageUrl": "/images/products/1/1.jpg",
  "ProductUnitName": "Kg",
  "UnitPrice": 50000
}'::jsonb),
       (1036, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 2, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1037, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1038, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 3, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1039, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1040, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 2, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1041, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1042, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 4, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1043, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1044, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1045, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 2, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1046, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1047, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 3, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1048, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1049, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb);

-- ==============================================================================
-- 3. TẠO HÀNH TRÌNH VẬN CHUYỂN (Chỉ tạo mốc Đã Giao - Delivered)
-- ==============================================================================
INSERT INTO "OrderShipping" ("OrderId", "ShippingStatus", "OccurredAt")
VALUES (1035, 3, '2026-05-16 10:00:00+00'),
       (1036, 3, '2026-05-16 14:00:00+00'),
       (1037, 3, '2026-05-17 09:30:00+00'),
       (1038, 3, '2026-05-17 15:00:00+00'),
       (1039, 3, '2026-05-18 11:00:00+00'),
       (1040, 3, '2026-05-18 16:30:00+00'),
       (1041, 3, '2026-05-19 10:15:00+00'),
       (1042, 3, '2026-05-20 08:45:00+00'),
       (1043, 3, '2026-05-20 13:20:00+00'),
       (1044, 3, '2026-05-21 09:00:00+00'),
       (1045, 3, '2026-05-21 15:10:00+00'),
       (1046, 3, '2026-05-22 10:00:00+00'),
       (1047, 3, '2026-05-22 14:30:00+00'),
       (1048, 3, '2026-05-23 09:45:00+00'),
       (1049, 3, '2026-05-23 11:15:00+00');


-- ==============================================================================
-- 4. TẠO 15 ĐÁNH GIÁ SẢN PHẨM (PRODUCT REVIEWS) TƯƠNG ỨNG
-- Có thêm cột CustomerId theo yêu cầu.
-- ==============================================================================
INSERT INTO "ProductReviews" ("OrderId", "ProductId", "Rating", "Comment", "CreatedAt", "CommentClassification",
                              "AssignedCustomerSupportId", "CustomerId", "ResolutionMessage", "ResolvedAt")
VALUES
-- Rating 5: Positive (1)
(1035, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 5,
 'Táo tươi, giòn ngọt đúng ý mình lắm. Mua lần nào cũng ưng.', '2026-05-16 12:00:00+00', 1,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 4: Positive (1)
(1036, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 4,
 'Chất lượng táo ổn định, quả to nhưng thỉnh thoảng hơi lạt.', '2026-05-17 08:30:00+00', 1,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 2: Negative Not Resolved (2)
(1037, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 2,
 'Táo mua đợt này bị dập vài quả, đóng gói chưa cẩn thận.', '2026-05-18 14:00:00+00', 2,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 1: Negative Resolved (3)
(1038, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1,
 'Mình nhận được táo dập nát hết không ăn được. Quá tệ!', '2026-05-18 09:00:00+00', 3,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 'Đánh giá này đã được chuyển tới bộ phận hỗ trợ vào ngày 17/05/2026', '2026-05-18 11:30:00+00'),

-- Rating 5: Positive (1)
(1039, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 5,
 'Táo ăn giòn rụm, ngon nha mọi người, giao hàng siêu nhanh.', '2026-05-19 15:00:00+00', 1,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 3: Unclassified (0) - Vừa đánh giá xong chưa phân loại
(1040, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 3,
 'Cũng bình thường, không quá giòn như đợt trước mình mua.', '2026-05-19 09:20:00+00', 0,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 5: Positive (1)
(1041, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 5, '10 điểm không có nhưng, táo đỉnh của chóp.',
 '2026-05-20 10:15:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 4: Positive (1)
(1042, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 4, 'Shop đóng hàng kỹ. Táo to đều màu.',
 '2026-05-21 16:30:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 2: Negative Not Resolved (2)
(1043, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 2, 'Shipper giao hàng chậm quá làm táo hơi héo cuống.',
 '2026-05-21 08:45:00+00', 2, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 5: Positive (1)
(1044, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 5, 'Rất chất lượng, ăn hết mình lại đặt tiếp nhe shop.',
 '2026-05-22 13:20:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 1: Negative Not Resolved (2)
(1045, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1,
 'Trái bị sâu bên trong khá nhiều, shop kiểm tra lại nguồn hàng nhé.', '2026-05-22 19:10:00+00', 2,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 5: Positive (1)
(1046, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 5, 'Ngon xỉu. 5 saoooooo.', '2026-05-23 09:00:00+00', 1,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 3: Negative Resolved (3)
(1047, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 3, 'Mua 3kg nhưng cân lên bị thiếu một chút nhé.',
 '2026-05-23 10:30:00+00', 3, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 'Đánh giá này đã được chuyển tới bộ phận hỗ trợ vào ngày 22/05/2026', '2026-05-23 11:00:00+00'),

-- Rating 4: Positive (1)
(1048, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 4, 'Hương vị ok, phù hợp giá tiền.',
 '2026-05-23 12:45:00+00', 1, (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL),

-- Rating 5: Positive (1)
(1049, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 5,
 'Rất hài lòng về chất lượng sản phẩm dịch vụ của shop!', '2026-05-23 13:15:00+00', 1,
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer-support1@app.com'),
 (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'), NULL, NULL);