using Ardalis.Specification;
using NetForemost.Core.Entities.JobRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NetForemost.Core.Specifications.JobRoles
{
    public class GetJobRolesByCompanyIdSpecification : Specification<JobRole>
    {
        public GetJobRolesByCompanyIdSpecification(int companyId)
        {
            Query.Where(jobrole => jobrole.CompanyId == companyId);
        }
    }
}
