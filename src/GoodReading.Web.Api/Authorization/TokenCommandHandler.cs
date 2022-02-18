using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace GoodReading.Web.Api.Authorization
{
    public class TokenCommandHandler :  IRequestHandler<TokenCommand, string>
    {
        private readonly ITokenService _tokenService;
       
        public TokenCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<string> Handle(TokenCommand request, CancellationToken cancellationToken)
        {
            return _tokenService.GetToken();
        }
    }
}
