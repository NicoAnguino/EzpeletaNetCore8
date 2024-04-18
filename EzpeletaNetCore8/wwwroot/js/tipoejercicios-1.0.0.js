
window.onload = ListadoTipoEjercicios();

function ListadoTipoEjercicios(){
 
    $.ajax({
        // la URL para la petición
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {  },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (tipoDeEjercicios) {

            $("#ModalTipoEjercicio").modal("hide");
            LimpiarModal();
            //$("#tbody-tipoejercicios").empty();
            let contenidoTabla = ``;

            $.each(tipoDeEjercicios, function (index, tipoDeEjercicio) {  
                
                contenidoTabla += `
                <tr>
                    <td>${tipoDeEjercicio.descripcion}</td>
                    <td class="text-center">
                    <button type="button" class="btn btn-success" onclick="AbrirModalEditar(${tipoDeEjercicio.tipoEjercicioID})">
                    Editar
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button" class="btn btn-danger" onclick="EliminarRegistro(${tipoDeEjercicio.tipoEjercicioID})">
                    Eliminar
                    </button>
                    </td>
                </tr>
             `;

                //  $("#tbody-tipoejercicios").append(`
                //     <tr>
                //         <td>${tipoDeEjercicio.descripcion}</td>
                //         <td class="text-center"></td>
                //         <td class="text-center"></td>
                //     </tr>
                //  `);
            });

            document.getElementById("tbody-tipoejercicios").innerHTML = contenidoTabla;

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
    document.getElementById("TipoEjercicioID").value = 0;
    document.getElementById("descripcion").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Tipo de Ejercicio");
}

function AbrirModalEditar(tipoEjercicioID){
    
    $.ajax({
        // la URL para la petición
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: tipoEjercicioID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (tipoDeEjercicios) {
            let tipoDeEjercicio = tipoDeEjercicios[0];

            document.getElementById("TipoEjercicioID").value = tipoEjercicioID;
            $("#ModalTitulo").text("Editar Tipo de Ejercicio");
            document.getElementById("descripcion").value = tipoDeEjercicio.descripcion;
            $("#ModalTipoEjercicio").modal("show");
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
    let tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    let descripcion = document.getElementById("descripcion").value;
    console.log(descripcion);
    $.ajax({
        // la URL para la petición
        url: '../../TipoEjercicios/GuardarTipoEjercicio',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { tipoEjercicioID: tipoEjercicioID, descripcion: descripcion},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {

            
            ListadoTipoEjercicios();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }
    });    
}

function EliminarRegistro(tipoEjercicioID){
    $.ajax({
        // la URL para la petición
        url: '../../TipoEjercicios/EliminarTipoEjercicio',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { tipoEjercicioID: tipoEjercicioID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {           
            ListadoTipoEjercicios();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al eliminar el registro.');
        }
    });    

}