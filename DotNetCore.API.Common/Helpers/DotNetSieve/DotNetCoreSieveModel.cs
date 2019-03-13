using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace DotNetCore.API.Common.DotNetCoreSieve
{
    public class DotNetCoreSieveModel : DotNetCoreSieveModel<FilterTerm, SortTerm, FieldTerm> { }

    [DataContract]
    public class DotNetCoreSieveModel<TFilterTerm, TSortTerm, TFieldTerm> : IDotNetCoreSieveModel<TFilterTerm, TSortTerm, TFieldTerm>
        where TFilterTerm : IFilterTerm, new()
        where TSortTerm : ISortTerm, new()
        where TFieldTerm : IFieldTerm, new()
    {
        [DataMember]
        public string Filters { get; set; }

        [DataMember]
        public string Sorts { get; set; }

        [DataMember, Range(1, int.MaxValue)]
        public int? Page { get; set; }

        [DataMember, Range(1, int.MaxValue)]
        public int? PageSize { get; set; }

        [DataMember]
        public string Fields { get; set; }

        public List<TFilterTerm> GetFiltersParsed()
        {
            if (Filters != null)
            {
                var value = new List<TFilterTerm>();
                foreach (var filter in Filters.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(filter)) continue;

                    if (filter.StartsWith("("))
                    {
                        var filterOpAndVal = filter.Substring(filter.LastIndexOf(")") + 1);
                        var subfilters = filter.Replace(filterOpAndVal, "").Replace("(", "").Replace(")", "");
                        var filterTerm = new TFilterTerm
                        {
                            Filter = subfilters + filterOpAndVal
                        };
                        if (!value.Any(f => f.Names.Any(n => filterTerm.Names.Any(n2 => n2 == n))))
                        {
                            value.Add(filterTerm);
                        }
                    }
                    else
                    {
                        var filterTerm = new TFilterTerm
                        {
                            Filter = filter
                        };
                        value.Add(filterTerm);
                    }
                }
                return value;
            }
            else
            {
                return null;
            }
        }

        public List<TSortTerm> GetSortsParsed()
        {
            if (Sorts != null)
            {
                var value = new List<TSortTerm>();
                foreach (var sort in Sorts.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(sort)) continue;

                    var sortTerm = new TSortTerm()
                    {
                        Sort = sort
                    };
                    if (!value.Any(s => s.Name == sortTerm.Name))
                    {
                        value.Add(sortTerm);
                    }
                }
                return value;
            }
            else
            {
                return null;
            }

        }

        public List<TFieldTerm> GetFieldsParsed()
        {
            if (Fields != null)
            {
                var value = new List<TFieldTerm>();
                foreach (var field in Fields.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(field)) continue;

                    var fieldTerm = new TFieldTerm()
                    {
                        Field = field
                    };
                    if (!value.Any(s => s.Name == fieldTerm.Name))
                    {
                        value.Add(fieldTerm);
                    }
                }
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool AreFiltersValid() => GetFiltersParsed().Any(f => f.Value == null) ? false : true;

        public bool AreFieldsValid(PropertyInfo[] properties, out string message)
        {
            foreach(var field in GetFieldsParsed())
            {
                if (!properties.Any(p => p.Name.Equals(field.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    message = "One or more requested fields are not part or the properties of the current entity.";
                    return false;
                }
            }

            foreach (var filter in GetFiltersParsed())
            {
                if (!properties.Any(p => p.Name.Equals(filter.Names[0], StringComparison.OrdinalIgnoreCase)))
                {
                    message = "One or more requested fields are not part or the properties of the current entity.";
                    return false;
                }
            }

            foreach (var sort in GetSortsParsed())
            {
                if (!properties.Any(p => p.Name.Equals(sort.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    message = "One or more requested fields are not part or the properties of the current entity.";
                    return false;
                }
            }

            message = "";
            return true;

        }
    }
}