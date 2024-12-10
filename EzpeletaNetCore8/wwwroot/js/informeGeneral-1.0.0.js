
window.onload = ListadoEjercicios();

function ListadoEjercicios() {
    let fechaDesdeBuscar = $("#FechaDesdeBuscar").val();
    let fechaHastaBuscar = $("#FechaHastaBuscar").val();
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/ArmarInformeGeneral',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {
            FechaDesdeBuscar: fechaDesdeBuscar,
            FechaHastaBuscar: fechaHastaBuscar
        },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (listadoEventosN1) {


            let contenidoTabla = ``;

            $.each(listadoEventosN1, function (index, eventoN1) {


                contenidoTabla += `
                <tr class="table-success">
                    <td class="anchoCelda">${eventoN1.descripcion}</td>
                    <td class="anchoCelda"></td>   
                    <td class="anchoCelda"></td>   
                    <td class="text-center anchoCelda"></td>
                    <td class="anchoCelda"></td>
                    <td class="text-center anchoCelda"></td>
                        <td class="text-center anchoCelda"></td>
                    <td class="anchoCelda"></td>
                    <td class="text-center anchoCelda anchoBotones"></td>
                    <td class="text-center anchoBotones"></td>
                </tr>
             `;

                $.each(eventoN1.listadoLugaresN2, function (index, lugarN2) {
                    contenidoTabla += `
                    <tr class="table-info">
                        <td class="anchoCelda"></td>
                        <td class="anchoCelda">${lugarN2.descripcion}</td>   
                        <td class="anchoCelda"></td>   
                        <td class="text-center anchoCelda"></td>
                        <td class="text-center anchoCelda"></td>
                              <td class="text-center anchoCelda"></td>
                        <td class="text-center anchoCelda"></td>
                        <td class="anchoCelda"></td>                  
                        <td class="anchoCelda"></td>
                        <td class="text-center anchoCelda anchoBotones"></td>
                    </tr>
                 `;


                    $.each(lugarN2.listadoTipoEjerciciosN3, function (index, tipoEjercicioN3) {
                        contenidoTabla += `
                    <tr class="table-warning">
                        <td class="anchoCelda"></td>      
                        <td class="anchoCelda"></td>   
                        <td class="anchoCelda">${tipoEjercicioN3.descripcion}</td>              
                        <td class="text-center anchoCelda"></td>
                        <td class="text-center anchoCelda"></td>
                           <td class="text-center anchoCelda"></td>
                        <td class="text-center anchoCelda"></td>
                        <td class="anchoCelda"></td>                  
                        <td class="anchoCelda"></td>
                        <td class="text-center anchoCelda anchoBotones"></td>
                    </tr>
                 `;

                 $.each(tipoEjercicioN3.vistaEjercicioFisico, function (index, ejercicio) {
                    contenidoTabla += `
                <tr>
                    <td class="anchoCelda"></td>      
                         <td class="anchoCelda"></td>   
                              <td class="anchoCelda"></td>              
                    <td class="text-center anchoCelda">${ejercicio.inicioString}</td>
                    <td class="text-center anchoCelda">${ejercicio.finString}</td>
                    <td class="text-center anchoCelda">${ejercicio.intervaloEjercicio}</td>
                              <td style="text-align: right" class="anchoCelda">${ejercicio.caloriasQuemadas.toFixed(2)}</td>
                    <td class="anchoCelda">${ejercicio.estadoEmocionalInicioString}</td>                  
                    <td class="anchoCelda">${ejercicio.estadoEmocionalFinString}</td>
                    <td class="text-center anchoCelda anchoBotones">${ejercicio.observaciones}</td>
                </tr>
             `;
                });

                    });

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
