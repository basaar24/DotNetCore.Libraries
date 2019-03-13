using System.Collections.Generic;

namespace DotNetCore.API.Common.DotNetCoreSieve
{
    public interface IDotNetCoreSieveModel : IDotNetCoreSieveModel<IFilterTerm, ISortTerm, IFieldTerm> { }

    public interface IDotNetCoreSieveModel<TFilterTerm, TSortTerm, TFieldTerm>
        where TFilterTerm : IFilterTerm
        where TSortTerm : ISortTerm
        where TFieldTerm : IFieldTerm
    {
        string Filters { get; set; }

        string Sorts { get; set; }

        int? Page { get; set; }

        int? PageSize { get; set; }

        string Fields { get; set; }

        List<TFilterTerm> GetFiltersParsed();

        List<TSortTerm> GetSortsParsed();

        List<TFieldTerm> GetFieldsParsed();
    }
}
