using System.Threading.Tasks;

namespace ApplicationServices.Helpers
{
    public interface IAuthHelper
    {
        public Task<string> GenerateToken(string Email);
    }
}