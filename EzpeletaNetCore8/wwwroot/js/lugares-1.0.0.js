
window.onload = ListadoLugares();

function ListadoLugares(){
 
    $.ajax({
        // la URL para la petición
        url: '../../Lugares/ListadoLugares',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {  },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (lugares) {

            $("#ModalLugar").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(lugares, function (index, lugar) {  
                
                contenidoTabla += `
                <tr>
                    <td>${lugar.descripcion}</td>
                    <td class="text-center">
                    <button type="button" class="btn btn-success btn-sm" onclick="AbrirModalEditar(${lugar.lugarID})">
                    <i class="fa-solid fa-marker"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button" class="btn btn-danger btn-sm" onclick="EliminarRegistro(${lugar.lugarID})">
                    <i class="fa-solid fa-trash"></i>
                    </button>
                    </td>
                </tr>
             `;
            });

            document.getElementById("tbody-lugares").innerHTML = contenidoTabla;

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
    document.getElementById("LugarID").value = 0;
    document.getElementById("descripcion").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Lugar");
}

function AbrirModalEditar(lugarID){
    
    $.ajax({
        // la URL para la petición
        url: '../../Lugares/ListadoLugares',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: lugarID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (lugares) {
            let lugar = lugares[0];

            document.getElementById("LugarID").value = lugarID;
            $("#ModalTitulo").text("Editar Lugar");
            document.getElementById("descripcion").value = lugar.descripcion;
            $("#ModalLugar").modal("show");
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
    let lugarID = document.getElementById("LugarID").value;
    let descripcion = document.getElementById("descripcion").value;
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO SON DE INGRESO DE VALORES Y NO SE NECESITA VERIFICAR EN BASES DE DATOS
    //LUEGO POR OTRO LADO HACER VERIFICACIONES DE DATOS EN EL BACK, SI EXISTE EL ELEMENTO SI NECESITAMOS LA BASE DE DATOS.
    console.log(descripcion);
    $.ajax({
        // la URL para la petición
        url: '../../Lugares/GuardarLugar',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { lugarID: lugarID, descripcion: descripcion},
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
            ListadoLugares();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }
    });    
}

function EliminarRegistro(lugarID){
    $.ajax({
        // la URL para la petición
        url: '../../Lugares/EliminarLugar',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { lugarID: lugarID},
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
            ListadoLugares();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al eliminar el registro.');
        }
    });    

}