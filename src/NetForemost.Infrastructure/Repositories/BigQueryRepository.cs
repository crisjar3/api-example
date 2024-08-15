using Google.Cloud.BigQuery.V2;
using Microsoft.EntityFrameworkCore;
using NetForemost.Infrastructure.Contexts;
using NetForemost.Infrastructure.Extensions;
using NetForemost.SharedKernel.Interfaces;
using Newtonsoft.Json;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;

public class BigQueryRepository<T> : IBigQueryRepository<T> where T : class
{
    private readonly BigQueryClient _bigQueryClient;
    private readonly BigQueryTable _table;
    private string[] properties;

    public BigQueryRepository(BigQueryClient bigQueryClient)
    {
        _bigQueryClient = bigQueryClient;
    }

    private string[] getParameters()
    {
        return typeof(T).GetProperties()
            .Select(property => property.Name.AddUndercoresToSentence())
            .ToArray();
    }

    public async Task<IEnumerable<T>> ListAsync(int pageNumber = 0, int perPage = 10)
    {
        //var query = new QueryBuilder()
        //    .Select(properties)
        //    .From(_table)
        //    .Limit(perPage)
        //    .Offset(perPage * pageNumber)
        //    .Build();

        //BigQueryResults results = await _bigQueryClient.ExecuteQueryAsync(query, null);

        //var entities = results.Select(MapEntity);

        //return entities;

        throw new NotImplementedException();

    }

    public async Task<IEnumerable<T>> ListAsync(ICustomSpecification<T> specification)
    {
        var query = specification.Query.ToQueryString();

        var objectQuery = (ObjectQuery<T>)specification.Query;
        var parameters = objectQuery.Parameters.Select(MapEntityFrameworkParameterToBigQueryParameter);

        BigQueryResults results = await _bigQueryClient.ExecuteQueryAsync(query, parameters);

        var entities = results.Select(MapEntity);

        return entities;
    }

    private BigQueryParameter MapEntityFrameworkParameterToBigQueryParameter(ObjectParameter entityFrameworkParameter)
    {
        var name = entityFrameworkParameter.Name;
        var value = entityFrameworkParameter.Value;

        switch (value)
        {
            case int intValue:
                return new BigQueryParameter(name, BigQueryDbType.Int64, intValue);

            case string stringValue:
                return new BigQueryParameter(name, BigQueryDbType.String, stringValue);

            case bool boolValue:
                return new BigQueryParameter(name, BigQueryDbType.Bool, boolValue);

            default:
                throw new ArgumentException($"we could map the map types {name}");
        }
    }

    private T MapEntity(BigQueryRow row)
    {
        try
        {
            var entity = Activator.CreateInstance<T>();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {

                if (row.Schema.Fields.Any(field => field.Name == property.Name))
                {
                    var value = row[property.Name];

                    if (value is not null)
                    {
                        property.SetValue(entity, Convert.ChangeType(value, property.PropertyType));
                    }
                }

            }

            return entity;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<IEnumerable<T>> ListAsync(IQueryBuilder query)
    {
        var queryBuilt = query.Build();

        var results = await _bigQueryClient.ExecuteQueryAsync(queryBuilt, null);

        var entities = results.Select(MapEntity);


        return entities;

    }

    public async Task<int> CountAsync(IQueryBuilder query)
    {
        var queryBuilt = query.BuildCount();
        var results = await _bigQueryClient.ExecuteQueryAsync(queryBuilt, null);
        var mapped = results.Select(row => row["TotalRecords"]);
        return Convert.ToInt32(mapped.FirstOrDefault());
    }


    public async Task<IEnumerable<T>> ListJsonAsync(IQueryBuilder query)
    {
        var results = await _bigQueryClient.ExecuteQueryAsync(query.Build(), null);

        var ap = results.Select(GetModels);

        return ap;

    }

    public async Task<T> FirstOrDefault(IQueryBuilder query)
    {
        var queryBuilt = query.Build();

        var results = await _bigQueryClient.ExecuteQueryAsync(queryBuilt, null);

        var entities = results.Select(MapEntity);

        return entities.FirstOrDefault();
    }

    public T GetModels(BigQueryRow row)
    {
        var jsonString = row["JsonData"].ToString();

        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}

public class CustomSpecification<T> : ICustomSpecification<T>
    where T : class
{
    private readonly NetForemostContext _context;

    public IQueryable<T> Query { get; set; }

    public CustomSpecification(NetForemostContext context)
    {
        _context = context;
        Query = _context.Set<T>();
    }

    public void Where(Expression<Func<T, bool>> criteria)
    {
        Query = Query.Where(criteria);
    }
    public string GetQuery()
    {
        return Query.ToQueryString();
    }
}