
window.onload = ListadoEjercicios();

function ListadoEjercicios(){
    let fechaDesdeBuscar = $("#FechaDesdeBuscar").val();
    let fechaHastaBuscar = $("#FechaHastaBuscar").val();
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/MostrarEjerciciosPorTipo',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { FechaDesdeBuscar: fechaDesdeBuscar,
            FechaHastaBuscar:fechaHastaBuscar
         },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (tiposEjerciosMostrar) {


            let contenidoTabla = ``;

            $.each(tiposEjerciosMostrar, function (index, tipoEjercicio) {  
                

                contenidoTabla += `
                <tr class="table-success">
                    <td class="anchoCelda">${tipoEjercicio.descripcion}</td>
                    <td class="text-center anchoCelda"></td>
                    <td class="anchoCelda"></td>
                    <td class="text-center anchoCelda"></td>
                     <td class="text-center anchoCelda"></td>
                    <td class="anchoCelda"></td>
                    <td class="text-center anchoCelda anchoBotones"></td>
                    <td class="text-center anchoBotones"></td>
                </tr>
             `;

                $.each(tipoEjercicio.listadoEjercicios, function (index, ejercicioFisico) {  
                    contenidoTabla += `
                    <tr>
                        <td class="anchoCelda"></td>
                        <td class="text-center anchoCelda">${ejercicioFisico.inicioString}</td>
                        <td class="text-center anchoCelda">${ejercicioFisico.finString}</td>
                        <td class="text-center anchoCelda">${ejercicioFisico.intervaloEjercicio}</td>
                        <td style="text-align: right" class="anchoCelda">${ejercicioFisico.caloriasQuemadas.toFixed(2)}</td>
                        <td class="anchoCelda">${ejercicioFisico.estadoEmocionalInicioString}</td>                  
                        <td class="anchoCelda">${ejercicioFisico.estadoEmocionalFinString}</td>
                        <td class="text-center anchoCelda anchoBotones">${ejercicioFisico.observaciones}</td>
                    </tr>
                 `;
                });


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
