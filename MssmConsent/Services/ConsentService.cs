using Microsoft.Extensions.Configuration;
using MssmConsent.Models.ViewModel;
using MssmConsent.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MssmConsent.Services
{
    public class ConsentService : IConsentService
    {
        private readonly ISqlService _sqlService;

        public ConsentService(ISqlService sqlService, IConfiguration configuration)
        {
            _sqlService = sqlService;
            _sqlService.ConnectionString = configuration.GetConnectionString("MssmConsentContext");
        }

        public async Task<ConsentViewModel> AddConsent(ConsentViewModel consentVM)
        {
            var parameters = new Dictionary<string, string>()
                {
                    {"ConsentName", consentVM.ConsentName},
                    {"CreatedAt" , DateTime.Now.ToString() },
                    {"CreatedBy", consentVM.CreatedBy },
                    {"Title", consentVM.Title },
                    {"Content", consentVM.Content },
                    {"LanguageID", consentVM.LanguageID.ToString() }
                };

            var createdConsent = await _sqlService.ExecuteQueryAsync<ConsentViewModel>("[dbo].[spAddConsent]", System.Data.CommandType.StoredProcedure, parameters);
            return createdConsent?.FirstOrDefault();
        }

        public async Task<ConsentViewModel> AddConsentLanguage(ConsentViewModel consentVM)
        {
            var parameters = new Dictionary<string, string>()
                {
                  {"ConsentID", consentVM.ID.ToString()},
                    {"LanguageID", consentVM.LanguageID.ToString() },
                    {"LastModifiedAt", DateTime.Now.ToString() },
                    {"LastModifiedBy", consentVM.LastModifiedBy },
                    {"Title", consentVM.Title },
                    {"Content", consentVM.Content },
                };

            var updatedConsent = await _sqlService.ExecuteQueryAsync<ConsentViewModel>("[dbo].[spConsentLanguageAdd]", System.Data.CommandType.StoredProcedure, parameters);
            return updatedConsent?.FirstOrDefault();
        }

        public async Task DeleteConsent(int id)
        {
            var parameters = new Dictionary<string, string>()
                {
                    {"ConsentId", id.ToString()},
                };

            await _sqlService.ExecuteQueryAsync<object>("[dbo].[spConsentDelete]", System.Data.CommandType.StoredProcedure, parameters);
        }

        public async Task<IEnumerable<ConsentViewModel>> GetConsent(int id, int? languageId)
        {
            var parameters = new Dictionary<string, string>()
                {
                    {"ConsentId", id.ToString()},
                    {"@LanguageId", languageId?.ToString()},
                };

            var consents = await _sqlService.ExecuteQueryAsync<ConsentViewModel>("[dbo].[spConsentGet]", System.Data.CommandType.StoredProcedure, parameters);

            return consents;
        }

        public async Task<ConsentViewModel> UpdateConsent(ConsentViewModel consentVM)
        {
            var parameters = new Dictionary<string, string>()
                {
                    {"ConsentID", consentVM.ID.ToString()},
                    {"ConsentName" , consentVM.ConsentName },
                    {"LanguageID", consentVM.LanguageID.ToString() },
                    {"LastModifiedAt", DateTime.Now.ToString() },
                    {"LastModifiedBy", consentVM.LastModifiedBy },
                    {"Title", consentVM.Title },
                    {"Content", consentVM.Content },
                };

            var updatedConsent = await _sqlService.ExecuteQueryAsync<ConsentViewModel>("[dbo].[spConsentUpdate]", System.Data.CommandType.StoredProcedure, parameters);
            return updatedConsent?.FirstOrDefault();
        }
    }
}
