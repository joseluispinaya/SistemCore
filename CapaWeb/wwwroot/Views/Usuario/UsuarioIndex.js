
let tablaData;
let idEditar = 0;
const controlador = "Usuario";
const modal = "mdData";
const preguntaEliminar = "Desea inactivar al usuario";
const confirmaEliminar = "El usuario fue modificado.";
const confirmaRegistro = "Usuario registrado!";

$(function () {
    obtenerRoles();

    tablaData = $('#tbData').DataTable({
        responsive: true,
        "ajax": {
            "url": `/${controlador}/ListaUsuarios`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idUsuario", "visible": false, "searchable": false },
            { title: "Nro CI", "data": "nroCi" },
            { title: "Nombres", "data": "nombre" },
            { title: "Apellidos", "data": "apellido" },
            { title: "Correo", "data": "correo" },
            { title: "Rol", "data": "nombreRol" },
            {
                title: "Estado",
                "data": "activo",
                render: function (data) {
                    return data
                        ? '<span class="badge badge-success">Activo</span>'
                        : '<span class="badge badge-danger">No Activo</span>';
                }
            },
            {
                title: "Acciones",
                data: "idUsuario",
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

function obtenerRoles() {
    fetch(`/Usuario/ListaRoles`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.length > 0) {
            $("#cboRolUser").append($("<option>").val("").text(""));
            responseJson.data.forEach((item) => {
                $("#cboRolUser").append($("<option>").val(item.idRolUsuario).text(item.nombre));
            });
            $('#cboRolUser').select2({
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

    idEditar = data.idUsuario;
    $("#txtNroDocumento").val(data.nroCi);
    $("#txtNombres").val(data.nombre);
    $("#txtApellidos").val(data.apellido);
    $("#txtCorreo").val(data.correo);
    $("#txtClave").val(data.clave);

    $("#cboRolUser").val(data.idRolUsuario.toString()).trigger("change");

    $("#myLargeModalLabel").text("Editar Usuario");
    $(`#${modal}`).modal('show');

});

$('#tbData tbody').on('click', '.btn-detalle', function () {
    let fila = $(this).closest('tr');
    if (fila.hasClass('child')) fila = fila.prev();
    let data = tablaData.row(fila).data();

    swal("Mensaje", data.idUsuario, "warning");

    console.log("Detalless:", data);
});

$("#btnNuevo").on("click", function () {
    idEditar = 0;
    $("#txtNroDocumento").val("");
    $("#txtNombres").val("");
    $("#txtApellidos").val("");
    $("#txtCorreo").val("");
    $("#txtClave").val("");
    $("#cboRolUser").val("").trigger('change');

    $("#myLargeModalLabel").text("Registrar Usuario");

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

    if ($("#cboRolUser").val() === "") {
        toastr.warning("", "Debe Seleccionar un Rol");
        habilitarBoton();
        return;
    }

    const objeto = {
        IdUsuario: idEditar,
        NroCi: $("#txtNroDocumento").val().trim(),
        Nombre: $("#txtNombres").val().trim(),
        Apellido: $("#txtApellidos").val().trim(),
        Correo: $("#txtCorreo").val().trim(),
        Clave: $("#txtClave").val().trim(),
        IdRolUsuario: $("#cboRolUser").val()
    }

    $(`#${modal}`).find("div.modal-content").LoadingOverlay("show");

    if (idEditar != 0) {

        fetch(`/${controlador}/Editar`, {
            method: "PUT",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
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
                $(`#${modal}`).find("div.modal-content").LoadingOverlay("hide");
                habilitarBoton();
            });
    } else {
        fetch(`/${controlador}/Guardar`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto)
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