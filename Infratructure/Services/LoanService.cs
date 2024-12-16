using System.Net;
using Dapper;
using Domain.Entities;
using Infratructure.DataContext;
using Infratructure.Responses;
using Npgsql;

namespace Infratructure.Services;

public class LoanService(IContext context) : IGenericService<Loan>, ILoanService
{
    public ApiResponse<List<Loan>> GetAll()
    {
        using var connection = context.Connection();
        string sql = "select * from loans where deletedat is null";
        var res = connection.Query<Loan>(sql).ToList();
        return new ApiResponse<List<Loan>>(res);
    }

    public ApiResponse<Loan> GetById(int id)
    {
        using var connection = context.Connection();
        string sql = "select * from loans where loanid = @Id and deletedat is null";
        var res = connection.QuerySingleOrDefault<Loan>(sql, new { Id = id });
        if (res == null) return new ApiResponse<Loan>(HttpStatusCode.NotFound, "Loan not found");
        return new ApiResponse<Loan>(res);
    }

    public ApiResponse<bool> Add(Loan data)
    {
        using var connection = context.Connection();
        string sql = """
                     insert into loans(loanamount, dateissued, createdat, customerid, branchid)
                     values(@LoanAmount, @DateIssued, current_date, @CustomerId, @BranchId);
                     """;
        var res = connection.Execute(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<bool> Update(Loan data)
    {
        using var connection = context.Connection();
        string sql = """
                     update loans set loanamount = @LoanAmount, dateissued = @DateIssued, customerid = @CustomerId, branchid = @BranchId
                     where loanid = @LoanId and deletedat is null;
                     """;
        var res = connection.Execute(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<bool> Delete(int id)
    {
        using var connection = context.Connection();
        string sql = "update loans set deletedat = current_date where loanid = @Id and deletedat is null";
        var res = connection.Execute(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "Loan not found or already deleted");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<decimal> Sum()
    {
        using var connection = context.Connection();
        string sql = "select sum(loanamount) from loans;";
        var res = connection.ExecuteScalar<decimal>(sql);
        return new ApiResponse<decimal>(res);
    }
}
