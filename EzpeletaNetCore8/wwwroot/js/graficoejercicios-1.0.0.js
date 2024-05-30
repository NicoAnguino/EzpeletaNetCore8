
window.onload = GraficoCircular();

let graficoEjercicio;
let graficoCircularEjercicio;

function GraficoCircular(){

    let mesBuscar = $("#MesEjercicioBuscar").val();
    let anioActividadBuscar = $("#AnioEjercicioBuscar").val();
    $.ajax({
        type: "POST",
        url: '../../Home/GraficoTortaTipoActividades',
        data: {Mes: mesBuscar, Anio: anioActividadBuscar},
        success: function (vistaTipoEjercicioFisico) {
           
            var labels = [];
            var data = [];
            var fondo = [];
            $.each(vistaTipoEjercicioFisico, function (index, tipoEjercicio) {

                labels.push(tipoEjercicio.descripcion);
                var color = generarColorVerde();
                fondo.push(color);
                data.push(tipoEjercicio.cantidadMinutos);

            });

            var ctxPie = document.getElementById("grafico-circular");
            graficoCircularEjercicio = new Chart(ctxPie, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: fondo,
                    }],
                },
            });

            MostrarGrafico(); 
        },
        error: function (data) {
        }
    });
}

//DEBEMOS CREAR LA FUNCION PARA QUE BUSQUE EN BASE DE DATOS EN LA TABLA DE EJERCICIOS LOS EJERCICIOS QUE
//COINCIDAN CON EL ELEMENTO TipoEjercicioID, CON EL MES Y EL AÑO

$("#TipoEjercicioID").change(function () {
    graficoEjercicio.destroy();
    MostrarGrafico();
});

$("#MesEjercicioBuscar, #AnioEjercicioBuscar").change(function () {
    graficoCircularEjercicio.destroy();
    graficoEjercicio.destroy();
    GraficoCircular();
});

function MostrarGrafico() {
    let tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    let mesEjercicioBuscar = document.getElementById("MesEjercicioBuscar").value;
    let anioEjercicioBuscar = document.getElementById("AnioEjercicioBuscar").value;

    //console.log(tipoEjercicioID + " - " + mesEjercicioBuscar + " - " + anioEjercicioBuscar);

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
            let diasConEjercicios = 0;
            let minutosTotales = 0;

            $.each(ejerciciosPorDias, function (index, ejercicioDia) { 
                labels.push(ejercicioDia.dia + " " + ejercicioDia.mes);
                data.push(ejercicioDia.cantidadMinutos);
                minutosTotales += ejercicioDia.cantidadMinutos;
                if (ejercicioDia.cantidadMinutos > 0){
                    diasConEjercicios += 1;
                }
            });

            // Obtener el elemento <select>
            var inputTipoEjercicioID = document.getElementById("TipoEjercicioID");
        
            // Obtener el texto de la opción seleccionada
            var ejercicioNombre = inputTipoEjercicioID.options[inputTipoEjercicioID.selectedIndex].text;

            let diasSinEjercicios = ejerciciosPorDias.length - diasConEjercicios;
            $("#texto-card-total-ejercicios").text(minutosTotales + " MINUTOS EN " + diasConEjercicios + " DÍAS");
            $("#texto-card-sin-ejercicios").text(diasSinEjercicios + " DÍAS SIN "+ ejercicioNombre);

            const ctx = document.getElementById('grafico-area');

            graficoEjercicio = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'CANTIDAD DE MINUTOS',
                        data: data,
                        borderWidth: 2,
                        borderRadius: 3,
                        backgroundColor: "rgba(0,129,112,0.2)",
                        borderColor: "rgba(0,129,112,1)",
                        pointRadius: 5,
                        pointBackgroundColor: "rgba(0,129,112,1)",
                        pointBorderColor: "rgba(255,255,255,0.8)",
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(0,116,100,1)",
                        pointHitRadius: 50,
                        pointBorderWidth: 2,
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
            console.log('Disculpe, existió un problema al crear el gráfico.');
        }
    });
}


function generarColorVerde() {
    // El valor de GG será alto (de 128 a 255) para garantizar que predomine el verde.
    // Los valores de RR y BB serán bajos (de 0 a 127).

    let rr = Math.floor(Math.random() * 128); // 0 a 127
    let gg = Math.floor(Math.random() * 128) + 128; // 128 a 255
    let bb = Math.floor(Math.random() * 128); // 0 a 127

    // Convertimos a hexadecimal y formateamos para que tenga siempre dos dígitos.
    let colorHex = `#${rr.toString(16).padStart(2, '0')}${gg.toString(16).padStart(2, '0')}${bb.toString(16).padStart(2, '0')}`;
    return colorHex;
}

