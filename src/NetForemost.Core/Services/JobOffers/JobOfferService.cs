using Ardalis.Result;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Policies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.Core.Entities.Skills;
using NetForemost.Core.Interfaces.JobOffers;
using NetForemost.Core.Specifications.JobOffers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;


namespace NetForemost.Core.Services.JobOffers
{
    public class JobOfferService : IJobOfferService
    {
        private readonly IAsyncRepository<City> _cityRepository;
        private readonly IAsyncRepository<Country> _countryRepository;
        private readonly IAsyncRepository<Company> _companyRepository;
        private readonly IAsyncRepository<Benefit> _benefitRepository;
        private readonly IAsyncRepository<JobRole> _jobRoleRepository;
        private readonly IAsyncRepository<Seniority> _seniorityRepository;
        private readonly IAsyncRepository<Skill> _skillRepository;
        private readonly IAsyncRepository<JobOffer> _JobOfferRepository;
        private readonly IAsyncRepository<Policy> _policyRepository;
        private readonly IAsyncRepository<ContractType> _contractTypeRepository;
        private readonly IAsyncRepository<Language> _languageRepository;
        private readonly IAsyncRepository<LanguageLevel> _languageLevelRepository;
        private readonly IAsyncRepository<Project> _projectRepository;

        public JobOfferService(
            IAsyncRepository<City> cityRepository,
            IAsyncRepository<Country> countryRepository,
            IAsyncRepository<Company> companyRepository,
            IAsyncRepository<Benefit> benefitRepository,
            IAsyncRepository<JobRole> jobRoleRepository,
            IAsyncRepository<Seniority> seniorityRepository,
            IAsyncRepository<Skill> skillRepository,
            IAsyncRepository<JobOffer> jobOfferRepository,
            IAsyncRepository<Policy> policyRepository,
            IAsyncRepository<ContractType> contractTypeRepository,
            IAsyncRepository<Language> languageRepository,
            IAsyncRepository<LanguageLevel> languageLevelRepository,
            IAsyncRepository<Project> projectRepository
            )
        {
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _companyRepository = companyRepository;
            _benefitRepository = benefitRepository;
            _jobRoleRepository = jobRoleRepository;
            _seniorityRepository = seniorityRepository;
            _skillRepository = skillRepository;
            _JobOfferRepository = jobOfferRepository;
            _policyRepository = policyRepository;
            _contractTypeRepository = contractTypeRepository;
            _languageRepository = languageRepository;
            _languageLevelRepository = languageLevelRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Result<JobOffer>> CreateJobOfferAsync(JobOffer jobOffer, string userId)
        {
            try
            {
                City? existCity = null;
                //Check if a city id is sent
                if (jobOffer.CityId is not null)
                {
                    //Check if city exist
                    existCity = await _cityRepository.GetByIdAsync(jobOffer.CityId);

                    if (existCity is null)
                        return Result<JobOffer>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CityNotFound, ErrorCode = NameStrings.HttpError_BadRequest } });
                }

                Country? existCountry = null;
                //Check if a country id is sent
                if (jobOffer.CountryId is not null)
                {
                    //Check if country exist
                     existCountry = await _countryRepository.GetByIdAsync(jobOffer.CountryId);

                    if (existCountry is null)
                        return Result<JobOffer>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CountryNotFound, ErrorCode = NameStrings.HttpError_BadRequest } });
                }

                Project? existProject = null;
                //Check if a project id is sent
                if (jobOffer.ProjectId is not null)
                {
                    //Check if project exist
                    existProject = await _projectRepository.GetByIdAsync(jobOffer.ProjectId);

                    if (existProject is null)
                        return Result<JobOffer>.Invalid(new() { new() { ErrorMessage = ErrorStrings.ProjectNotFound, ErrorCode = NameStrings.HttpError_BadRequest } });
                }

                //Check if copany exist
                var existCompany = await _companyRepository.GetByIdAsync(jobOffer.CompanyId);

                if (existCompany is null)
                    return Result<JobOffer>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CompanyNotFound, ErrorCode = NameStrings.HttpError_BadRequest } });

                //Check that the list of benefits is valid.

                foreach (var benefit in jobOffer.Benefits)
                {
                    benefit.Benefit = await _benefitRepository.GetByIdAsync(benefit.BenefitId);

                    if (benefit.Benefit is null)
                        return Result<JobOffer>.Invalid(
                            new()
                            {
                                new()
                                {
                                    ErrorMessage = ErrorStrings.BenefitIdNotFound.Replace("[id]", benefit.BenefitId.ToString()),
                                    ErrorCode = NameStrings.HttpError_BadRequest
                                }
                            });

                    benefit.CreatedBy = userId;
                    benefit.CreatedAt = DateTime.UtcNow;
                }

                //Check that the list of rquest job roles is valid.
                foreach (var talent in jobOffer.Talents)
                {
                    //Check job role
                    var existJobRole = await _jobRoleRepository.GetByIdAsync(talent.JobRoleId);

                    if (existJobRole is null)
                        return Result<JobOffer>.Invalid(
                            new()
                            {
                                new()
                                {
                                    ErrorMessage = ErrorStrings.JobRoleIdNotFound.Replace("[id]", talent.JobRoleId.ToString()),
                                    ErrorCode = NameStrings.HttpError_BadRequest
                                }
                            });

                    //Check seniority
                    var existSeniority = await _seniorityRepository.GetByIdAsync(talent.SeniorityId);
                    
                    if (existSeniority is null)
                        return Result<JobOffer>.Invalid(
                            new()
                            {
                                new()
                                {
                                    ErrorMessage = ErrorStrings.SeniorityIdNotFound.Replace("[id]", talent.SeniorityId.ToString()),
                                    ErrorCode = NameStrings.HttpError_BadRequest
                                }
                            });

                    //Check policy
                    var existPolicy = await _policyRepository.GetByIdAsync(talent.PolicyId);

                    if (existPolicy is null)
                        return Result<JobOffer>.Invalid(
                            new()
                            {
                                new()
                                {
                                    ErrorMessage = ErrorStrings.PolicyNotFound.Replace("[id]", talent.PolicyId.ToString()),
                                    ErrorCode = NameStrings.HttpError_BadRequest
                                }
                            });

                    //Check contract type
                    var existContractType = await _contractTypeRepository.GetByIdAsync(talent.ContractTypeId);

                    if (existContractType is null)
                        return Result<JobOffer>.Invalid(
                            new()
                            {
                                new()
                                {
                                    ErrorMessage = ErrorStrings.ContractTypeNotFound.Replace("[id]", talent.ContractTypeId.ToString()),
                                    ErrorCode = NameStrings.HttpError_BadRequest
                                }
                            });

                    //Check language
                    var existTalent = await _languageRepository.GetByIdAsync(talent.LanguageId);

                    if (existTalent is null)
                        return Result<JobOffer>.Invalid(
                            new()
                            {
                                new()
                                {
                                    ErrorMessage = ErrorStrings.LanguageNotFound.Replace("[id]", talent.LanguageId.ToString()),
                                    ErrorCode = NameStrings.HttpError_BadRequest
                                }
                            });

                    //Check language level
                    var existLanguageLevel = await _languageLevelRepository.GetByIdAsync(talent.LanguageLevelId);

                    if (existLanguageLevel is null)
                        return Result<JobOffer>.Invalid(
                            new()
                            {
                                new()
                                {
                                    ErrorMessage = ErrorStrings.LanguageLevelNotFound.Replace("[id]", talent.LanguageLevelId.ToString()),
                                    ErrorCode = NameStrings.HttpError_BadRequest
                                }
                            });

                    //Check the list of skills
                    foreach (var talentSkill in talent.JobOfferTalentSkills)
                    {
                        talentSkill.Skill = await _skillRepository.GetByIdAsync(talentSkill.SkillId);

                        if (talentSkill.Skill is null)
                            return Result<JobOffer>.Invalid(
                                new()
                                {
                                    new()
                                    {
                                        ErrorMessage = ErrorStrings.SkillIdNotFound.Replace("[id]", talentSkill.SkillId.ToString()),
                                        ErrorCode = NameStrings.HttpError_BadRequest
                                    }
                                });

                        talentSkill.CreatedBy = userId;
                        talentSkill.CreatedAt = DateTime.UtcNow;
                    }

                    // Validate the number of vacancies
                    if (!(talent.Vacancie > 0))
                    {
                        return Result<JobOffer>.Invalid(
                            new()
                            {
                                new()
                                {
                                    ErrorMessage = "The number of vacancies must be greater than zero",
                                    ErrorCode = NameStrings.HttpError_BadRequest
                                }
                            });
                    }

                    talent.CreatedBy = userId;
                    talent.CreatedAt = DateTime.UtcNow;
                }

                jobOffer.CreatedAt = DateTime.UtcNow;
                jobOffer.CreatedBy = userId;

                //Insert record
                await _JobOfferRepository.AddAsync(jobOffer);

                //Add navegation entites
                jobOffer.City = existCity;
                jobOffer.Country = existCountry;
                jobOffer.Project = existProject;
                jobOffer.Company = existCompany;

                return Result<JobOffer>.Success(jobOffer);
            }
            catch (Exception ex)
            {
                return Result<JobOffer>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        public async Task<Result<JobOffer>> UpdateJobOfferStatus(int jobOfferId)
        {
            try
            {
                var jobOffer = await _JobOfferRepository.GetByIdAsync(jobOfferId);

                if (jobOffer is null) return Result<JobOffer>.Invalid(new() { new() { ErrorMessage = ErrorStrings.JobOfferNotExist } });

                jobOffer.IsActive = !jobOffer.IsActive;

                await _JobOfferRepository.UpdateAsync(jobOffer);

                return Result<JobOffer>.Success(jobOffer);
            }
            catch (Exception ex)
            {
                return Result<JobOffer>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        public async Task<Result<List<JobOffer>>> FindJobOffersAsync()
        {
            try
            {
                //Get all records
                var jobOffers = await _JobOfferRepository.ListAsync(new FindAllJobOffersSpecification(includeRelations: true));

                return Result<List<JobOffer>>.Success(jobOffers);
            }
            catch (Exception ex)
            {
                return Result<List<JobOffer>>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }
    }
}
