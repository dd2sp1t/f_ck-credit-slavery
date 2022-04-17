using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Entities.DataTransferObjects;
using F_ckCreditSlavery.Entities.Models.Links;
using Microsoft.Net.Http.Headers;

namespace F_ckCreditSlavery.WebApi.Utility;

public class CreditAccountChangeLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<CreditAccountChangeGetDto> _dataShaper;

    public CreditAccountChangeLinks(
        LinkGenerator linkGenerator,
        IDataShaper<CreditAccountChangeGetDto> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }

    public LinkResponse TryGenerateLinks(
        IEnumerable<CreditAccountChangeGetDto> changes,
        string fields,
        int creditAccountId,
        HttpContext httpContext)
    {
        var shapedChanges = ShapeData(changes, fields);

        return ShouldGenerateLinks(httpContext) 
            ? ReturnLinkedChanges(changes, fields, creditAccountId, httpContext, shapedChanges) 
            : ReturnShapedChanges(shapedChanges);
    }

    private List<Entity> ShapeData(IEnumerable<CreditAccountChangeGetDto> changes, string fields)
    {
        return _dataShaper.ShapeData(changes, fields)
            .Select(e => e.Entity)
            .ToList();
    }
    
    private static bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue) httpContext.Items["AcceptHeaderMediaType"];
        return mediaType!.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.CurrentCultureIgnoreCase);
    }
    
    private static LinkResponse ReturnShapedChanges(List<Entity> shapedChanges) => 
        new LinkResponse {ShapedEntities = shapedChanges};

    private LinkResponse ReturnLinkedChanges(
        IEnumerable<CreditAccountChangeGetDto> changes,
        string fields,
        int creditAccountId,
        HttpContext httpContext,
        List<Entity> shapedChanges)
    {
        var changesList = changes.ToList();

        for (var index = 0; index < changesList.Count; index++)
        {
            var changeLinks = CreateLinksForChange(httpContext, creditAccountId, changesList[index].Id, fields);
            shapedChanges[index].Add("Links", changeLinks);
        }

        var changeCollection = new LinkCollectionWrapper<Entity>(shapedChanges);
        var linkedChanges = CreateLinkForChanges(httpContext, changeCollection);

        return new LinkResponse {HasLinks = true, LinkedEntities = linkedChanges};
    }
    
    private List<Link> CreateLinksForChange(
        HttpContext httpContext,
        int creditAccountId,
        int changeId,
        string fields)
    {
        var links = new List<Link>
        {
            new Link(
                _linkGenerator.GetUriByAction(
                    httpContext, 
                    LinkHelper.CreditAccountChangeGet, 
                    LinkHelper.CreditAccountChangeController, 
                    values: new { creditAccountId, changeId, fields }
                    ),
                "self",
                "GET"),
            new Link(
                _linkGenerator.GetUriByAction(
                    httpContext,
                    LinkHelper.CreditAccountChangeDelete,
                    LinkHelper.CreditAccountChangeController,
                    values: new { creditAccountId, changeId }),
                "delete_credit_account_change",
                "DELETE"),
            new Link(
                _linkGenerator.GetUriByAction(
                    httpContext,
                    LinkHelper.CreditAccountChangeUpdate,
                    LinkHelper.CreditAccountChangeController,
                    values: new { creditAccountId, changeId }),
                "update_credit_account_change",
                "PUT")
        };
        
        return links;
    }
    
    private LinkCollectionWrapper<Entity> CreateLinkForChanges(
        HttpContext httpContext,
        LinkCollectionWrapper<Entity> changesWrapper)
    {
        changesWrapper.Links.Add(
            new Link(
                _linkGenerator.GetUriByAction(
                    httpContext,
                    LinkHelper.CreditAccountChangeGetAll,
                    LinkHelper.CreditAccountChangeController,
                    values: new { }),
                "self",
                "GET")
            );

        return changesWrapper;
    }
}