using AutoMapper;
using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Entities.DataTransferObjects;
using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Entities.RequestFeatures;
using F_ckCreditSlavery.WebApi.Filters.Action;
using F_ckCreditSlavery.Contracts.Repositories;
using F_ckCreditSlavery.WebApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace F_ckCreditSlavery.WebApi.Controllers;

[ApiController]
[Route("api/credit-accounts/{creditAccountId:int}/changes")]
public class CreditAccountChangeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly CreditAccountChangeLinks _changeLinks;

    public CreditAccountChangeController(
        IRepositoryManager repository,
        ILoggerManager logger,
        IMapper mapper,
        CreditAccountChangeLinks changeLinks)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _changeLinks = changeLinks;
    }

    [HttpGet]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetCreditAccountChanges(
        int creditAccountId,
        [FromQuery] CreditAccountChangeParameters parameters)
    {
        if (!parameters.IsValidDataRange) return BadRequest("Max date can't be less than min date.");
            
        var creditAccount = await _repository.CreditAccount.GetAsync(creditAccountId, false);

        if (creditAccount == null)
        {
            _logger.LogInfo($"Credit account with id: {creditAccountId} doesn't exist in the database.");
            return NotFound();
        }

        var changes = await _repository.CreditAccountChangeRepository.GetAsync(
            creditAccountId,
            false,
            parameters);
            
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(changes.MetaData));
            
        var changesToReturn = _mapper.Map<List<CreditAccountChangeGetDto>>(changes);

        var links = _changeLinks.TryGenerateLinks(
            changesToReturn,
            parameters.Fields,
            creditAccountId,
            HttpContext);
        
        return links.HasLinks 
            ? Ok(links.LinkedEntities)
            : Ok(links.ShapedEntities);
    }
        
    [HttpGet("{changeId:int}", Name = "GetCreditAccountChange")]
    public async Task<ActionResult<CreditAccountChangeGetDto>> GetCreditAccountChange(
        int creditAccountId,
        int changeId)
    {
        var creditAccount = await _repository.CreditAccount.GetAsync(creditAccountId, false);

        if (creditAccount == null)
        {
            _logger.LogInfo($"Credit account with id: {creditAccountId} doesn't exist in the database.");
            return NotFound();
        }

        var changeEntity = await _repository.CreditAccountChangeRepository.GetAsync(changeId, false);
            
        if (changeEntity == null)
        {
            _logger.LogInfo($"Credit account change with id: {changeId} doesn't exist in the database.");
            return NotFound();
        }
            
        var changeToReturn = _mapper.Map<CreditAccountChangeGetDto>(changeEntity);

        return Ok(changeToReturn);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCreditAccountChange(
        int creditAccountId,
        [FromBody] CreditAccountChangePostDto changeDto)
    {
        var creditAccount = await _repository.CreditAccount.GetAsync(creditAccountId, false);
            
        if (creditAccount == null)
        {
            _logger.LogInfo($"Credit account with id: {creditAccountId} doesn't exist in the database.");
            return NotFound();
        }

        var changeEntity = _mapper.Map<CreditAccountChange>(changeDto);

        _repository.CreditAccountChangeRepository.CreateCreditAccountChange(creditAccountId, changeEntity);
        await _repository.SaveAsync();

        var changeToReturn = _mapper.Map<CreditAccountChangeGetDto>(changeEntity);

        return CreatedAtRoute(
            "GetCreditAccountChange", 
            new {creditAccountId, changeId = changeToReturn.Id},
            changeToReturn);
    }

    [HttpDelete("{changeId:int}")]
    [ServiceFilter(typeof(IfCreditAccountChangeExistsAttribute))]
    public async Task<IActionResult> DeleteCreditAccountChange(int creditAccountId, int changeId)
    {
        GetCreditAccountChangeFromContext(out var changeEntity);
            
        _repository.CreditAccountChangeRepository.DeleteCreditAccountChange(changeEntity);
        await _repository.SaveAsync();

        return NoContent();
    }

    [HttpPut]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ServiceFilter(typeof(IfCreditAccountChangeExistsAttribute))]
    public async Task<IActionResult> UpdateCreditAccountChange(
        int creditAccountId,
        int changeId,
        [FromBody] CreditAccountChangeUpdateDto changeDto)
    {
        GetCreditAccountChangeFromContext(out var changeEntity);
            
        _mapper.Map(changeDto, changeEntity);
        await _repository.SaveAsync();

        return NoContent();
    }
        
    private void GetCreditAccountChangeFromContext(out CreditAccountChange change)
    {
        change = HttpContext.Items[nameof(change)] as CreditAccountChange;
    } 
}