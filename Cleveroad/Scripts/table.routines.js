var id, table, row, indexOfRow, b;

function OnDeleteClick(e)
        {
            var table = $('#OrdersTable').dataTable();
            id = e.target.id;
            //indexOfRow = $(this).closest("tr").index();
            //b = table.fnFindCellRowIndexes(id, 1);  // Search only column 2
            row = $(this).closest('tr');
     
           

                 if (confirm('Are you really want to delete order # ' + id + ' permanently?'))
	 {
                $.ajax({
                    url: '/home/remove',
                    type: 'POST',
                    data: { id: id },
                    dataType: 'json',
                    success:   function (result) {
                        alert(result);
                        table = $('#OrdersTable').dataTable();
                        table.fnDeleteRow(row[0]);//fnDeleteRow(indexOfRow);
                    }, 
                    error: function () { alert('Error!'); }
                }); 
            }
            return false;
        }
