using Domain.Entities;
using Infratructure.DataContext;
using Infratructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IContext,DapperContext>();
builder.Services.AddScoped<IGenericService<Customer>, CustomerService>();
builder.Services.AddScoped<IGenericService<Account>, AccountService>();
builder.Services.AddScoped<IGenericService<Branch>, BranchService>();
builder.Services.AddScoped<IGenericService<Loan>, LoanService>();
builder.Services.AddScoped<IGenericService<Transaction>, TransactionService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "BankManagment"));
}

app.MapControllers();
app.Run();

