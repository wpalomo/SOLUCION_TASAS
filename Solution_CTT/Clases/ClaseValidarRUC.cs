using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solution_CTT.Clases
{
    public class ClaseValidarRUC
    {
        //VALIDAR RUC PRIVADO
        public bool validarRucPrivado(string sRuc)
        {
            long lNumero;
            const int iLongitudRUC = 13;
            const int iModulo = 11;
            const int iTercerDigito = 9;
            int iTotal = 0;
            const string sEstablecimiento = "001";

            int[] iCoeficientes = { 4, 3, 2, 7, 6, 5, 4, 3, 2 };

            if (long.TryParse(sRuc, out lNumero) && sRuc.Length.Equals(iLongitudRUC))
            {
                var numeroProvincia = Convert.ToInt32(string.Concat(sRuc[0] + string.Empty, sRuc[1] + string.Empty));
                var sociedadPrivada = Convert.ToInt32(sRuc[2] + string.Empty);

                if ((numeroProvincia >= 1) && (numeroProvincia <= 24) && (sociedadPrivada == iTercerDigito) && (sRuc.Substring(10, 3) == sEstablecimiento))
                {
                    var digitoVerificadorRecibido = Convert.ToInt32(sRuc[9] + string.Empty);

                    for (var i = 0; i < iCoeficientes.Length; i++)
                    {
                        iTotal = iTotal + (iCoeficientes[i] * Convert.ToInt32(sRuc[i] + string.Empty));
                    }

                    var digitoVerificadorObtenido = (iTotal % iModulo) == 0 ? 0 : iModulo - (iTotal % iModulo);

                    return digitoVerificadorRecibido == digitoVerificadorObtenido;
                }

                return false;
            }

            return false;
        }

        //VALIDAR RUC PUBLICO
        public bool validarRucPublico(string sRuc)
        {
            long lNumero;
            const int iLongitudRUC = 13;
            const int iModulo = 11;
            const int iTercerDigito = 6;
            int iTotal = 0;
            const string sEstablecimiento = "0001";

            int[] iCoeficientes = { 3, 2, 7, 6, 5, 4, 3, 2 };

            if (long.TryParse(sRuc, out lNumero) && sRuc.Length.Equals(iLongitudRUC))
            {
                var numeroProvincia = Convert.ToInt32(string.Concat(sRuc[0] + string.Empty, sRuc[1] + string.Empty));
                var sociedadPublica = Convert.ToInt32(sRuc[2] + string.Empty);

                if ((numeroProvincia >= 1) && (numeroProvincia <= 24) && (sociedadPublica == iTercerDigito) && (sRuc.Substring(9, 4) == sEstablecimiento))
                {
                    var digitoVerificadorRecibido = Convert.ToInt32(sRuc[8] + string.Empty);

                    for (var i = 0; i < iCoeficientes.Length; i++)
                    {
                        iTotal = iTotal + (iCoeficientes[i] * Convert.ToInt32(sRuc[i] + string.Empty));
                    }

                    var digitoVerificadorObtenido = iModulo - (iTotal % iModulo);

                    return digitoVerificadorRecibido == digitoVerificadorObtenido;
                }

                return false;
            }

            return false;
        }

        //VALIDAR RUC PERSONA NATURAL
        public bool validarRucNatural(string ruc)
        {
            //VALIDAR LA CEDULA
            int sumap = 0;
            int sumai = 0;
            int x = 10;
            int aux1;

            int digito, i;
            int valor;

            if ((ruc == "2222222222001") || (ruc == "1616161616001") || (ruc == "1212121212001") || (ruc == "9876543210001") || (ruc == "1234567890001"))
            {
                //Mensaje de cedula erronea
                return false;
            }

            else if ((ruc.Substring(10, 3) != "001") && (ruc.Substring(10, 3) != "002") && (ruc.Substring(10, 3) != "003"))
            {
                //Mensaje de cedula erronea
                return false;
            }

            else
            {
                //VERIFICACION DE PROVINCIA
                aux1 = (Convert.ToInt32(ruc.Substring(0, 1)) * 10) + Convert.ToInt32(ruc.Substring(1, 1));

                if (aux1 > 24 || aux1 < 1)
                {
                    //Mensaje de cedula erronea
                    return false;
                }
                else
                {

                    for (i = 0; i < 9; i++)
                    {
                        valor = Convert.ToInt32(ruc.Substring(i, 1));
                        if (i % 2 == 0)
                        {
                            valor = valor * 2;
                            if (valor > 9)
                            {
                                valor = valor - 9;
                            }
                            sumai = sumai + valor;
                        }
                        else
                        {
                            sumap = sumap + valor;
                        }
                    }
                    valor = sumap + sumai;

                    while (x < valor)
                    {
                        x = x + 10;
                    }

                    digito = x - valor;

                    valor = Convert.ToInt32(ruc.Substring(9, 1));

                    if (digito != valor)
                    {
                        //Mensaje de cedula erronea
                        return false;
                    }
                    else
                    {
                        //mensaje de céula correcta
                        return true;
                    }
                }
            }
        }
    }
}
