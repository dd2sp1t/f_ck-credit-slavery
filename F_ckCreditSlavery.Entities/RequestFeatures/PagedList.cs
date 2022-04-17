namespace F_ckCreditSlavery.Entities.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; }
    
    public PagedList(List<T> items, int pageNumber, int pageSize, int count)
    {
        MetaData = new MetaData(
            totalCount: count,
            pageSize: pageSize,
            currentPage: pageNumber,
            totalPages: (int) Math.Ceiling(count / (double) pageSize));
        
        AddRange(items);
    }
}