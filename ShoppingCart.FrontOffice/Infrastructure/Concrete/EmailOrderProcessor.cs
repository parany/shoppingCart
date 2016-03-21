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
using ShoppingCart.Models.Models.User;
using System.ComponentModel;
using System.Net.Configuration;
using System.Configuration;

namespace ShoppingCart.Infrastructure.Concrete
{
    public class EmailSettings
    {
        public static SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        public string MailToAddress = "shoppingcart@yopmail.com";
        public string MailFromAddress = section.From;
        public bool UseSsl = section.Network.EnableSsl;
        public string Username = section.Network.UserName;
        public string Password = section.Network.Password;
        public string ServerName = section.Network.Host;
        public int ServerPort = section.Network.Port;
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

        public bool ProcessOrder(Cart cart, ShippingDetail shippingInfo, ApplicationUser user, IGenericRepository<Product> productRepo)
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
                    .AppendFormat("Hello {0},\n\n", shippingInfo.Name)
                    .AppendLine("You have submitted a new order from the shopping store web site.")
                    .AppendLine("Please see below the details of this order.")
                    .AppendLine("")
                    .AppendLine("Items ordered:");

                foreach (var line in cart.CartLines)
                {
                    Product p = productRepo.GetSingle(x=>x.Id == line.ProductId);
                    var subtotal = p.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} => subtotal: {2} Euros\n", line.Quantity, p.Name, subtotal);
                }

                body.AppendFormat("\nTotal order value: {0} Euros\n", ComputeTotalValue(cart, productRepo))
                    .AppendLine("--------------------------")
                    .AppendLine("")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Address)
                    .AppendLine(shippingInfo.PhoneNumber)
                    .AppendLine("--------------------------")
                    .AppendLine("")
                    .AppendLine("Thank you for your order.");

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    user.Email ?? emailSettings.MailToAddress,
                    "New order submitted!",
                    body.ToString()
                );

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                bool result= true;
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception e)
                {
                    if(e != null)
                    {
                        result = false;
                    }
                }
                return result;  
            }            
        }

        public decimal ComputeTotalValue(Cart cart, IGenericRepository<Product> productRepo)
        {
            List<CartLine> list = new List<CartLine>();
            foreach(var l in cart.CartLines)
            {
                Product p = productRepo.GetSingle(x => x.Id == l.ProductId);
                l.Product = p;
                list.Add(l);
            }
            return list.Sum(e => e.Product.Price * e.Quantity);
        }
    }
}