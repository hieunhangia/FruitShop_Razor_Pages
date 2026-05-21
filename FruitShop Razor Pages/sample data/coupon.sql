-- ==============================================================================
-- 2. TẠO 30 MÃ GIẢM GIÁ (COUPONS)
-- DiscountType: 0 (Percentage), 1 (FixedAmount)
-- ==============================================================================
INSERT INTO "Coupons" ("Description", "DiscountValue", "DiscountType", "MaxDiscountAmount", "MinOrderAmount",
                       "LoyaltyPointsCost", "IsActive")
VALUES
-- ==========================================================
-- DẠNG 1: GIẢM GIÁ THEO PHẦN TRĂM (DiscountType = 0)
-- Format: Giảm ngay xxx% cho đơn hàng từ xxxK, giảm tối đa xxxK
-- ==========================================================
('Giảm ngay 5% cho đơn hàng từ 100K, giảm tối đa 30K', 5, 0, 30000, 100000, 50, true),
('Giảm ngay 10% cho đơn hàng từ 200K, giảm tối đa 50K', 10, 0, 50000, 200000, 100, true),
('Giảm ngay 15% cho đơn hàng từ 300K, giảm tối đa 70K', 15, 0, 70000, 300000, 150, true),
('Giảm ngay 20% cho đơn hàng từ 500K, giảm tối đa 100K', 20, 0, 100000, 500000, 200, true),
('Giảm ngay 8% cho đơn hàng từ 150K, giảm tối đa 40K', 8, 0, 40000, 150000, 80, true),
('Giảm ngay 12% cho đơn hàng từ 250K, giảm tối đa 60K', 12, 0, 60000, 250000, 120, true),
('Giảm ngay 25% cho đơn hàng từ 400K, giảm tối đa 150K', 25, 0, 150000, 400000, 250, true),
('Giảm ngay 5% cho đơn hàng từ 0đ', 5, 0, NULL, NULL, 100, true),
('Giảm ngay 10% cho đơn hàng từ 0đ, giảm tối đa 30K', 10, 0, 30000, NULL, 120, true),
('Giảm ngay 18% cho đơn hàng từ 350K, giảm tối đa 80K', 18, 0, 80000, 350000, 180, true),
('Giảm ngay 30% cho đơn hàng từ 500K, giảm tối đa 200K', 30, 0, 200000, 500000, 300, false), -- Hết hạn/Ngừng hoạt động
('Giảm ngay 7% cho đơn hàng từ 150K', 7, 0, NULL, 150000, 70, true),
('Giảm ngay 9% cho đơn hàng từ 200K, giảm tối đa 45K', 9, 0, 45000, 200000, 90, true),
('Giảm ngay 11% cho đơn hàng từ 250K, giảm tối đa 55K', 11, 0, 55000, 250000, 110, true),
('Giảm ngay 14% cho đơn hàng từ 300K, giảm tối đa 65K', 14, 0, 65000, 300000, 140, true),

-- ==========================================================
-- DẠNG 2: GIẢM GIÁ SỐ TIỀN CỐ ĐỊNH (DiscountType = 1)
-- Format: Giảm ngay xxxK cho đơn hàng từ xxxK
-- Lời mô tả không cần vế "tối đa" vì mức giảm đã là con số cố định
-- ==========================================================
('Giảm ngay 20K cho đơn hàng từ 100K', 20000, 1, NULL, 100000, 100, true),
('Giảm ngay 30K cho đơn hàng từ 200K', 30000, 1, NULL, 200000, 150, true),
('Giảm ngay 50K cho đơn hàng từ 300K', 50000, 1, NULL, 300000, 250, true),
('Giảm ngay 100K cho đơn hàng từ 500K', 100000, 1, NULL, 500000, 500, true),
('Giảm ngay 15K cho đơn hàng từ 0đ', 15000, 1, NULL, NULL, 75, true),
('Giảm ngay 25K cho đơn hàng từ 150K', 25000, 1, NULL, 150000, 125, true),
('Giảm ngay 40K cho đơn hàng từ 250K', 40000, 1, NULL, 250000, 200, true),
('Giảm ngay 60K cho đơn hàng từ 400K', 60000, 1, NULL, 400000, 300, true),
('Giảm ngay 150K cho đơn hàng từ 1000K', 150000, 1, NULL, 1000000, 700, true),
('Giảm ngay 10K cho đơn hàng từ 0đ', 10000, 1, NULL, NULL, 50, true),
('Giảm ngay 35K cho đơn hàng từ 200K', 35000, 1, NULL, 200000, 175, true),
('Giảm ngay 45K cho đơn hàng từ 300K', 45000, 1, NULL, 300000, 225, true),
('Giảm ngay 70K cho đơn hàng từ 500K', 70000, 1, NULL, 500000, 350, true),
('Giảm ngay 80K cho đơn hàng từ 600K', 80000, 1, NULL, 600000, 400, false),                  -- Hết hạn/Ngừng hoạt động
('Giảm ngay 200K cho đơn hàng từ 2000K', 200000, 1, NULL, 2000000, 1000, true);



-- ==============================================================================
-- 3. TẠO 10 MÃ GIẢM GIÁ CHO CUSTOMER 1 (CUSTOMER COUPONS)
-- Lấy ngẫu nhiên 10 Coupon ID từ 1 đến 10 vừa tạo ở trên
-- ==============================================================================
INSERT INTO "CustomerCoupons" ("IsUsed", "ExpiryDate", "CustomerId", "CouponId")
VALUES
-- Đã sử dụng
(true, '2026-04-30 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 0)),
(true, '2026-05-10 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 15)),
(true, '2026-05-12 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 3)),
-- Chưa sử dụng (còn hạn)
(false, '2026-06-30 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 1)),
(false, '2026-06-15 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 16)),
(false, '2026-07-01 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 2)),
(false, '2026-08-01 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 17)),
(false, '2026-05-30 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 4)),
-- Chưa sử dụng (nhưng đã hết hạn)
(false, '2026-01-01 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 18)),
(false, '2026-03-15 23:59:59+00', (SELECT "Id" FROM "Users" WHERE "Email" = 'customer1@app.com'),
 (SELECT "Id" FROM "Coupons" LIMIT 1 OFFSET 5));
