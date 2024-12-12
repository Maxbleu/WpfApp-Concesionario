using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WpfApp_Concesionario.Models;

namespace WpfApp_Concesionario.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly CocheService _cocheService;
        private readonly IConfiguration _configuration;

        private readonly string _urlApiAuth;
        private string JwtToken { get; set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(this.JwtToken) ? true : false; 

        public AuthService(CocheService cocheService, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _cocheService = cocheService;
            _configuration = configuration;
            _urlApiAuth = $"{_configuration["ApiBaseUrl"]}{_configuration["ApiAuth"]}";
        }

        /// <summary>
        /// Realiza la petición de login al servidor y si las 
        /// credenciales son correctas guarda el token JWT
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(UserModel user)
        {
            //  Serializamos el objeto UserModel a JSON
            string objectSerialized = JsonSerializer.Serialize(user);

            //  Creamos el contenido de la petición
            HttpContent content = new StringContent(objectSerialized, Encoding.UTF8, "application/json");

            try
            {
                //  Realizamos la petición POST
                HttpResponseMessage httpResponse = await this._httpClient.PostAsync(this._urlApiAuth, content);

                //  Comprobamos que la petición es correcta
                if (httpResponse.IsSuccessStatusCode)
                {
                    //  Guardamos el token JWT
                    this.JwtToken = await httpResponse.Content.ReadAsStringAsync();
                    this._cocheService.LoadJwtToken(this.JwtToken);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError de conexión: {ex.Message}");
            }

            return false;
        }
    }
}
