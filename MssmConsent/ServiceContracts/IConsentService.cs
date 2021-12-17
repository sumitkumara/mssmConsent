using Microsoft.AspNetCore.Mvc;
using MssmConsent.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MssmConsent.ServiceContracts
{
    public interface IConsentService
    {
        Task<ConsentViewModel> AddConsent(ConsentViewModel consentVM);
        Task<ConsentViewModel> UpdateConsent(ConsentViewModel consentVM);
        Task<ConsentViewModel> AddConsentLanguage(ConsentViewModel consentVM);
        Task<IEnumerable<ConsentViewModel>> GetConsent(int id, int? languageId);
        Task DeleteConsent(int id);
    }
}
