using System;
using System.Configuration;

namespace ShoppingCart
{
    public class OAuthSection : ConfigurationSection
    {

        [ConfigurationProperty("Google", IsRequired = false)]
        public GoogleElement Google
        {
            get { return (GoogleElement)this["Google"]; }
            set { this["Google"] = value; }
        }

        public class GoogleElement : ConfigurationElement
        {
            [ConfigurationProperty("clientId", IsRequired = false)]
            public String ClientId
            {
                get
                {
                    return (String)this["clientId"];
                }
                set
                {
                    this["clientId"] = value;
                }
            }

            [ConfigurationProperty("clientSecret", IsRequired = false)]
            public String ClientSecret
            {
                get
                { return (String)this["clientSecret"]; }
                set
                { this["clientSecret"] = value; }
            }
        }

        [ConfigurationProperty("Facebook", IsRequired = false)]
        public FacebookElement Facebook
        {
            get { return (FacebookElement)this["Facebook"]; }
            set { this["Facebook"] = value; }
        }

        public class FacebookElement : ConfigurationElement
        {
            [ConfigurationProperty("appId", IsRequired = false)]
            public String AppId
            {
                get
                {
                    return (String)this["appId"];
                }
                set
                {
                    this["appId"] = value;
                }
            }

            [ConfigurationProperty("appSecret", IsRequired = false)]
            public String AppSecret
            {
                get
                { return (String)this["appSecret"]; }
                set
                { this["appSecret"] = value; }
            }
        }

        [ConfigurationProperty("Microsoft", IsRequired = false)]
        public MicrosoftElement Microsoft
        {
            get { return (MicrosoftElement)this["Microsoft"]; }
            set { this["Microsoft"] = value; }
        }

        public class MicrosoftElement : ConfigurationElement
        {
            [ConfigurationProperty("clientId", IsRequired = false)]
            public String ClientId
            {
                get
                {
                    return (String)this["clientId"];
                }
                set
                {
                    this["clientId"] = value;
                }
            }

            [ConfigurationProperty("clientSecret", IsRequired = false)]
            public String ClientSecret
            {
                get
                { return (String)this["clientSecret"]; }
                set
                { this["clientSecret"] = value; }
            }
        }

        [ConfigurationProperty("Twitter", IsRequired = false)]
        public TwitterElement Twitter
        {
            get { return (TwitterElement)this["Twitter"]; }
            set { this["Twitter"] = value; }
        }

        public class TwitterElement : ConfigurationElement
        {
            [ConfigurationProperty("consumerKey", IsRequired = false)]
            public String ConsumerKey
            {
                get
                {
                    return (String)this["consumerKey"];
                }
                set
                {
                    this["consumerKey"] = value;
                }
            }

            [ConfigurationProperty("consumerSecret", IsRequired = false)]
            public String ConsumerSecret
            {
                get
                { return (String)this["consumerSecret"]; }
                set
                { this["consumerSecret"] = value; }
            }
        }
    }
}