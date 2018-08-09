using System;
using System.Diagnostics;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace GM.Twilio.AspNet.Core
{
    public static class GmTwilioClient
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static TwilioCofig _config;

        public static void Init(string twilioClaim, string csr)
        {
            _config = Decrypt(twilioClaim, csr);
            TwilioClient.Init(_config.TwilioAccountSid, _config.TwilioAuthToken);
        }

        public static string Send(SmsMsg msg)
        {
            if(_config==null) throw new Exception("Not initialized, please execute GmTwilioClient.Init().");
            var to = new PhoneNumber(msg.To);
            var from = new PhoneNumber(_config.TwilioPhone);
            var message = MessageResource.Create(to: to, from: from, body: msg.Msg);

            return message.Sid;
        }

        private static TwilioCofig Decrypt(string twilioClaim, string privateKey)
        {
            if (string.IsNullOrEmpty(twilioClaim)) throw new ArgumentException(nameof(twilioClaim));
            if (string.IsNullOrEmpty(privateKey)) throw new ArgumentException(nameof(privateKey));

            using (var rsa = new RsaHelper(Encoding.UTF8, privateKey))
            {
                var decrypted = rsa.Decrypt(twilioClaim);
                var arr = decrypted.Split("|".ToCharArray());
                return new TwilioCofig()
                {
                    TwilioAccountSid = arr[0],
                    TwilioAuthToken = arr[1],
                    TwilioPhone = arr[2]
                };
            }
        }

        //private static string Encrypt(TwilioCofig config, string publicKey)
        //{
        //    if (config == null) throw new ArgumentException(nameof(config));
        //    if (string.IsNullOrEmpty(publicKey)) throw new ArgumentException(nameof(publicKey));

        //    using (var rsa = new RsaHelper(Encoding.UTF8, null, publicKey))
        //    {
        //        return rsa.Encrypt($"{config.TwilioAccountSid}|{config.TwilioAuthToken}|{config.TwilioPhone}");
        //    }
        //}

        private class TwilioCofig
        {
            public string TwilioAccountSid { get; set; }
            public string TwilioAuthToken { get; set; }
            public string TwilioPhone { get; set; }
        }
    }
}
