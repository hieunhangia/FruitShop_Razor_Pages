function initAddressDropdowns(provinceSelectId, communeSelectId, initialProvinceCode = '', initialCommuneCode = '') {
    const provinceSelect = document.getElementById(provinceSelectId);
    const communeSelect = document.getElementById(communeSelectId);

    // Kiểm tra xem DOM elements có tồn tại không để tránh lỗi
    if (!provinceSelect || !communeSelect) return;

    // Tải danh sách Tỉnh/Thành phố
    async function fetchProvinces() {
        try {
            const response = await fetch('/api/address/provinces');
            const provinces = await response.json();
            provinces.forEach(p => {
                provinceSelect.add(new Option(p.name, p.code));
            });
        } catch (error) {
            console.error("Lỗi khi tải danh sách tỉnh:", error);
        }
    }

    // Tải danh sách Xã/Phường theo mã Tỉnh
    async function fetchCommunes(provinceCode) {
        communeSelect.innerHTML = '<option value="">-- Chọn xã/phường --</option>';
        if (!provinceCode) return;

        try {
            const response = await fetch(`/api/address/provinces/${provinceCode}/communes`);
            const communes = await response.json();
            communes.forEach(c => {
                communeSelect.add(new Option(c.name, c.code));
            });
        } catch (error) {
            console.error("Lỗi khi tải danh sách xã/phường:", error);
        }
    }

    // Khởi tạo dữ liệu khi load trang
    async function init() {
        await fetchProvinces();

        // Nếu có giá trị tỉnh mặc định (Trường hợp trang Update)
        if (initialProvinceCode) {
            provinceSelect.value = initialProvinceCode;
            await fetchCommunes(initialProvinceCode);

            // Set giá trị xã/phường nếu có
            if (initialCommuneCode) {
                communeSelect.value = initialCommuneCode;
            }
        }
    }

    // Lắng nghe sự kiện thay đổi Tỉnh/Thành phố
    provinceSelect.addEventListener("change", function () {
        fetchCommunes(this.value);
    });

    // Bắt đầu chạy
    init();
}