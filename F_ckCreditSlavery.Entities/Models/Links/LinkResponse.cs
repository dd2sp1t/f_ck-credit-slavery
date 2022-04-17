using F_ckCreditSlavery.Entities.DataTransferObjects;

namespace F_ckCreditSlavery.Entities.Models.Links;

public class LinkResponse
{
    public bool HasLinks { get; set; }
    public List<Entity> ShapedEntities { get; set; }
    public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }
    
    public LinkResponse()
    {
        LinkedEntities = new LinkCollectionWrapper<Entity>();
        ShapedEntities = new List<Entity>();
    }
}