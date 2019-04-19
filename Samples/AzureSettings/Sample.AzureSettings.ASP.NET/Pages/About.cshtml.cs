using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.KeyVault.Models;

namespace Sample.AzureSettings.ASP.NET.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGetAsync()
        {
            Message = "Your application description page.";
            //int retries = 0;
            //bool retry = false;
            //do
            {
                try
                {
                    Settings.LoadSettings();
                    Message = "KeyVault secrete is: " + Settings.Secret;
                }
                catch (KeyVaultErrorException keyVaultException)
                {
                    Message = keyVaultException.Message;
                    if ((int)keyVaultException.Response.StatusCode == 429)
                    {
                        //retry = true;
                    }
                }
                /* The below do while logic is to handle throttling errors thrown by Azure Key Vault. It shows how to do exponential backoff which is the recommended client side throttling*/
                //long waitTime = Math.Min(getWaitTime(retries), 2000000);
                //retry = false;
            }
            //while (retry && (retries++ < 10));
        }

        // This method implements exponential backoff incase of 429 errors from Azure Key Vault
        private static long getWaitTime(int retryCount)
        {
            long waitTime = ((long)Math.Pow(2, retryCount) * 100L);
            return waitTime;
        }
    }
}
