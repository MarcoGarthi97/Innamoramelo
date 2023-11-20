using AutoMapper;
using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();

        [HttpGet("GetUser", Name = "GetUser")]
        public ActionResult<UserDTO> GetUser()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                        return Ok(userDTO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            

            return null;
        }

        [HttpPost("InsertUser", Name = "InsertUser")]
        public ActionResult<UserDTO> InsertUser(UserCreateViewModel user)
        {
            try
            {
                if (Validator.ValidateFields(user))
                {
                    var userAPI = new UserAPI();

                    var loginCredential = new LoginCredentials(user.Email);
                    if (!userAPI.CheckUser(loginCredential, true))
                    {
                        var result = userAPI.InsertUser(user);

                        return Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        [HttpPatch("UpdateUser", Name = "UpdateUser")]
        public ActionResult<UserDTO> UpdateUser(UserUpdateViewModel user)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var userAPI = new UserAPI();
                        var result = userAPI.UpdateUser(userDTO.Id, user);

                        if (result != null)
                        {
                            return Ok(result); // Operazione completata con successo, restituisci il risultato
                        }
                        else
                        {
                            return BadRequest(new ProblemDetails
                            {
                                Title = "Update failed",
                                Detail = "Impossibile aggiornare l'utente.",
                                Status = 400
                            });
                        }
                    }
                    else
                    {
                        return NotFound(new ProblemDetails
                        {
                            Title = "Unauthorized",
                            Detail = "Utente non autorizzato.",
                            Status = 404
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "Si è verificato un errore interno.",
                    Status = 500
                });
            }

            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Richiesta non valida.",
                Status = 400
            });
        }

        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        public ActionResult<bool> DeleteUser()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        //TO DO: Elimanare tutto ciò che è dell'utente
                        var secretCodeAPI = new SecretCodeAPI();
                        var result = secretCodeAPI.DeleteSecretCodeByUser(userDTO.Id);

                        var userAPI = new UserAPI();
                        result = userAPI.DeleteUser(userDTO.Id);

                        return Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
