using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Web.UI.WebControls;
using System.IO;
//
using System.Web.UI;

namespace Solution_CTT.Clases
{
    public class ClaseEnviarCorreo
    {
        MailMessage msj = new MailMessage();
        SmtpClient smtp = new SmtpClient();

        char[] delimitador_cc = { ',' };
        char[] delimitador_adjunto = { '|' };

        public bool SendMail(string from, string password, string smtp_account, int port, string to, string subject, string message, ListBox listAtt, string tempPath)
        {
            //ACTIVAR ACCESO MENOS SEGURO DE GMAIL
            //https://myaccount.google.com/lesssecureapps?utm_source=google-account&utm_medium=web

            try
            {
                msj.From = new MailAddress(from);
                msj.To.Add(new MailAddress(to));
                msj.Subject = subject;
                msj.SubjectEncoding = System.Text.Encoding.UTF8;
                msj.Body = message;
                msj.BodyEncoding = System.Text.Encoding.UTF8;
                msj.IsBodyHtml = true;

                foreach (ListItem l in listAtt.Items)
                {
                    //MemoryStream ms = new MemoryStream();
                    //ms = HttpContext.Current.Server.MapPath(Path.Combine(tempPath, l.Value));

                    string fichero = HttpContext.Current.Server.MapPath(Path.Combine(tempPath, l.Value));//ACCEDER AL PATH MEDIANTE HTTPCONTEXT
                    msj.Attachments.Add(new System.Net.Mail.Attachment(fichero));

                }                                

                //CREDENCIALES
                smtp.Host = smtp_account;
                smtp.Port = port;
                smtp.Credentials = new NetworkCredential(from, password);
                smtp.EnableSsl = true; 

                smtp.Send(msj);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // Función para enviar un correo
        public string[] enviarCorreo(string host, int puerto, string remitente, string contraseña, string nombre, string destinatarios, string cc, string bcc, string asunto, string adjuntos, string cuerpo, int iEnableSSL)
        {
            try
            {
                SmtpClient cliente = new SmtpClient(host, puerto);
                MailMessage correo = new MailMessage();

                correo.From = new MailAddress(remitente, nombre);
                correo.Body = cuerpo;
                correo.Subject = asunto;
                if (destinatarios == "") { }
                else
                {
                    string[] cadena = destinatarios.Split(delimitador_cc);
                    foreach (string word in cadena) correo.To.Add(word.Trim());
                }
                if (cc == "") { }
                else
                {
                    string[] cadena1 = cc.Split(delimitador_cc);
                    foreach (string word in cadena1) correo.CC.Add(word.Trim());
                }

                if (bcc == "") { }
                else
                {
                    string[] cadena1 = bcc.Split(delimitador_cc);
                    foreach (string word in cadena1) correo.Bcc.Add(word.Trim());
                }


                if (adjuntos == "") { }
                else
                {
                    string[] cadena2 = adjuntos.Split(delimitador_adjunto);
                    foreach (string word in cadena2) correo.Attachments.Add(new Attachment(word));
                }
                cliente.Credentials = new NetworkCredential(remitente, contraseña);

                if (iEnableSSL == 1)
                    cliente.EnableSsl = true;

                cliente.Send(correo);

                string[] datos = new string[2] { "1", "El correo se ha enviado correctamente" };
                return datos;

            }
            catch (Exception ex)
            {
                string[] datos = new string[2] { "0", ex.Message };
                return datos;
            }
        }
    }
}