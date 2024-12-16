using System.Net;
using Dapper;
using Domain.Entities;
using Infratructure.DataContext;
using Infratructure.Responses;
using Npgsql;

namespace Infratructure.Services;

public class BranchService(IContext context) : IGenericService<Branch>
{
    public ApiResponse<List<Branch>> GetAll()
    {
        using var connection = context.Connection();
        string sql = "select * from branches where deletedat is null";
        var res = connection.Query<Branch>(sql).ToList();
        return new ApiResponse<List<Branch>>(res);
    }

    public ApiResponse<Branch> GetById(int id)
    {
        using var connection = context.Connection();
        string sql = "select * from branches where branchid = @Id and deletedat is null";
        var res = connection.QuerySingleOrDefault<Branch>(sql, new { Id = id });
        if (res == null) return new ApiResponse<Branch>(HttpStatusCode.NotFound, "Branch not found");
        return new ApiResponse<Branch>(res);
    }

    public ApiResponse<bool> Add(Branch data)
    {
        using var connection = context.Connection();
        string sql = """
                     insert into branches(branchname, branchlocation, createdat)
                     values(@BranchName, @BranchLocation, current_date);
                     """;
        var res = connection.Execute(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<bool> Update(Branch data)
    {
        using var connection = context.Connection();
        string sql = """
                     update branches set branchname = @BranchName, branchlocation = @BranchLocation
                     where branchid = @BranchId and deletedat is null;
                     """;
        var res = connection.Execute(sql, data);
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Internal server error");
        return new ApiResponse<bool>(res > 0);
    }

    public ApiResponse<bool> Delete(int id)
    {
        using var connection = context.Connection();
        string sql = "update branches set deletedat = current_date where branchid = @Id and deletedat is null";
        var res = connection.Execute(sql, new { Id = id });
        if (res == 0) return new ApiResponse<bool>(HttpStatusCode.NotFound, "Branch not found or already deleted");
        return new ApiResponse<bool>(res > 0);
    }
}
