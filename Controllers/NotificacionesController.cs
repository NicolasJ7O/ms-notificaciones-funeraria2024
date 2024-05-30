using Microsoft.AspNetCore.Mvc;
using ms_notificaciones.Models;
/* using SendGrid;
using SendGrid.Helpers.Mail; */
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;


namespace ms_notificaciones.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificacionesController : ControllerBase
{

    /* [Route("correo-bienvenida")]
    [HttpPost]
    public async Task<ActionResult> EnviarCorreoBienvenida(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.CrearMensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("WELCOME_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new
        {
            name = datos.nombreDestino,
            message = "Bienvenido a la comunidad de la inmobiliaria."
        });
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a la dirección " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
        }
    } */


    /* [Route("correo-recuperacion-clave")]
    [HttpPost]
    public async Task<ActionResult> EnviarCorreoRecuperacionClave(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.CrearMensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("WELCOME_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new
        {
            name = datos.nombreDestino,
            message = "Esta es su nuevla clave... no la comparta."
        });
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a la dirección " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
        }
    } */

/* [Route("enviar-correoPqr")]
    [HttpPost]
    public async Task<ActionResult> EnviarCorreoPQR(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.CrearMensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("WELCOME_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new
        {
            name = datos.nombreDestino,
            message = "Esta es su nuevla clave... no la comparta."
        });
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a la dirección " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
        }
    }
 */


    /* [Route("enviar-correo-2fa")]
    [HttpPost]
    public async Task<ActionResult> EnviarCorreo2fa(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.CrearMensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("TwoFA_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new
        {
            nombre = datos.nombreDestino,
            mensaje = datos.contenidoCorreo,
            asunto = datos.asuntoCorreo
        });
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a la dirección " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
        }
    }


    private SendGridMessage CrearMensajeBase(ModeloCorreo datos)
    {
        var from = new EmailAddress(Environment.GetEnvironmentVariable("EMAIL_FROM"), Environment.GetEnvironmentVariable("NAME_FROM"));
        var subject = datos.asuntoCorreo;
        var to = new EmailAddress(datos.correoDestino, datos.nombreDestino);
        var plainTextContent = datos.contenidoCorreo;
        var htmlContent = datos.contenidoCorreo;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        return msg;
    } */

    // Envío de SMS

    [Route("correo-recuperacion-clave")]
        [HttpPost]
        public async Task<ActionResult> EnviarCorreoRecuperacionClave(ModeloCorreo datos)
        {
            // Cargar configuración de AWS
            var awsAccessKeyId = Environment.GetEnvironmentVariable("ACCESS_KEY_AWSSES");
            var awsSecretAccessKey = Environment.GetEnvironmentVariable("SECRET_KEY_AWSSES");

            using (var client = new AmazonSimpleEmailServiceClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USEast2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = "nicolas.1701812174@ucaldas.edu.co", // Reemplaza con tu correo verificado en SES
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { datos.correoDestino }
                    },
                    Message = new Message
                    {
                        Subject = new Content(datos.asuntoCorreo ?? "Recuperación de Clave"),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = datos.contenidoCorreo ?? $"<p>Hola {datos.nombreDestino},</p><p>Esta es su nueva clave. No la comparta con nadie.</p>"
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = datos.contenidoCorreo ?? $"Hola {datos.nombreDestino},\nEsta es su nueva clave. No la comparta con nadie."
                            }
                        }
                    }
                };

                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return Ok("Correo enviado a la dirección " + datos.correoDestino);
                    }
                    else
                    {
                        return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest("Exception caught sending email: " + ex.Message);
                }
            }
        }

 [Route("enviar-correo-2fa")]
        [HttpPost]
        public async Task<ActionResult> EnviarCorreo2fa(ModeloCorreo datos)
        {
            // Cargar configuración de AWS
            var awsAccessKeyId = Environment.GetEnvironmentVariable("ACCESS_KEY_AWSSES");
            var awsSecretAccessKey = Environment.GetEnvironmentVariable("SECRET_KEY_AWSSES");

            using (var client = new AmazonSimpleEmailServiceClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USEast2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = "nicolas.1701812174@ucaldas.edu.co", // Reemplaza con tu correo verificado en SES
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { datos.correoDestino }
                    },
                    Message = new Message
                    {
                        Subject = new Content(datos.asuntoCorreo ?? "Autenticación de Dos Factores"),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = datos.contenidoCorreo ?? $"<p>Hola {datos.nombreDestino},</p><p>Su código de autenticación de dos factores es: {datos.contenidoCorreo}</p>"
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = datos.contenidoCorreo ?? $"Hola {datos.nombreDestino},\nSu código de autenticación de dos factores es: {datos.contenidoCorreo}"
                            }
                        }
                    }
                };

                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return Ok("Correo enviado a la dirección " + datos.correoDestino);
                    }
                    else
                    {
                        return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest("Exception caught sending email: " + ex.Message);
                }
            }
        }


    [Route("enviar-sms")]
    [HttpPost]
    public async Task<ActionResult> EnviarSMSNuevaClave(ModeloSms datos)
    {
        var accessKey = Environment.GetEnvironmentVariable("ACCESS_KEY_AWSSNS");
        var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY_AWSSNS");
        var client = new AmazonSimpleNotificationServiceClient(accessKey, secretKey, RegionEndpoint.USEast2);
        var messageAttributes = new Dictionary<string, MessageAttributeValue>();
        var smsType = new MessageAttributeValue
        {
            DataType = "String",
            StringValue = "Transactional"
        };

        messageAttributes.Add("AWS.SNS.SMS.SMSType", smsType);

        PublishRequest request = new PublishRequest
        {
            Message = datos.contenidoMensaje,
            PhoneNumber = datos.numeroDestino,
            MessageAttributes = messageAttributes
        };
        try
        {
            await client.PublishAsync(request);
            return Ok("Mensaje enviado");
        }
        catch
        {
            return BadRequest("Error enviando el sms");
        }
    }


 [Route("enviar-correoPqr")]
        [HttpPost]
        public async Task<ActionResult> EnviarCorreoPQR(ModeloCorreo datos)
        {
            // Cargar configuración de AWS
            var awsAccessKeyId = Environment.GetEnvironmentVariable("ACCESS_KEY_AWSSES");
            var awsSecretAccessKey = Environment.GetEnvironmentVariable("SECRET_KEY_AWSSES");

            using (var client = new AmazonSimpleEmailServiceClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USEast2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = "nicolas.1701812174@ucaldas.edu.co", // Reemplaza con tu correo verificado en SES
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { datos.correoDestino }
                    },
                    Message = new Message
                    {
                        Subject = new Content(datos.asuntoCorreo ?? "Bienvenido a la comunidad de la funeraria"),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = datos.contenidoCorreo ?? $"<h1>Buen día, {datos.nombreDestino}</h1><p>Bienvenido a la comunidad de la funeraria.</p>"
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = datos.contenidoCorreo ?? $"Bienvenido, {datos.nombreDestino}Bienvenido a la comunidad de la funeraria."
                            }
                        }
                    }
                };

                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return Ok("Correo enviado a la dirección " + datos.correoDestino);
                    }
                    else
                    {
                        return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest("Exception caught sending email: " + ex.Message);
                }
            }
        }

}