namespace NetForemost.SharedKernel.Interfaces;

public interface IQueryBuilder
{
    IQueryBuilder Select(string columns);
    IQueryBuilder From(string table);
    IQueryBuilder Join(string table, string condition, string joinType = "INNER");
    IQueryBuilder Where(string condition);
    IQueryBuilder Limit(int limit);
    IQueryBuilder Offset(int offset, int limit);
    IQueryBuilder GroupBy(string groupBy);
    IQueryBuilder OrderBy(string orderBy);
    IQueryBuilder In<T>(IEnumerable<T> conditions);
    IQueryBuilder WithOpen(string name);
    IQueryBuilder WithClose();
    IQueryBuilder Union(string typeUnion);
    IQueryBuilder Procedure(string query);
    IQueryBuilder Having(string query);
    string BuildCount();
    string Build();
}
