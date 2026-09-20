using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PDMSRestServices.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using PDMSRestServices.Facade;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Principal;
using MAXIMUS.Core.Libraries;


namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private IConfiguration iconfiguration;
        private static Dictionary<string, string> _refreshTokens = new Dictionary<string, string>();

        public LoginController(ILogger<LoginController> logger, IConfiguration iconfiguration)
        {
            _logger = logger;
            this.iconfiguration = iconfiguration;
        }

        private JwtSecurityToken GenerateAccessToken(string userName)
        {
            int restTokenExp = Convert.ToInt32(iconfiguration.GetValue<string>("RESTTokenExp"));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
            };
            var token = new JwtSecurityToken(
                issuer: iconfiguration["JwtSettings:Issuer"],
                audience: iconfiguration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(restTokenExp), // Token expiration time
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconfiguration["JwtSettings:SecretKey"])),
                    SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
        

        [EnableCors("RestrictPolicy")]
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try {
                string apipass = string.Empty;
                string secretsEnabled = AppSettings.Get("EnableUserValidation");
                if (secretsEnabled.Equals("1") || secretsEnabled.Equals("2"))
                {
                    string secretRegion = iconfiguration["SecretsManagerRegion"].ToString();
                    string secretDictionary = iconfiguration["SecretsManagerDictionary"].ToString();
                    string secretKey = iconfiguration["SecretsManagerKey"].ToString();
                    var secret = new AmazonSecretsManager(secretRegion);
                    var secretResult = secret.GetSuperSecretPassword(secretDictionary);
                    secretResult.Wait();
                    apipass = secretResult.Result[secretKey];
                }
                else
                {
                    apipass = iconfiguration["JwtSettings:SecretKey"].ToString();
                }

                if (!string.IsNullOrEmpty(apipass))
                {
                    if (apipass == model.Password)
                    {
                        if (secretsEnabled.Equals("3") || secretsEnabled.Equals("1"))
                        {
                            DataSet dataSet = HelperFacade.CheckIfUserExists(model.Username);
                            if (dataSet != null)
                            {
                                DataTable dt = dataSet.Tables[0];
                                if (dt == null || dt.Rows.Count == 0)
                                {
                                    return BadRequest();
                                }
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        var token = GenerateAccessToken(model.Username);
                        var refreshToken = Guid.NewGuid().ToString();

                        // Store the refresh token (in-memory for simplicity)
                        _refreshTokens[refreshToken] = model.Username;
                        return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = refreshToken });
                    }
                }
            }
            catch(Exception ex)
            {
                Models.ErrorDetail ed = new Models.ErrorDetail();
                ed.Code = "500";
                ed.Description = ex.Message.ToString() + " " + ex.StackTrace.ToString();
                return new JsonResult(ed);
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            if (_refreshTokens.TryGetValue(request.RefreshToken, out var userId))
            {
                // Generate a new access token
                var token = GenerateAccessToken(userId);

                // Return the new access token to the client
                return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest("Invalid refresh token");
        }
    }
}