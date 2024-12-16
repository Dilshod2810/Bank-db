using System.Net;
using Domain.Entities;
using Infratructure.Responses;
using Infratructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IGenericService<Account> _accountService;
    private readonly IAccountService _accountExtra;

    public AccountController(IGenericService<Account> accountService)
    {
        _accountService = accountService;
    }

    public AccountController(IAccountService accountExtra)
    {
        _accountExtra = accountExtra;
    }

    [HttpGet]
    public ApiResponse<List<Account>> GetAll()
    {
        return _accountService.GetAll();
    }

    [HttpGet("{id:int}")]
    public ApiResponse<Account> GetById(int id)
    {
        return _accountService.GetById(id);
    }

    [HttpPost]
    public ApiResponse<bool> Add(Account account)
    {
        return _accountService.Add(account);
    }

    [HttpPut]
    public ApiResponse<bool> Update(Account account)
    {
        return _accountService.Update(account);
    }

    [HttpDelete("{id:int}")]
    public ApiResponse<bool> Delete(int id)
    {
        return _accountService.Delete(id);
    }

    [HttpGet]
    public ApiResponse<int> Balance()
    {
        return _accountExtra.Balance();
    }
}