using NetForemost.SharedKernel.Interfaces;
using System.Text;

namespace NetForemost.Infrastructure.Helpers;

public class QueryBuilder : IQueryBuilder
{
    private StringBuilder _query;
    private HashSet<string> _criterias = new HashSet<string>();
    //public string _table { get; set; }

    //private readonly string tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
    public QueryBuilder()
    {
        _query = new StringBuilder();
        //_table = "public." + tableName;
    }

    public IQueryBuilder Select(string columns)
    {
        _query.Append($"SELECT {columns}");
        return this;
    }

    public IQueryBuilder From(string table)
    {
        _query.Append($" FROM {table} ");
        return this;
    }
    public IQueryBuilder Select()
    {
        //var properties = typeof(T).GetProperties()
        //    .Select(property => property.Name.AddUndercoresToSentence());

        //_query.Append($"SELECT {string.Join(", ", properties)} FROM {_table}");
        return this;
    }

    public IQueryBuilder Join(string table, string condition, string joinType = "INNER")
    {
        _query.Append($" {joinType} JOIN {table} ON {condition}");
        return this;
    }

    public IQueryBuilder Where(string condition)
    {
        if (_criterias.Count() == 0)
        {
            _query.Append($" WHERE {condition}");
            _criterias.Add(condition);
        }
        if (!_criterias.Contains(condition))
        {
            _query.Append($" AND {condition}");
            _criterias.Add(condition);
        }
        return this;
    }

    public IQueryBuilder In<T>(IEnumerable<T> conditions)
    {
        string result;

        if (typeof(T) == typeof(string))
        {
            result = string.Join(",", conditions.Select(item => $"'{item}'"));
        }
        else
        {
            result = string.Join(",", conditions);
        }

        _query.Append($" IN ( {result} )");

        return this;
    }

    public IQueryBuilder Limit(int limit)
    {
        _query.Append($" LIMIT {limit} ");
        return this;
    }

    public IQueryBuilder Offset(int offset, int limit)
    {
        _query.Append($" OFFSET {(offset - 1) * limit} ");
        return this;
    }

    public IQueryBuilder GroupBy(string group)
    {
        _query.Append($" GROUP BY {group} ");
        return this;
    }

    public IQueryBuilder OrderBy(string order)
    {
        _query.Append($" ORDER BY {order} ");
        return this;
    }

    public IQueryBuilder WithOpen(string name)
    {
        _query.Append($"WITH {name} AS ( ");
        return this;
    }

    public IQueryBuilder Procedure(string query)
    {
        _query.Append(query);
        return this;
    }
    public IQueryBuilder WithClose()
    {
        _query.Append($" ) ");
        return this;
    }

    public IQueryBuilder Union(String typeUnion)
    {
        _query.Append($" UNION {typeUnion} ");
        return this;
    }

    public IQueryBuilder Having(string condition)
    {
        _query.Append($" HAVING {condition} ");
        return this;
    }

    public string Build()
    {
        var queryString = _query.ToString().Trim();
        _query = new StringBuilder();
        _criterias.Clear();
        return queryString;
    }

    public string BuildCount()
    {
        var queryString = "WITH subquery AS (" + _query.ToString().Trim() + ") SELECT COUNT(*) AS TotalRecords FROM subquery;";
        return queryString;
    }

}