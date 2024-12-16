using System.Net;
using Dapper;
using Domain.Entities;
using Infratructure.DataContext;
using Infratructure.Responses;
using Npgsql;

namespace Infratructure.Services;

public class AccountService(IContext context) : IGenericService<Account>,IAccountService
{
    public ApiResponse<List<Account>> GetAll()
    {
        using var connection = context.Connection();
        string sql = "select * from accounts where deletedat is null";
        var res = connection.Query<Account>(sql).ToList();
        return new ApiResponse<List<Account>>(res);
    }

    public ApiResponse<Account> GetById(int id)
    {
        using var connection = context.Connection();
        string sql = "select * from accounts where accountid = @Id and deletedat is null";
        var res = connection.QuerySingleOrDefault<Account>(sql, new { Id = id });
        if (res == null) return new ApiResponse<Account>(HttpStatusCode.NotFound, "Account not found");
        return new ApiResponse<Account>(res);
    }

    public ApiResponse<bool> Add(Account data)
    {
        using var connection = context.Connection();
        string sql = """
                     insert into accounts(balance, accountstatus, accounttype, currency, createdat)
                     values(@Balance, @AccountStatus, @AccountType, @Currency, current_date);
                     """;
        var res = connection.Execute(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<bool> Update(Account data)
    {
        using var connection = context.Connection();
        string sql = """
                     update accounts set balance = @Balance, accountstatus = @AccountStatus, accounttype = @AccountType, currency = @Currency
                     where accountid = @AccountId and deletedat is null;
                     """;
        var res = connection.Execute(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<bool> Delete(int id)
    {
        using var connection = context.Connection();
        string sql = "update accounts set deletedat = current_date where accountid = @Id and deletedat is null";
        var res = connection.Execute(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "Account not found or already deleted");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<int> Balance()
    {
        using var connection = context.Connection();
        string sql = "select count(balance) from accounts where balance<2000;";
        var res = connection.ExecuteScalar<int>(sql);
        return new ApiResponse<int>(res);
    }
}
