using Infratructure.Responses;

namespace Infratructure.Services;

public interface ILoanService
{
    ApiResponse<decimal> Sum();
}