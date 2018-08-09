namespace GM.Twilio.AspNet.Core
{
    public class SmsMsg
    {
        /// <summary>
        /// To PhoneNumber
        /// </summary>
        /// <example>+886123456789</example>
        public string To { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Msg { get; set; }
    }
}