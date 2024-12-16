using Infratructure.Responses;

namespace Infratructure.Services;

public interface IAccountService
{
    ApiResponse<int> Balance();
}