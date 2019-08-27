using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clases
{
    public class ValidarRUC
    {
        //VALIDAR RUC SOCIEDADES PRIVADAS
        public string validarRucJuridico(string ruc)
        {
            int coeficiente = 4;
            Int32 suma = 0;

            string cadena = ruc.Substring(10, 3);
            if ((cadena == "001") || (cadena == "002") || (cadena == "003"))
            {
                for (int i = 0; i < 9; i++)
                {
                    suma = (Convert.ToInt32(ruc.Substring(i, 1)) * coeficiente) + suma;
                    coeficiente--;
                    if (coeficiente == 1)
                        coeficiente = 7;
                }
                suma = 11 - (suma % 11);

                if (Convert.ToInt32(ruc.Substring(9, 1)) == suma)
                {
                    return "SI";
                }
                else
                {
                    return "NO";
                }
            }
            else
            {
                return "NO";
            }
        }

        //VALIDAR RUC SECTOR PÚBLICO
        public string validarRucPublico(string ruc)
        {
            int coeficiente = 3;
            Int32 suma = 0;

            string cadena = ruc.Substring(9, 4);
            if ((cadena == "0001") || (cadena == "0002") || (cadena == "0003"))
            {
                for (int i = 0; i < 8; i++)
                {
                    suma = (Convert.ToInt32(ruc.Substring(i, 1)) * coeficiente) + suma;
                    coeficiente--;
                    if (coeficiente == 1)
                        coeficiente = 7;
                }
                suma = 11 - (suma % 11);

                if (Convert.ToInt32(ruc.Substring(8, 1)) == suma)
                {
                    return "SI";
                }
                else
                {
                    return "NO";
                }
            }
            else
            {
                return "NO";
            }
        }

        //VALIDAR RUC PERSONA NATURAL
        public string validarRucNatural(string ruc)
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
                return "NO";
            }

            else if ((ruc.Substring(10, 3) != "001") && (ruc.Substring(10, 3) != "002") && (ruc.Substring(10, 3) != "003"))
            {
                //Mensaje de cedula erronea
                return "NO";
            }

            else
            {
                //VERIFICACION DE PROVINCIA
                aux1 = (Convert.ToInt32(ruc.Substring(0, 1)) * 10) + Convert.ToInt32(ruc.Substring(1, 1));

                if (aux1 > 50 || aux1 < 1)
                //if (aux1 > 24 || aux1 < 1 || aux1 != 30 || aux1 != 50)
                {
                    //Mensaje de cedula erronea
                    return "NO";
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
                        return "NO";
                    }
                    else
                    {
                        //mensaje de céula correcta
                        return "SI";
                    }
                }
            }
        }
    }
}