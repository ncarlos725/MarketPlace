using System.Security.Claims;

namespace MarketPlacedouh.Models
{
    public class Jtw
    {
        public string key { get; set; }
        public string Issuer { get; set; }
        public string Audiencie { get; set; }
        public string Subject { get; set; }

        public static dynamic validarToken(ClaimsIdentity identity)
        {
            try // como dectecto si el token es valido ? aqui la respuesta
            {
                if (identity == null || !identity.Claims.Any())
                {
                    return new
                    {
                        success = false,
                        message = "Verificar si estás enviando un token válido",
                        result = ""
                    };
                }


                // nesecito saber que usario le pertenece ese token que esta en el if que entro como parametro en la funcion estatica y dinamica validarToken
                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value ; // en el token especificamo un id 
                Usuario usuario = Usuario.DB().FirstOrDefault(x => x.idUsuario == id); // con esto ya seleccione el usuario que tenga ese Id   -- esto cambiar despues por Base de dato real
                return new
                {
                    succes = true,
                    message = "Exito2",
                    result = usuario
                };
            }
            catch (Exception ex) 
            {
                return new
                {
                    succes = "false",
                    message = "catch:" +ex.Message,
                    result = ""
                };
            }
        }

    }// fin public class JtW
} // nampespace
