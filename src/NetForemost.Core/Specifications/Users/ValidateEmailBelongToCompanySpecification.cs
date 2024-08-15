using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;

namespace NetForemost.Core.Specifications.Users;

public class ValidateEmailBelongToCompanySpecification : Specification<CompanyUser>
{
    public ValidateEmailBelongToCompanySpecification(string email, int companyId)
    {
        Query.Include(companyUser => companyUser.User);

        Query.Where(companyUser =>
                                    companyUser.User.Email == email &&
                                    companyUser.CompanyId == companyId &&
                                    companyUser.isDeleted == false
        );
    }
}
