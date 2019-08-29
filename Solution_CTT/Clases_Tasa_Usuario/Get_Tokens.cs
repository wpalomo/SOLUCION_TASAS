using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clases_Tasa_Usuario
{
    public class Get_Tokens
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("msj")]
        public Msj_Get_Tokens[] Msj { get; set; }
    }

    public class Msj_Get_Tokens
    {
        // Properties
        [JsonProperty("cant_actual")]
        public long CantActual { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("estatus_id")]
        public long EstatusId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("max_cant")]
        public long MaxCant { get; set; }

        [JsonProperty("max_sec")]
        public long MaxSec { get; set; }

        [JsonProperty("oficina_id")]
        public long OficinaId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }

}