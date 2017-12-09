using System.Threading.Tasks;

namespace AuthorizedServer.Repositories
{
    /// <summary>Interface for Token Repository</summary>
    public interface IRTokenRepository
    {
        /// <summary>Add token</summary>
        /// <param name="token"></param>
        Task<bool> AddToken(RToken token);

        /// <summary>Expire token</summary>
        /// <param name="token"></param>
        Task<bool> ExpireToken(RToken token);

        /// <summary>Get token</summary>
        /// <param name="refresh_token"></param>
        /// <param name="client_id"></param>
        Task<RToken> GetToken(string refresh_token,string client_id);
    }
}
