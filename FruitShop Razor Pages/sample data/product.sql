INSERT INTO "ProductUnits" ("Name")
VALUES ('Kg'),
       ('Quả'),
       ('Hộp'),
       ('Giỏ');

-----------------------------------------

INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageUrl", "IsActive", "Quantity",
                        "HeldQuantity", "DisplayOrder")
VALUES ('Táo đỏ',
        'Táo đỏ nhập khẩu, quả to đều, mọng nước, vị ngọt thanh, giàu vitamin C và chất xơ. Thích hợp ăn trực tiếp, làm salad hoặc nước ép.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/1.jpg', true, 58, 0, 0),
       ('Táo vàng',
        'Táo vàng Mỹ, giòn, ngọt, mọng nước, giàu chất chống oxy hóa và vitamin. Phù hợp ăn trực tiếp, làm bánh hoặc salad.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/2.jpg', true, 42, 0, 1),
       ('Cam sành',
        'Cam sành Việt Nam, ngọt đậm đà, ít chua, giàu vitamin C và folate. Phù hợp vắt nước hoặc ăn trực tiếp.', 35000,
        (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), 'http://localhost:9000/public/images/products/3.jpg',
        true, 65, 0, 2),
       ('Cam vàng',
        'Cam vàng tươi ngon, mọng nước, ngọt tự nhiên và giàu vitamin C giúp tăng cường sức đề kháng. Phù hợp ăn trực tiếp hoặc làm nước ép.',
        40000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/4.jpg', true, 39, 0, 3),
       ('Chuối tiêu',
        'Chuối tiêu chín vàng, mềm, ngọt tự nhiên, giàu kali và năng lượng. Thích hợp ăn trực tiếp, làm sinh tố hoặc bánh trái cây.',
        25000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/5.jpg', true, 68, 0, 4),
       ('Dâu tây',
        'Dâu tây Đà Lạt siêu ngọt, tươi ngon, mọng nước, giàu vitamin C và chất chống oxy hóa. Lý tưởng cho ăn trực tiếp, salad hoặc làm mứt.',
        70000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
        'http://localhost:9000/public/images/products/6.jpg', true, 37, 0, 5),
       ('Dưa hấu',
        'Dưa hấu mọng nước, ngọt thanh, giàu lycopene và vitamin C. Phù hợp ăn trực tiếp, làm nước ép hoặc tráng miệng giải khát.',
        25000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/7.jpg', true, 55, 0, 6),
       ('Dưa lưới',
        'Dưa lưới vàng ngọt, thơm đặc trưng, mọng nước, giàu beta-carotene. Ăn trực tiếp làm tráng miệng hoặc sinh tố.',
        45000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/8.jpg', true, 61, 0, 7),
       ('Nho xanh',
        'Nho xanh không hạt, giòn ngọt, mọng nước, giàu resveratrol và vitamin K. Thích hợp ăn trực tiếp, làm rượu vang hoặc salad.',
        85000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/9.jpg', true, 48, 0, 8),
       ('Nho đỏ',
        'Nho đỏ không hạt, mọng nước, ngọt thanh, giàu chất chống oxy hóa và vitamin. Thích hợp ăn trực tiếp, làm salad hoặc rượu vang tại nhà.',
        90000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
        'http://localhost:9000/public/images/products/10.jpg', true, 53, 0, 9),
       ('Nho tím',
        'Nho tím không hạt, mọng, ngọt, giàu chất chống oxy hóa và vitamin. Thích hợp ăn trực tiếp, làm salad hoặc rượu vang tại nhà.',
        80000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
        'http://localhost:9000/public/images/products/11.jpg', true, 40, 0, 10),
       ('Xoài cát',
        'Xoài cát Hòa Lộc, vàng ươm, thơm ngọt, thịt mềm, giàu vitamin A và C. Phù hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/12.jpg', true, 69, 0, 11),
       ('Xoài tượng',
        'Xoài tượng da xanh, quả to, thịt dày, ngọt thanh, giàu vitamin A và C. Thích hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.',
        55000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/13.jpg', true, 36, 0, 12),
       ('Xoài keo',
        'Xoài keo chín vàng, ngọt mát, thịt mềm, giàu vitamin A và C. Phù hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/14.jpg', true, 50, 0, 13),
       ('Dừa xiêm',
        'Dừa xiêm xanh, nước ngọt mát, cùi dừa non béo ngậy, giàu chất điện giải và vitamin. Thích hợp uống trực tiếp hoặc làm sinh tố, chè.',
        40000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'),
        'http://localhost:9000/public/images/products/15.jpg', true, 63, 0, 14),
       ('Dưa chuột',
        'Dưa chuột tươi ngon, giòn mát, mọng nước, giàu vitamin K và chất xơ. Thích hợp ăn sống, làm salad hoặc nước detox thanh mát.',
        12000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/16.jpg', true, 44, 0, 15),
       ('Bưởi da xanh',
        'Bưởi da xanh đặc sản miền Tây, múi mọng, ngọt thanh, giàu vitamin C và chất chống oxy hóa. Ăn trực tiếp hoặc làm nước ép đều thơm ngon.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'),
        'http://localhost:9000/public/images/products/17.jpg', true, 41, 0, 16),
       ('Chôm chôm',
        'Chôm chôm vỏ mỏng, ngọt nước, mọng, giàu vitamin C và chất chống oxy hóa. Phù hợp ăn trực tiếp hoặc làm món tráng miệng.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/18.jpg', true, 66, 0, 17),
       ('Thanh long đỏ',
        'Thanh long đỏ mọng nước, ngọt nhẹ, giàu vitamin C và chất chống oxy hóa. Ăn trực tiếp hoặc làm sinh tố giải khát.',
        40000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/19.jpg', true, 38, 0, 18),
       ('Mít',
        'Mít chín vàng, ngọt thơm, giàu vitamin C, chất xơ và năng lượng. Ăn trực tiếp hoặc làm sinh tố, bánh trái cây.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/20.jpg', true, 52, 0, 19),
       ('Kiwi',
        'Kiwi nhập khẩu, chua ngọt vừa phải, giàu vitamin C, K và chất xơ, tốt cho tiêu hóa và miễn dịch. Ăn trực tiếp hoặc làm nước ép.',
        90000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/21.jpg', true, 60, 0, 20),
       ('Mơ',
        'Mơ chín mọng, vị ngọt nhẹ và hơi chua, giàu vitamin A, C, chất chống oxy hóa. Ăn trực tiếp hoặc làm mứt, nước ép.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/22.jpg', true, 54, 0, 21),
       ('Ổi',
        'Ổi tươi ngon, giòn, mọng nước, giàu vitamin C và chất xơ, hỗ trợ tiêu hóa. Ăn trực tiếp hoặc làm nước ép, salad.',
        30000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/23.jpg', true, 43, 0, 22),
       ('Măng cụt',
        'Măng cụt chín mọng, vị ngọt thanh, giàu chất chống oxy hóa, vitamin và khoáng chất. Ăn trực tiếp hoặc làm món tráng miệng.',
        100000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/24.jpg', true, 67, 0, 23),
       ('Hồng giòn',
        'Hồng giòn chín mọng, vị ngọt dịu, giàu chất xơ và vitamin, tốt cho hệ tiêu hóa. Ăn trực tiếp hoặc làm salad.',
        70000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/25.jpg', true, 39, 0, 24),
       ('Đu đủ',
        'Đu đủ chín vàng, ngọt mát, giàu vitamin A, C và enzyme papain hỗ trợ tiêu hóa. Ăn trực tiếp hoặc làm sinh tố.',
        30000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/26.jpg', true, 59, 0, 25),
       ('Cherry',
        'Cherry đỏ tươi ngon, ngọt thanh, giàu chất chống oxy hóa và melatonin tự nhiên. Tốt cho giấc ngủ và chống viêm. Ăn trực tiếp hoặc làm mứt.',
        150000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
        'http://localhost:9000/public/images/products/27.jpg', true, 49, 0, 26),
       ('Táo xanh',
        'Táo xanh Granny Smith, vị chua ngọt đặc trưng, giàu pectin và chất xơ. Thích hợp ăn trực tiếp, làm bánh hoặc salad.',
        55000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/28.jpg', true, 62, 0, 27),
       ('Bưởi hồng',
        'Bưởi hồng ruột đỏ, ngọt thanh, ít đắng, giàu lycopene và vitamin C. Ăn trực tiếp hoặc làm salad trái cây.',
        65000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'),
        'http://localhost:9000/public/images/products/29.jpg', true, 46, 0, 28),
       ('Vải thiều',
        'Vải thiều Lục Ngạn, thịt trắng ngọt lịm, thơm đặc trưng, giàu vitamin C. Ăn trực tiếp hoặc làm nước ép, mứt.',
        80000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/30.jpg', true, 56, 0, 29),
       ('Nhãn',
        'Nhãn tươi ngọt thanh, thịt trắng trong, giàu vitamin C và glucose tự nhiên. Ăn trực tiếp hoặc nấu chè, làm mứt.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/31.jpg', true, 64, 0, 30),
       ('Mãng cầu',
        'Mãng cầu chín mềm, vị ngọt đặc biệt, giàu vitamin C và chất xơ. Ăn trực tiếp hoặc làm kem, sinh tố.', 70000,
        (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), 'http://localhost:9000/public/images/products/32.jpg',
        true, 38, 0, 31),
       ('Demo1', 'Demo', 360, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        'http://localhost:9000/public/images/products/demo.png',
        true, 36, 0, 32),
       ('Demo2', 'Demo', 690, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
        'http://localhost:9000/public/images/products/demo.png', true,
        69, 0, 33),
       ('Demo3', 'Demo', 100, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'),
        'http://localhost:9000/public/images/products/demo.png', true,
        100, 0, 34);

INSERT INTO "Categories" ("Name", "IsActive")
VALUES ('Trái Cây Nhập Khẩu', true),
       ('Trái Cây Nội Địa', true),
       ('Trái Cây Nhiệt Đới', true),
       ('Trái Cây Có Múi', true),
       ('Táo', true),
       ('Nho Các Loại', true),
       ('Dưa Các Loại', true),
       ('Quả Mọng', true),
       ('Xoài Các Loại', true),
       ('Trái Cây Đặc Sản', true);

-----------------------------------

INSERT INTO "ProductCategories"("ProductsId", "CategoriesId")
VALUES
-- Táo đỏ
((SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Táo')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Táo vàng
((SELECT "Id" FROM "Products" WHERE "Name" = 'Táo vàng'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Táo')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Táo vàng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Cam sành
((SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Có Múi')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),

-- Cam vàng
((SELECT "Id" FROM "Products" WHERE "Name" = 'Cam vàng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Có Múi')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Cam vàng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Chuối tiêu
((SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối tiêu'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối tiêu'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Dâu tây
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Quả Mọng')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Dưa hấu
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Dưa Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Dưa lưới
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Dưa Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),

-- Nho xanh
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Nho Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Nho đỏ
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nho đỏ'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Nho Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nho đỏ'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Nho tím
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Nho Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Xoài cát
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài cát'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Xoài Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài cát'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài cát'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),

-- Xoài tượng
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài tượng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Xoài Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài tượng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài tượng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Xoài keo
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài keo'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Xoài Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài keo'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài keo'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Dừa xiêm
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dừa xiêm'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dừa xiêm'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Dưa chuột (xếp vào Nội Địa là hợp lý nhất)
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa chuột'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),


-- Bưởi da xanh
((SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Có Múi')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Chôm chôm
((SELECT "Id" FROM "Products" WHERE "Name" = 'Chôm chôm'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Chôm chôm'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),

-- Thanh long đỏ
((SELECT "Id" FROM "Products" WHERE "Name" = 'Thanh long đỏ'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Thanh long đỏ'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Mít
((SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Kiwi
((SELECT "Id" FROM "Products" WHERE "Name" = 'Kiwi'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Mơ
((SELECT "Id" FROM "Products" WHERE "Name" = 'Mơ'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Ổi
((SELECT "Id" FROM "Products" WHERE "Name" = 'Ổi'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Ổi'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Măng cụt
((SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Hồng giòn
((SELECT "Id" FROM "Products" WHERE "Name" = 'Hồng giòn'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Hồng giòn'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),

-- Đu đủ
((SELECT "Id" FROM "Products" WHERE "Name" = 'Đu đủ'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Đu đủ'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Cherry
((SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Quả Mọng')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Táo xanh
((SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Táo')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhập Khẩu')),

-- Bưởi hồng
((SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi hồng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Có Múi')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi hồng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Vải thiều
((SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Nhãn
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Mãng cầu
((SELECT "Id" FROM "Products" WHERE "Name" = 'Mãng cầu'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Mãng cầu'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản'));
