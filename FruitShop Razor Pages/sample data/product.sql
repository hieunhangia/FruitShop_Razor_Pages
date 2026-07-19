INSERT INTO "ProductUnits" ("Name", "IsActive")
VALUES ('Kg', true),
       ('Quả', true),
       ('Hộp', true),
       ('Giỏ', true);

-----------------------------------------

INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageFilePath", "IsActive", "Quantity",
                        "HeldQuantity", "DisplayOrder")
VALUES ('Táo đỏ',
'## Táo Đỏ Nhập Khẩu Cao Cấp

Táo đỏ nhập khẩu nổi bật với sắc đỏ tươi bắt mắt, quả to đều, thịt giòn và mọng nước. Đây không chỉ là một loại trái cây tráng miệng thơm ngon mà còn là nguồn cung cấp dinh dưỡng dồi dào cho cả gia đình.

![Táo đỏ tươi ngon](http://localhost:9000/public/images/products/1_description1.jpg)

### Giá trị dinh dưỡng vượt trội
* **Giàu Vitamin C:** Hỗ trợ tăng cường hệ miễn dịch, làm sáng da và chống lại các bệnh cảm cúm thông thường.
* **Nguồn chất xơ tự nhiên:** Rất tốt cho hệ tiêu hóa, giúp tạo cảm giác no lâu, hỗ trợ quá trình giảm cân và duy trì vóc dáng.
* **Chất chống oxy hóa:** Chứa nhiều polyphenol giúp ngăn ngừa lão hóa, bảo vệ tế bào khỏi sự tấn công của các gốc tự do.

### Hướng dẫn chế biến và sử dụng
Táo đỏ có vị ngọt thanh tự nhiên, vô cùng đa dụng trong gian bếp. Bạn có thể thưởng thức trực tiếp sau khi rửa sạch, thái lát trộn cùng xà lách để làm món salad thanh mát, hoặc kết hợp với cà rốt để làm nước ép detox mỗi buổi sáng.

![Món ngon từ táo đỏ](http://localhost:9000/public/images/products/1_description2.jpg)

**Mẹo bảo quản:** Để giữ được độ giòn ngọt nguyên bản, hãy bọc táo bằng giấy báo hoặc túi nilon đục lỗ và cất trong ngăn mát tủ lạnh (nhiệt độ từ 4-8 độ C). Tránh để cạnh các loại trái cây sinh khí ethylene chín nhanh như chuối.',
50000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/1.jpg', true, 58, 0, 0),
('Táo vàng',
'## Táo Vàng Mỹ Giòn Ngọt Đậm Đà

Táo vàng Mỹ luôn là lựa chọn hàng đầu của những tín đồ yêu thích sự giòn tan và hương thơm đặc trưng. Lớp vỏ mỏng màu vàng óng ả bao bọc lấy phần thịt quả trắng ngà, mọng nước và ngọt lịm.

![Táo vàng nguyên quả](http://localhost:9000/public/images/products/2_description1.jpg)

### Tại sao nên ăn táo vàng mỗi ngày?
Táo vàng không chỉ ngon miệng mà còn mang lại vô vàn lợi ích sức khỏe:
1. Cung cấp năng lượng tức thì nhờ lượng đường fructose tự nhiên.
2. Giảm lượng cholesterol xấu trong máu, tốt cho sức khỏe tim mạch.
3. Chứa nhiều khoáng chất như Kali, Canxi giúp xương chắc khỏe hơn.

![Nước ép táo vàng](http://localhost:9000/public/images/products/2_description2.jpg)
 
### Cách thưởng thức tuyệt vời
Với độ giòn hoàn hảo, táo vàng rất thích hợp để ăn tươi hoặc làm nguyên liệu cho các món bánh nướng (Apple Pie), mứt táo ăn kèm bánh mì, hoặc xắt hạt lựu trộn cùng sữa chua không đường.

![Bánh nướng táo vàng](http://localhost:9000/public/images/products/2_description3.jpg)

**Gợi ý bảo quản:** Tương tự như táo đỏ, táo vàng nên được giữ lạnh ngay sau khi mua về. Hạn chế rửa táo nếu chưa ăn ngay để tránh làm mất đi lớp sáp bảo vệ tự nhiên ngoài vỏ.',
60000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/2.jpg', true, 42, 0, 1),
('Cam sành',
'## Cam Sành Việt Nam - Thức Uống Thanh Mát Ngày Hè

Cam sành là một trong những loại trái cây đặc sản nổi tiếng của Việt Nam. Trái cam vỏ sần sùi, màu xanh mướt nhưng bên trong lại chứa đựng những tép cam vàng cam rực rỡ, mọng nước và mang hương vị ngọt đậm đà xen lẫn chút chua thanh cực kỳ đưa miệng.

![Cam sành tươi cắt lát](http://localhost:9000/public/images/products/3_description1.jpg)

### Kho báu dinh dưỡng từ thiên nhiên
* **Hàm lượng Vitamin C cực cao:** Cam sành là "vua" trong việc bổ sung vitamin C, giúp phục hồi sức khỏe nhanh chóng khi ốm, tăng đề kháng mạnh mẽ.
* **Folate (Vitamin B9):** Cực kỳ quan trọng và tốt cho phụ nữ mang thai, hỗ trợ sự phát triển khỏe mạnh của tế bào.
* **Nguồn Kali dồi dào:** Giúp cân bằng huyết áp và duy trì sự ổn định của hệ thống tim mạch.

### Gợi ý sử dụng và bảo quản
Cam sành là nguyên liệu hoàn hảo nhất để vắt nước uống giải khát, đặc biệt trong những ngày nắng nóng hoặc sau khi vận động thể thao. Bạn có thể thêm một chút mật ong và đá lạnh để thức uống thêm phần bùng nổ hương vị.

![Ly nước cam sành mật ong](http://localhost:9000/public/images/products/3_description2.jpg)

**Bảo quản:** Cam sành khá dễ tính. Bạn có thể để cam ở nhiệt độ phòng, nơi thoáng mát trong khoảng 1 tuần, hoặc cất vào ngăn mát tủ lạnh để kéo dài thời gian sử dụng lên đến 2-3 tuần.',
35000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/3.jpg', true, 65, 0, 2),
('Cam vàng',
'## Cam Vàng Nhập Khẩu Mọng Nước, Ngọt Lịm

Cam vàng (Navel) nổi danh với ngoại hình bắt mắt: quả to, vỏ màu vàng ươm láng mịn, rất dễ bóc vỏ và đặc biệt là không có hạt. Thịt cam chắc, tép cam to và giữ được rất nhiều nước.

![Cam vàng nhập khẩu](http://localhost:9000/public/images/products/4_description1.jpg)

### Điểm nhấn dinh dưỡng
Cam vàng là nguồn cung cấp vitamin C, Kali và chất xơ tuyệt vời. Đặc biệt, lớp cùi trắng của cam vàng chứa nhiều flavonoid – một hoạt chất chống oxy hóa mạnh giúp kháng viêm và bảo vệ cơ thể. Mùi tinh dầu từ vỏ cam cũng mang lại tác dụng thư giãn tinh thần rất tốt.

### Ứng dụng trong ẩm thực
Nhờ ưu điểm không hạt và vị ngọt sắc không bị chua, cam vàng sinh ra là để ăn trực tiếp. Bạn chỉ cần bổ cau và thưởng thức. Ngoài ra, cam vàng còn xuất hiện nhiều trong các món Âu:
* Làm sốt cam cho món vịt áp chảo hoặc cá hồi nướng.
* Decor làm đẹp cho các ly cocktail, mocktail sang trọng.
* Trộn cùng ức gà và rau bina để làm salad ăn kiêng.

![Sốt cam vàng cá hồi](http://localhost:9000/public/images/products/4_description2.jpg)

**Cách bảo quản:** Nên bọc từng quả cam bằng nilon thực phẩm và cất tủ mát. Việc này giúp vỏ cam không bị héo và phần tép bên trong giữ nguyên độ mọng nước.',
40000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/4.jpg', true, 39, 0, 3);

INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageFilePath", "IsActive", "Quantity",
                        "HeldQuantity", "DisplayOrder")
VALUES ('Chuối tiêu',
'## Chuối Tiêu Chín Tự Nhiên - Nguồn Năng Lượng Dồi Dào

Chuối tiêu là loại trái cây quen thuộc nhưng mang lại giá trị dinh dưỡng vô cùng to lớn. Những quả chuối chín vàng ươm, mềm mịn và tỏa hương thơm nức mũi sẽ là bữa phụ hoàn hảo cho mọi lứa tuổi.

![Chuối tiêu chín vàng](http://localhost:9000/public/images/products/5_description1.jpg)

### Lợi ích tuyệt vời cho sức khỏe
* **Giàu Kali:** Hỗ trợ điều hòa huyết áp, giảm nguy cơ mắc bệnh tim mạch và đột quỵ.
* **Năng lượng tức thì:** Carbohydrate tự nhiên trong chuối giúp bổ sung năng lượng nhanh chóng trước và sau khi tập luyện thể thao.
* **Tốt cho hệ tiêu hóa:** Hàm lượng chất xơ cao giúp cải thiện đường ruột, ngăn ngừa táo bón hiệu quả.

### Đa dạng cách thưởng thức
Chuối tiêu ngon nhất là khi ăn trực tiếp lúc vỏ bắt đầu xuất hiện lốm đốm đen (chuối trứng cuốc) vì lúc này độ ngọt đạt mức cao nhất. Ngoài ra, bạn có thể biến tấu chuối tiêu thành nhiều món ngon:
* Sinh tố chuối bơ đậu phộng bổ dưỡng.
* Bánh mì chuối (Banana Bread) thơm lừng góc bếp.
* Kem chuối mát lạnh giải nhiệt mùa hè.

![Bánh mì chuối thơm ngon](http://localhost:9000/public/images/products/5_description2.jpg)

**Cách bảo quản:** Nên để chuối ở nhiệt độ phòng, treo trên giá để chuối chín đều và không bị giập. Không nên cho chuối chưa chín vào tủ lạnh vì sẽ làm gián đoạn quá trình chín tự nhiên, khiến vỏ bị thâm đen nhưng ruột vẫn sượng.',
25000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/5.jpg', true, 68, 0, 4),
('Dâu tây',
'## Dâu Tây Đà Lạt - Quả Mọng Ngọt Ngào Từ Cao Nguyên

Dâu tây Đà Lạt nổi tiếng với vẻ ngoài đỏ mọng quyến rũ, hình dáng trái tim đẹp mắt và hương vị chua ngọt hòa quyện đầy mê hoặc. Được trồng trong môi trường khí hậu ôn đới lý tưởng, mỗi quả dâu tây đều đạt chuẩn chất lượng cao nhất.

![Dâu tây Đà Lạt tươi mọng](http://localhost:9000/public/images/products/6_description1.jpg)

### Tinh túy dinh dưỡng trong từng quả dâu
* **Siêu thực phẩm làm đẹp:** Lượng vitamin C và chất chống oxy hóa dồi dào giúp sản sinh collagen, làm mờ vết thâm và mang lại làn da sáng mịn rạng rỡ.
* **Hỗ trợ thị lực:** Các dưỡng chất trong dâu tây giúp bảo vệ mắt khỏi tác hại của tia UV và ngăn ngừa đục thủy tinh thể.
* **Kiểm soát đường huyết:** Dâu tây có chỉ số đường huyết thấp, rất an toàn và tốt cho người bệnh tiểu đường nếu dùng với lượng vừa phải.

### Thưởng thức dâu tây chuẩn vị
Dâu tây ngon nhất khi được ướp lạnh và ăn trực tiếp để cảm nhận trọn vẹn độ tươi mới. Bên cạnh đó, dâu tây còn là linh hồn của các món tráng miệng:
* Trang trí bánh kem, bánh tart thêm phần bắt mắt.
* Trộn cùng sữa chua Hy Lạp và hạt granola cho bữa sáng healthy.
* Nấu mứt dâu tây thủ công ăn kèm bánh mì sandwich.

![Bánh tart dâu tây tuyệt đẹp](http://localhost:9000/public/images/products/6_description2.jpg)
![Mứt dâu tây ngọt ngào](http://localhost:9000/public/images/products/6_description3.jpg)

**Cách bảo quản:** Dâu tây rất mỏng manh và dễ dập nát. Chỉ rửa dâu tây ngay trước khi ăn. Phần chưa dùng đến, hãy lót giấy thấm hút ẩm vào hộp thủy tinh, xếp dâu tây thành một lớp rồi bảo quản trong ngăn mát tủ lạnh.',
70000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
'images/products/6.jpg', true, 37, 0, 5),
('Dưa hấu',
'## Dưa Hấu Thanh Mát - Đánh Bay Cơn Khát Ngày Hè

Dưa hấu vỏ mỏng, ruột đỏ tươi, ít hạt và đặc biệt là cực kỳ mọng nước. Vị ngọt thanh mát của dưa hấu sẽ ngay lập tức làm dịu đi cái nóng oi bức, mang lại cảm giác sảng khoái tột độ.

![Dưa hấu bổ luống tươi mát](http://localhost:9000/public/images/products/7_description1.jpg)

### Lợi ích bất ngờ từ quả dưa hấu
* **Dưỡng ẩm cho cơ thể:** Chứa đến 92% là nước, dưa hấu là giải pháp bù nước tuyệt vời, đặc biệt trong những ngày hè hoặc sau khi vận động ra nhiều mồ hôi.
* **Bảo vệ sức khỏe tim mạch:** Cung cấp dồi dào Lycopene - một chất chống oxy hóa mạnh mẽ giúp giảm mức cholesterol và huyết áp.
* **Giảm đau nhức cơ bắp:** Axit amin Citrulline trong dưa hấu được chứng minh là có khả năng làm giảm tình trạng đau mỏi cơ bắp hiệu quả.

### Bí quyết giải nhiệt với dưa hấu
Chỉ cần bổ dưa hấu thành từng miếng nhỏ và cất vào tủ lạnh, bạn đã có ngay một món tráng miệng tuyệt hảo. Thú vị hơn, hãy thử:
* Xay sinh tố hoặc ép nước dưa hấu nguyên chất.
* Cắt khối vuông làm thạch rau câu dưa hấu lạ miệng.
* Xiên que kết hợp cùng phô mai feta và lá bạc hà.

![Nước ép dưa hấu đá lạnh](http://localhost:9000/public/images/products/7_description2.jpg)

**Bảo quản:** Dưa hấu chưa bổ có thể để ở nhiệt độ phòng nơi râm mát khoảng 1-2 tuần. Sau khi bổ, cần bọc màng bọc thực phẩm kín phần mặt cắt và bảo quản trong ngăn mát, nên dùng hết trong 3 ngày để dưa không bị úng ruột.',
25000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/7.jpg', true, 55, 0, 6),
('Dưa lưới',
'## Dưa Lưới Giòn Ngọt - Trái Cây Thanh Lọc Cơ Thể

Dưa lưới nổi bật với lớp vỏ gân lưới đan xen độc đáo, phần thịt quả dày màu cam vô cùng bắt mắt. Khi thưởng thức, bạn sẽ cảm nhận được độ giòn rụm, vị ngọt lịm đặc trưng cùng hương thơm dịu nhẹ quyến luyến mãi không thôi.

![Dưa lưới vàng ươm](http://localhost:9000/public/images/products/8_description1.jpg)

### Dinh dưỡng vàng trong mỗi lát dưa
* **Cải thiện thị lực:** Màu cam của dưa lưới đến từ Beta-carotene, tiền chất của Vitamin A, rất tốt cho mắt và giúp ngăn ngừa thoái hóa điểm vàng.
* **Tăng cường hệ miễn dịch:** Lượng Vitamin C cao giúp cơ thể sản sinh bạch cầu, chống lại các tác nhân gây nhiễm trùng.
* **Làm đẹp da:** Các khoáng chất và nước trong dưa lưới giúp dưỡng ẩm da từ sâu bên trong, mang lại làn da căng bóng.

### Ứng dụng ẩm thực tinh tế
Dưa lưới thường được dùng làm món tráng miệng trong các bữa tiệc sang trọng. Ngoài ăn tươi, bạn có thể:
* Kết hợp cùng thịt xông khói Parma (Prosciutto e Melone) - món khai vị trứ danh của Ý.
* Thái hạt lựu làm bingsu (đá bào) dưa lưới mát lạnh.
* Trộn salad trái cây đầy màu sắc.

![Món khai vị dưa lưới thịt xông khói](http://localhost:9000/public/images/products/8_description2.jpg)

**Cách chọn và bảo quản:** Nên chọn quả có vân lưới nổi gồ ghề, sờ vào thấy cứng cáp, cầm nặng tay và có mùi thơm thoang thoảng ở phần cuống. Dưa mua về để ngoài nhiệt độ phòng đến khi cuống hơi héo rồi cho vào tủ lạnh để tăng độ ngọt.',
45000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/8.jpg', true, 61, 0, 7),
('Nho xanh',
'## Nho Xanh Không Hạt - Giòn Tan Bùng Nổ Vị Giác

Nho xanh không hạt nhập khẩu là định nghĩa hoàn hảo của một loại trái cây ăn nhẹ tiện lợi và đẳng cấp. Quả nho thuôn dài, vỏ mỏng tang, thịt nho giòn sần sật và căng mọng nước, mang đến vị ngọt thanh khiết vô cùng dễ chịu.

![Chùm nho xanh không hạt](http://localhost:9000/public/images/products/9_description1.jpg)

### Lợi ích sức khỏe toàn diện
* **Tốt cho xương khớp:** Hàm lượng Vitamin K dồi dào trong nho xanh đóng vai trò quan trọng trong việc cải thiện mật độ xương.
* **Chống oxy hóa mạnh mẽ:** Chứa nhiều resveratrol, giúp bảo vệ tế bào não và làm chậm quá trình lão hóa.
* **Hỗ trợ giảm cân:** Ít calo nhưng lại giàu nước và chất xơ, nho xanh là món ăn vặt lý tưởng cho những ai đang ăn kiêng.

### Thưởng thức nho xanh đúng điệu
Với ưu điểm không hạt, nho xanh vô cùng an toàn cho trẻ nhỏ và người lớn tuổi. Bạn có thể bốc ăn cả quả một cách thỏa thích, hoặc:
* Cắt đôi nho xanh cho vào đĩa phô mai tổng hợp (Cheese board).
* Trang trí lên trên ly sinh tố xanh hoặc yến mạch ngâm qua đêm.
* Ép lấy nước nho trong vắt, ngọt thanh sảng khoái.

![Đĩa phô mai và nho xanh](http://localhost:9000/public/images/products/9_description2.jpg)

**Mẹo bảo quản và rửa nho:** Nho xanh được phủ một lớp phấn trắng tự nhiên (bloom) để bảo vệ vỏ. Chỉ nên rửa nho trước khi ăn bằng cách ngâm nước muối loãng hoặc dung dịch baking soda trong 5 phút. Nếu chưa ăn, hãy cất vào hộp kín, để trong ngăn mát để nho luôn giữ được độ giòn vốn có.',
85000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/9.jpg', true, 48, 0, 8);

INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageFilePath", "IsActive", "Quantity",
                        "HeldQuantity", "DisplayOrder")
VALUES ('Nho đỏ',
'## Nho Đỏ Không Hạt - Vị Ngọt Ngào Khó Cưỡng

Nho đỏ không hạt luôn là một trong những loại trái cây nhập khẩu được ưa chuộng nhất nhờ sắc đỏ ruby tuyệt đẹp, thịt quả mọng nước và độ ngọt đậm đà đánh thức mọi giác quan.

![Nho đỏ ruby mọng nước](http://localhost:9000/public/images/products/10_description1.jpg)

### Bí mật dinh dưỡng bên trong lớp vỏ
* **Chống oxy hóa đỉnh cao:** Vỏ nho đỏ chứa lượng lớn Anthocyanin và Resveratrol, giúp bảo vệ tim mạch, chống lão hóa và ngăn ngừa sự phát triển của các tế bào xấu.
* **Tăng cường trí nhớ:** Nho đỏ giúp cải thiện lưu lượng máu lên não, hỗ trợ giảm căng thẳng và tăng cường khả năng tập trung.
* **Tốt cho tiêu hóa:** Nguồn chất xơ và khoáng chất tự nhiên giúp thanh lọc cơ thể nhẹ nhàng.

### Cách biến tấu với nho đỏ
Sự ngọt ngào của nho đỏ rất dễ dàng chinh phục cả trẻ em lẫn người lớn. Trải nghiệm tuyệt vời nhất là bứt từng quả nho mát lạnh thưởng thức trực tiếp, hoặc bạn có thể:
* Cắt đôi trộn cùng salad rau mầm và sốt mè rang.
* Ép cùng dưa hấu và táo để tạo ra ly nước ép thanh lọc cơ thể.
* Dùng làm nguyên liệu ngâm rượu vang trái cây tại nhà.

![Salad nho đỏ thanh mát](http://localhost:9000/public/images/products/10_description2.jpg)

**Gợi ý bảo quản:** Nho đỏ cực kỳ nhạy cảm với nước trước khi được làm lạnh. Hãy bọc kín chùm nho bằng giấy nilon đục lỗ, để ở nhiệt độ từ 0-4 độ C trong tủ lạnh. Chỉ rửa nho ngay trước khi ăn để nho không bị mềm và nhạt vị.',
90000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
'images/products/10.jpg', true, 53, 0, 9),
('Nho tím',
'## Nho Tím Mọng Nước - "Thần Dược" Cho Sức Khỏe

Nho tím mang trên mình một màu sắc huyền bí, lấp lánh dưới lớp phấn trắng mỏng tự nhiên. Với hương vị đậm đà, kết cấu thịt chắc và hoàn toàn không hạt, nho tím hứa hẹn mang lại một trải nghiệm ẩm thực vô cùng cao cấp.

![Chùm nho tím lấp lánh phấn](http://localhost:9000/public/images/products/11_description1.jpg)

### Lý do nên đưa nho tím vào thực đơn
* **Bảo vệ hệ tim mạch:** Nho tím được chứng minh có khả năng làm giảm quá trình đông máu và giảm mức cholesterol hiệu quả.
* **Ngăn ngừa lão hóa:** Sắc tố tím tự nhiên là minh chứng cho lượng chất chống oxy hóa khổng lồ, giúp duy trì làn da trẻ trung, đàn hồi.
* **Phục hồi năng lượng:** Lượng đường tự nhiên giúp phục hồi năng lượng tức thì sau những giờ làm việc mệt mỏi.

### Gợi ý chế biến độc đáo
Ngoài việc thưởng thức tươi như một món tráng miệng sang chảnh, nho tím còn rất được việc trong nhà bếp:
* Làm mứt nho tím phết bánh mì giòn tan cho bữa sáng.
* Thêm vào yến mạch trộn sữa chua tạo màu sắc hấp dẫn.
* Trang trí tinh tế cho các loại bánh ngọt, bánh tart.

![Mứt nho tím phết bánh mì](http://localhost:9000/public/images/products/11_description2.jpg)

**Mẹo nhỏ khi mua và lưu trữ:** Hãy chọn những chùm nho có cuống còn xanh tươi và hạt nho dính chặt vào cuống. Bảo quản trong hộp nhựa có lỗ thông khí ở ngăn mát tủ lạnh, nho tím có thể giữ được độ tươi ngon lên đến 1-2 tuần.',
80000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
'images/products/11.jpg', true, 40, 0, 10),
('Xoài cát',
'## Xoài Cát Hòa Lộc - Vua Của Các Loại Trái Cây Nhiệt Đới

Nhắc đến đặc sản Nam Bộ, không thể không nhắc đến Xoài Cát Hòa Lộc. Khi chín, quả xoài khoác lên mình màu vàng ươm rực rỡ, thịt xoài dày, mịn màng, hoàn toàn không có xơ và tỏa ra một mùi thơm nồng nàn quyến rũ.

![Xoài cát Hòa Lộc vàng ươm](http://localhost:9000/public/images/products/12_description1.jpg)

### Kho tàng Vitamin A và C
* **Tốt cho đôi mắt:** Lượng Vitamin A dồi dào trong xoài cát giúp ngăn ngừa khô mắt và quáng gà.
* **Tăng cường sức đề kháng:** Vitamin C kết hợp với hàng chục loại polyphenol giúp cơ thể xây dựng một hàng rào miễn dịch vững chắc.
* **Hỗ trợ tiêu hóa:** Chứa các enzyme bẻ gãy protein, giúp hệ tiêu hóa hoạt động trơn tru, nhẹ nhàng.

### Những món ngon từ xoài cát
Chỉ cần gọt vỏ, cắt lát và ướp lạnh, xoài cát đã là một món ăn "ngốn" không biết bao nhiêu lời khen ngợi. Để thêm phần phong phú, bạn có thể làm:
* Sinh tố xoài sữa chua béo ngậy.
* Chè xoài trân châu cốt dừa thơm lừng.
* Xôi xoài cốt dừa - món tráng miệng kiểu Thái "vạn người mê".

![Xôi xoài cốt dừa siêu hấp dẫn](http://localhost:9000/public/images/products/12_description2.jpg)

**Cách bảo quản xoài chín:** Nếu xoài đã chín mềm, hãy gọt vỏ, cắt thành từng miếng vừa ăn và cho vào hộp kín cất tủ lạnh để duy trì độ ngọt và cấu trúc thịt trái.',
60000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/12.jpg', true, 69, 0, 11),
('Xoài tượng',
'## Xoài Tượng Giòn Ngon - Chua Ngọt Hài Hòa

Đúng như tên gọi, Xoài Tượng có kích thước "khổng lồ", quả to, mình thuôn dài và chắc nịch. Lớp vỏ xanh bóng bẩy bao bọc lấy phần thịt giòn rụm bên trong, mang đến vị chua chua ngọt ngọt kích thích vị giác tột độ.

![Xoài tượng da xanh trái lớn](http://localhost:9000/public/images/products/13_description1.jpg)

### Dưỡng chất bất ngờ từ quả xoài xanh
* **Giàu chất xơ, ít calo:** Rất phù hợp cho người ăn kiêng, tạo cảm giác no lâu và hạn chế thèm ăn.
* **Phòng chống các vấn đề về máu:** Hàm lượng sắt và vitamin C trong xoài xanh hỗ trợ hấp thụ sắt tốt hơn, ngăn ngừa thiếu máu.
* **Cải thiện sức khỏe gan:** Ăn xoài tượng xanh giúp bài tiết axit mật và làm sạch gan một cách tự nhiên.

### Trải nghiệm ẩm thực kích thích vị giác
Xoài tượng sinh ra là để dành cho những tín đồ ăn vặt. Món ngon nhất và đơn giản nhất chính là xoài xanh chấm muối tôm hoặc muối ớt. Tiếng giòn rụm kết hợp cùng vị cay mặn ngọt chắc chắn sẽ khiến bạn không thể dừng lại. Ngoài ra:
* Gỏi xoài tôm thịt hay gỏi xoài tai heo nhâm nhi ngày cuối tuần.
* Xoài lắc muối ớt siêu hot trên đường phố.

![Gỏi xoài tôm thịt chua cay](http://localhost:9000/public/images/products/13_description2.jpg)

**Gợi ý chọn xoài:** Để ăn giòn, hãy chọn quả có vỏ xanh đậm, sờ vào cứng tay. Nếu muốn ăn xoài tượng chín, bạn có thể để xoài ở nhiệt độ phòng thêm vài ngày cho đến khi vỏ ngả vàng nhẹ, thịt sẽ mềm và ngọt hơn.',
55000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/13.jpg', true, 36, 0, 12),
('Xoài keo',
'## Xoài Keo - Món Quà Đặc Sản Từ Vùng Biên Giới

Xoài keo đặc trưng bởi hình dáng thon dài, hạt rất lép và phần thịt cực kỳ dày. Dù ăn lúc còn xanh giòn hay lúc đã chín mềm, xoài keo đều giữ được vị ngọt thanh đặc biệt không lẫn vào đâu được.

![Xoài keo thịt dày hạt lép](http://localhost:9000/public/images/products/14_description1.jpg)

### Tác dụng tuyệt vời đối với cơ thể
* **Làm đẹp da:** Vitamin C hỗ trợ sản xuất collagen, trong khi Vitamin E giúp giữ ẩm và làm sáng da.
* **Cung cấp khoáng chất vi lượng:** Magie và Canxi trong xoài keo giúp ổn định thần kinh và hệ cơ xương.
* **Giải nhiệt, thanh lọc:** Chứa nhiều nước và chất khoáng giúp hạ nhiệt cơ thể hiệu quả trong những ngày hè.

### Thỏa sức sáng tạo cùng xoài keo
Xoài keo là một loại quả cực kỳ linh hoạt. Bạn có thể thưởng thức theo vô vàn cách khác nhau:
* Ăn sống chấm muối ớt Tây Ninh lúc xoài vừa hái sểnh (vỏ xanh, ruột hơi ngả vàng).
* Khi xoài chín mềm, xay sinh tố cùng một chút sữa đặc và đá bào.
* Thái sợi làm nộm cá trê chiên giòn chua ngọt cực đưa cơm.

![Nộm xoài xanh giòn ngon](http://localhost:9000/public/images/products/14_description2.jpg)

**Mẹo bảo quản:** Với xoài keo xanh, bạn có thể để ở không gian bếp thoáng mát. Khi xoài bắt đầu có mùi thơm và mềm tay, hãy bảo quản trong tủ lạnh để hãm độ chín và thưởng thức dần.',
50000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/14.jpg', true, 50, 0, 13);

INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageFilePath", "IsActive", "Quantity",
                        "HeldQuantity", "DisplayOrder")
VALUES ('Dừa xiêm',
'## Dừa Xiêm Xanh - Nước Giải Khát Thiên Nhiên Tuyệt Đỉnh

Dừa xiêm xanh nổi tiếng với phần nước trong vắt, ngọt thanh và vô cùng mát lành. Lớp cùi dừa non bên trong mỏng, mềm và béo ngậy, mang lại cảm giác vô cùng sảng khoái khi thưởng thức vào những ngày oi bức.

![Dừa xiêm tươi mát](http://localhost:9000/public/images/products/15_description1.jpg)

### Nguồn điện giải tự nhiên hoàn hảo
* **Bù nước nhanh chóng:** Nước dừa chứa nhiều kali, magie và canxi, là thức uống bù điện giải tự nhiên tốt nhất sau khi tập thể thao hoặc khi cơ thể mệt mỏi.
* **Tăng cường hệ miễn dịch:** Chứa axit lauric có tác dụng kháng khuẩn, chống virus và bảo vệ cơ thể khỏi các bệnh nhiễm trùng nhẹ.
* **Đẹp da, mượt tóc:** Cytokinin trong nước dừa giúp điều hòa sự phát triển của tế bào da, làm chậm quá trình lão hóa và giúp da luôn căng bóng.

### Thưởng thức và chế biến
Cách tuyệt vời nhất là ướp lạnh quả dừa, cắm ống hút và thưởng thức trực tiếp nước dừa nguyên chất. Phần cùi non có thể dùng thìa nạo ăn ngay. Ngoài ra, dừa xiêm còn được dùng để:
* Làm thạch rau câu dừa tráng miệng thanh mát.
* Nấu thịt kho tàu, gà kho sả ớt mang lại vị ngọt tự nhiên không cần dùng đường.
* Xay sinh tố dừa sáp béo ngậy.

![Thạch rau câu dừa](http://localhost:9000/public/images/products/15_description2.jpg)

**Mẹo bảo quản:** Quả dừa tươi nguyên vỏ có thể để ở nhiệt độ phòng nơi râm mát trong khoảng 1 tuần. Nếu đã gọt vỏ xanh (dừa gọt trọc), bạn bắt buộc phải bọc màng bọc thực phẩm và giữ trong ngăn mát tủ lạnh để cùi dừa không bị hỏng chua.',
40000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'),
'images/products/15.jpg', true, 63, 0, 14),
('Dưa chuột',
'## Dưa Chuột Tươi Giòn - Xanh Mát Mỗi Ngày

Dưa chuột (hay dưa leo) là loại quả không thể thiếu trong gian bếp của mọi gia đình. Quả dưa thon dài, vỏ xanh mướt, ruột đặc và mọng nước, mang đến độ giòn sần sật và vị thanh mát cực kỳ dễ chịu.

![Dưa chuột xanh giòn](http://localhost:9000/public/images/products/16_description1.jpg)

### Ưu điểm sức khỏe không thể bỏ qua
* **Thanh lọc cơ thể:** Chứa tới 95% là nước, dưa chuột giúp đào thải độc tố ra khỏi cơ thể qua hệ tiết niệu.
* **Hỗ trợ giảm cân:** Hàm lượng calo cực thấp nhưng lại giàu chất xơ, giúp bạn có cảm giác no lâu mà không lo tăng cân.
* **Cung cấp Vitamin K:** Hỗ trợ quá trình đông máu và duy trì hệ xương khớp chắc khỏe.

### Sự linh hoạt trong ẩm thực và làm đẹp
Dưa chuột ăn sống rất ngon, thường được dùng như một loại rau ăn kèm để chống ngán cho các món đồ nướng, đồ chiên rán. Những cách sử dụng phổ biến khác:
* Cắt lát mỏng đắp mặt nạ dưỡng ẩm, làm dịu da cháy nắng và giảm quầng thâm mắt.
* Trộn salad dầu dấm cà chua thanh đạm.
* Ngâm nước detox cùng chanh và lá bạc hà.

![Nước detox dưa chuột chanh](http://localhost:9000/public/images/products/16_description2.jpg)

**Cách bảo quản:** Nên rửa sạch, để thật ráo nước, sau đó cho dưa chuột vào túi zip hoặc bọc bằng khăn giấy rồi cất vào ngăn mát tủ lạnh. Dưa chuột rất nhạy cảm với nhiệt độ quá lạnh, nên để ở hộc đựng rau củ là phù hợp nhất.',
12000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/16.jpg', true, 44, 0, 15),
('Bưởi da xanh',
'## Bưởi Da Xanh Bến Tre - Đặc Sản Trứ Danh

Bưởi da xanh là niềm tự hào của trái cây miền Tây. Quả bưởi hình cầu, vỏ mỏng có màu xanh hơi ngả vàng khi chín. Điểm đặc biệt nhất chính là phần ruột màu hồng ngọc bắt mắt, tép bưởi to, ráo nước, vị ngọt thanh và hoàn toàn không bị the đắng.

![Bưởi da xanh múi hồng](http://localhost:9000/public/images/products/17_description1.jpg)

### Lợi ích vàng cho sức khỏe
* **Giàu Vitamin C:** Tăng cường hệ miễn dịch, bảo vệ cơ thể khỏi các bệnh lây nhiễm thông thường.
* **Tốt cho hệ tim mạch:** Chứa nhiều chất xơ pectin giúp giảm lượng cholesterol xấu trong máu.
* **Đốt cháy mỡ thừa:** Enzyme trong bưởi có tác dụng phân hủy đường, hỗ trợ quá trình giảm cân và duy trì vóc dáng thon gọn.

### Món ngon từ bưởi da xanh
Chỉ cần bóc lớp vỏ, tách từng múi bưởi hồng hào là bạn đã có thể thưởng thức ngay vị ngọt thơm khó cưỡng. Hơn thế nữa, bưởi da xanh còn là nguyên liệu cao cấp cho nhiều món ăn:
* Gỏi bưởi tôm thịt chua ngọt giòn giòn cực kỳ bắt miệng.
* Chè bưởi nấu từ phần cùi trắng (đã khử đắng) giòn sần sật, béo ngậy nước cốt dừa.
* Ép nước bưởi nguyên chất thanh lọc cơ thể.

![Gỏi bưởi tôm thịt](http://localhost:9000/public/images/products/17_description2.jpg)
![Ly nước ép bưởi hồng](http://localhost:9000/public/images/products/17_description3.jpg)

**Bảo quản:** Bưởi da xanh để càng lâu ở nhiệt độ phòng (nơi thoáng mát) thì vỏ sẽ héo lại nhưng ruột bưởi lại càng "xuống nước" và ngọt đậm đà hơn. Bạn có thể bảo quản bưởi nguyên quả từ 2-3 tuần.',
60000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'),
'images/products/17.jpg', true, 41, 0, 16),
('Chôm chôm',
'## Chôm Chôm Tróc Hạt - Ngọt Ngào Vị Quê Hương

Chôm chôm thu hút ánh nhìn bởi lớp vỏ đỏ rực rỡ xen lẫn những sợi lông gai mềm mại. Lớp thịt bên trong trắng ngần, dày dặn, mọng nước và dễ dàng tróc khỏi hạt, mang đến hương vị ngọt lịm khó quên.

![Chôm chôm đỏ tươi](http://localhost:9000/public/images/products/18_description1.jpg)

### Công dụng bất ngờ từ quả chôm chôm
* **Tăng cường năng lượng:** Hàm lượng carbohydrate và protein dồi dào giúp cung cấp năng lượng hoạt động cho cả ngày dài.
* **Bổ máu, giảm mệt mỏi:** Chứa nhiều sắt và đồng, chôm chôm hỗ trợ cơ thể sản sinh hồng cầu, ngăn ngừa tình trạng thiếu máu.
* **Kháng khuẩn tự nhiên:** Các hoạt chất trong quả chôm chôm có đặc tính chống viêm, bảo vệ cơ thể khỏi nhiễm trùng.

### Thưởng thức chôm chôm đúng cách
Cách ăn chôm chôm đơn giản nhất là dùng tay bóc vỏ và đưa trực tiếp vào miệng. Tuy nhiên, nếu bạn mua được lượng lớn, hãy thử biến tấu:
* Nấu chè chôm chôm hạt sen thanh mát giải nhiệt mùa hè.
* Làm chôm chôm ngâm nước đường phèn để pha nước uống dần.
* Trộn gỏi chôm chôm tai heo độc lạ.

![Chè chôm chôm hạt sen](http://localhost:9000/public/images/products/18_description2.jpg)

**Lưu ý bảo quản:** Chôm chôm rất nhanh bị héo và đen vỏ nếu để ngoài trời nắng nóng. Khi mua về, nên dùng dao cắt bớt cành lá, cho quả vào túi nilon đục lỗ và cất trong ngăn mát. Nên tiêu thụ trong vòng 3-5 ngày để đảm bảo độ tươi ngon nhất.',
50000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/18.jpg', true, 66, 0, 17),
('Thanh long đỏ',
'## Thanh Long Ruột Đỏ - Món Quà Rực Rỡ Từ Thiên Nhiên

Thanh long ruột đỏ không chỉ sở hữu ngoại hình độc đáo với lớp vỏ hồng sậm và các "tai" xanh viền cong, mà còn gây ấn tượng mạnh bởi phần ruột đỏ thẫm lấp lánh những hạt đen li ti. Vị của thanh long đỏ thường ngọt đậm hơn so với thanh long ruột trắng.

![Thanh long đỏ cắt lát](http://localhost:9000/public/images/products/19_description1.jpg)

### Siêu thực phẩm giàu dinh dưỡng
* **Chống oxy hóa cực mạnh:** Sắc tố đỏ Betacyanin giúp ngăn chặn sự phát triển của các khối u và làm chậm quá trình lão hóa tế bào.
* **Tốt cho hệ tiêu hóa:** Lượng chất xơ prebiotics phong phú là thức ăn nuôi dưỡng các vi khuẩn có lợi trong đường ruột.
* **Bảo vệ tim mạch:** Những hạt đen nhỏ xíu trong thanh long chứa nhiều axit béo Omega-3 và Omega-9, giúp làm giảm cholesterol xấu.

### Biến tấu đa dạng đầy màu sắc
Nhờ màu đỏ tự nhiên tuyệt đẹp, thanh long đỏ là nguyên liệu tạo màu thực phẩm vô cùng an toàn và bắt mắt:
* Làm sinh tố thanh long đỏ mix chuối và sữa chua.
* Xay nhuyễn lấy nước cốt nhào bột làm bánh mì thanh long đỏ.
* Đổ thạch rau câu trái cây nhiều tầng hấp dẫn.

![Sinh tố thanh long đỏ](http://localhost:9000/public/images/products/19_description2.jpg)

**Cách bảo quản:** Bạn có thể để quả thanh long ở nhiệt độ phòng nơi râm mát từ 3-5 ngày. Để trái cây ngon và giòn hơn, hãy bọc màng nilon và cất vào ngăn mát tủ lạnh, thưởng thức khi thanh long đã được ướp lạnh mọng nước.',
40000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/19.jpg', true, 38, 0, 18);

INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageFilePath", "IsActive", "Quantity",
                        "HeldQuantity", "DisplayOrder")
VALUES ('Mít',
'## Mít Thái Chín Cây - Vàng Ươm, Thơm Lừng Góc Bếp

Mít là thức quà dân dã nhưng lại mang sức hút mãnh liệt bởi mùi thơm nồng nàn đặc trưng. Những múi mít to, thịt dày, màu vàng ươm óng ả, khi cắn vào có độ giòn sần sật và vị ngọt lịm tan nơi đầu lưỡi.

![Múi mít vàng ươm giòn ngọt](http://localhost:9000/public/images/products/20_description1.jpg)

### Kho năng lượng dồi dào
* **Tăng cường năng lượng:** Mít chứa nhiều đường fructose và sucrose tự nhiên, là nguồn bổ sung năng lượng hoàn hảo cho cơ thể.
* **Tốt cho tuyến giáp:** Hàm lượng đồng trong mít đóng vai trò quan trọng trong việc duy trì chức năng tuyến giáp khỏe mạnh.
* **Cải thiện hệ tiêu hóa:** Lượng chất xơ cao giúp ngăn ngừa táo bón và làm sạch đường ruột hiệu quả.

### Những biến tấu ẩm thực đặc sắc
Mít bóc sẵn ăn trực tiếp đã cực kỳ ngon, nhưng bạn cũng có thể sáng tạo thêm nhiều món ăn vặt hấp dẫn:
* Làm kem mít cốt dừa mát lạnh cho ngày hè oi bức.
* Nguyên liệu không thể thiếu trong ly chè Thái hay chè sương sa hạt lựu.
* Sấy khô thành mít sấy giòn rụm nhâm nhi lúc xem phim.

![Chè Thái mít thơm lừng](http://localhost:9000/public/images/products/20_description2.jpg)

**Mẹo bảo quản:** Mít bóc sẵn rất dễ lên men và bị chua nếu để bên ngoài. Bạn nên cho các múi mít vào hộp đậy nắp thật kín (để mùi mít không ám sang các thực phẩm khác) và cất trong ngăn mát tủ lạnh, dùng ngon nhất trong 2-3 ngày.',
50000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/20.jpg', true, 52, 0, 19),
('Kiwi',
'## Kiwi Xanh Nhập Khẩu - "Nữ Hoàng" Vitamin C

Kiwi sở hữu lớp vỏ màu nâu phủ một lớp lông tơ mịn màng, nhưng ẩn chứa bên trong là phần thịt quả xanh ngọc bích rực rỡ, điểm xuyết những hạt đen li ti đẹp mắt. Vị chua chua ngọt ngọt thanh tao của Kiwi luôn làm say lòng những thực khách sành ăn nhất.

![Kiwi xanh cắt lát rực rỡ](http://localhost:9000/public/images/products/21_description1.jpg)

### Dinh dưỡng vượt trội từ quả Kiwi
* **Vượt mặt cam về lượng Vitamin C:** Một quả Kiwi cung cấp đủ lượng Vitamin C cần thiết cho cả ngày, giúp tăng cường hệ miễn dịch và bảo vệ tế bào khỏi stress oxy hóa.
* **Tốt cho hệ tiêu hóa:** Enzyme Actinidin đặc biệt trong Kiwi giúp phân giải protein nhanh chóng, làm giảm cảm giác đầy bụng sau những bữa ăn nhiều thịt.
* **Duy trì huyết áp ổn định:** Hàm lượng Kali cao giúp điều hòa huyết áp, bảo vệ tim mạch.

### Ứng dụng trong các bữa ăn Healthy
Với hương vị chua ngọt hài hòa, Kiwi là lựa chọn tuyệt vời cho các chế độ ăn kiêng và làm đẹp:
* Làm mứt Kiwi chua ngọt ăn kèm bánh mì sandwich buổi sáng.
* Thái hạt lựu trộn salad ngũ sắc hoặc mix cùng yến mạch.
* Ép nước giải khát hoặc làm sinh tố xanh (Green Smoothie) thanh lọc cơ thể.

![Sinh tố Kiwi xanh mát](http://localhost:9000/public/images/products/21_description2.jpg)
![Mứt Kiwi ăn kèm bánh mì](http://localhost:9000/public/images/products/21_description3.jpg)

**Gợi ý bảo quản:** Để Kiwi chín tự nhiên, bạn có thể để ở nhiệt độ phòng. Khi quả đã mềm tay và tỏa mùi thơm nhẹ, hãy cất vào tủ lạnh để dùng dần trong 1-2 tuần.',
90000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/21.jpg', true, 60, 0, 20),
('Mơ',
'## Mơ Tươi Mộc Châu - Chua Thanh, Thơm Dịu Mùa Hè

Những quả mơ tươi căng tròn, lớp vỏ phủ lớp lông tơ mịn màng và có màu vàng ửng đỏ khi chín. Hương thơm của mơ vô cùng đặc trưng, mang theo sự thanh mát của núi rừng Tây Bắc, vị chua thanh hòa quyện vị ngọt dịu kích thích mọi giác quan.

![Mơ tươi chín vàng Mộc Châu](http://localhost:9000/public/images/products/22_description1.jpg)

### Công dụng tuyệt vời của quả mơ
* **Thanh nhiệt, giải khát:** Nước mơ ngâm là thức uống giải nhiệt mùa hè "quốc dân", giúp bù nước, giảm mệt mỏi hiệu quả.
* **Cải thiện thị lực và làn da:** Hàm lượng Vitamin A và Beta-carotene dồi dào giúp mắt sáng khỏe và làn da tươi trẻ hơn.
* **Hỗ trợ tiêu hóa:** Chất xơ và tính axit nhẹ trong mơ giúp kích thích tiêu hóa, giảm cảm giác chán ăn.

### Cách chế biến quả mơ truyền thống
Mơ tươi thường khá chua nên ít khi được ăn trực tiếp với số lượng nhiều. Người Việt thường dùng mơ để:
* Ngâm với đường phèn làm nước giải khát mùa hè.
* Ngâm rượu mơ (Umeshu) thơm lừng, có tác dụng an thần và kích thích tiêu hóa.
* Làm ô mai mơ mặn ngọt chua cay nhâm nhi những ngày se lạnh.

![Bình mơ ngâm đường phèn](http://localhost:9000/public/images/products/22_description2.jpg)

**Mẹo xử lý mơ:** Trước khi ngâm đường hoặc ngâm rượu, cần rửa sạch, ngâm nước muối loãng, sau đó vớt ra để thật ráo nước và dùng tăm khều sạch phần núm cuống để mơ không bị chát và nổi váng.',
60000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/22.jpg', true, 54, 0, 21),
('Ổi',
'## Ổi Nữ Hoàng Giòn Ngọt - Siêu Trái Cây Quen Thuộc

Ổi là loại trái cây vô cùng quen thuộc nhưng giá trị dinh dưỡng lại đứng ở hàng "siêu thực phẩm". Quả ổi Nữ Hoàng có đặc điểm vỏ láng, mình to, ruột đặc ít hạt, khi cắn vào có độ giòn rụm và vị ngọt thanh vô cùng sảng khoái.

![Ổi Nữ Hoàng giòn ngọt](http://localhost:9000/public/images/products/23_description1.jpg)

### Đỉnh cao của lượng Vitamin C
* **Vô địch Vitamin C:** Đánh bại cả cam và chanh, ổi là nguồn bổ sung Vitamin C tuyệt vời nhất giúp củng cố hệ miễn dịch.
* **Tốt cho người bệnh tiểu đường:** Chỉ số đường huyết của ổi thấp, kết hợp cùng lượng chất xơ cao giúp kiểm soát đường huyết hiệu quả.
* **Làm đẹp da toàn diện:** Khả năng chống oxy hóa mạnh giúp bảo vệ da khỏi tác động của tia UV và ô nhiễm môi trường.

### Món ăn vặt không thể thiếu
Ổi gọt vỏ (hoặc để nguyên vỏ), cắt miếng vừa ăn rồi chấm cùng muối ớt hay muối tôm là món ăn vặt kinh điển không bao giờ nhàm chán. Bên cạnh đó:
* Nước ép ổi nguyên chất là thức uống giảm cân, đẹp da được hội chị em cực kỳ ưa chuộng.
* Trộn gỏi ổi tai heo giòn sần sật, chua ngọt bắt miệng.

![Nước ép ổi thanh mát](http://localhost:9000/public/images/products/23_description2.jpg)

**Cách bảo quản:** Ổi có thể để ở nhiệt độ phòng vài ngày cho quả chín và mềm hơn, nhưng nếu bạn thích ăn giòn, hãy cất ổi vào ngăn mát tủ lạnh ngay khi mua về.',
30000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/23.jpg', true, 43, 0, 22),
('Măng cụt',
'## Măng Cụt - "Nữ Hoàng" Trái Cây Nhiệt Đới

Được mệnh danh là "Nữ hoàng trái cây", măng cụt sở hữu lớp vỏ dày màu tím sẫm quyền lực, nhưng ẩn chứa bên trong là những múi thịt trắng muốt, tinh khiết như bông. Hương vị của măng cụt là sự pha trộn hoàn hảo giữa vị ngọt lịm và chút chua thanh cực kỳ tinh tế.

![Măng cụt ruột trắng muốt](http://localhost:9000/public/images/products/24_description1.jpg)

### Tinh hoa dưỡng chất
* **Hợp chất Xanthones quý giá:** Măng cụt chứa lượng lớn Xanthones - một siêu chất chống oxy hóa giúp kháng viêm, chống ung thư và làm chậm lão hóa vô cùng hiệu quả.
* **Kiểm soát thể trọng:** Cung cấp năng lượng nhưng ít calo, không chứa chất béo bão hòa, rất tốt cho quá trình giữ dáng.
* **Giải nhiệt cơ thể:** Măng cụt có tính mát, thường được dùng để trung hòa tính nhiệt (nóng) của sầu riêng, tạo nên bộ đôi ẩm thực hoàn hảo.

### Chế biến măng cụt sành điệu
Thưởng thức măng cụt ngon nhất là dùng dao sắc khứa nhẹ một vòng quanh bụng quả rồi tách lớp vỏ ra, ăn trực tiếp những múi thịt trắng ngần. Ngoài ra, tại vùng Nam Bộ, người ta còn sáng tạo ra món:
* Gỏi gà măng cụt xanh: Món ăn đặc sản lừng danh với vị giòn ngọt, chua nhẹ của măng cụt sống kết hợp cùng thịt gà ta xé phay đậm đà.

![Gỏi gà măng cụt đặc sản](http://localhost:9000/public/images/products/24_description2.jpg)

**Mẹo chọn măng cụt ngon:** Nên chọn những quả có vỏ màu tím sẫm, cuống tươi xanh, sờ vào thấy mềm đều, không bị sượng cứng. Dưới đáy quả có hình bông hoa nhỏ, số cánh hoa tương ứng với số múi măng cụt bên trong.',
100000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/24.jpg', true, 67, 0, 23);

INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageFilePath", "IsActive", "Quantity",
                        "HeldQuantity", "DisplayOrder")
VALUES ('Hồng giòn',
'## Hồng Giòn Chín Mọng - Vị Ngọt Dịu Dàng Mùa Thu

Hồng giòn là thức quà đặc trưng của mùa thu. Quả hồng vuông vức, lớp vỏ nhẵn bóng màu vàng cam. Khác với hồng đỏ chín mềm, hồng giòn có phần thịt chắc, cắn vào giòn sần sật, không hề bị chát và mang lại vị ngọt thanh vô cùng dễ chịu.

![Hồng giòn Đà Lạt cắt lát](http://localhost:9000/public/images/products/25_description1.jpg)

### Ưu điểm dinh dưỡng tuyệt vời
* **Kho tàng Vitamin và Khoáng chất:** Bổ sung Vitamin A, C, Canxi và Sắt, giúp cơ thể khỏe mạnh và tăng cường hệ miễn dịch trước lúc chuyển mùa.
* **Tốt cho hệ tiêu hóa:** Hàm lượng chất xơ cao (pectin) trong hồng giòn hỗ trợ tiêu hóa rất tốt, ngăn ngừa các vấn đề về dạ dày và đường ruột.
* **Ngăn ngừa lão hóa:** Chứa lượng lớn catechin và polyphenol giúp chống oxy hóa, bảo vệ làn da khỏi các nếp nhăn.

### Thưởng thức hồng giòn sao cho ngon?
Hồng giòn ngon nhất là sau khi được ướp lạnh, gọt vỏ và ăn trực tiếp để cảm nhận trọn vẹn độ giòn ngọt. Bên cạnh đó, bạn có thể:
* Cắt hạt lựu trộn cùng sữa chua không đường và yến mạch.
* Làm món salad trái cây kết hợp cùng táo và dưa lưới.

![Salad trái cây với hồng giòn](http://localhost:9000/public/images/products/25_description2.jpg)

**Mẹo bảo quản:** Để giữ được độ giòn lâu nhất, sau khi mua về bạn không nên rửa ngay mà hãy bọc từng quả bằng giấy báo hoặc cho vào túi nilon, buộc kín và bảo quản trong ngăn mát tủ lạnh.',
70000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/25.jpg', true, 39, 0, 24),
('Đu đủ',
'## Đu Đủ Chín Vàng - Ngọt Mát Bổ Dưỡng

Đu đủ chín nổi bật với lớp vỏ vàng ươm, ruột đỏ cam quyến rũ và hương thơm ngát đặc trưng. Thịt đu đủ mềm mịn, ngọt mát, tan ngay trong miệng, là món tráng miệng lý tưởng và rất dễ tiêu hóa.

![Đu đủ chín vàng ngọt lịm](http://localhost:9000/public/images/products/26_description1.jpg)

### Bài thuốc quý từ thiên nhiên
* **Bảo vệ thị lực:** Cung cấp dồi dào Vitamin A, giúp ngăn ngừa thoái hóa điểm vàng và giữ cho đôi mắt luôn sáng khỏe.
* **Hỗ trợ tiêu hóa cực tốt:** Enzyme Papain đặc biệt trong đu đủ giúp phân giải protein nhanh chóng, là "cứu tinh" cho những người hay bị đầy bụng, khó tiêu.
* **Phục hồi và làm đẹp da:** Vitamin E và C giúp cấp ẩm, làm đều màu da và kích thích sản sinh collagen.

### Những biến tấu ẩm thực hấp dẫn
Ngoài ăn tươi như một loại trái cây giải khát, đu đủ chín còn rất được ưa chuộng khi làm:
* Sinh tố đu đủ béo ngậy, mix thêm chút sữa đặc và đá xay mịn.
* Chè đu đủ nấm tuyết táo đỏ - món chè dưỡng nhan nổi tiếng.
* Thêm vào mâm trái cây dầm sữa chua mát lạnh.

![Sinh tố đu đủ thơm ngon](http://localhost:9000/public/images/products/26_description2.jpg)

**Cách bảo quản:** Đu đủ chín rất nhanh mềm và hỏng ở nhiệt độ phòng. Khi quả đã đạt độ chín mong muốn, bạn nên gọt vỏ, thái miếng và cho vào hộp kín để trong tủ lạnh, nên dùng hết trong 1-2 ngày.',
30000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/26.jpg', true, 59, 0, 25),
('Cherry',
'## Cherry Đỏ Nhập Khẩu - Viên Ngọc Quý Của Trái Cây

Cherry (Anh đào) luôn nằm trong top những loại trái cây cao cấp nhất nhờ vẻ ngoài sang trọng, màu đỏ thẫm bóng bẩy, cuống xanh tươi. Thịt cherry vô cùng chắc, mọng nước, mang đến vị ngọt thanh tao, xen lẫn chút chua nhẹ đầy lôi cuốn.

![Cherry đỏ căng mọng](http://localhost:9000/public/images/products/27_description1.jpg)

### Siêu thực phẩm đắt giá
* **Cải thiện giấc ngủ:** Cherry là một trong số ít những loại trái cây chứa Melatonin tự nhiên, giúp điều hòa chu kỳ sinh học và mang lại giấc ngủ sâu hơn.
* **Chống viêm, giảm đau nhức:** Lượng chất chống oxy hóa Anthocyanin khổng lồ giúp giảm viêm khớp và đau nhức cơ bắp hiệu quả.
* **Bảo vệ trái tim:** Bổ sung Kali và giảm lượng cholesterol, giúp hệ tim mạch hoạt động khỏe mạnh.

### Nâng tầm món ăn với Cherry
Sự xuất hiện của cherry luôn làm cho mọi món ăn trở nên đẳng cấp và đắt tiền hơn:
* Trang trí tuyệt đẹp trên các loại bánh kem, bánh tart hoặc cupcake.
* Ngâm rượu mứt (Maranaschino cherries) dùng cho các ly cocktail hảo hạng.
* Thưởng thức trực tiếp để trân trọng hương vị nguyên bản nhất.

![Bánh tart trang trí cherry](http://localhost:9000/public/images/products/27_description2.jpg)

**Bí quyết bảo quản:** Cherry cực kỳ kỵ nước và nhiệt độ cao. Khi mua về, hãy để cherry khô ráo hoàn toàn, xếp vào hộp nhựa có lót giấy thấm và cất ngay vào tủ lạnh (nhiệt độ 0-4 độ C). Chỉ rửa số lượng vừa đủ trước khi ăn.',
150000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
'images/products/27.jpg', true, 49, 0, 26),
('Táo xanh',
'## Táo Xanh Granny Smith - Chua Thanh, Giòn Rụm

Táo xanh Granny Smith gây ấn tượng mạnh bởi lớp vỏ màu xanh lục rực rỡ, độ giòn vô địch và đặc biệt là hương vị chua thanh, ít ngọt hơn hẳn so với các loại táo đỏ hay táo vàng. Đây là chân ái cho những ai không thích ăn trái cây quá ngọt.

![Táo xanh Granny Smith](http://localhost:9000/public/images/products/28_description1.jpg)

### Lý do táo xanh được ưa chuộng
* **Kiểm soát cân nặng:** Hàm lượng đường thấp nhất trong các dòng táo, lượng chất xơ cực cao giúp no lâu, rất lý tưởng cho chế độ Keto hoặc Eat Clean.
* **Tốt cho răng miệng:** Việc nhai táo xanh kích thích tiết nước bọt, giúp làm sạch mảng bám và bảo vệ men răng.
* **Cải thiện hệ tiêu hóa:** Chứa nhiều chất xơ hòa tan Pectin giúp nuôi dưỡng lợi khuẩn trong đường ruột.

### Nguyên liệu vàng trong làm bánh
Nhờ kết cấu cứng và vị chua đặc trưng, táo xanh không bị nát hay mất vị khi chịu nhiệt độ cao. Do đó, chúng được sử dụng rộng rãi để:
* Làm nhân bánh Tart táo (Apple Pie), bánh Crumble kết hợp cùng bơ và quế.
* Xay sinh tố giảm cân mix cùng cần tây và dưa chuột.
* Ép nước thanh lọc cơ thể sảng khoái đầu ngày.

![Bánh nướng nhân táo xanh](http://localhost:9000/public/images/products/28_description2.jpg)

**Mẹo nhỏ:** Táo xanh có thời gian bảo quản khá lâu. Chỉ cần bọc kín trong túi nilon và để tủ mát, táo có thể giữ được độ giòn và tươi mới từ 3-4 tuần.',
55000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/28.jpg', true, 62, 0, 27),
('Bưởi hồng',
'## Bưởi Hồng Ruột Đỏ - Thức Quà Tươi Mát Giàu Dưỡng Chất

Bưởi hồng thu hút mọi ánh nhìn với phần ruột màu hồng đào rực rỡ. Tép bưởi mọng nước, vị ngọt thanh xen lẫn vị chua nhẹ và ít đắng, mang đến sự sảng khoái và thanh mát ngay từ miếng cắn đầu tiên.

![Bưởi hồng ruột đỏ rực rỡ](http://localhost:9000/public/images/products/29_description1.jpg)

### Tinh túy từ màu hồng tự nhiên
* **Nguồn Lycopene dồi dào:** Sắc hồng của bưởi đến từ Lycopene - một chất chống oxy hóa cực mạnh giúp ngăn ngừa tổn thương tế bào và bảo vệ da khỏi tia cực tím.
* **Tăng cường miễn dịch:** Giống như các loại trái cây có múi khác, bưởi hồng cung cấp lượng Vitamin C khổng lồ.
* **Hỗ trợ giảm mỡ máu:** Giúp giảm thiểu mức triglyceride và cholesterol xấu, bảo vệ thành mạch máu.

### Thưởng thức bưởi hồng đa dạng
Ngoài cách tách múi ăn tươi, bưởi hồng còn góp mặt trong rất nhiều công thức ẩm thực lành mạnh:
* Trộn salad trái cây tôm thịt thanh đạm, ít calo.
* Ép nguyên chất làm thức uống giải rượu, detox cơ thể siêu hiệu quả.
* Làm thạch bưởi trong suốt vô cùng đẹp mắt.

![Nước ép bưởi hồng đẹp da](http://localhost:9000/public/images/products/29_description2.jpg)

**Cách bảo quản:** Bưởi nguyên quả có thể để ngoài không gian thoáng mát từ 1-2 tuần. Nếu đã bóc vỏ, nên cho phần múi bưởi vào hộp nhựa đậy nắp kín, bảo quản ngăn mát để tránh làm bưởi bị khô và mất đi lượng nước quý giá.',
65000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'),
'images/products/29.jpg', true, 46, 0, 28);

INSERT INTO "Products" ("Name", "Description", "Price", "ProductUnitId", "ImageFilePath", "IsActive", "Quantity",
                        "HeldQuantity", "DisplayOrder")
VALUES ('Vải thiều',
'## Vải Thiều Lục Ngạn - Ngọt Lịm Tinh Hoa Mùa Hè

Vải thiều Lục Ngạn tự hào là đặc sản trái cây nức tiếng, với vỏ ngoài đỏ hồng gai nhẵn, hạt nhỏ (thậm chí nhiều quả hạt lép) và phần cùi trắng ngần, dày dặn. Hương vị ngọt lịm, thơm nồng nàn của quả vải chắc chắn sẽ làm say đắm bất cứ ai.

![Vải thiều Lục Ngạn chín mọng](http://localhost:9000/public/images/products/30_description1.jpg)

### Dưỡng chất dồi dào
* **Bổ sung Vitamin C cực tốt:** Chỉ cần một nắm nhỏ vải thiều đã đáp ứng đủ nhu cầu Vitamin C hàng ngày, giúp tăng cường hệ miễn dịch.
* **Ngăn ngừa ung thư:** Chứa hợp chất Flavonoid giúp kháng viêm và bảo vệ tế bào khỏi những gốc tự do gây hại.
* **Tăng cường tuần hoàn máu:** Hàm lượng đồng và sắt trong vải giúp thúc đẩy sản sinh hồng cầu, mang lại làn da hồng hào khỏe mạnh.

### Cách thưởng thức trọn vẹn
Cách tốt nhất để cảm nhận vị ngon của vải là bóc vỏ ăn tươi sau khi đã ướp lạnh. Ngoài ra, bạn có thể thử những biến tấu giải nhiệt cực đỉnh:
* Trà vải hoa hồng thanh mát, thức uống "hot trend" mỗi dịp hè về.
* Chè khúc bạch tráng miệng kết hợp cùng cùi vải thiều giòn ngọt.
* Nấu canh vải nhồi thịt mặn ngọt lạ miệng, thanh lọc cơ thể.

![Ly trà vải hoa hồng giải nhiệt](http://localhost:9000/public/images/products/30_description2.jpg)

**Mẹo bảo quản:** Vải rất nhanh bị khô vỏ và mất nước. Khi mua về, bạn nên cắt bớt cuống (để lại khoảng 1cm), cho vào túi nilon đục lỗ hoặc hộp kín đậy lớp giấy mỏng, bảo quản trong ngăn mát tủ lạnh để vải tươi lâu từ 5-7 ngày.',
80000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/30.jpg', true, 56, 0, 29),
('Nhãn',
'## Nhãn Lồng Hương Thơm Quyến Rũ - Ngọt Thanh Đậm Đà

Nhãn là loại trái cây nhiệt đới mang hương vị ngọt thanh quyến rũ. Lớp vỏ nâu mỏng manh bọc lấy phần cùi thịt mọng nước, trong suốt và giòn dai, tỏa ra mùi thơm dịu nhẹ cực kỳ cuốn hút.

![Chùm nhãn tươi lồng mọng nước](http://localhost:9000/public/images/products/31_description1.jpg)

### Tác dụng tuyệt vời của quả nhãn
* **Bổ máu, an thần:** Theo Đông y, long nhãn (cùi nhãn sấy khô) là vị thuốc quý giúp bổ khí huyết, xoa dịu thần kinh và chữa chứng mất ngủ.
* **Cung cấp năng lượng tức thì:** Nguồn Glucose và Sucrose tự nhiên giúp cơ thể nhanh chóng phục hồi sinh lực sau khi vận động.
* **Tốt cho da dẻ:** Nguồn Vitamin C và Riboflavin dồi dào hỗ trợ bảo vệ da và làm chậm quá trình lão hóa.

### Gợi ý ẩm thực cùng quả nhãn
Bạn có thể bóc vỏ ăn trực tiếp hoặc dùng nhãn làm nguyên liệu cho những món chè truyền thống tuyệt hảo:
* Chè hạt sen long nhãn thanh mát, bổ dưỡng, giải nhiệt hiệu quả.
* Chè khúc bạch thơm ngậy, không thể thiếu những cùi nhãn giòn sần sật.
* Ngâm rượu nhãn thơm lừng bồi bổ sức khỏe.

![Chè hạt sen long nhãn](http://localhost:9000/public/images/products/31_description2.jpg)

**Cách lưu trữ tươi ngon:** Nhãn tươi nên được cắt rời thành từng chùm nhỏ, rửa sạch bụi bẩn, để ráo nước rồi cho vào hộp kín lót giấy thấm. Trữ ở ngăn mát tủ lạnh giúp nhãn giòn ngọt và giữ được hương vị trong khoảng 1 tuần.',
60000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/31.jpg', true, 64, 0, 30),
('Mãng cầu',
'## Mãng Cầu Xiêm - Vị Chua Ngọt Lôi Cuốn

Mãng cầu xiêm (hay mãng cầu gai) là loại trái cây mang hình dáng độc đáo với lớp vỏ xanh đầy gai mềm. Phần thịt bên trong trắng muốt, chia thành nhiều múi đan xen, mang hương vị kết hợp hoàn hảo giữa dứa, dâu tây và chút hương cam chanh vô cùng tươi mát.

![Mãng cầu xiêm tươi cắt lát](http://localhost:9000/public/images/products/32_description1.jpg)

### Cỗ máy dinh dưỡng tự nhiên
* **Kháng viêm và chống oxy hóa mạnh:** Được nhiều nghiên cứu chỉ ra rằng các chiết xuất từ mãng cầu xiêm có tác dụng ức chế sự phát triển của nhiều loại vi khuẩn và hỗ trợ phòng ngừa ung thư.
* **Bảo vệ hệ miễn dịch:** Chứa hàm lượng lớn Vitamin C, giúp cơ thể khỏe mạnh và chống lại cảm lạnh.
* **Giúp tiêu hóa trơn tru:** Lượng chất xơ khổng lồ trong mãng cầu đẩy lùi tình trạng táo bón và bảo vệ niêm mạc ruột.

### Món ngon giải nhiệt
Mãng cầu chín mềm rất thích hợp để làm các thức uống giải khát:
* Sinh tố mãng cầu sữa đặc chua chua béo ngậy.
* Mãng cầu dầm đá đường sảng khoái đánh bay cơn khát mùa hè.
* Trà mãng cầu xiêm - thức uống cực "hot" được giới trẻ yêu thích.

![Ly sinh tố mãng cầu béo ngậy](http://localhost:9000/public/images/products/32_description2.jpg)

**Bảo quản:** Nếu mãng cầu còn cứng, hãy để ngoài nhiệt độ phòng đến khi vỏ hơi sậm màu và mềm tay. Sau khi chín, cần lột vỏ, bỏ hạt, cho phần thịt vào hộp kín và trữ đông hoặc để ngăn mát tủ lạnh.',
70000,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/32.jpg', true, 38, 0, 31),
('Demo1',
'## Sản Phẩm Demo 1 - Thiết Kế Sang Trọng

Sản phẩm Demo 1 được tạo ra nhằm mục đích kiểm thử hiển thị giao diện. Tuy chỉ là dữ liệu mẫu, chúng tôi vẫn đảm bảo chất lượng hình ảnh và nội dung chuẩn SEO để bạn dễ dàng mường tượng ra thiết kế thực tế.

![Demo 1 Image](http://localhost:9000/public/images/products/33_description1.jpg)

* **Chất lượng:** Tuyệt hảo.
* **Đóng gói:** Chuẩn mực.
* **Bảo hành:** Cam kết hài lòng.',
360,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Kg'),
'images/products/demo.png', true, 36, 0, 32),
('Demo2',
'## Sản Phẩm Demo 2 - Đỉnh Cao Công Nghệ

Sản phẩm Demo 2 là minh chứng cho việc bố cục nội dung chi tiết. Hình ảnh sắc nét, mô tả rõ ràng, cấu trúc mạch lạc sẽ giúp khách hàng có cái nhìn thiện cảm nhất đối với gian hàng của bạn.

![Demo 2 Image](http://localhost:9000/public/images/products/34_description1.jpg)',
690,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Hộp'),
'images/products/demo.png', true, 69, 0, 33),
('Demo3',
'## Sản Phẩm Demo 3 - Món Quà Hoàn Hảo

Hoàn thiện gian hàng của bạn với Sản Phẩm Demo 3. Đây là phiên bản hiển thị mẫu cho các dòng sản phẩm bán theo đơn vị "Quả". 

![Demo 3 Image](http://localhost:9000/public/images/products/35_description1.jpg)',
100,
(SELECT "Id" FROM "ProductUnits" WHERE "Name" = 'Quả'),
'images/products/demo.png', true, 100, 0, 34);

INSERT INTO "Categories" ("Name", "IsActive", "DisplayOrder")
VALUES ('Trái Cây Nhập Khẩu', true, 0),
       ('Trái Cây Nội Địa', true, 1),
       ('Trái Cây Nhiệt Đới', true, 2),
       ('Trái Cây Có Múi', true, 3),
       ('Táo', true, 4),
       ('Nho Các Loại', true, 5),
       ('Dưa Các Loại', true, 6),
       ('Quả Mọng', true, 7),
       ('Xoài Các Loại', true, 8),
       ('Trái Cây Đặc Sản', true, 9);

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
