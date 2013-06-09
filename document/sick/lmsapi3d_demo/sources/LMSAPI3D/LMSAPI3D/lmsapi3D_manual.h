#ifndef LMSAPI_MANUAL_H_INCLUDED
#define LMSAPI_MANUAL_H_INCLUDED

/* LMSAPI : API PARA EL CONTROL DEL SISTEMA DE MEDICION LASER SICK LMS200
 * Copyright (c) 2007 Francisco León, Universidad Manuela Beltrán.
 * Bogotá, Colombia.
 * Derechos Reservados para la UMB bajo la licencia BSD.
 *
 * La redistribución y el uso de las fuentes y codigo binario, con o sin modificación,
 * están permitidas bajo las siguiuentes condiciones:
 *
 * 1. La redistribución del codigo fuente debe preservar los derechos de copia anteriormente
 *    descritos, las condiciones de uso y la clausula legál de este documento.
 * 2. La redistribución en forma binaria debe reproducir derechos de copia anteriormente
 *    descritos, las condiciones de uso y la clausula legál tanto en los manuales de ayuda
 *    como también en otros materiales provistos en la distribución del software.
 * 3. No se usarán los nombres de los autores para promocioinar productos derivados
 *    de este software sin el permiso por escrito de los autores.
 *
 * Clausula Legal:
 * ESTE SOFTWARE SE OFRECE CON EL DERECHO DE COPIA DE LOS DUEÑOS Y SUS CONTRIBUYENTES
 * 'TAL COMO ESTA' SIN NINGUNA GARANTIA O CONDICION ASOCIADA AL EJERCICIO MERCANTIL
 * O ECONOMICO DE PARTICULARES. SE EXCLUYE A LOS AUTORES O CONTRIBUYENTES DE ESTE SOFTWARE
 * DE TODA RESPONSABILIDAD JURÍDICA POR POSIBLES DAÑOS, PERJUICIOS, SUBSTITUCION
 * DE BIENES O SERVICIOS, PERDIDA DE GANANCIAS, PERDIDA DE EXCLUSIVIDAD, DESLEALDAD,
 * DESPRESTIGIO, DAÑOS A LA CONFIDENCIALIDAD DE INFORMACIÓN O LUCRO CESANTE QUE PODRÍAN SER CAUSADOS
 * POR LA EXISTENCIA O USO NEGLIGENTE DE ESTE SOFTWARE.
 */



//Pagina web principal
/*! \mainpage LMSAPI3D: APLICACIÓN DE GRAFICACIÓN DE SOLIDOS CON EL LASER SICK LMS200
*\section INTRO INTRODUCCIÓN
*
<p><b>LMSAPI3D</b> es una aplicación de ejemplo realizada en el entorno <b>.NET</b>, la cual se incorpora la funcionalidad de la librería
<a href="http://lmsapi.sf.net">LMSAPI</a> en la medición de sólidos, y utiliza el poder gráfico de <a href ="http://irrlichtnetcp.sourceforge.net/">IRRLICHT.NET</a> en la visualización de las mediciones.
</p>
<p>
- Descarge en <a href="http://sf.net/projects/lmsapi">Proyecto LMSAPI</a> los aplicativos de demostración, incluyendo <b>LMSAPI3D</b>.
- \subpage REQ_MIN "REQUERIMIENTOS MÍNIMOS DE INSTALACIÓN"
</p>
*\section vista VISTA DE LA APLICACIÓN LMSAPI3D
*\image html LMSAPI3D_app.png "LMSAPI3D"
*\section CONNECTION CONEXIÓN
*\image html LMSAPI3D_conexion.png "Panel de conexión con el sensor"
<p>En la pestaña de conexión se establecen los parámetros de medición y de configuración del
sensor láser. Aqui se especifican los rangos de medición durante todo el barrido, y el puerto de conexión.</p>
<p>También cuenta con un modo de <b>simulación</b> el cuál se puede utilizar para probar el desempeño del aplicativo sin necesidad de tener conexión con el sensor láser. Este simulador genera datos aleatorios que posteriormente serán graficados en 3D.</p>
*\section TRAVERSE BARRIDO Y SENSADO
*\image html LMSAPI3D_barrido.png "Panel de configuración del barrido"
<p>Para medir sólidos es necesario realizar mediciones sucesivas desplazando el sensor en forma vertical.</p>
<p><b>LMSAPI3D</b> permite registrar barridos <b>angulares</b> los cuales tienen intervalos en ángulos, o <b>lineales</b> los cuales tienen intervalos en centímetros.</p>
<p>En este panel se especifican el rango de medición vertical, el número de mediciones y la resolución de la gráfica. La resolución máxima es de <b>1</b> la cual representa todos los datos de la simulación.</p>
<p>Cuando se dá la orden de <b>graficar</b>, el sistema pedirá confirmación en cada instante en el cual el sensor tomará secciones horizontales del sólido.</p>
*\image html LMSAPI3D_confirmar.png "Confirmación de la medición de cada sección."
*\section EXPORT EXPORTAR LA GRÁFICA
<b>LMSAPI3D</b> permite exportar la malla 3D a tres formatos: 3D Studio (*.3ds), Wavefront (*.obj) y VRML (*.wrl):
*\image html LMSAPI3D_export.png "Exporta la gráfica a un formato de malla 3D."
*/
/*! \page REQ_MIN REQUERIMIENTOS.
*\section REQ_HARD REQUERIMIENTOS DE HARDWARE.
- Computador con procesador Pentium 3 o superior, 256 megabytes en ram, disco duro de 1 GB y Puertos de comunicación compatibles con el estándar RS232.
- Procesador gráfico compatible con OpenGL.
- Cable de comunicaciones RS232.
- Sensor Láser SICK LMS200.
- Fuente de poder que soporte un voltaje de 24 V DC +/- 15%.
- Fuente de poder que soporte un voltaje de 24 V DC +/- 15%.
*\section REQ_SOFT REQUERIMIENTOS DE SOFTWARE.
- Sistema operativo Windows 98/NT/2000/XP/Vista o superior.
- Microsoft .NET Framework, versión 2.0. Disponible en <a href="http://www.microsoft.com/downloads/details.aspx?displaylang=es&FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5">Descargas de Microsoft.</a>
*/
#endif // LMSAPI_CONSOLE_H_INCLUDED
