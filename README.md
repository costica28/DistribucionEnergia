# DistribucionEnergia

Para ejecutar correctamente el api es importante tener presense lo siguiente:
  * Modificar la cadena de conexion que se encuentra en el archivo appSettings.json en la carpeta __src/Presentation/DistribucionEnergia.Presentation.Api__
  * Tener instalado VS 2022 con el SDK .Net8
  * Si deseas utilizar el enpoint para cargar la informacion de excel __api/Tramo/CargaInformacion__
    * Debes de tener cargada la informacion de los tramos y los sectores(tipo clientes) que se encuentra en la carpeta __[ScriptsDB/data.sql](https://github.com/costica28/DistribucionEnergia/blob/main/ScriptsDB/data.sql)__
    * Configurar que los separadores de miles sean coma y los decimales punto en el excel
    * Formatear las columnas de las fechas en dd/MM/yyyy
