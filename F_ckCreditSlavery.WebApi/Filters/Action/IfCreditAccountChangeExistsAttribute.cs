using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F_ckCreditSlavery.WebApi.Filters.Action;

public class IfCreditAccountChangeExistsAttribute : IAsyncActionFilter
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryManager _repository;

    public IfCreditAccountChangeExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        GetActionParamsFromContext(context, out var creditAccountId, out var changeId, out var trackChanges);
        
        var creditAccount = await _repository.CreditAccount.GetAsync(creditAccountId, false);
        
        if (creditAccount == null)
        {
            _logger.LogInfo($"Credit account with id: {creditAccountId} doesn't exist in the database.");
            context.Result = new NotFoundResult();
            return;
        }

        var change = await _repository.CreditAccountChangeRepository.GetAsync(changeId, trackChanges);

        if (change == null)
        {
            _logger.LogInfo($"Credit account change with id: {changeId} doesn't exist in the database.");
            context.Result = new NotFoundResult();
        }
        else
        {
            context.HttpContext.Items.Add(nameof(change), change);
            await next();
        }
    }

    private static void GetActionParamsFromContext(
        ActionExecutingContext context,
        out int creditAccountId,
        out int changeId,
        out bool trackChanges)
    {
        creditAccountId = (int) context.ActionArguments[nameof(creditAccountId)]!;
        changeId = (int) context.ActionArguments[nameof(changeId)]!;
        trackChanges = context.HttpContext.Request.Method.Equals("PUT");
    } 
}