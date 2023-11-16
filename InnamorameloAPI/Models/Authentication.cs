﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InnamorameloAPI.Models
{
    public class Authentication
    {
        static private readonly string _secretKey = File.ReadAllText("C:\\Users\\marco\\source\\repos\\_MyCredentials\\SecretKeyAPI.txt"); // Chiave segreta per la firma del token (conservalo in modo sicuro)
        static private readonly string _issuer = "InnamorameloAPI"; // L'emettitore del token (ad esempio il nome dell'applicazione)

        internal string GenerateToken(string email, string password)
        {
            if (CheckIfUserExistsInDatabase(email, password, true))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, email),
                    new Claim(ClaimTypes.Name, email)
                    // Puoi aggiungere altre informazioni sull'utente come ruoli, autorizzazioni, ecc.
                };

                var token = new JwtSecurityToken(
                    _issuer,
                    _issuer,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(5), // Scadenza del token
                    signingCredentials: credentials
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.WriteToken(token);
            }
            else
            {
                Console.WriteLine("L'utente non è stato trovato nel database.");
                return "";
            }
        }

        internal bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Controlla la scadenza del token
                DateTime expirationDate = validatedToken.ValidTo;
                bool isTokenExpired = expirationDate < DateTime.UtcNow;

                if (isTokenExpired)
                {
                    Console.WriteLine("Il token è scaduto.");
                    return false;
                }

                // Esempio di come estrarre l'ID dell'utente dal token
                string email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                // Ora, puoi confrontare l'ID dell'utente estratto con i dati nel tuo database

                if (CheckIfUserExistsInDatabase(email, "", true))
                {
                    Console.WriteLine("L'utente è autenticato e presente nel database.");
                    return true;
                }
                else
                {
                    Console.WriteLine("L'utente non è stato trovato nel database.");
                    return false;
                }
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Errore nella validazione del token: {ex.Message}");
                return false;
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer, // Assicurati di utilizzare lo stesso issuer (emettitore) usato per generare il token
                ValidateAudience = false, // Puoi impostare a true se vuoi validare anche l'audience (destinatario) del token
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_secretKey)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true // Verifica la scadenza del token
            };
        }

        private bool CheckIfUserExistsInDatabase(string email, string password, bool onlyUser = false)
        {
            var user = new User();
            user.Email = email;
            user.Password = password;

            if (user.GetUser(user, onlyUser) != null)
                return true;
            else
                return false;
        }
    }
}