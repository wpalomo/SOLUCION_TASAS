using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clases
{
    public class ClaseManejoCaracteres
    {
        string sRetorno;
        int iLimite;
        int iBand;

        public string saltoLinea(string sCadena, int iInicio)
        {
            sRetorno = "";

            if (iInicio == 0)
            {
                sRetorno = saltoLineaSinPad(sCadena);
            }

            else
            {

                iBand = 0;
                iLimite = 40 - iInicio;

                for (int i = 0; i < sCadena.Length; i = i + iLimite)
                {
                    if (i < iLimite)
                    {
                        if (iBand == 0)
                        {
                            sRetorno += sCadena.Substring(i, iLimite) + Environment.NewLine;
                            iBand = 1;
                        }

                        else
                        {
                            sRetorno += "".PadRight(iInicio, ' ') + sCadena.Substring(i, iLimite) + Environment.NewLine;
                        }
                    }

                    else
                    {
                        sRetorno += "".PadRight(iInicio, ' ') + sCadena.Substring(i) + Environment.NewLine;
                    }
                }
            }

            return sRetorno;
        }

        private string saltoLineaSinPad(string sCadena)
        {
            iLimite = sCadena.Length;

            for (int i = 0; i < sCadena.Length; i = i + 40)
            {
                if (iLimite > 40)
                {
                    sRetorno += sCadena.Substring(i, 40) + Environment.NewLine;
                    iLimite = iLimite - 40;
                }

                else
                {
                    sRetorno += sCadena.Substring(i) + Environment.NewLine;
                }

            }

            return sRetorno;
        }
    }
}