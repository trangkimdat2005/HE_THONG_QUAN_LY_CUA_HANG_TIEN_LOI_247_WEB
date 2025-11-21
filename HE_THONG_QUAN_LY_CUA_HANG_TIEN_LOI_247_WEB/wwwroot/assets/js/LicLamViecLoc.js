
        $(document).ready(function () {
            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var minVal = $('#min').val();
                    var maxVal = $('#max').val();

                    var dateString = data[2];

                    if (!dateString) return false;

                    var parts = dateString.split("/");
                    var dateRow = new Date(parts[2], parts[1] - 1, parts[0]);
                    var dateRowTime = dateRow.getTime();

                    var minDate = minVal ? new Date(minVal).getTime() : null;
                    var maxDate = maxVal ? new Date(maxVal).getTime() : null;

                    // Logic so sánh
                    if (
                        (minVal === "" && maxVal === "") || 
                        (minVal === "" && dateRowTime <= maxDate) ||
                        (minDate <= dateRowTime && maxVal === "") || 
                        (minDate <= dateRowTime && dateRowTime <= maxDate)
                    ) {
                        return true; 
                    }
                    return false;
                }
            );

        var table = $('#sampleTable').DataTable();

        $('#btn-filter').click(function () {
            table.draw(); 
            });

             //$('#min, #max').change(function () {table.draw(); });
        });
