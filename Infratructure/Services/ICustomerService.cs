using Infratructure.Responses;

namespace Infratructure.Services;

public interface ICustomerService
{
    ApiResponse<int> Count();
}