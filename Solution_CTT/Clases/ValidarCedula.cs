using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_CTT.Clases
{
    public class ValidarCedula
    {

        //FUNCION PARA VALIDAR LA CEDULA
        public string validarCedulaConsulta(string cedula)
        {
            int sumap = 0;
            int sumai = 0;
            int x = 10;
            int aux1;
            int digito, i;
            int valor;
            string longi = cedula.Length.ToString();

            if ((cedula == "2222222222") || (cedula == "1616161616") || (cedula == "1212121212") || (cedula == "9876543210") || (cedula == "1234567890"))
            {
                //Mensaje de cedula erronea
                return "NO";
            }
            else
            {
                if (longi == "10")
                {

                    //VERIFICACION DE PROVINCIA
                    aux1 = (Convert.ToInt32(cedula.Substring(0, 1)) * 10) + Convert.ToInt32(cedula.Substring(1, 1));

                    if (aux1 > 50 || aux1 < 1)
                    //if (aux1 > 24 || aux1 < 1 || aux1 != 30 || aux1 != 50)
                    {
                        //Mensaje de cedula erronea
                        return "NO";
                    }

                    else if ((Convert.ToInt32(cedula.Substring(2,1)) > 6) && (Convert.ToInt32(cedula.Substring(2,1)) < 0))
                    {
                        //Mensaje de cedula erronea
                        return "NO";
                    }

                    else

                        if (aux1 <= 25 || aux1 >= 1)
                        {
                            for (i = 0; i < 9; i++)
                            {
                                valor = Convert.ToInt32(cedula.Substring(i, 1));
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

                            valor = Convert.ToInt32(cedula.Substring(9, 1));

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
                        else
                        {
                            //Mensaje de cedula erronea
                            return "NO";
                        }
                }
                else
                {
                    //Mensaje de cedula erronea
                    return "NO";
                }
            }
        }
    }
}
