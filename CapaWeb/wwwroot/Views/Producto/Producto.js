
const modal = "mdData";

$("#btnNuevo").on("click", function () {
    $(`#${modal}`).modal('show');
})

//document.addEventListener("DOMContentLoaded", function () {

//    $("#loadin").LoadingOverlay("show");

//    fetch(`/Home/ListaDetaCatego`, {
//        method: "GET",
//        headers: { 'Content-Type': 'application/json;charset=utf-8' }
//    })
//        .then(response => {
//            return response.ok ? response.json() : Promise.reject(response);
//        })
//        .then(responseJson => {

//            $("#loadin").LoadingOverlay("hide");

//            if (responseJson.data.length > 0) {

//                $("#contenedor-product").html("");

//                responseJson.data.forEach(function (item) {

//                    $("#contenedor-product").append(
//                        `
//                        <div class="col mb-4">
//                            <div class="card h-100 card-doctor" style="cursor:pointer;">
//                                <div class="card-body text-center">
//                                    <h5 class="card-title"><i class="fa-solid fa-chart-bar"></i> ${item.nombre} Cant. Prod ${item.cantidadProductos}</h5>
//                                </div>
//                            </div>
//                        </div>
//                        `
//                    );
//                });
//            }
//        })
//        .catch(error => {
//            $("#loadin").LoadingOverlay("hide");
//            console.log(error);
//            Swal.fire({
//                title: "Error!",
//                text: "No se pudo obtener la información.",
//                icon: "error"
//            });
//        });

//});

//fin