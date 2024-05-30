
window.onload = MostrarGrafico();

let graficoEjercicio;

//DEBEMOS CREAR LA FUNCION PARA QUE BUSQUE EN BASE DE DATOS EN LA TABLA DE EJERCICIOS LOS EJERCICIOS QUE
//COINCIDAN CON EL ELEMENTO TipoEjercicioID, CON EL MES Y EL AÑO

$("#TipoEjercicioID, #MesEjercicioBuscar, #AnioEjercicioBuscar").change(function () {
    graficoEjercicio.destroy();
    MostrarGrafico();
});

function MostrarGrafico() {
    let tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    let mesEjercicioBuscar = document.getElementById("MesEjercicioBuscar").value;
    let anioEjercicioBuscar = document.getElementById("AnioEjercicioBuscar").value;

    console.log(tipoEjercicioID + " - " + mesEjercicioBuscar + " - " + anioEjercicioBuscar);

    $.ajax({
        // la URL para la petición
        url: '../../Home/GraficoTipoEjercicioMes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { TipoEjercicioID: tipoEjercicioID, Mes: mesEjercicioBuscar, Anio: anioEjercicioBuscar },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (ejerciciosPorDias) {

            let labels = [];
            let data = []; 

            $.each(ejerciciosPorDias, function (index, ejercicioDia) { 
                labels.push(ejercicioDia.dia + " DE " + ejercicioDia.mes);
                data.push(ejercicioDia.cantidadMinutos);
            });

            const ctx = document.getElementById('grafico-area');

            graficoEjercicio = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Cantidad de Minutos',
                        data: data,
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al crear el grafico.');
        }
    });
}

//LUEGO CUANDO CAMBIA ALGUNO DE LOS FILTROS DEBEMOS VOLVER A DESENCADENAR DICHA FUNCION


