using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Solution_CTT
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);

            Application["idEmpresa"] = null;
            Application["idLocalidad"] = null;
            Application["cgEmpresa"] = null;

            //VARIABLES DE APLICACION PARA PARAMETROS GENERALES
            Application["iva"] = null;
            Application["ice"] = null;
            Application["facturacion_electronica"] = null;
            Application["configuracion_decimales"] = null;
            Application["maneja_nota_venta"] = null;
            Application["vista_previa_impresion"] = null;
            Application["idPersonaSinDatos"] = null;
            Application["idPersonaMenorEdad"] = null;
            Application["demo"] = null;
            Application["cgMoneda"] = null;
            Application["consumidor_final"] = null;
            Application["numero_consumidor_final"] = null;
            Application["id_comprobante"] = null;
            Application["id_producto_extra"] = null;
            Application["nombre_producto_extra"] = null;
            Application["precio_producto_extra"] = null;
            Application["ciudad_default"] = null;
            Application["telefono_default"] = null;
            Application["correo_default"] = null;
            Application["base_clientes"] = null;
            Application["registro_civil"] = null;
            Application["numero_id_sin_datos"] = null;
            Application["numero_id_menor_edad"] = null; 
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //VARIABLES DE APLICACION PARA PARAMETROS POR TERMINAL
            Session["id_pueblo"] = null;
            Session["nombre_terminal"] = null;
            Session["cgCiudad"] = null;
            Session["idVendedor"] = null;
            Session["pago_administracion"] = null;
            Session["porcentaje_retencion"] = null;
            Session["id_producto_retencion"] = null;
            Session["id_producto_pagos"] = null;
            Session["paga_iva_retencion"] = null;
            Session["paga_iva_pagos"] = null;
            Session["genera_tasa_usuario"] = null;
            Session["cantidad_manifiesto"] = null;
            Session["ejecuta_cobro_administrativo"] = null;

            //VARIABLES DE SESION PARA EL CIERRE DE CAJA
            Session["idCierreCaja"] = null;
            Session["fechaApertura"] = null;
            Session["horaApertura"] = null;
            Session["fechaCierre"] = null;
            Session["horaCierre"] = null;
            Session["estadoCierre"] = null;

            //VARIABLES DE SESION DE LAS TASAS DE USUARIO
            Session["tasaDevesofft"] = null;
            Session["tasaContifico"] = null;
            Session["tokenSMARTT"] = null;
            Session["idLocalidadSMARTT"] = null;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}