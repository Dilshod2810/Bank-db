using System.Net;
using Domain.Entities;
using Infratructure.Responses;
using Infratructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BranchController : ControllerBase
{
    private readonly IGenericService<Branch> _branchService;

    public BranchController(IGenericService<Branch> branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public ApiResponse<List<Branch>> GetAll()
    {
        return _branchService.GetAll();
    }

    [HttpGet("{id:int}")]
    public ApiResponse<Branch> GetById(int id)
    {
        return _branchService.GetById(id);
    }

    [HttpPost]
    public ApiResponse<bool> Add(Branch branch)
    {
        return _branchService.Add(branch);
    }

    [HttpPut]
    public ApiResponse<bool> Update(Branch branch)
    {
        return _branchService.Update(branch);
    }

    [HttpDelete("{id:int}")]
    public ApiResponse<bool> Delete(int id)
    {
        return _branchService.Delete(id);
    }
}