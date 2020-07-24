using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MenuItemListing;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace OrderItem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public static HttpClient client = new HttpClient();
        [HttpPost("{menuitemid}")]
        public Cart POST(int menuitemid)
        {
            string token = GetToken("http://52.143.250.249/api/Token");

            Cart orderItem = new Cart();
            orderItem.Id = 1;
            orderItem.UserId = 1;
            orderItem.menuItemId = menuitemid;
            orderItem.menuItemName = getname(menuitemid,token).ToString(); 
            return orderItem;
            
            
            


        }
        static string GetToken(string url)
        {
            var user = new User { Name="Jignesh",Password="Jignesh123"};
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
               

                var response = client.PostAsync(url, data).Result;
                string name = response.Content.ReadAsStringAsync().Result;
                dynamic details = JObject.Parse(name);
                return details.token;
            }
        }

        private string getname(int id,string token)
        {
            string name;
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri("http://52.143.250.249/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = new HttpResponseMessage();
                
                response = client.GetAsync("api/MenuItem/"+id).Result;


                if (response.IsSuccessStatusCode)
                {
                    string name1 = response.Content.ReadAsStringAsync().Result;
                    name = JsonConvert.DeserializeObject<string>(name1);
                }
                else
                    name = null;

            }
            return name;
        }


    }
}
