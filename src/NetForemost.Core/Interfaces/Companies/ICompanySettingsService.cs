using Ardalis.Result;
using Microsoft.AspNetCore.JsonPatch;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Interfaces.Companies
{
    public interface ICompanySettingsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companySettings"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<Result<CompanySettings>> CreateCompanySettingsAsync(CompanySettings companySettings, string userId, int companyId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newCompanySettings"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<CompanySettings>> UpdateCompanySettingsAsync(CompanySettings newCompanySettings, string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<CompanySettings>> FindCompanySettingsAsync(string userId);

        /// <summary>
        /// Updates partially the information of a company settings register
        /// </summary>
        /// <param name="userId">User indetificator</param>
        /// <returns>The company settings register with the updated date</returns>
        Task<Result<CompanySettings>> PatchCompanySettingsAsync(JsonPatchDocument<CompanySettings> patchDocument, string userId);
    }
}