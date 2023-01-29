using AutoMapper;
using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Entities.DataTransferObjects;
using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Entities.RequestFeatures;
using F_ckCreditSlavery.WebApi.ModelBinders;
using F_ckCreditSlavery.WebApi.Filters.Action;
using F_ckCreditSlavery.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace F_ckCreditSlavery.WebApi.Controllers;

[ApiController]
[Route("api/credit-accounts")]
public class CreditAccountController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<CreditAccountGetDto> _dataShaper;

    public CreditAccountController(
        IRepositoryManager repository,
        ILoggerManager logger,
        IMapper mapper,
        IDataShaper<CreditAccountGetDto> dataShaper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }
        
    [HttpOptions]
    public IActionResult GetCompaniesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT");
        return Ok();
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedList<Entity>>> GetCreditAccounts(
        [FromQuery] CreditAccountParameters parameters)
    {
        if (!parameters.IsValidStartDataRange) return BadRequest("Max start date can't be less than min start date.");
        if (!parameters.IsValidEndDataRange) return BadRequest("Max end date can't be less than min end date.");
        
        var accounts = await _repository.CreditAccount.GetAsync(parameters, false);
            
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(accounts.MetaData));
            
        var accountsDto = _mapper.Map<List<CreditAccountGetDto>>(accounts);
        
        return Ok(_dataShaper.ShapeData(accountsDto, parameters.Fields));
    }

    // todo: update OpenAPI specification
    [HttpGet("collection/({ids})", Name = "CreditAccountCollection")]
    public async Task<ActionResult<List<CreditAccountGetDto>>> GetCreditAccountCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IList<int> ids)
    {
        if (ids == null)
        {
            _logger.LogError($"Parameter {nameof(ids)} is null.");
            return BadRequest($"Parameter {nameof(ids)} is null.");
        }

        var creditAccountEntities = await _repository.CreditAccount.GetAsync(ids, false);

        if (ids.Count != creditAccountEntities.Count)
        {
            _logger.LogError("Some ids are not valid in a collection.");
            return NotFound();
        }

        var creditAccountsToReturn = _mapper.Map<List<CreditAccountGetDto>>(creditAccountEntities);
        return Ok(creditAccountsToReturn);
    }

    [HttpGet("{id:int}", Name = "CreditAccountById")]
    public async Task<ActionResult<CreditAccountGetDto>> GetCreditAccount(int id)
    {
        var account = await _repository.CreditAccount.GetAsync(id, false);

        if (account == null)
        {
            _logger.LogInfo($"Credit account with id: {id} doesn't exist in the database.");
            return NotFound();
        }

        var accountDto = _mapper.Map<CreditAccountGetDto>(account);
        return Ok(accountDto);
    }

    [HttpPost, Authorize]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCreditAccount([FromBody] CreditAccountPostDto creditAccount)
    {
        var creditAccountEntity = _mapper.Map<CreditAccount>(creditAccount);

        _repository.CreditAccount.CreateCreditAccount(creditAccountEntity);
        await _repository.SaveAsync();

        var creditAccountToReturn = _mapper.Map<CreditAccountGetDto>(creditAccountEntity);

        return CreatedAtRoute(
            "CreditAccountById",
            new {id = creditAccountToReturn.Id},
            creditAccountToReturn);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCreditAccountCollection(
        [FromBody] List<CreditAccountPostDto> creditAccounts)
    {
        if (creditAccounts == null)
        {
            _logger.LogError("Credit account collection sent from client is null.");
            return BadRequest("Credit account collection is null.");
        }
            
        var creditAccountEntities = _mapper.Map<List<CreditAccount>>(creditAccounts);
            
        foreach (var creditAccount in creditAccountEntities)
        {
            _repository.CreditAccount.CreateCreditAccount(creditAccount);
        }

        await _repository.SaveAsync();
            
        var companyCollectionToReturn =
            _mapper.Map<List<CreditAccountGetDto>>(creditAccountEntities);
            
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            
        return CreatedAtRoute(
            "CreditAccountCollection",
            new { ids },
            companyCollectionToReturn);
    }

    [HttpDelete("{id:int}")]
    [ServiceFilter(typeof(IfCreditAccountExistsAttribute))]
    public async Task<IActionResult> DeleteCreditAccount(int id)
    {
        GetCreditAccountFromContext(out var creditAccountEntity);

        _repository.CreditAccount.DeleteCreditAccount(creditAccountEntity);
        await _repository.SaveAsync();

        return NoContent();
    }

    [HttpPut]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ServiceFilter(typeof(IfCreditAccountExistsAttribute))]
    public async Task<IActionResult> UpdateCreditAccount(int id, [FromBody] CreditAccountUpdateDto creditAccountDto)
    {
        GetCreditAccountFromContext(out var creditAccountEntity);
            
        _mapper.Map(creditAccountDto, creditAccountEntity);
        await _repository.SaveAsync();

        return NoContent();
    }

    private void GetCreditAccountFromContext(out CreditAccount creditAccount)
    {
        creditAccount = HttpContext.Items[nameof(creditAccount)] as CreditAccount;
    } 
}