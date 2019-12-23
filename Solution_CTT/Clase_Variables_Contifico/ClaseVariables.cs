using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clase_Variables_Contifico
{
    //VARIABLES OBTENER TOKEN
    public class obtenerToken
    {
        public string token { get; set; }
    }

    //VARIABLES OBTENER LOCALIDADES
    public class Localidades
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public Result[] results { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string ruc { get; set; }
        public string nombre_comercial { get; set; }
        public string direccion_matriz { get; set; }
        public double tarifa_tasa { get; set; }
        public string tiempo_gracia { get; set; }
    }
  
    //VARIABLE PARA OBTENER RUTAS
    public class Rutas
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public ResultRutas[] results { get; set; }
    }

    public class ResultRutas
    {
        public string via { get; set; }
        public string destino_nombre { get; set; }
        public int anden { get; set; }
        public Parada[] paradas { get; set; }
    }

    public class Parada
    {
        public string parada_nombre { get; set; }
        public int orden_llegada { get; set; }
        public Tarifa[] tarifas { get; set; }
        public bool is_enable { get; set; }
    }

    public class Tarifa
    {
        public int id { get; set; }
        public int tipo_servicio { get; set; }
        public string tipo_servicio_nombre { get; set; }
        public int tipo_cliente { get; set; }
        public string tipo_cliente_nombre { get; set; }
        public Decimal tarifa { get; set; }
        public bool especial { get; set; }
        public bool is_active { get; set; }
        public bool is_enable { get; set; }
        public DateTime actualizacion { get; set; }
    }

    //VARIABLE PARA OBTENER LAS FRECUENCIAS
    public class ResultFrecuencias
    {
        public string hora_salida { get; set; }
        public string destino_nombre { get; set; }
        public string via { get; set; }
        public int tipo { get; set; }
        public string tipo_nombre { get; set; }
        public string fecha_validez { get; set; }
        public List<int> dias { get; set; }
    }

    public class Frecuencias
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public ResultFrecuencias[] results { get; set; }
    }

    //VARIABLES PARA OBTENER LOS BUSES
    public class ResultBuses
    {
        public int disco { get; set; }
        public object conductor { get; set; }
        public int capacidad { get; set; }
        public int anio_fabricacion { get; set; }
        public string placa { get; set; }
        public string marca_nombre { get; set; }
        public object fecha_emision_matricula { get; set; }
        public object fecha_vencimiento_matricula { get; set; }
    }

    public class Buses
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public ResultBuses[] results { get; set; }
    }

    //VARIABLES PARA OBTENER LOS CONDUCTORES
    public class ResultConductores
    {
        public int id { get; set; }
        public int tipo { get; set; }
        public string tipo_nombre { get; set; }
        public string identificacion { get; set; }
        public string nombre { get; set; }
    }

    public class Conductores
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public ResultConductores[] results { get; set; }
    }

    //VARIABLES PARA CONSULTA DE VIAJES
    public class ResultConsultaViajes
    {
        public int id { get; set; }
        public string conductor_identificacion { get; set; }
        public string bus_disco { get; set; }
        public string fecha { get; set; }
        public string hora_salida { get; set; }
        public string conductor_nombre { get; set; }
        public string ruta_nombre { get; set; }
        public int localidad_origen { get; set; }
    }

    public class ConsultaViajes
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<ResultConsultaViajes> results { get; set; }
    }

    //VARIABLES PARA OBTENER LA RESPUESTA DE LA CREACION DE VIAJE
    public class CrearViaje
    {
        public int id { get; set; }
        public string conductor_identificacion { get; set; }
        public string bus_disco { get; set; }
        public string fecha { get; set; }
        public string hora_salida { get; set; }
        public string conductor_nombre { get; set; }
        public string ruta_nombre { get; set; }
        public int localidad_origen { get; set; }
    }

    //VARIABLES PARA OBTENER LA RESPUESTA DE LA TASA DE USUARIO SMARTT
    public class Cliente
    {
        public string identificacion { get; set; }
        public int id { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string tipo_cliente { get; set; }
        public object direccion { get; set; }
        public object telefono { get; set; }
        public string tipo_identificacion { get; set; }
        public bool extranjero { get; set; }
        public bool is_active { get; set; }
        public bool is_enable { get; set; }
        public DateTime actualizacion { get; set; }
    }

    public class Pasajero
    {
        public string identificacion { get; set; }
        public int id { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string tipo_cliente { get; set; }
        public object direccion { get; set; }
        public object telefono { get; set; }
        public string tipo_identificacion { get; set; }
        public bool extranjero { get; set; }
        public bool is_active { get; set; }
        public bool is_enable { get; set; }
        public DateTime actualizacion { get; set; }
    }

    public class Boleto
    {
        public int id { get; set; }
        public int asiento { get; set; }
        public string asiento_nombre { get; set; }
        public int nivel { get; set; }
        public double valor { get; set; }
        public int localidad_embarque { get; set; }
        public int tipo_cliente { get; set; }
        public object parada_embarque { get; set; }
        public int parada_destino { get; set; }
        public Pasajero pasajero { get; set; }
        public string tasa { get; set; }
        public int estado { get; set; }
        public string estado_nombre { get; set; }
        public bool is_active { get; set; }
        public bool is_enable { get; set; }
        public DateTime actualizacion { get; set; }
    }

    public class TasaUsuarioSmartt
    {
        public int id { get; set; }
        public DateTime fecha_hora_venta { get; set; }
        public string numero_documento { get; set; }
        public object clave_acceso { get; set; }
        public string numero_documento_tasa { get; set; }
        public string clave_acceso_tasa { get; set; }
        public double total_tasas { get; set; }
        public int viaje { get; set; }
        public int forma_de_pago { get; set; }
        public Cliente cliente { get; set; }
        public List<Boleto> boletos { get; set; }
        public string uuid { get; set; }
        public int estado { get; set; }
        public string estado_nombre { get; set; }
        public bool offline { get; set; }
        public object emision { get; set; }
        public int cooperativa { get; set; }
        public string destino { get; set; }
        public bool is_active { get; set; }
        public bool is_enable { get; set; }
        public DateTime actualizacion { get; set; }
    }

    //VARIABLES DE LA LIBERACION DE ASIENTOS


}