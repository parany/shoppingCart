﻿using System.Configuration;
using System.Net.Configuration;

namespace ShoppingCart.CommonController.Infrastructure.Email
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
}