using System;
using AuthorizedServer.Helper;
using AuthorizedServer.Logger;
using AuthorizedServer.Repositories;
using AuthorizedServer.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Examples;

namespace AuthorizedServer.Controllers
{
    /// <summary>Controller to get JWT token</summary>
    [Route("api/token")]
    public class TokenController : Controller
    {
        /// <summary></summary>
        public AuthHelper authHelper = new AuthHelper();
        /// <summary></summary>
        private IOptions<Audience> _settings;
        /// <summary></summary>
        private IRTokenRepository _repo;

        /// <summary></summary>
        /// <param name="settings"></param>
        /// <param name="repo"></param>
        public TokenController(IOptions<Audience> settings, IRTokenRepository repo)
        {
            this._settings = settings;
            this._repo = repo;
        }

        /// <summary>Get JWT token</summary>
        /// <param name="parameters"></param>
        /// <response code="999">Returns JWT token</response> 
        /// <response code="909">Fails to return JWT</response> 
        /// <response code="901">When parameters are null</response> 
        /// <response code="904">When request is bad</response> 
        /// <response code="400">If process run into a exception</response> 
        [HttpGet("auth")]
        [SwaggerRequestExample(typeof(Parameters), typeof(ParameterDetails))]
        [ProducesResponseType(typeof(ResponseData), 999)]
        [ProducesResponseType(typeof(ResponseData), 909)]
        [ProducesResponseType(typeof(ResponseData), 901)]
        [ProducesResponseType(typeof(ResponseData), 904)]
        public ActionResult Auth([FromQuery]Parameters parameters)
        {
            try
            {
                if (parameters == null)
                {
                    return Json(new ResponseData
                    {
                        Code = "901",
                        Message = "null of parameters",
                        Data = null
                    });
                }

                if (parameters.grant_type == "password")
                {
                    return Ok(Json(authHelper.DoPassword(parameters, _repo, _settings)));
                }
                else if (parameters.grant_type == "refresh_token")
                {
                    return Ok(Json(authHelper.DoRefreshToken(parameters, _repo, _settings)));
                }
                else
                {
                    return Json(new ResponseData
                    {
                        Code = "904",
                        Message = "bad request",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("TokenController", "Auth", "Auth", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = null
                });
            }
        }
    }
}
