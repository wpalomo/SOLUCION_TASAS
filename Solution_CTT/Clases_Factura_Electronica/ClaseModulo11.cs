using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solution_CTT.Clases_Factura_Electronica
{
    class ClaseModulo11
    {
        
        private String invertirCadena(String cadena)
        {
            String cadenaInvertida = "";

            for (int x = cadena.Length - 1; x >= 0; x--)
            {
                cadenaInvertida = cadenaInvertida + cadena.Substring(x, 1);
            }
            return cadenaInvertida;
        }

        public int obtenerSumaPorDigitos(String cadena)
        {
            int pivote = 2;
            int longitudCadena = cadena.Length;
            int cantidadTotal = 0;
            int temporal, b = 1;
            for (int i = 0; i < longitudCadena; i++)
            {
                if (pivote == 8)
                {
                    pivote = 2;
                }

                temporal = Convert.ToInt32(cadena.Substring(i, b));
                
                b++;
                temporal *= pivote;
                pivote++;
                cantidadTotal += temporal;
            }
            cantidadTotal = 11 - cantidadTotal % 11;
            return cantidadTotal;
        }
    }
}
