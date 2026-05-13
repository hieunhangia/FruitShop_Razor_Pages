INSERT INTO "ProductUnits" ("Name")
VALUES ('Kg'),
       ('Quả'),
       ('Hộp'),
       ('Giỏ');

-----------------------------------------


INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageUrl", "IsActive", "Quantity",
                        "HeldQuantity")
VALUES ('Demo1', 'Demo', 360, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/demo.jpg', true,
        36, 0),
       ('Demo2', 'Demo', 690, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'), '/images/products/demo.jpg', true,
        69, 0),
       ('Demo3', 'Demo', 100, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'), '/images/products/demo.jpg', true,
        100, 0),
       ('Táo đỏ',
        'Táo đỏ nhập khẩu, quả to đều, mọng nước, vị ngọt thanh, giàu vitamin C và chất xơ. Thích hợp ăn trực tiếp, làm salad hoặc nước ép.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/1/1.jpg', true, 58, 0),
       ('Táo vàng',
        'Táo vàng Mỹ, giòn, ngọt, mọng nước, giàu chất chống oxy hóa và vitamin. Phù hợp ăn trực tiếp, làm bánh hoặc salad.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/2/1.jpg', true, 42, 0),
       ('Cam sành',
        'Cam sành Việt Nam, ngọt đậm đà, ít chua, giàu vitamin C và folate. Phù hợp vắt nước hoặc ăn trực tiếp.', 35000,
        (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/3/1.jpg', true, 65, 0),
       ('Cam vàng',
        'Cam vàng tươi ngon, mọng nước, ngọt tự nhiên và giàu vitamin C giúp tăng cường sức đề kháng. Phù hợp ăn trực tiếp hoặc làm nước ép.',
        40000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/4/1.jpg', true, 39, 0),
       ('Quýt',
        'Quýt ngọt lịm, mọng nước, giàu vitamin C và chất xơ, giúp tăng cường hệ miễn dịch và tiêu hóa. Thích hợp ăn trực tiếp hoặc làm nước ép.',
        45000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/5/1.jpg', true, 51, 0),
       ('Chuối tiêu',
        'Chuối tiêu chín vàng, mềm, ngọt tự nhiên, giàu kali và năng lượng. Thích hợp ăn trực tiếp, làm sinh tố hoặc bánh trái cây.',
        25000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/6/1.jpg', true, 68, 0),
       ('Chuối sứ',
        'Chuối sứ chín tự nhiên, mềm, ngọt dịu, giàu kali và năng lượng. Thích hợp ăn trực tiếp, làm sinh tố hoặc bánh trái cây.',
        30000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/7/1.jpg', true, 45, 0),
       ('Dâu tây',
        'Dâu tây Đà Lạt siêu ngọt, tươi ngon, mọng nước, giàu vitamin C và chất chống oxy hóa. Lý tưởng cho ăn trực tiếp, salad hoặc làm mứt.',
        70000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'), '/images/products/8/1.jpg', true, 37, 0),
       ('Dưa hấu',
        'Dưa hấu mọng nước, ngọt thanh, giàu lycopene và vitamin C. Phù hợp ăn trực tiếp, làm nước ép hoặc tráng miệng giải khát.',
        25000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/9/1.jpg', true, 55, 0),
       ('Dưa lưới',
        'Dưa lưới vàng ngọt, thơm đặc trưng, mọng nước, giàu beta-carotene. Ăn trực tiếp làm tráng miệng hoặc sinh tố.',
        45000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/10/1.jpg', true, 61, 0),
       ('Nho xanh',
        'Nho xanh không hạt, giòn ngọt, mọng nước, giàu resveratrol và vitamin K. Thích hợp ăn trực tiếp, làm rượu vang hoặc salad.',
        85000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/11/1.jpg', true, 48, 0),
       ('Nho đỏ',
        'Nho đỏ không hạt, mọng nước, ngọt thanh, giàu chất chống oxy hóa và vitamin. Thích hợp ăn trực tiếp, làm salad hoặc rượu vang tại nhà.',
        90000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'), '/images/products/12/1.jpg', true, 53, 0),
       ('Nho tím',
        'Nho tím không hạt, mọng, ngọt, giàu chất chống oxy hóa và vitamin. Thích hợp ăn trực tiếp, làm salad hoặc rượu vang tại nhà.',
        80000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'), '/images/products/13/1.jpg', true, 40, 0),
       ('Xoài cát',
        'Xoài cát Hòa Lộc, vàng ươm, thơm ngọt, thịt mềm, giàu vitamin A và C. Phù hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/14/1.jpg', true, 69, 0),
       ('Xoài tượng',
        'Xoài tượng da xanh, quả to, thịt dày, ngọt thanh, giàu vitamin A và C. Thích hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.',
        55000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/15/1.jpg', true, 36, 0),
       ('Xoài keo',
        'Xoài keo chín vàng, ngọt mát, thịt mềm, giàu vitamin A và C. Phù hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/16/1.jpg', true, 50, 0),
       ('Dừa xiêm',
        'Dừa xiêm xanh, nước ngọt mát, cùi dừa non béo ngậy, giàu chất điện giải và vitamin. Thích hợp uống trực tiếp hoặc làm sinh tố, chè.',
        40000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'), '/images/products/17/1.jpg', true, 63, 0),
       ('Dưa chuột',
        'Dưa chuột tươi ngon, giòn mát, mọng nước, giàu vitamin K và chất xơ. Thích hợp ăn sống, làm salad hoặc nước detox thanh mát.',
        12000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/18/1.jpg', true, 44, 0),
       ('Quýt hồng',
        'Quýt hồng ngọt lịm, mọng nước, giàu vitamin C và chất xơ. Phù hợp ăn trực tiếp hoặc làm nước ép giải khát.',
        40000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/19/1.jpg', true, 57, 0),
       ('Bưởi da xanh',
        'Bưởi da xanh đặc sản miền Tây, múi mọng, ngọt thanh, giàu vitamin C và chất chống oxy hóa. Ăn trực tiếp hoặc làm nước ép đều thơm ngon.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'), '/images/products/20/1.jpg', true, 41, 0),
       ('Chôm chôm',
        'Chôm chôm vỏ mỏng, ngọt nước, mọng, giàu vitamin C và chất chống oxy hóa. Phù hợp ăn trực tiếp hoặc làm món tráng miệng.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/21/1.jpg', true, 66, 0),
       ('Thanh long đỏ',
        'Thanh long đỏ mọng nước, ngọt nhẹ, giàu vitamin C và chất chống oxy hóa. Ăn trực tiếp hoặc làm sinh tố giải khát.',
        40000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/22/1.jpg', true, 38, 0),
       ('Mít',
        'Mít chín vàng, ngọt thơm, giàu vitamin C, chất xơ và năng lượng. Ăn trực tiếp hoặc làm sinh tố, bánh trái cây.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/23/1.jpg', true, 52, 0),
       ('Kiwi',
        'Kiwi nhập khẩu, chua ngọt vừa phải, giàu vitamin C, K và chất xơ, tốt cho tiêu hóa và miễn dịch. Ăn trực tiếp hoặc làm nước ép.',
        90000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/24/1.jpg', true, 60, 0),
       ('Dưa vàng',
        'Dưa vàng thơm ngọt, mọng nước, giàu vitamin A và C, tốt cho da và mắt. Ăn trực tiếp hoặc làm sinh tố, salad.',
        50000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/25/1.jpg', true, 47, 0),
       ('Mơ',
        'Mơ chín mọng, vị ngọt nhẹ và hơi chua, giàu vitamin A, C, chất chống oxy hóa. Ăn trực tiếp hoặc làm mứt, nước ép.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/26/1.jpg', true, 54, 0),
       ('Ổi',
        'Ổi tươi ngon, giòn, mọng nước, giàu vitamin C và chất xơ, hỗ trợ tiêu hóa. Ăn trực tiếp hoặc làm nước ép, salad.',
        30000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/27/1.jpg', true, 43, 0),
       ('Măng cụt',
        'Măng cụt chín mọng, vị ngọt thanh, giàu chất chống oxy hóa, vitamin và khoáng chất. Ăn trực tiếp hoặc làm món tráng miệng.',
        100000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/28/1.jpg', true, 67, 0),
       ('Hồng giòn',
        'Hồng giòn chín mọng, vị ngọt dịu, giàu chất xơ và vitamin, tốt cho hệ tiêu hóa. Ăn trực tiếp hoặc làm salad.',
        70000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/29/1.jpg', true, 39, 0),
       ('Đu đủ',
        'Đu đủ chín vàng, ngọt mát, giàu vitamin A, C và enzyme papain hỗ trợ tiêu hóa. Ăn trực tiếp hoặc làm sinh tố.',
        30000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/30/1.jpg', true, 59, 0),
       ('Cherry',
        'Cherry đỏ tươi ngon, ngọt thanh, giàu chất chống oxy hóa và melatonin tự nhiên. Tốt cho giấc ngủ và chống viêm. Ăn trực tiếp hoặc làm mứt.',
        150000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'), '/images/products/31/1.jpg', true, 49, 0),
       ('Táo xanh',
        'Táo xanh Granny Smith, vị chua ngọt đặc trưng, giàu pectin và chất xơ. Thích hợp ăn trực tiếp, làm bánh hoặc salad.',
        55000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/32/1.jpg', true, 62, 0),
       ('Bưởi hồng',
        'Bưởi hồng ruột đỏ, ngọt thanh, ít đắng, giàu lycopene và vitamin C. Ăn trực tiếp hoặc làm salad trái cây.',
        65000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'), '/images/products/33/1.jpg', true, 46, 0),
       ('Vải thiều',
        'Vải thiều Lục Ngạn, thịt trắng ngọt lịm, thơm đặc trưng, giàu vitamin C. Ăn trực tiếp hoặc làm nước ép, mứt.',
        80000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/34/1.jpg', true, 56, 0),
       ('Nhãn',
        'Nhãn tươi ngọt thanh, thịt trắng trong, giàu vitamin C và glucose tự nhiên. Ăn trực tiếp hoặc nấu chè, làm mứt.',
        60000, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/35/1.jpg', true, 64, 0),
       ('Mãng cầu',
        'Mãng cầu chín mềm, vị ngọt đặc biệt, giàu vitamin C và chất xơ. Ăn trực tiếp hoặc làm kem, sinh tố.', 70000,
        (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'), '/images/products/36/1.jpg', true, 38, 0),
       ('Hồng xiêm tươi', 'Trái cây tươi ngon nhất', 12345, (SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
        '/images/products/37/1.jpg', false, 38, 0);

-----------------------------------------

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

-- Quýt
((SELECT "Id" FROM "Products" WHERE "Name" = 'Quýt'), (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Có Múi')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Quýt'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Chuối tiêu
((SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối tiêu'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối tiêu'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nội Địa')),

-- Chuối sứ
((SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối sứ'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Nhiệt Đới')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối sứ'),
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

-- Quýt hồng
((SELECT "Id" FROM "Products" WHERE "Name" = 'Quýt hồng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Có Múi')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Quýt hồng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Trái Cây Đặc Sản')),

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

-- Dưa vàng
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa vàng'),
 (SELECT "Id" FROM "Categories" WHERE "Name" = 'Dưa Các Loại')),
((SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa vàng'),
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
        (SELECT "Id" FROM "Products" WHERE "Name" = 'Quýt hồng'), false),
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
(1030, '2026-05-13 08:30:00+00', 1, 0, 65000, '{
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
  "ImageUrl": "/images/products/1/1.jpg",
  "ProductUnitName": "Kg",
  "UnitPrice": 50000
}'::jsonb),
       (1001, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageUrl": "/images/products/28/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),

       (1002, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), 1, '{
         "Name": "Cherry",
         "ImageUrl": "/images/products/31/1.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 150000
       }'::jsonb),

       (1003, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'), 5, '{
         "Name": "Dâu tây",
         "ImageUrl": "/images/products/8/1.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 70000
       }'::jsonb),

       (1004, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'), 1, '{
         "Name": "Cam sành",
         "ImageUrl": "/images/products/3/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 35000
       }'::jsonb),
       (1004, (SELECT "Id" FROM "Products" WHERE "Name" = 'Quýt'), 1, '{
         "Name": "Quýt",
         "ImageUrl": "/images/products/5/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),

       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 2, '{
         "Name": "Nho xanh",
         "ImageUrl": "/images/products/11/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),
       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'), 2, '{
         "Name": "Dưa lưới",
         "ImageUrl": "/images/products/10/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),
       (1005, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'), 2, '{
         "Name": "Bưởi da xanh",
         "ImageUrl": "/images/products/20/1.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 60000
       }'::jsonb),

       (1006, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài cát'), 1, '{
         "Name": "Xoài cát",
         "ImageUrl": "/images/products/14/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1006, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài tượng'), 1, '{
         "Name": "Xoài tượng",
         "ImageUrl": "/images/products/15/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),

       (1007, (SELECT "Id" FROM "Products" WHERE "Name" = 'Kiwi'), 2, '{
         "Name": "Kiwi",
         "ImageUrl": "/images/products/24/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 90000
       }'::jsonb),
       (1007, (SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'), 1, '{
         "Name": "Vải thiều",
         "ImageUrl": "/images/products/34/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 80000
       }'::jsonb),

       (1008, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho đỏ'), 2, '{
         "Name": "Nho đỏ",
         "ImageUrl": "/images/products/12/1.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 90000
       }'::jsonb),

       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Thanh long đỏ'), 3, '{
         "Name": "Thanh long đỏ",
         "ImageUrl": "/images/products/22/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 40000
       }'::jsonb),
       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối sứ'), 1, '{
         "Name": "Chuối sứ",
         "ImageUrl": "/images/products/7/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),
       (1009, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'), 1, '{
         "Name": "Nhãn",
         "ImageUrl": "/images/products/35/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),

       (1010, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'), 1, '{
         "Name": "Mít",
         "ImageUrl": "/images/products/23/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1010, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'), 1, '{
         "Name": "Dưa hấu",
         "ImageUrl": "/images/products/9/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),

       (1011, (SELECT "Id" FROM "Products" WHERE "Name" = 'Hồng giòn'), 2, '{
         "Name": "Hồng giòn",
         "ImageUrl": "/images/products/29/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 70000
       }'::jsonb),

       (1012, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mơ'), 2, '{
         "Name": "Mơ",
         "ImageUrl": "/images/products/26/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1012, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mãng cầu'), 1, '{
         "Name": "Mãng cầu",
         "ImageUrl": "/images/products/36/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 70000
       }'::jsonb),

       (1013, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa chuột'), 2, '{
         "Name": "Dưa chuột",
         "ImageUrl": "/images/products/18/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 12000
       }'::jsonb),
       (1013, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dừa xiêm'), 1, '{
         "Name": "Dừa xiêm",
         "ImageUrl": "/images/products/17/1.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 40000
       }'::jsonb),

       (1014, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), 2, '{
         "Name": "Cherry",
         "ImageUrl": "/images/products/31/1.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 150000
       }'::jsonb),
       (1014, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 2, '{
         "Name": "Nho xanh",
         "ImageUrl": "/images/products/11/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),

       (1015, (SELECT "Id" FROM "Products" WHERE "Name" = 'Ổi'), 3, '{
         "Name": "Ổi",
         "ImageUrl": "/images/products/27/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),

       (1016, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 11, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1017, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dâu tây'), 3, '{
         "Name": "Dâu tây",
         "ImageUrl": "/images/products/8/1.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 70000
       }'::jsonb),

       (1018, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa hấu'), 3, '{
         "Name": "Dưa hấu",
         "ImageUrl": "/images/products/9/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),

       (1019, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho tím'), 4, '{
         "Name": "Nho tím",
         "ImageUrl": "/images/products/13/1.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 80000
       }'::jsonb),

       (1020, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam vàng'), 2, '{
         "Name": "Cam vàng",
         "ImageUrl": "/images/products/4/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 40000
       }'::jsonb),
       (1020, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi hồng'), 1, '{
         "Name": "Bưởi hồng",
         "ImageUrl": "/images/products/33/1.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 65000
       }'::jsonb),

       (1021, (SELECT "Id" FROM "Products" WHERE "Name" = 'Vải thiều'), 2, '{
         "Name": "Vải thiều",
         "ImageUrl": "/images/products/34/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 80000
       }'::jsonb),

       (1022, (SELECT "Id" FROM "Products" WHERE "Name" = 'Chôm chôm'), 2, '{
         "Name": "Chôm chôm",
         "ImageUrl": "/images/products/21/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1022, (SELECT "Id" FROM "Products" WHERE "Name" = 'Bưởi da xanh'), 1, '{
         "Name": "Bưởi da xanh",
         "ImageUrl": "/images/products/20/1.jpg",
         "ProductUnitName": "Quả",
         "UnitPrice": 60000
       }'::jsonb),

       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Kiwi'), 3, '{
         "Name": "Kiwi",
         "ImageUrl": "/images/products/24/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 90000
       }'::jsonb),
       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'), 1, '{
         "Name": "Táo xanh",
         "ImageUrl": "/images/products/32/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),
       (1023, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nho xanh'), 1, '{
         "Name": "Nho xanh",
         "ImageUrl": "/images/products/11/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 85000
       }'::jsonb),

       (1024, (SELECT "Id" FROM "Products" WHERE "Name" = 'Xoài keo'), 1, '{
         "Name": "Xoài keo",
         "ImageUrl": "/images/products/16/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1024, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cam sành'), 1, '{
         "Name": "Cam sành",
         "ImageUrl": "/images/products/3/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 35000
       }'::jsonb),

       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageUrl": "/images/products/28/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),
       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa lưới'), 1, '{
         "Name": "Dưa lưới",
         "ImageUrl": "/images/products/10/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 45000
       }'::jsonb),
       (1025, (SELECT "Id" FROM "Products" WHERE "Name" = 'Mít'), 1, '{
         "Name": "Mít",
         "ImageUrl": "/images/products/23/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Nhãn'), 2, '{
         "Name": "Nhãn",
         "ImageUrl": "/images/products/35/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Đu đủ'), 1, '{
         "Name": "Đu đủ",
         "ImageUrl": "/images/products/30/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 30000
       }'::jsonb),
       (1026, (SELECT "Id" FROM "Products" WHERE "Name" = 'Măng cụt'), 1, '{
         "Name": "Măng cụt",
         "ImageUrl": "/images/products/28/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 100000
       }'::jsonb),

       (1027, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo vàng'), 2, '{
         "Name": "Táo vàng",
         "ImageUrl": "/images/products/2/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 60000
       }'::jsonb),
       (1027, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo xanh'), 1, '{
         "Name": "Táo xanh",
         "ImageUrl": "/images/products/32/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 55000
       }'::jsonb),

       (1028, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa vàng'), 2, '{
         "Name": "Dưa vàng",
         "ImageUrl": "/images/products/25/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),
       (1028, (SELECT "Id" FROM "Products" WHERE "Name" = 'Dưa chuột'), 1, '{
         "Name": "Dưa chuột",
         "ImageUrl": "/images/products/18/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 12000
       }'::jsonb),

       (1029, (SELECT "Id" FROM "Products" WHERE "Name" = 'Cherry'), 3, '{
         "Name": "Cherry",
         "ImageUrl": "/images/products/31/1.jpg",
         "ProductUnitName": "Hộp",
         "UnitPrice": 150000
       }'::jsonb),
       (1029, (SELECT "Id" FROM "Products" WHERE "Name" = 'Táo đỏ'), 1, '{
         "Name": "Táo đỏ",
         "ImageUrl": "/images/products/1/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 50000
       }'::jsonb),

       (1030, (SELECT "Id" FROM "Products" WHERE "Name" = 'Chuối tiêu'), 2, '{
         "Name": "Chuối tiêu",
         "ImageUrl": "/images/products/6/1.jpg",
         "ProductUnitName": "Kg",
         "UnitPrice": 25000
       }'::jsonb),
       (1030, (SELECT "Id" FROM "Products" WHERE "Name" = 'Quýt hồng'), 1, '{
         "Name": "Quýt hồng",
         "ImageUrl": "/images/products/19/1.jpg",
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