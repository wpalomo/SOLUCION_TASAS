﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clases_Tasa_Usuario
{
    public partial class TasaUsuarioLote
    {
        [JsonProperty("error")]
        public Error[] Error { get; set; }

        [JsonProperty("msj")]
        public Msj_Lote[] Msj { get; set; }
    }

    public partial class Error
    {
        [JsonProperty("error")]
        public string[] error_1 { get; set; }

        [JsonProperty("id_tasa")]
        public long IdTasa { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }
    }

    public partial class Msj_Lote
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}