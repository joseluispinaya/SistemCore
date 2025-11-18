
let tablaData;
let idEditar = 0;
const controlador = "Producto";
const modal = "mdData";
const confirmaRegistro = "Producto registrado!";

$(function () {
    obtenerCategorias();

    tablaData = $('#tbData').DataTable({
        responsive: true,
        "ajax": {
            "url": `/${controlador}/Lista`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idProducto", "visible": false, "searchable": false },
            {
                title: "Imagen",
                data: "imageFull",
                width: "80px",
                render: function (data) {
                    return `<img src="${data}" class="rounded mx-auto d-block" style="max-width: 40px; max-height: 40px; border-radius: 8px" />`;
                }
            },
            { title: "Nombre", "data": "nombre" },
            { title: "Pre Compra", "data": "precioCompra" },
            { title: "Pre Venta", "data": "precioVenta" },
            {
                title: "Categoria", "data": "categoria", render: function (data) {
                    return data.nombre
                }
            },
            { title: "Cantidad", "data": "cantidad" },
            {
                title: "Acciones",
                data: "idProducto",
                width: "100px",
                orderable: false,
                render: function (id) {
                    return `
                        <button class="btn btn-primary btn-editar btn-sm mr-2" data-id="${id}">
                            <i class="fas fa-pencil-alt"></i>
                        </button>
                        <button class="btn btn-info btn-detalle btn-sm" data-id="${id}">
                            <i class="fas fa-eye"></i>
                        </button>
                    `;
                }
            }
        ],
        order: [[0, "desc"]],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

});

function obtenerCategorias() {
    fetch(`/Producto/ListaCategorias`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.length > 0) {
            $("#cboCategoria").append($("<option>").val("").text(""));
            responseJson.data.forEach((item) => {
                $("#cboCategoria").append($("<option>").val(item.idCategoria).text(item.nombre));
            });
            $('#cboCategoria').select2({
                //theme: 'bootstrap-5',
                dropdownParent: $('#mdData'),
                placeholder: "Seleccionar"
            });
        }
    }).catch((error) => {
        swal("Error!", "No se encontraron coincidencias.", "warning");
    })
}

$('#tbData tbody').on('click', '.btn-editar', function () {

    let fila = $(this).closest('tr');

    if (fila.hasClass('child')) {
        fila = fila.prev();
    }

    let data = tablaData.row(fila).data();

    idEditar = data.idProducto;
    $("#txtNombre").val(data.nombre);
    $("#cboCategoria").val(data.categoria.idCategoria.toString()).trigger("change");
    $("#txtDescripcion").val(data.descripcion);
    $("#txtCantidad").val(data.cantidad);
    $("#txtCompra").val(data.precioCompra);
    $("#txtVenta").val(data.precioVenta);
    $("#imgProducto").attr("src", data.imageFull);
    $("#txtImagenPro").val(data.imagenPro);
    $("#txtImagen").val("");
    $(".custom-file-label").text('Ningún archivo seleccionado');
    $("#myLargeModalLabel").text("Editar Producto");

    //swal("Mensaje", data.nombre, "success");
    $(`#${modal}`).modal('show');

});


$('#tbData tbody').on('click', '.btn-detalle', function () {
    let fila = $(this).closest('tr');
    if (fila.hasClass('child')) fila = fila.prev();
    let data = tablaData.row(fila).data();

    swal("Mensaje", data.idProducto, "warning");

    console.log("Detalless:", data);
});

function esImagen(file) {
    return file && file.type.startsWith("image/");
}

function mostrarImagenSeleccionadaP(input) {
    let file = input.files[0];
    let reader = new FileReader();

    // Si NO se seleccionó archivo (ej: presionaron "Cancelar")
    if (!file) {
        $('#imgProducto').attr('src', "https://joseluis1989-007-site1.ltempurl.com/Imagenes/sinimagen.png");
        $(input).next('.custom-file-label').text('Ningún archivo seleccionado');
        return;
    }

    // Validación: si no es imagen, mostramos error
    if (!esImagen(file)) {
        swal("Error", "El archivo seleccionado no es una imagen válida.", "error");
        $('#imgProducto').attr('src', "https://joseluis1989-007-site1.ltempurl.com/Imagenes/sinimagen.png");
        $(input).next('.custom-file-label').text('Ningún archivo seleccionado');
        input.value = ""; // Limpia el input de archivo
        return;
    }

    // Si todo es válido → mostrar vista previa
    reader.onload = (e) => $('#imgProducto').attr('src', e.target.result);
    reader.readAsDataURL(file);

    // Mostrar nombre del archivo
    $(input).next('.custom-file-label').text(file.name);
}

$("#txtImagen").on("change", function () {
    mostrarImagenSeleccionadaP(this);
});

$("#btnNuevo").on("click", function () {
    idEditar = 0;
    $("#txtNombre").val("");
    $("#cboCategoria").val("").trigger('change');
    $("#txtDescripcion").val("");
    $("#txtCantidad").val("");
    $("#txtCompra").val("");
    $("#txtVenta").val("");
    $("#imgProducto").attr("src", "/images/sinimagen.png");
    $("#txtImagenPro").val("");
    $("#txtImagen").val("");
    $(".custom-file-label").text('Ningún archivo seleccionado');

    $("#myLargeModalLabel").text("Registrar Producto");

    $(`#${modal}`).modal('show');
})



//fin