using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clases_Tasa_Usuario
{
    public partial class TasaDetalleTransacciones
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("msj")]
        public Msj_TasaDetalleTransacciones[] Msj { get; set; }
    }

    public partial class Msj_TasaDetalleTransacciones
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("cantidad")]
        public long Cantidad { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("confirmacion")]
        public long Confirmacion { get; set; }

        [JsonProperty("usos")]
        public long Usos { get; set; }

        [JsonProperty("oficina_id")]
        public long OficinaId { get; set; }

        [JsonProperty("clientes_ruc")]
        public long ClientesRuc { get; set; }

        [JsonProperty("tipo_tasa")]
        public long TipoTasa { get; set; }

        [JsonProperty("estatus_id")]
        public long EstatusId { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("token")]
        public long Token { get; set; }

        [JsonProperty("secuencial")]
        public long Secuencial { get; set; }

        [JsonProperty("ruta_id")]
        public long RutaId { get; set; }
    }

    public class ClasesTasaUsuario
    {
    }
}