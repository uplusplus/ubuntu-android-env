/*
Aplicación de prueba del sensor.

Esta aplicación de consola leerá los datos de medición y los almacenará en una
hoja de cálculo de Excel XML.
*/

#include "lmsapi_sensor.h"
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>

void write_excel_file(const char * filename,LMSAPI_LASER_DATA * data);

int main()
{

	// obtener los parámetros de entrada.
	printf("***********************PRUEBA DEL LMS200**********************************\n\n");
	printf("Por favor escriba el número del puerto COM donde está conectado el sensor (por ejemplo 1):");
	int port = 1;
	scanf("%d",&port);
	printf("\nPor favor seleccione el rango angular:\n");
	printf("*. 100 grados (presione 0):\n");
	printf("*. 180 grados (presione 1):\n");
	int rangoangular = 0;
	scanf("%d",&rangoangular);
	if(rangoangular==1) rangoangular = 180;
	else rangoangular = 100;
	printf("\nPor favor seleccione la resolución angular:\n");
	printf("*. 1 grado     (presione 0):\n");
	printf("*. 0.5 grados  (presione 1):\n");
	printf("*. 0.25 grados (presione 2):\n");
	int resangular = 0;
	scanf("%d",&resangular);
	if(resangular==1) resangular = 50;
	if(resangular==2) resangular = 25;
	else resangular = 100;

	printf("\nPor favor seleccione el rango de distancia :\n");
	printf("*. 8  metros (presione 0):\n");
	printf("*. 80 metros (presione 1):\n");
	int distancia = 0;
	scanf("%d",&distancia);
	if(distancia==1) distancia = 80;
	else distancia = 8;


	//Pasos basicos para la utilización del sensor.

	//Paso 1b: Crear una variable de conexión
	LMSAPI_CONNECTION *  miconexion = NULL;

	//Paso 2: Abrir conexión con el sensor
	miconexion = lmsapi_open_terminal(
		port,//Puerto COM 1
		rangoangular, // width, o rango angular de 100 grados
		resangular,// Resolución de 1 grado de circunferencia.
		distancia,// Rango de distancias hasta de 8 metros
		0 // Sin información de intensidad
		);

	if(miconexion==0) //Tenemos un error
	{
		exit(0);
	}


	//Paso 3: leemos datos de medición
	// Paso3a: creamos una variable para almacenar los datos
	LMSAPI_LASER_DATA  * mediciones = lmsapi_laser_data_create();

	// Paso3b: Enviamos la solicitud al sensor y obtenemos la respuesta en el acto.
	int result =  lmsapi_request_measurement(
					miconexion,
					mediciones,10.0f/*Centimetros*/);

	if(result!=0) //Tenemos un error
	{
	    lmsapi_laser_data_destroy(mediciones);
		lmsapi_close_terminal(miconexion);
		exit(0);
	}
	// Guardar datos en excel
	write_excel_file("lms200_mediciones.xml",mediciones);
	write_excel_file("lms200_mediciones.csv",mediciones);

	lmsapi_console_out("Operacion exitosa! los datos han sido guardados en \'lms200_mediciones.xml\'");

	//Cerramos la conexion
	lmsapi_laser_data_destroy(mediciones);
	lmsapi_close_terminal(miconexion);

	lmsapi_console_out("Presione una tecla..");
	getche();

	return 0;
}


void write_excel_file(const char * filename,LMSAPI_LASER_DATA * data)
{
	FILE * fp = fopen(filename,"w");

	//escribir encabezado  del archivo de excel
	fprintf(fp,"<?xml version=\"1.0\"?>\n");
	fprintf(fp,"<?mso-application progid=\"Excel.Sheet\"?>\n");
	fprintf(fp,"<?mso-application progid=\"Excel.Sheet\"?>\n");
	fprintf(fp,"<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" \n");
 	fprintf(fp,"xmlns:o=\"urn:schemas-microsoft-com:office:office\" \n");
 	fprintf(fp,"xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \n");
 	fprintf(fp,"xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" \n");
 	fprintf(fp,"xmlns:html=\"http://www.w3.org/TR/REC-html40\">\n");
 	fprintf(fp,"<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">\n");
 	fprintf(fp,"</DocumentProperties> \n");
 	fprintf(fp,"<Worksheet ss:Name=\"Mediciones_LMS200\">\n");
 	//Encabezado de tabla
  	fprintf(fp,"<Table>\n");
  	//con los titulos
   	fprintf(fp,"<Row>\n");
	fprintf(fp,"<Cell><Data ss:Type=\"String\">angulo</Data></Cell>\n");
    fprintf(fp,"<Cell><Data ss:Type=\"String\">distancia</Data></Cell>\n");
   	fprintf(fp,"</Row>\n");

	//Imprimir tabla de datos
	float current_angle = data->min_angle;
	float angle_increment = data->resolution;

	for (size_t i=0;i<data->distances_count;i++)
	{

		fprintf(fp,"<Row>\n");
		fprintf(fp,"<Cell><Data ss:Type=\"Number\">");
		fprintf(fp,"%f</Data></Cell>\n",current_angle);//imprimir el angulo

		fprintf(fp,"<Cell><Data ss:Type=\"Number\">");
		fprintf(fp,"%f</Data></Cell>\n",data->distances[i]);//imprimir la distancia
		fprintf(fp,"</Row>\n");

		current_angle+=angle_increment;
	}

   	//Pie de terminacion de archivo
  	fprintf(fp,"</Table> \n");
 	fprintf(fp,"</Worksheet>\n");
	fprintf(fp,"</Workbook>\n");
	fclose(fp);

}


void write_csv_file(const char * filename,LMSAPI_LASER_DATA * data)
{
	FILE * fp = fopen(filename,"w");

	//escribir encabezado  del archivo cvs
	fprintf(fp,"angulo,distancia\n");

	//Imprimir tabla de datos
	float current_angle = data->min_angle;
	float angle_increment = data->resolution;

	for (size_t i=0;i<data->distances_count;i++)
	{

		fprintf(fp,"%f,%f\n",current_angle,data->distances[i]);//imprimir el angulo y la distancia.
		current_angle+=angle_increment;
	}
	fclose(fp);
}
