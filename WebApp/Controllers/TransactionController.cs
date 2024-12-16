using System.Net;
using Domain.Entities;
using Infratructure.Responses;
using Infratructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly IGenericService<Transaction> _transactionService;

    public TransactionController(IGenericService<Transaction> transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public ApiResponse<List<Transaction>> GetAll()
    {
        return _transactionService.GetAll();
    }

    [HttpGet("{id:int}")]
    public ApiResponse<Transaction> GetById(int id)
    {
        return _transactionService.GetById(id);
    }

    [HttpPost]
    public ApiResponse<bool> Add(Transaction transaction)
    {
        return _transactionService.Add(transaction);
    }

    [HttpPut]
    public ApiResponse<bool> Update(Transaction transaction)
    {
        return _transactionService.Update(transaction);
    }

    [HttpDelete("{id:int}")]
    public ApiResponse<bool> Delete(int id)
    {
        return _transactionService.Delete(id);
    }
}