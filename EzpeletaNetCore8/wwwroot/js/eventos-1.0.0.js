
window.onload = ListadoEventos();

function ListadoEventos(){
 
    $.ajax({
        // la URL para la petición
        url: '../../Eventos/ListadoEventos',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {  },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (eventos) {

            $("#ModalEvento").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(eventos, function (index, evento) {  
                
                contenidoTabla += `
                <tr>
                    <td>${evento.descripcion}</td>
                    <td class="text-center">
                    <button type="button" class="btn btn-success btn-sm" onclick="AbrirModalEditar(${evento.eventoID})">
                    <i class="fa-solid fa-marker"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button" class="btn btn-danger btn-sm" onclick="EliminarRegistro(${evento.eventoID})">
                    <i class="fa-solid fa-trash"></i>
                    </button>
                    </td>
                </tr>
             `;
            });

            document.getElementById("tbody-eventos").innerHTML = contenidoTabla;

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
    document.getElementById("EventoID").value = 0;
    document.getElementById("descripcion").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Evento");
}

function AbrirModalEditar(eventoID){
    
    $.ajax({
        // la URL para la petición
        url: '../../Eventos/ListadoEventos',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: eventoID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (eventos) {
            let evento = eventos[0];

            document.getElementById("EventoID").value = eventoID;
            $("#ModalTitulo").text("Editar Evento");
            document.getElementById("descripcion").value = evento.descripcion;
            $("#ModalEvento").modal("show");
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
    let eventoID = document.getElementById("EventoID").value;
    let descripcion = document.getElementById("descripcion").value;
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO SON DE INGRESO DE VALORES Y NO SE NECESITA VERIFICAR EN BASES DE DATOS
    //LUEGO POR OTRO LADO HACER VERIFICACIONES DE DATOS EN EL BACK, SI EXISTE EL ELEMENTO SI NECESITAMOS LA BASE DE DATOS.
    console.log(descripcion);
    $.ajax({
        // la URL para la petición
        url: '../../Eventos/GuardarEvento',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { eventoID: eventoID, descripcion: descripcion},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {

            if(resultado != ""){
                alert(resultado);
            }
            ListadoEventos();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }
    });    
}

function EliminarRegistro(eventoID){
    $.ajax({
        // la URL para la petición
        url: '../../Eventos/EliminarEvento',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { eventoID: eventoID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {  
            if(!resultado){
                alert("No se puede eliminar, existen ejercicios asociados.");
            }         
            ListadoEventos();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al eliminar el registro.');
        }
    });    

}