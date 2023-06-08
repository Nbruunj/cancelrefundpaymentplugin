using Microsoft.Xrm.Sdk;
using System;
using System.Net.Http;
using System.ServiceModel;

namespace putpaymentdatacancel
{
    public class putpaymentdatacancel : IPlugin
    {
        public async void Execute(IServiceProvider serviceProvider)
        {

            // Obtain the tracing service
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            string email = (string)context.InputParameters["nal_Email_Name"]; //email på en given bruger
            string price = (string)context.InputParameters["nal_Price_Name"]; // prisen på det givend kursus
            string user = (string)context.InputParameters["nal_User_Name"]; // brugers name
            string kursususerid = (string)context.InputParameters["nal_Kursususerid_Name"]; // det id hans betaling har inde på dynmaic
            string paymentstatus = (string)context.InputParameters["nal_Paymentstatus_Name"]; // den status betaling har 
            string kundeurl = (string)context.InputParameters["nal_kundeurl_name"]; //kunden dynamic url 
            string kundesquickpayapikey = (string)context.InputParameters["nal_kundesquickpayapikey_name"]; // den quickpay api key




            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            try
            {

                string username = "john123";
                string password = "mypassword";
                int phoneNumber = 88887888;
                context.OutputParameters["nal_Response"] = "hej"; //await response.Content.ReadAsStringAsync();
                tracingService.Trace("hej");
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://feriehusudlejning.azurewebsites.net/api/user"); // skal laves om til den url der kommer når man får den public url for web apien 
                //var request = new HttpRequestMessage(HttpMethod.Post, "https://org22b50cb8.crm4.dynamics.com/api/data/v9.2/nal_PostPaymentData");
                request.Headers.Add("Cookie", "ARRAffinity=a544f2c7a222ce45b9597862ebbf6fc1d1b2ee338f4bf5d2d1aeb2c33eb0086d; ARRAffinitySameSite=a544f2c7a222ce45b9597862ebbf6fc1d1b2ee338f4bf5d2d1aeb2c33eb0086d");
                string json = "\r\n{\r\n    \"Name\": \"" + kundeurl + "\",\r\n    \"username\": \"" + kundesquickpayapikey + "\",\r\n    \"password\": \"" + password + "\",\r\n    \"phoneNumber\": " + phoneNumber + " \r\n\r\n}";

                //var request = new HttpRequestMessage(HttpMethod.Put, "https://feriehusudlejning.azurewebsites.net/api/user"); // skal laves om til den url der kommer når man får den public url for web apien 
                //string json = "\r\n{\r\n    \"orderid\": \"" + kursususerid + "\",\r\n    \"nal_Email_Name\": \"" + email + "\",\r\n    \"nal_Price_Name\": \"" + price + "\",\r\n    \"nal_User_Name\": \"" + user + "\",\r\n    \"nal_kundesquickpayapikey_name\": " + kundesquickpayapikey + "\",\r\n    \"nal_Paymentstatus_Name\": " + paymentstatus + " \r\n\r\n}";
                var content = new StringContent(json, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                context.OutputParameters["nal_Response"] = "hej"; //await response.Content.ReadAsStringAsync();
                tracingService.Trace("hej");

                response.EnsureSuccessStatusCode();
                //Console.WriteLine(await response.Content.ReadAsStringAsync());




            }
            catch (Exception ex)
            {
                tracingService.Trace("Create contact Exception: {0}", ex.ToString());
                throw;
            }
            try
            {
                // Plug-in business logic goes here.  
            }

            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
            }

            catch (Exception ex)
            {
                tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                throw;
            }
        }
    }
}
