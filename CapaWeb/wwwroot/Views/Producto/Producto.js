
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
            "url": `/${controlador}/ListaProducDto`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idProducto", "visible": false, "searchable": false },
            {
                title: "Imagen",
                "data": "imageFull",
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return `<img src="${data}" class="rounded mx-auto d-block" style="max-width: 40px; max-height: 40px" />`;
                }
            },
            { title: "Nombre", "data": "nombre" },
            { title: "Pre Compra", "data": "precioCompra" },
            { title: "Pre Venta", "data": "precioVenta" },
            { title: "Categoria", "data": "nombreCategoria" },
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
    //console.log("Detalless:", data);

    idEditar = data.idProducto;
    $("#txtNombre").val(data.nombre);
    $("#cboCategoria").val(data.idCategoria.toString()).trigger("change");
    $("#txtDescripcion").val(data.descripcion);
    $("#txtCantidad").val(data.cantidad);
    $("#txtCompra").val(data.precioCompra);
    $("#txtVenta").val(data.precioVenta);
    $("#imgProducto").attr("src", data.imageFull);
    $("#txtImagenPro").val(data.imagenPro);
    $("#txtImagen").val("");
    $(".custom-file-label").text('Ningún archivo seleccionado');

    $("#myLargeModalLabel").text("Editar Producto");
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
    $("#txtCantidad").val("0");
    $("#txtCompra").val("");
    $("#txtVenta").val("");
    $("#imgProducto").attr("src", "/images/sinimagen.png");
    $("#txtImagenPro").val("");
    $("#txtImagen").val("");
    $(".custom-file-label").text('Ningún archivo seleccionado');

    $("#myLargeModalLabel").text("Registrar Producto");

    $(`#${modal}`).modal('show');
})

function habilitarBoton() {
    $('#btnGuardar').prop('disabled', false);
}

$("#btnGuardar").on("click", function () {

    $('#btnGuardar').prop('disabled', true);

    const inputs = $("input.model").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() === "");

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo : "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje);
        //$(`input[name="${inputs_sin_valor[0].name}"]`).focus();
        $(`input[name="${inputs_sin_valor[0].name}"]`).trigger("focus");
        habilitarBoton();
        return;
    }

    if ($("#cboCategoria").val() === "") {
        toastr.warning("", "Debe Seleccionar una categoria");
        habilitarBoton();
        return;
    }

    if ($("#txtDescripcion").val().trim() === "") {
        toastr.warning("", "Debe ingresar una Descripcion");
        $("#txtDescripcion").trigger("focus");
        habilitarBoton();
        return;
    }

    let cantidadStr = $("#txtCantidad").val().trim();

    // Verificar si la cantidad es un número válido, no vacío
    if (cantidadStr === "" || isNaN(cantidadStr) || parseInt(cantidadStr) < 0) {
        toastr.warning("", "Debe ingresar una cantidad válida (positivo o use 0)");
        $("#txtCantidad").trigger("focus");
        habilitarBoton();
        return;
    }

    const fileInput = document.getElementById('txtImagen');
    let precioCompraStr = $("#txtCompra").val().trim();
    let precioVentaStr = $("#txtVenta").val().trim();

    // Verificar si el precio compra es un número válido, no vacío, y mayor que 0
    if (precioCompraStr === "" || isNaN(precioCompraStr) || parseFloat(precioCompraStr) <= 0) {
        toastr.warning("", "Debe ingresar un monto de compra válido (mayor a 0)");
        $("#txtCompra").trigger("focus");
        habilitarBoton();
        return;
    }

    // Verificar si el precio venta es un número válido, no vacío, y mayor que 0
    if (precioVentaStr === "" || isNaN(precioVentaStr) || parseFloat(precioVentaStr) <= 0) {
        toastr.warning("", "Debe ingresar un monto de venta válido (mayor a 0)");
        $("#txtVenta").trigger("focus");
        habilitarBoton();
        return;
    }

    const precioCompra = Number(parseFloat(precioCompraStr).toFixed(2));
    const precioVenta = Number(parseFloat(precioVentaStr).toFixed(2));

    let modelo = {
        IdProducto: idEditar,
        Nombre: $("#txtNombre").val().trim(),
        Descripcion: $("#txtDescripcion").val().trim(),
        PrecioCompra: precioCompra,
        PrecioVenta: precioVenta,
        Cantidad: parseInt(cantidadStr),
        IdCategoria: $("#cboCategoria").val()
        //ImagenPro: $("#txtImagenPro").val().trim()
    }

    const formData = new FormData();
    //formData.append("foto", fileInput.files[0]);

    if (fileInput.files.length > 0) {
        formData.append("foto", fileInput.files[0]);
    }

    formData.append("modelo", JSON.stringify(modelo));

    $(`#${modal}`).find("div.modal-content").LoadingOverlay("show");

    if (idEditar != 0) {

        fetch(`/${controlador}/Editar`, {
            method: "PUT",
            body: formData
        })
            .then(response => response.ok ? response.json() : Promise.reject(response))
            .then(responseJson => {
                if (responseJson.estado) {
                    swal("Mensaje", responseJson.mensaje, "success");
                    $(`#${modal}`).modal('hide');
                    idEditar = 0;
                    tablaData.ajax.reload();
                } else {
                    swal("Error!", responseJson.mensaje, "warning");
                }
            })
            .catch(() => {
                swal("Error!", "No se pudo editar.", "warning");
            })
            .finally(() => {
                // finally se ejecuta siempre (success o catch)
                $(`#${modal}`).find("div.modal-content").LoadingOverlay("hide");
                habilitarBoton();
            });
    } else {
        fetch(`/${controlador}/Guardar`, {
            method: "POST",
            body: formData
        })
            .then(response => response.ok ? response.json() : Promise.reject(response))
            .then(responseJson => {
                if (responseJson.estado) {
                    swal("Mensaje", responseJson.mensaje, "success");
                    $(`#${modal}`).modal('hide');
                    tablaData.ajax.reload();
                } else {
                    swal("Error!", responseJson.mensaje, "warning");
                }
            })
            .catch(() => {
                swal("Error!", "No se pudo registrar.", "warning");
            })
            .finally(() => {
                $(`#${modal}`).find("div.modal-content").LoadingOverlay("hide");
                habilitarBoton();
            });
    }
});


//fin