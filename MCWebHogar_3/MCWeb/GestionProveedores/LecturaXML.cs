using MCWebHogar.GestionProveedores;
using OpenPop.Mime;
using OpenPop.Mime.Decode;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Xml;

namespace MCWebHogar.ControlPedidos.Proveedores
{
    public class LecturaXML
    {
        protected void TransferFiles()
        {
            try
            {
                string directoryName = HttpContext.Current.Server.MapPath("~");
                var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string[] files = Directory.GetFiles(Path.Combine(dirPath, @"Facturas"));
                foreach (var file in files)
                {
                    if (Path.GetExtension(file) == ".xml")
                    {
                        string fileName = Path.GetFileName(file);
                        using (var client = new WebClient())
                        {
                            client.Credentials = new NetworkCredential("mikfepz-001", "desiree15");
                            client.UploadFile(Path.Combine("ftp://win5091.site4now.net/mikfe/Facturas/", fileName), WebRequestMethods.Ftp.UploadFile, file);
                        }
                        File.Move(file, Path.Combine(directoryName, @"Facturas", fileName));
                    }
                    else
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception e)
            {

            }                
        }

        public void ReadEmails()
        {
            try
            {
                EncodingFinder.AddMapping("text/plain", Encoding.UTF8);                
            }
            catch (Exception e)
            {

            }
            
            int i = 0;
            try
            {
                Pop3Client pop3Client;

                pop3Client = new Pop3Client();
                pop3Client.Connect("pop.gmail.com", 995, true);
                pop3Client.Authenticate("recent:facturas.mikfe@gmail.com", "facturas2022");
                // pop3Client.Authenticate("recent:facturacion.mikfecr@gmail.com", "facturas2022");                
                
                int count = pop3Client.GetMessageCount();
                for (i = 1; i <= count; i++)
                {
                    Message message = pop3Client.GetMessage(i);
                    
                    Email mail = new Email();
                    mail.EmailFrom = message.Headers.From.ToString();
                    mail.Subject = message.Headers.Subject;
                    try
                    {
                        mail.FechaMail = Convert.ToDateTime(message.Headers.Date);
                    }
                    catch (Exception ex)
                    {
                        mail.FechaMail = Convert.ToDateTime("1900-01-01");
                    }
                    
                    mail.MessageID = message.Headers.MessageId;
                    int mailID = mail.GuardarEmail();

                    List<MessagePart> attachmentList = message.FindAllAttachments();
                    if (attachmentList.Count > 0)
                    {
                        if (mailID > 0)
                        {
                            for (int j = 0; j < attachmentList.Count; j++)
                            {
                                string FileName = attachmentList[j].FileName.Trim();
                                Attachment attachment = new Attachment();
                                attachment.MailID = mailID;
                                attachment.FileName = FileName;

                                try
                                {
                                    if (attachmentList[j] != null)
                                    {
                                        byte[] content = attachmentList[j].Body;

                                        // Save file to server path  
                                        string[] stringParts = FileName.Split(new char[] { '.' });
                                        string strType = stringParts[1];

                                        if (strType == "xml")
                                        {
                                            string directoryName = HttpContext.Current.Server.MapPath("~");
                                            File.WriteAllBytes(Path.Combine(directoryName, @"Facturas", FileName), attachmentList[j].Body);
                                            attachment.GuardarAttachment();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {                            
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void ReadXML()
        {
            // TransferFiles();
            try
            {
                string directoryName = HttpContext.Current.Server.MapPath("~");
                
                string firstTag = "";
                string secondTag = "";
                string thirdTag = "";
                string fourthTag = "";
                string claveFactura = "";
                string codigoEmisor = "";
                string identificacionReceptor = "";
                DateTime fechaFactura = Convert.ToDateTime("1900-01-01");

                // string[] files = Directory.GetFiles(Path.Combine(directoryName, @"Facturas"));
                DirectoryInfo info = new DirectoryInfo(Path.Combine(directoryName, @"Facturas"));
                FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                foreach (var file in files)
                {
                    Factura factura = new Factura();
                    Emisor emisor = new Emisor();
                    LineaDetalle linea = new LineaDetalle();

                    #region Procesar
                    if (Path.GetExtension(file.Name) == ".xml")
                    {
                        using (XmlReader reader = XmlReader.Create(file.FullName))
                        {
                            firstTag = "";
                            secondTag = "";
                            thirdTag = "";
                            fourthTag = "";
                            claveFactura = "";
                            codigoEmisor = "";
                            identificacionReceptor = "";
                            fechaFactura = Convert.ToDateTime("1900-01-01");

                            bool stop = false;

                            while (!stop && reader.Read())
                            {
                                if (reader.IsStartElement())
                                {
                                    if (firstTag == "")
                                        firstTag = reader.Name.ToString();
                                    else if (secondTag == "")
                                        secondTag = reader.Name.ToString();
                                    else if (thirdTag == "")
                                        thirdTag = reader.Name.ToString();
                                    else
                                        fourthTag = reader.Name.ToString();
                                    switch (reader.Name.ToString())
                                    {
                                        #region Encabezado
                                        case "MensajeHacienda":
                                            stop = true;
                                            break;
                                        case "MensajeReceptor":
                                            stop = true;
                                            break;
                                        case "NotaDebitoElectronica":
                                            stop = true;
                                            break;
                                        case "NotaCreditoElectronica":
                                            stop = true;
                                            break;
                                        case "FacturaElectronica":
                                            firstTag = "";
                                            break;
                                        #endregion

                                        #region Factura
                                        case "Clave":
                                            factura.claveFactura = reader.ReadString();
                                            claveFactura = factura.claveFactura;
                                            firstTag = "";
                                            break;
                                        case "NumeroConsecutivo":
                                            factura.numeroConsecuetivoFactura = reader.ReadString();
                                            firstTag = "";
                                            break;
                                        case "FechaEmision":
                                            factura.fechaFactura = Convert.ToDateTime(reader.ReadString());
                                            fechaFactura = factura.fechaFactura;
                                            firstTag = "";
                                            break;
                                        case "TotalGravado":
                                            factura.totalGravado = Convert.ToDecimal(reader.ReadString());
                                            secondTag = "";
                                            break;
                                        case "TotalExento":
                                            factura.totalExento = Convert.ToDecimal(reader.ReadString());
                                            secondTag = "";
                                            break;
                                        case "TotalExonerado":
                                            factura.totalExonerado = Convert.ToDecimal(reader.ReadString());
                                            secondTag = "";
                                            break;
                                        case "TotalVenta":
                                            factura.totalVenta = Convert.ToDecimal(reader.ReadString());
                                            secondTag = "";
                                            break;
                                        case "TotalDescuentos":
                                            factura.totalDescuento = Convert.ToDecimal(reader.ReadString());
                                            secondTag = "";
                                            break;
                                        case "TotalVentaNeta":
                                            factura.totalVentaNeta = Convert.ToDecimal(reader.ReadString());
                                            secondTag = "";
                                            break;
                                        case "TotalImpuesto":
                                            factura.totalImpuesto = Convert.ToDecimal(reader.ReadString());
                                            secondTag = "";
                                            break;
                                        case "TotalComprobante":
                                            factura.totalComprobante = Convert.ToDecimal(reader.ReadString());
                                            secondTag = "";
                                            break;
                                        #endregion

                                        #region Emisor
                                        case "Nombre":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.nombre = reader.ReadString();
                                                secondTag = "";
                                            }
                                            if (firstTag == "Receptor")
                                            {
                                                emisor.nombreComercialReceptor = reader.ReadString();
                                                secondTag = "";
                                            }
                                            break;
                                        case "Tipo":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.tipoIdentificacion = reader.ReadString();
                                                thirdTag = "";
                                            }
                                            break;
                                        case "Numero":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.numeroIdentificacion = reader.ReadString();
                                                codigoEmisor = emisor.numeroIdentificacion;
                                                thirdTag = "";
                                            }
                                            else if (firstTag == "Receptor" && secondTag == "Identificacion")
                                            {
                                                emisor.numeroIdentificacionReceptor = reader.ReadString();
                                                identificacionReceptor = emisor.numeroIdentificacionReceptor;
                                                stop = emisor.numeroIdentificacionReceptor != "3101485961" && emisor.numeroIdentificacionReceptor != "115210651";
                                                thirdTag = "";
                                            }
                                            break;
                                        case "NombreComercial":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.nombreComercial = reader.ReadString();
                                                secondTag = "";
                                            }
                                            if (firstTag == "Receptor")
                                            {
                                                emisor.nombreComercialReceptor = reader.ReadString();
                                                secondTag = "";
                                            }
                                            break;
                                        case "Provincia":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.provincia = reader.ReadString();
                                                thirdTag = "";
                                            }
                                            break;
                                        case "Canton":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.canton = reader.ReadString();
                                                thirdTag = "";
                                            }
                                            break;
                                        case "Distrito":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.distrito = reader.ReadString();
                                                thirdTag = "";
                                            }
                                            break;
                                        case "Barrio":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.barrio = reader.ReadString();
                                                thirdTag = "";
                                            }
                                            break;
                                        case "OtrasSenas":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.otrasSenas = reader.ReadString();
                                                thirdTag = "";
                                            }
                                            break;
                                        case "NumTelefono":
                                            if (firstTag == "Emisor" && secondTag == "Telefono")
                                            {
                                                emisor.telefono = reader.ReadString();
                                                thirdTag = "";
                                            }
                                            break;
                                        case "CorreoElectronico":
                                            if (firstTag == "Emisor")
                                            {
                                                emisor.correoEmisor = reader.ReadString();
                                                secondTag = "";
                                            }
                                            break;
                                        #endregion

                                        #region LineaDetalle
                                        case "NumeroLinea":
                                            linea.numeroLinea = Convert.ToInt32(reader.ReadString());
                                            thirdTag = "";
                                            break;
                                        case "Codigo":
                                            if (thirdTag == "Codigo" && fourthTag == "")
                                            {
                                                linea.codigoProducto = reader.ReadString();
                                                thirdTag = "";
                                            }
                                            break;
                                        case "Cantidad":
                                            linea.cantidad = Convert.ToDecimal(reader.ReadString());
                                            thirdTag = "";
                                            break;
                                        case "UnidadMedida":
                                            linea.unidadMedida = reader.ReadString();
                                            thirdTag = "";
                                            break;
                                        case "UnidadMedidaComercial":
                                            linea.unidadMedida = reader.ReadString();
                                            thirdTag = "";
                                            break;
                                        case "Detalle":
                                            linea.detalleProducto = reader.ReadString();
                                            thirdTag = "";
                                            break;
                                        case "PrecioUnitario":
                                            linea.precioUnitario = Convert.ToDecimal(reader.ReadString());
                                            thirdTag = "";
                                            break;
                                        case "MontoTotal":
                                            linea.montoTotal = Convert.ToDecimal(reader.ReadString());
                                            thirdTag = "";
                                            break;
                                        case "MontoDescuento":
                                            linea.montoDescuento += Convert.ToDecimal(reader.ReadString());
                                            fourthTag = "";
                                            break;
                                        case "SubTotal":
                                            linea.subTotal = Convert.ToDecimal(reader.ReadString());
                                            thirdTag = "";
                                            break;
                                        case "Tarifa":
                                            linea.porcentajeImpuesto = Convert.ToDecimal(reader.ReadString());
                                            fourthTag = "";
                                            break;
                                        case "Monto":
                                            linea.montoImpuesto = Convert.ToDecimal(reader.ReadString());
                                            fourthTag = "";
                                            break;
                                        case "MontoTotalLinea":
                                            linea.montoTotalIVA = Convert.ToDecimal(reader.ReadString());
                                            thirdTag = "";
                                            break;
                                        #endregion
                                    }
                                }
                                else if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    if (firstTag == reader.Name.ToString())
                                        firstTag = "";
                                    else if (secondTag == reader.Name.ToString())
                                        secondTag = "";
                                    else if (thirdTag == reader.Name.ToString())
                                        thirdTag = "";
                                    else
                                        fourthTag = "";
                                    switch (reader.Name.ToString())
                                    {
                                        case "Receptor":
                                            emisor.GuardarEmisor();
                                            emisor = new Emisor();
                                            break;
                                        case "ResumenFactura":
                                            factura.identificacionEmisor = codigoEmisor;
                                            factura.identificacionReceptor = identificacionReceptor;
                                            factura.GuardarFactura();
                                            factura = new Factura();
                                            stop = true;
                                            break;
                                        case "LineaDetalle":
                                            linea.fechaFactura = fechaFactura;
                                            linea.claveFactura = claveFactura;
                                            linea.identificacionEmisor = codigoEmisor;
                                            linea.identificacionReceptor = identificacionReceptor;
                                            linea.GuardarLineaDetalle();
                                            linea = new LineaDetalle();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    File.Delete(file.FullName);
                    #endregion
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}