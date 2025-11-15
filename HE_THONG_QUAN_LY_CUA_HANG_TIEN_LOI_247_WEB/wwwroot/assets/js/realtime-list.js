// realtime-list.js

window.appRealtimeList = (function () {
    let connection = null;
    const reloadActions = {};   // key -> function reload

    async function startConnection() {
        if (connection) return; // đã start rồi thì thôi

        connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/reload")
            .withAutomaticReconnect()
            .build();

        connection.on("ReloadData", async function (key) {
            console.log("[SignalR] ReloadData:", key);
            const fn = reloadActions[key];
            if (fn) {
                await fn();
            } else {
                console.warn("Không có reload action cho key:", key);
            }
        });

        try {
            await connection.start();
            console.log("[SignalR] connected");
        } catch (err) {
            console.error("[SignalR] error:", err);
        }
    }

    // Đăng ký 1 entity: DanhMuc, NhanHieu, SanPham...
    async function initEntityTable({ key, apiUrl, tableId, tbodyId, buildRow }) {
        const $table = $('#' + tableId);
        const tbody = document.getElementById(tbodyId);

        let dataTable = null;

        // Hàm load data riêng cho entity này
        async function loadData() {
            tbody.innerHTML = ''; // xóa sạch

            try {
                const res = await fetch(apiUrl, { method: 'GET' });
                if (!res.ok) {
                    console.error(`Lỗi API ${apiUrl}:`, await res.text());
                    tbody.innerHTML = '';
                    return;
                }

                const data = await res.json();
                if (!data || data.length === 0) {
                    tbody.innerHTML = '';
                    return;
                }

                let html = '';
                data.forEach(item => {
                    html += buildRow(item);
                });
                tbody.innerHTML = html;
            } catch (e) {
                console.error(`Lỗi load dữ liệu cho ${key}:`, e);
                tbody.innerHTML = '';
            }
        }

        // Lần đầu: load + init DataTables
        await loadData();
        dataTable = $table.DataTable();

        // Đăng ký reload cho key này (SignalR sẽ gọi)
        reloadActions[key] = async () => {
            // cách đơn giản: destroy + load lại + init lại
            if (dataTable) {
                dataTable.destroy();
            }
            await loadData();
            dataTable = $table.DataTable();
        };

        // Start SignalR (nếu chưa)
        await startConnection();
    }

    return {
        initEntityTable
    };
})();
