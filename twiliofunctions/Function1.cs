using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace twiliofunctions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string accountSid = Environment.GetEnvironmentVariable("ACCOUNTSID");
            string authToken = Environment.GetEnvironmentVariable("AUTHTOKEN");

            TwilioClient.Init(accountSid, authToken);

            var fromNumber = new PhoneNumber(Environment.GetEnvironmentVariable("FROMNUMBER"));
            var toNumber = new PhoneNumber(Environment.GetEnvironmentVariable("TONUMBER"));
            var call = CallResource.Create(
                toNumber,
                fromNumber,
                twiml: new Twiml("<Response><Play>https://demo.twilio.com/docs/classic.mp3</Play></Response>"));

            string responseMessage = call.Sid;

            return new OkObjectResult(new { message = responseMessage });
        }
    }
}
