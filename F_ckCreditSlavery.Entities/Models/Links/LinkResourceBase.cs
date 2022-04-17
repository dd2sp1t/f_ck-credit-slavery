namespace F_ckCreditSlavery.Entities.Models.Links;

public class LinkResourceBase
{
    public List<Link> Links { get; set; } = new List<Link>();
    protected LinkResourceBase() {}
}