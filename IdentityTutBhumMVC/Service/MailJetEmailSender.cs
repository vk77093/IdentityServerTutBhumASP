using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace IdentityTutBhumMVC.Service
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        public MailJetOptions mailJetOptions;

        public MailJetEmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
            // this.mailJetOptions = mailJetOptions;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //       mailJetOptions=configuration.GetSection("MailJet").Get<MailJetOptions>();
            //       //MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("****************************1234"), Environment.GetEnvironmentVariable("****************************abcd"))
            //       //{
            //       //    //Version = ApiVersion.V3_1,
            //       //};
            //       MailjetClient client = new MailjetClient(mailJetOptions.ApiKey, mailJetOptions.SecretKey);
            //       MailjetRequest request = new MailjetRequest
            //       {
            //           Resource = Send.Resource,
            //       }
            //        .Property(Send.Messages, new JArray {
            //new JObject {
            // {
            //  "From",
            //  new JObject {
            //   {"Email", "vijaykumar@cosmicfinite.com"},
            //   {"Name", "Vijay"}
            //  }
            // }, {
            //  "To",
            //  new JArray {
            //   new JObject {
            //    {
            //     "Email",
            //     email
            //    }, {
            //     "Name",
            //     "Vijay"
            //    }
            //   }
            //  }
            // }, {
            //  "Subject",
            //  subject
            // },  {
            //  "HTMLPart",
            // htmlMessage
            // }, 
            // //   {
            // // "CustomID",
            // // "AppGettingStartedTest"
            // //}
            //}
            //        });
            //       // await client.PostAsync(request);
            //       MailjetResponse response = await client.PostAsync(request);
            //       if (response.IsSuccessStatusCode)
            //       {
            //           Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
            //           Console.WriteLine(response.GetData());
            //       }
            //       else
            //       {
            //           Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
            //           Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
            //           Console.WriteLine(response.GetData());
            //           Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            //       }
            //   }
            mailJetOptions = configuration.GetSection("MailJet").Get<MailJetOptions>();
            MailjetClient client = new MailjetClient(mailJetOptions.ApiKey, mailJetOptions.SecretKey);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.FromEmail, "vijaykumar@cosmicfinite.com")
                .Property(Send.FromName, "Your Portal Name")
                .Property(Send.Subject, subject)
                .Property(Send.HtmlPart, htmlMessage)
                .Property(Send.Recipients, new JArray {
                    new JObject {
                        {"Email", email}
                    }
                });

            MailjetResponse response = await client.PostAsync(request);
        }
    }
}

