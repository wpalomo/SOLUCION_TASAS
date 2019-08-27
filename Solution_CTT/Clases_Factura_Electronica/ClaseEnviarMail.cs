using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace Solution_CTT.Clases_Factura_Electronica
{
    class ClaseEnviarMail
    {
        //ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        //Clases.ClaseEnviarCorreo enviarMail = new Clases.ClaseEnviarCorreo();

        //VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        //VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();

        //string sSql;
        //DataTable dtConsulta;
        //bool bRespuesta;

        //string T_St_Server;
        //string T_St_From;
        //string T_St_FromName;
        //string T_St_To;
        //string T_St_ToName;
        //string T_St_Cc;
        //string T_St_CcName;
        //string T_St_Bcc;
        //string T_St_Subject;
        //string T_St_Msg;
        //string T_St_Attach;
        //string T_St_UserName;
        //string T_St_Password;
        //string T_St_temp;
        //string T_St_TipoComprobante;
        //string T_St_numComprobante;
        //string T_St_Cliente_Proveedor;


        ////VARIABLES PARA PODER ENVIAR AL CORREO ELECTRONICO

        //char[] delimitador_cc = { ',' };
        //char[] delimitador_adjunto = { '|' };

        //// Función para enviar un correo
        //public bool enviarCorreo(string host, int puerto, string remitente, 
        //                           string contraseña, string nombre, string destinatarios, 
        //                           string cc, string bcc, string asunto, string adjuntos, 
        //                           string cuerpo, int iEnableSSL)
        //{
        //    try
        //    {
        //        SmtpClient cliente = new SmtpClient(host, puerto);
        //        MailMessage correo = new MailMessage();

        //        correo.From = new MailAddress(remitente, nombre);
        //        correo.Body = cuerpo;
        //        correo.Subject = asunto;
        //        if (destinatarios == "") { }
        //        else
        //        {
        //            string[] cadena = destinatarios.Split(delimitador_cc);
        //            foreach (string word in cadena) correo.To.Add(word.Trim());
        //        }
        //        if (cc == "") { }
        //        else
        //        {
        //            string[] cadena1 = cc.Split(delimitador_cc);
        //            foreach (string word in cadena1) correo.CC.Add(word.Trim());
        //        }

        //        if (bcc == "") { }
        //        else
        //        {
        //            string[] cadena1 = bcc.Split(delimitador_cc);
        //            foreach (string word in cadena1) correo.Bcc.Add(word.Trim());
        //        }


        //        if (adjuntos == "") { }
        //        else
        //        {
        //            string[] cadena2 = adjuntos.Split(delimitador_adjunto);
        //            foreach (string word in cadena2) correo.Attachments.Add(new Attachment(word));
        //        }
        //        cliente.Credentials = new NetworkCredential(remitente, contraseña);

        //        if (iEnableSSL == 1)
        //            cliente.EnableSsl = true;

        //        cliente.Send(correo);

        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
