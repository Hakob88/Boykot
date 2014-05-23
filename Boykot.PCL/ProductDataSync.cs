using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Boykot.PCL.Models;

namespace Boykot.PCL
{
    public class ProductDataSync
    {
        public static readonly string BaseUrl = "http://www.salamicupcake.freevar.com/";
        public static readonly Uri BootstrapUri = new Uri(BaseUrl + "products.php");

        private HttpClient _downloader = new HttpClient();

        public ProductDataSync()
        {
        }

        public async Task<Models.Bootstrap.BootstrapInfo> GetBootstrapInfo()
        {
            try
            {
                string data = await _downloader.GetStringAsync(BootstrapUri);

                var json = new Helpers.JSONParser<Models.Bootstrap.BootstrapInfo>();
                var post = json.parse(data);

                return post;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetBootStrapInfo failed: " + ex.Message);
                return null;
            }
        }
    }
}
