using MarketPlacedouh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MarketPlacedouh.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        public IConfiguration _configuration;
        
        public UsuarioController(IConfiguration configuration) {
            _configuration = configuration;
        } // constructor

        [HttpPost]
        [Route("login")] // ---------------------> LOGIN

        // Iniciar sesion
        public dynamic IniciarSesion([FromBody] Object optData) // LOS DATOS QUE VA A MANDAR EL USUARIO VA SER TIPO OJECT
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString()); // ES DINAMICO POR QUE NO SABEMOS QUE TIPO NOS MANDAN

            string user = data.usuario.ToString();  
            string password = data.password.ToString();

            Usuario usuario = Usuario.DB().Where(x => x.usuario == user && x.password == password).FirstOrDefault();

            if (usuario == null)
            {
                return new
                {
                    success = true,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            } // FIN IF NULL ? 
            var jwt = _configuration.GetSection("Jwt").Get<Jtw>(); // estamos optieniendo los datos del jwt del appsetingjson

            var claims = new[]  //  vamos encapsular todo lo que almacena mi token
            {
              new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
              new Claim("id",usuario.idUsuario),
              new Claim("usuario",usuario.usuario),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key)); //encriptacion de key
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // vamos a crear un inicio de secion
            var token = new JwtSecurityToken( // crear nuestro token
                    jwt.Issuer, // PROVEDOR DE LAS API
                    jwt.Audiencie, // AUDIENCIA
                    claims,
                    expires: DateTime.Now.AddMinutes(4), // tiempo de expiracion para generar un nuevo token ejemplo de aqui dura 4 min
                    signingCredentials: singIn
                 );

            return new // retornamos un nuevo objeto
            {
                succes = true,   
                message = "exito",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        } // FIN DEL INICIAR SECCION 
    }
}
