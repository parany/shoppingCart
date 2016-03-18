using System;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Infrastructure.Abstract;
using ShoppingCart.ViewModels;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.Infrastructure.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "shoppingcart@yopmail.com";
        public string MailFromAddress = "";
        public bool UseSsl = true;
        public string Username = "";
        public string Password = "";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"C:\shoppingcart\emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
            
        }

        public void ProcessOrder(Cart cart, ShippingDetail shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(
                    emailSettings.Username,
                    emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Items:");

                foreach (var line in cart.CartLines)
                {
                    //var subtotal = line.Product.Price * line.Quantity;
                    //body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity, line.Product.Name, subtotal);
                    body.AppendLine("Product Test 1: 1 x 2 = 2");
                }

                body//.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.UserId)
                    .AppendLine("---");
                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, 
                    emailSettings.MailToAddress,
                    "New order submitted!", 
                    body.ToString()
                );

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}