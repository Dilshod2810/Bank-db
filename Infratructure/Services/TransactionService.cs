using System.Net;
using Dapper;
using Domain.Entities;
using Infratructure.DataContext;
using Infratructure.Responses;
using Npgsql;

namespace Infratructure.Services;

public class TransactionService(IContext context) : IGenericService<Transaction>
{
    public ApiResponse<List<Transaction>> GetAll()
    {
        using var connection = context.Connection();
        string sql = "select * from transactions where deletedat is null";
        var res = connection.Query<Transaction>(sql).ToList();
        return new ApiResponse<List<Transaction>>(res);
    }

    public ApiResponse<Transaction> GetById(int id)
    {
        using var connection = context.Connection();
        string sql = "select * from transactions where transactionid = @Id and deletedat is null";
        var res = connection.QuerySingleOrDefault<Transaction>(sql, new { Id = id });
        if (res == null) return new ApiResponse<Transaction>(HttpStatusCode.NotFound, "Transaction not found");
        return new ApiResponse<Transaction>(res);
    }

    public ApiResponse<bool> Add(Transaction data)
    {
        using var connection = context.Connection();
        string sql = """
                     insert into transactions(transactionstatus, dateissued, amount, createdat, fromaccountid, toaccountid)
                     values(@TransactionStatus, @DateIssued, @Amount, current_date, @FromAccountId, @ToAccountId);
                     """;
        var res = connection.Execute(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<bool> Update(Transaction data)
    {
        using var connection = context.Connection();
        string sql = """
                     update transactions set transactionstatus = @TransactionStatus, dateissued = @DateIssued, amount = @Amount, fromaccountid = @FromAccountId, toaccountid = @ToAccountId
                     where transactionid = @TransactionId and deletedat is null;
                     """;
        var res = connection.Execute(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<bool> Delete(int id)
    {
        using var connection = context.Connection();
        string sql = "update transactions set deletedat = current_date where transactionid = @Id and deletedat is null";
        var res = connection.Execute(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "Transaction not found or already deleted");
        return new ApiResponse<bool>(res > 0);
    }
}
