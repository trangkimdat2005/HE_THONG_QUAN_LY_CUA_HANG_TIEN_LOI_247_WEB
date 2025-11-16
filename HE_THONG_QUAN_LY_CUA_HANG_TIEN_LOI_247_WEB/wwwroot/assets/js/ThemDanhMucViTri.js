document.addEventListener('DOMContentLoaded', function () {
    
    console.log('ThemDanhMucViTri.js loaded');
    
    // ===========================
    // ELEMENTS
    // ===========================
    const btnGenMa = document.getElementById('btnGenMa');
    const maViTriInput = document.getElementById('maViTri');
    const loaiViTriInput = document.getElementById('loaiViTri');
    const moTaTextarea = document.getElementById('moTa');
    const btnLuuLai = document.getElementById('btnLuuLai');
    
    console.log('Elements found:', {
        btnGenMa: !!btnGenMa,
        maViTriInput: !!maViTriInput,
        loaiViTriInput: !!loaiViTriInput,
        moTaTextarea: !!moTaTextarea,
        btnLuuLai: !!btnLuuLai
    });
    
    // ===========================
    // AUTO-GENERATE CODE
    // ===========================
    if (btnGenMa) {
        btnGenMa.addEventListener('click', function() {
            const loaiViTri = loaiViTriInput.value.trim().toLowerCase();
            let prefix = 'VT';
            
            console.log('Generating code for loaiViTri:', loaiViTri);
            
            if (loaiViTri) {
                if (loaiViTri.includes('kho lanh') || loaiViTri.includes('lanh')) {
                    prefix = 'KL';
                } else if (loaiViTri.includes('kho mat') || loaiViTri.includes('mat')) {
                    prefix = 'KM';
                } else if (loaiViTri.includes('trung bay') || loaiViTri.includes('ke')) {
                    prefix = 'KE';
                } else if (loaiViTri.includes('kho tam') || loaiViTri.includes('tam')) {
                    prefix = 'KT';
                } else if (loaiViTri.includes('kho')) {
                    prefix = 'KHO';
                }
            }
            
            const randomNum = Math.floor(Math.random() * 900) + 100;
            const maViTri = `${prefix}-${randomNum}`;
            
            maViTriInput.value = maViTri;
            console.log('Generated code:', maViTri);
        });
    }
    
    if (loaiViTriInput) {
        loaiViTriInput.addEventListener('change', function() {
            const currentMa = maViTriInput.value.trim();
            if (!currentMa) {
                btnGenMa.click();
            }
        });
    }
    
    // ===========================
    // SAVE
    // ===========================
    if (btnLuuLai) {
        btnLuuLai.addEventListener('click', async function(e) {
            e.preventDefault();
            
            console.log('=== BAT DAU LUU VI TRI ===');
            
            const maViTri = maViTriInput.value.trim();
            const loaiViTri = loaiViTriInput.value.trim();
            const moTa = moTaTextarea.value.trim();
            
            console.log('Form data:', { maViTri, loaiViTri, moTa });
            
            // Validation: Chi check empty, KHONG check format
            if (!loaiViTri) {
                console.error('Validation failed: Loai vi tri empty');
                alert('Vui long nhap loai vi tri!');
                loaiViTriInput.focus();
                return;
            }
            
            if (!maViTri) {
                console.error('Validation failed: Ma vi tri empty');
                alert('Vui long nhap ma vi tri!');
                maViTriInput.focus();
                return;
            }
            
            const requestData = {
                MaViTri: maViTri.toUpperCase(),
                LoaiViTri: loaiViTri,
                MoTa: moTa || null
            };
            
            console.log('Request data to send:', JSON.stringify(requestData, null, 2));
            
            try {
                btnLuuLai.disabled = true;
                const originalHTML = btnLuuLai.innerHTML;
                btnLuuLai.innerHTML = '<i class="fas fa-hourglass-half me-1"></i> Dang luu...';
                
                console.log('Sending POST request to /add-ViTri');
                
                const response = await fetch('/add-ViTri', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestData)
                });
                
                console.log('Response received:', {
                    status: response.status,
                    statusText: response.statusText,
                    ok: response.ok
                });
                
                const contentType = response.headers.get("content-type");
                console.log('Content-Type:', contentType);
                
                let result;
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    result = await response.json();
                    console.log('Response JSON:', result);
                } else {
                    const text = await response.text();
                    console.error('Response is not JSON:', text);
                    throw new Error('Server khong tra ve JSON');
                }
                
                if (response.ok) {
                    console.log('SUCCESS:', result.message);
                    alert(result.message || 'Them vi tri moi thanh cong!');
                    console.log('Redirecting to /QuanLyKhoHang/ViTriSanPham');
                    window.location.href = '/QuanLyKhoHang/ViTriSanPham';
                } else {
                    console.error('ERROR:', result.message);
                    alert('Loi: ' + (result.message || 'Khong the them vi tri'));
                    btnLuuLai.disabled = false;
                    btnLuuLai.innerHTML = originalHTML;
                }
            } catch (error) {
                console.error('EXCEPTION:', error);
                alert('Co loi xay ra: ' + error.message);
                btnLuuLai.disabled = false;
                btnLuuLai.innerHTML = '<i class="fas fa-save me-1"></i> Luu lai';
            }
        });
    } else {
        console.error('Button Luu lai not found!');
    }
});
