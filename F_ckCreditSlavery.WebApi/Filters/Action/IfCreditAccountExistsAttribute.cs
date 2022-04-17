using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F_ckCreditSlavery.WebApi.Filters.Action;

public class IfCreditAccountExistsAttribute : IAsyncActionFilter
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryManager _repository;

    public IfCreditAccountExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        GetActionParamsFromContext(context, out var id, out var trackChanges);
        
        var creditAccount = await _repository.CreditAccount.GetAsync(id, trackChanges);
        
        if (creditAccount == null)
        {
            _logger.LogInfo($"Credit account with id: {id} doesn't exist in the database.");
            context.Result = new NotFoundResult();
        }
        else
        {
            context.HttpContext.Items.Add(nameof(creditAccount), creditAccount);
            await next();
        }
    }
    
    private static void GetActionParamsFromContext(
        ActionExecutingContext context,
        out int id,
        out bool trackChanges)
    {
        id = (int) context.ActionArguments[nameof(id)]!;
        trackChanges = context.HttpContext.Request.Method.Equals("PUT");
    } 
}