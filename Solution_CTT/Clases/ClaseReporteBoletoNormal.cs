using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NEGOCIO;

namespace Solution_CTT.Clases
{
    public class ClaseReporteBoletoNormal
    {
        manejadorConexion conexionM = new manejadorConexion();
        Clases.ClaseManejoCaracteres caracter = new ClaseManejoCaracteres();
        Clases.ClaseParametros parametros = new ClaseParametros();

        DataTable dtConsulta;
        DataTable dtTipos;
        DataTable dtEmpresa;

        string sSql;
        string sTexto;
        string sAuxiliar;
        string sNombrePasajero;
        string sTipoCliente;

        int iIdPedido;
        int iIdFactura;

        bool bRespuesta;

        double dbCantidad;
        double dbPrecioUnitario;
        double dbDescuento;
        double dbTotal;

        string sRetorno;

        public string llenarBoletoNormal(int iIdPedido_P)
        {
            try
            {
                this.iIdPedido = iIdPedido_P;
                sTexto = "";
                sAuxiliar = "";
                dbTotal = 0;

                sSql = "";
                sSql += "select * from ctt_vw_factura" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexionM.consultarRegistro(sSql, dtConsulta);

                if (bRespuesta == false)
                {
                    return "ERROR";
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    return "ERROR";
                }

                sTexto += "TIPO DE VIAJE: " + dtConsulta.Rows[0][74].ToString().Trim() + Environment.NewLine;
                sTexto += ("BUS DISCO: " + dtConsulta.Rows[0][50].ToString() + " - " + dtConsulta.Rows[0][68].ToString()).PadRight(32, ' ') + "VIAJE COD: " + dtConsulta.Rows[0][69].ToString() + Environment.NewLine;
                sTexto += ("EMISION: " + Convert.ToDateTime(dtConsulta.Rows[0][1].ToString()).ToString("dd/MM/yyyy")).PadRight(32, ' ') + "RUTA: " + dtConsulta.Rows[0][25].ToString() + Environment.NewLine;
                sTexto += "ASIENTO: " + dtConsulta.Rows[0][60].ToString() + Environment.NewLine;
                sTexto += ("TIPO CLIENTE: " + dtConsulta.Rows[0][72].ToString()).PadRight(32, ' ') + "DESTINO" + dtConsulta.Rows[0][70].ToString() + Environment.NewLine;
                sTexto += ("ANDEN: " + dtConsulta.Rows[0][54].ToString()).PadRight(32, ' ') + "OFICINISTA: " + dtConsulta.Rows[0][55].ToString() + Environment.NewLine;
                //sTexto += ("SALIDA: " + Convert.ToDateTime(dtConsulta.Rows[0][52].ToString()).ToString("dd/MM/yyyy") + "  " + Convert.ToDateTime(dtConsulta.Rows[0][50].ToString()).ToString("HH:mm")).PadRight(32, ' ');
                sTexto += ("SALIDA: " + Convert.ToDateTime(dtConsulta.Rows[0][52].ToString()).ToString("dd/MM/yyyy") + "  " + Convert.ToDateTime(dtConsulta.Rows[0][53].ToString()).ToString("HH:mm")).PadRight(32, ' ');
                sTexto += dtConsulta.Rows.Count.ToString() + " PASAJE(S) PRECIO: $" + Environment.NewLine;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dbCantidad = Convert.ToDouble(dtConsulta.Rows[0][27].ToString());
                    dbPrecioUnitario = Convert.ToDouble(dtConsulta.Rows[0][28].ToString());
                    dbDescuento = Convert.ToDouble(dtConsulta.Rows[0][29].ToString());
                    dbTotal = dbTotal + (dbCantidad * (dbPrecioUnitario - dbDescuento));

                    sAuxiliar += ("C.I. PASAJERO: " + dtConsulta.Rows[0][59].ToString()).PadRight(32, ' ');

                    sNombrePasajero = (dtConsulta.Rows[0][58].ToString() + " " + dtConsulta.Rows[0][57].ToString()).Trim();

                    if (sNombrePasajero.Length > 28)
                    {
                        sAuxiliar += sNombrePasajero.Substring(0, 28) + Environment.NewLine;
                    }

                    else
                    {
                        sAuxiliar += sNombrePasajero + Environment.NewLine;
                    }

                }

                sTexto += dbTotal.ToString("N2") + Environment.NewLine;
                sTexto += sAuxiliar + Environment.NewLine;

                sTexto += "VALOR: $".PadLeft(40, ' ') + dbTotal.ToString("N2") + Environment.NewLine + Environment.NewLine + ".";


                return sTexto;
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }
    }
}