using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NetForemost.Core.Specifications.Companies
{
    public class GetCompanyByCompanyIdSpecification : Specification<Company>, ISingleResultSpecification
    {
        /// <summary>
        ///     Obtains the company by means of the company's Id.
        /// </summary>
        /// <param name="companyId">The companyId.</param>
        public GetCompanyByCompanyIdSpecification(int companyId)
        {
            Query.Include(company => company.City)
                .Include(company => company.TimeZone)
                .Include(company => company.Industry)
                .Include(company => company.CompanyUsers).ThenInclude(companyUser => companyUser.Role);

            Query.Where(company => company.Id == companyId);
        }
    }
}
