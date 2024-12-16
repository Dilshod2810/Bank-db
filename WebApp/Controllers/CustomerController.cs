using Domain.Entities;
using Infratructure.Responses;
using Infratructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IGenericService<Customer> _customerService;
    private readonly ICustomerService _customerExtra;

    public CustomerController(IGenericService<Customer> customerService)
    {
        _customerService = customerService;
    }

    public CustomerController(ICustomerService customerExtra)
    {
        _customerExtra = customerExtra;
    }
    [HttpGet]
    public ApiResponse<List<Customer>> GetAll()
    {
        return _customerService.GetAll();
    }
    [HttpGet("{id:int}")]
    public ApiResponse<Customer> GetById(int id)
    {
        return _customerService.GetById(id);
    }
    [HttpPost]
    public ApiResponse<bool> Add(Customer customer)
    {
        return _customerService.Add(customer);
    }
    [HttpPut]
    public ApiResponse<bool> Update(Customer customer)
    {
        return _customerService.Update(customer);
    }
    [HttpDelete]
    public ApiResponse<bool> Delete(int id)
    {
        return _customerService.Delete(id);
    }

    [HttpGet]
    public ApiResponse<int> Count()
    {
        return _customerExtra.Count();
    }
}