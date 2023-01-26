using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FreshFarmMarket_211283E.Services
{
    public class MessageService
    {
        private readonly SMSoptions _smsoptions;
        public MessageService(IOptions<SMSoptions> SMSoptions)
        {
            _smsoptions = SMSoptions.Value;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            var accountSid = _smsoptions.SMSAccountIdentification;
            // Your Auth Token from twilio.com/console
            var authToken = _smsoptions.SMSAccountPassword;

            TwilioClient.Init(accountSid, authToken);

            return MessageResource.CreateAsync(
              to: new PhoneNumber(number),
              from: new PhoneNumber(_smsoptions.SMSAccountFrom),
              body: message);
        }
    }
}
