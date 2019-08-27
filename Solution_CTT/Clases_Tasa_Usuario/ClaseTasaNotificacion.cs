using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clases_Tasa_Usuario
{
    public partial class TasaNotificacion
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("id_notificacion")]
        public long IdNotificacion { get; set; }
    }
}