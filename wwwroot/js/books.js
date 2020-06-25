let dataTable;

$(document).ready(function () {
  loadDataTable();
});

const loadDataTable = () => {
  dataTable = $('#DT_load').DataTable({
    ajax: {
      url: "/book/getallbooks/",
      type: "GET",
      datatype: "json"
    },
    columns: [
      {
        data: "name",
        width: "20%"
      },
      {
        data: "author",
        width: "20%"
      },
      {
        data: "isbn",
        width: "20 % "
      },
      {
        data: "id",
        render: data => {
          return `
            <div class="text-center">
              <a href="/Book/Upsert?id=${data}" 
                class='btn btn-success btn-sm form-control text-white'
                style='cursor:pointer; width:70px;'
            >
                Edit
              </a>
              &nbsp;
              <a class='btn btn-danger btn-sm form-control text-white' style='cursor:pointer; width:70px;'
                onclick=DeleteBook('/book/deletebook?id='+${data})
              >
                Delete
              </a>
            </div>`;
        },
        width: "40%"
      }
    ],
    language: {
      emptyTable: "No Data Found"
    },
    width: "100%"
  });
}

const DeleteBook = url => {
  Swal.fire({
    title: "Are you sure?",
    text: "Once deleted, you will not be able to recover the book!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: '#d9534f',
    cancelButtonColor: '#222222',
    confirmButtonText: "Delete"
  }).then(result => {
    if (result.value) {
      $.ajax({
        type: "DELETE",
        url: url,
        success: data => {
          if (data.success) {
            toastr.success(data.message);
            dataTable.ajax.reload();
          }
          else {
            toastr.error(data.message);
          }
        }
      });
    }
  });
}