document.addEventListener('DOMContentLoaded', function () {
    
    // --- T? ??NG T?O MÃ V? TRÍ ---
    const btnGenMa = document.getElementById('btnGenMa');
    const maViTriInput = document.getElementById('maViTri');
    
    if (btnGenMa) {
        btnGenMa.addEventListener('click', function() {
            const loaiViTri = document.getElementById('loaiViTri').value;
            let prefix = 'VT';
            
            // T?o prefix d?a trên lo?i v? trí
            if (loaiViTri) {
                switch(loaiViTri) {
                    case 'Kho':
                        prefix = 'KHO';
                        break;
                    case 'Kho l?nh':
                        prefix = 'KL';
                        break;
                    case 'Kho mát':
                        prefix = 'KM';
                        break;
                    case 'Tr?ng bày':
                        prefix = 'KE';
                        break;
                    case 'Kho t?m':
                        prefix = 'KT';
                        break;
                    default:
                        prefix = 'VT';
                }
            }
            
            // T?o s? ng?u nhiên
            const randomNum = Math.floor(Math.random() * 900) + 100; // 100-999
            const maViTri = `${prefix}-${randomNum}`;
            
            maViTriInput.value = maViTri;
        });
    }
    
    // --- NÚT L?U L?I ---
    const btnLuuLai = document.getElementById('btnLuuLai');
    
    if (btnLuuLai) {
        btnLuuLai.addEventListener('click', async function(e) {
            e.preventDefault();
            
            // L?y d? li?u t? form
            const maViTri = document.getElementById('maViTri').value.trim();
            const tenViTri = document.getElementById('tenViTri').value.trim();
            const loaiViTri = document.getElementById('loaiViTri').value;
            const sucChua = document.getElementById('sucChua').value;
            const moTa = document.getElementById('moTa').value.trim();
            
            // Validate
            if (!maViTri) {
                alert('Vui lòng nh?p mã v? trí!');
                document.getElementById('maViTri').focus();
                return;
            }
            
            if (!tenViTri) {
                alert('Vui lòng nh?p tên v? trí!');
                document.getElementById('tenViTri').focus();
                return;
            }
            
            if (!loaiViTri) {
                alert('Vui lòng ch?n lo?i v? trí!');
                document.getElementById('loaiViTri').focus();
                return;
            }
            
            // T?o object request
            const requestData = {
                maViTri: maViTri,
                loaiViTri: loaiViTri,
                moTa: moTa || null
            };
            
            console.log('Request data:', requestData);
            
            try {
                // Hi?n th? loading
                btnLuuLai.disabled = true;
                const originalHTML = btnLuuLai.innerHTML;
                btnLuuLai.innerHTML = '<i class="fas fa-hourglass-half me-1"></i> ?ang l?u...';
                
                // G?i API
                const response = await fetch('/add-ViTri', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestData)
                });
                
                console.log('Response status:', response.status);
                
                // Ki?m tra content type
                const contentType = response.headers.get("content-type");
                console.log('Content-Type:', contentType);
                
                let result;
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    result = await response.json();
                } else {
                    const text = await response.text();
                    console.log('Response text:', text);
                    throw new Error('Server không tr? v? JSON. Response: ' + text.substring(0, 200));
                }
                
                console.log('Response:', result);
                
                if (response.ok) {
                    alert(result.message || 'Thêm v? trí m?i thành công!');
                    window.location.href = '/QuanLyKhoHang/ViTriSanPham';
                } else {
                    alert('L?i: ' + (result.message || 'Không th? thêm v? trí'));
                    btnLuuLai.disabled = false;
                    btnLuuLai.innerHTML = originalHTML;
                }
            } catch (error) {
                console.error('Error:', error);
                alert('Có l?i x?y ra khi l?u v? trí: ' + error.message);
                btnLuuLai.disabled = false;
                btnLuuLai.innerHTML = '<i class="fas fa-save me-1"></i> L?u l?i';
            }
        });
    }
    
    // --- T? ??NG C?P NH?T MÃ V? TRÍ KHI CH?N LO?I ---
    const loaiViTriSelect = document.getElementById('loaiViTri');
    if (loaiViTriSelect) {
        loaiViTriSelect.addEventListener('change', function() {
            // N?u mã v? trí ?ang tr?ng ho?c là mã t? ??ng, t? ??ng c?p nh?t
            const currentMa = maViTriInput.value.trim();
            if (!currentMa || /^(KHO|KL|KM|KE|KT|VT)-\d{3}$/.test(currentMa)) {
                btnGenMa.click();
            }
        });
    }
});
