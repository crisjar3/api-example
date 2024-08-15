using Ardalis.Result;
using Microsoft.AspNetCore.JsonPatch;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Interfaces.Companies;

public interface ICompanyService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="company"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result<Company>> CreateCompanyAsync(Company company, User user);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<Result<Company>> FindCompanyAsync(int companyId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="company"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result<Company>> UpdateCompanyAsync(Company company, string userId);

    /// <summary>
    /// Add url image for company photo 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="UrlImage"></param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<Result<bool>> AddImageCompany(string userId, string UrlImage, int companyId);

    /// <summary>
    /// Updates the company details partially
    /// </summary>
    /// <param name="userId">User identificator for verifying if it belongs to the company and retrieve his company</param>
    /// <param name="patchCompany">Company object with all the company details</param>
    /// <param name="companyId">Company that will be updated</param>
    /// <returns>The company register with the updated details</returns>
    Task<Result<Company>> PatchCompanyDetailsAsync(string userId, JsonPatchDocument<Company> patchCompany, int companyId);
}