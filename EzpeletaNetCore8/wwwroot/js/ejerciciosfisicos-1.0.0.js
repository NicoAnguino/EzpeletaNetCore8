
window.onload = ListadoEjercicios();

function ListadoEjercicios(){
    let fechaDesdeBuscar = $("#FechaDesdeBuscar").val();
    let fechaHastaBuscar = $("#FechaHastaBuscar").val();
    let tipoEjercicioBuscarID = $("#TipoEjercicioBuscarID").val();
    let lugarBuscarID = $("#LugarBuscarID").val();
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/GetEjerciciosFisicos',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { FechaDesdeBuscar: fechaDesdeBuscar,
            FechaHastaBuscar: fechaHastaBuscar,
            TipoEjercicioBuscarID: tipoEjercicioBuscarID,
            LugarBuscarID: lugarBuscarID
         },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (ejercicioFisicosMostrar) {

            $("#ModalEjercicioFisico").modal("hide");
            LimpiarModal();

            let contenidoTabla = ``;

            $.each(ejercicioFisicosMostrar, function (index, ejercicioFisico) {  
                
                contenidoTabla += `
                <tr>
                    <td class="anchoCelda">${ejercicioFisico.tipoEjercicioNombre}</td>
                    <td class="anchoCelda">${ejercicioFisico.lugarNombre}</td>
                    <td class="text-center anchoCelda">${ejercicioFisico.inicioString}</td>
                    <td class="anchoCelda">${ejercicioFisico.estadoEmocionalInicioString}</td>
                    <td class="text-center anchoCelda">${ejercicioFisico.finString}</td>
                    <td class="anchoCelda">${ejercicioFisico.estadoEmocionalFinString}</td>
                    <td class="text-center anchoCelda anchoBotones">
                    <button type="button" class="btn btn-success btn-sm" onclick="AbrirModalEditar(${ejercicioFisico.ejercicioFisicoID})">
                    <i class="fa-solid fa-marker"></i>
                    </button>
                    </td>
                    <td class="text-center anchoBotones">
                    <button type="button" class="btn btn-danger btn-sm" onclick="EliminarRegistro(${ejercicioFisico.ejercicioFisicoID})">
                    <i class="fa-solid fa-trash"></i>
                    </button>
                    </td>
                </tr>
             `;
            });

            document.getElementById("tbody-ejerciciosfisicos").innerHTML = contenidoTabla;

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado');
        }
    });
}

function LimpiarModal(){
    document.getElementById("EjercicioFisicoID").value = 0;
    document.getElementById("TipoEjercicioID").value = 0;
    document.getElementById("LugarID").value = 0;
    document.getElementById("EstadoEmocionalInicio").value = 0;
    document.getElementById("EstadoEmocionalFin").value = 0;
    document.getElementById("Observaciones").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Ejercicio");
}

function AbrirModalEditar(ejercicioFisicoID){
    
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/GetEjerciciosFisicos',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: ejercicioFisicoID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (ejercicioFisicosMostrar) {
            let ejercicioFisico = ejercicioFisicosMostrar[0];

            document.getElementById("EjercicioFisicoID").value = ejercicioFisicoID;
            document.getElementById("TipoEjercicioID").value = ejercicioFisico.tipoEjercicioID;
            document.getElementById("LugarID").value = ejercicioFisico.lugarID;
            document.getElementById("EstadoEmocionalInicio").value = ejercicioFisico.estadoEmocionalInicio;
            document.getElementById("EstadoEmocionalFin").value = ejercicioFisico.estadoEmocionalFin;
            document.getElementById("FechaInicio").value = ejercicioFisico.inicio;
            document.getElementById("FechaFin").value = ejercicioFisico.fin;        
            document.getElementById("Observaciones").value = ejercicioFisico.observaciones;
            $("#ModalTitulo").text("Editar Ejercicio");
            $("#ModalEjercicioFisico").modal("show");
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al consultar el registro para ser modificado.');
        }
    });
}

function GuardarRegistro(){
    //GUARDAMOS EN UNA VARIABLE LO ESCRITO EN EL INPUT DESCRIPCION

    let ejercicioFisicoID = document.getElementById("EjercicioFisicoID").value;
    let tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    let lugarID = document.getElementById("LugarID").value;
    let estadoEmocionalInicio = document.getElementById("EstadoEmocionalInicio").value;
    let estadoEmocionalFin = document.getElementById("EstadoEmocionalFin").value;
    let fechaInicio = document.getElementById("FechaInicio").value;
    let fechaFin = document.getElementById("FechaFin").value;         
    let observaciones = document.getElementById("Observaciones").value;

    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO SON DE INGRESO DE VALORES Y NO SE NECESITA VERIFICAR EN BASES DE DATOS
    //LUEGO POR OTRO LADO HACER VERIFICACIONES DE DATOS EN EL BACK, SI EXISTE EL ELEMENTO SI NECESITAMOS LA BASE DE DATOS.
    //console.log(descripcion);
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/GuardarEjercicio',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { ejercicioFisicoID: ejercicioFisicoID, tipoEjercicioID: tipoEjercicioID, lugarID: lugarID, 
            estadoEmocionalInicio: estadoEmocionalInicio,
            estadoEmocionalFin: estadoEmocionalFin, fechaInicio: fechaInicio,fechaFin:fechaFin,observaciones:observaciones
        },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (error) {

            if(error == 0){
                ListadoEjercicios();
            }
            if(error == 1){
                alert("DEBE SELECCIONAR UN TIPO DE EJERCICIO");
            }
            if(error == 2){
                alert("FECHAS INCORRECTAS");
            }

            
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }
    });    
}

function EliminarRegistro(ejercicioFisicoID){
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/EliminarEjercicio',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { ejercicioFisicoID: ejercicioFisicoID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {           
            ListadoEjercicios();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al eliminar el registro.');
        }
    });    

}