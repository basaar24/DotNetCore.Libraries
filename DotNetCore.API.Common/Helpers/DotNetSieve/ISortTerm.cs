namespace DotNetCore.API.Common.DotNetCoreSieve
{
    public interface ISortTerm
    {
        string Sort { set; }
        bool Descending { get; }
        string Name { get; }
    }
}
