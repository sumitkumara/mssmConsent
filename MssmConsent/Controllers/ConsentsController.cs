using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MssmConsent.Data;
using MssmConsent.Models;
using MssmConsent.Models.ViewModel;
using MssmConsent.ServiceContracts;

namespace MssmConsent.Controllers
{
    public class ConsentsController : Controller
    {
        private readonly MssmConsentContext _context;
        private readonly ISqlService _sqlService;

        public ConsentsController(MssmConsentContext context, ISqlService sqlService, IConfiguration configuration)
        {
            _context = context;
            _sqlService = sqlService;
            _sqlService.ConnectionString = configuration.GetConnectionString("MssmConsentContext");
        }

        // GET: Consents
        public async Task<IActionResult> Index()
        {
            var consents = await _sqlService.ExecuteQueryAsync<ConsentViewModel>("[dbo].[spConsentGetAll]", System.Data.CommandType.StoredProcedure);

            //_context.Consent.Join(_context.ConsentSection,
            //    c => c.ID,
            //    cs => cs.ConsentID,
            //    (c, cs) => new { c.ConsentName });
            //var consents = await _context.Consent.ToListAsync();
            //var consentVM = new List<ConsentViewModel>();
            //foreach (var consent in consents)
            //{
            //    consentVM.Add(new ConsentViewModel
            //    {
            //        ConsentName = consent.ConsentName,
            //        ID = consent.ID,
            //        CreatedBy = consent.CreatedBy,
            //        LastModifiedAt = consent.LastModifiedAt,
            //        LastModifiedBy = consent.LastModifiedBy,

            //    });
            //}
            return View(consents);
        }

        // GET: Consents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var consent = await _context.Consent
            //    .FirstOrDefaultAsync(m => m.ID == id);
            //if (consent == null)
            //{
            //    return NotFound();
            //}

            var parameters = new Dictionary<string, string>()
                {
                    {"ConsentId", id.ToString()},
                    {"@LanguageId", null},
                };

            var consents = await _sqlService.ExecuteQueryAsync<ConsentViewModel>("[dbo].[spConsentGet]", System.Data.CommandType.StoredProcedure, parameters);

            var consent = consents?.FirstOrDefault();
            if (consent == null)
            {
                return NotFound();
            }

           
            return View(consent);
        }

        // GET: Consents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsentViewModel consentVM)
        {
            if (ModelState.IsValid)
            {
                var parameters = new Dictionary<string, string>()
                {
                    {"ConsentName", consentVM.ConsentName},
                    {"CreatedAt" , DateTime.Now.ToString() },
                    {"CreatedBy", consentVM.CreatedBy },
                    {"Title", consentVM.Title },
                    {"Content", consentVM.Content }
                };

                await _sqlService.ExecuteQueryAsync<object>("[dbo].[spAddConsent]", System.Data.CommandType.StoredProcedure, parameters);
                //var consent = new Consent()
                //{
                //    ConsentName = consentVM.ConsentName,
                //    CreatedAt = DateTime.Now,
                //    LastModifiedAt = DateTime.Now,
                //    CreatedBy = consentVM.CreatedBy,
                //    LastModifiedBy = consentVM.CreatedBy
                //};

                //var dbConsent = _context.Add(consent);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consentVM);
        }

        // GET: Consents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameters = new Dictionary<string, string>()
                {
                    {"ConsentId", id.ToString()},
                    {"@LanguageId", null},
                };

            var consents = await _sqlService.ExecuteQueryAsync<ConsentViewModel>("[dbo].[spConsentGet]", System.Data.CommandType.StoredProcedure, parameters);

            var consent = consents?.FirstOrDefault();
            if (consent == null)
            {
                return NotFound();
            }
            var vm = new ConsentViewModel()
            {
                ID = consent.ID,
                ConsentName = consent.ConsentName,
                CreatedBy = consent.CreatedBy,
                CreatedAt = consent.CreatedAt,
                LastModifiedAt = consent.LastModifiedAt,
                LastModifiedBy = consent.LastModifiedBy,
                Title = consent.Title,
                Content = consent.Content
            };

            //var consent = await _context.Consent.FindAsync(id);

            return View(vm);
        }

        // POST: Consents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConsentViewModel consentVM)
        {
            if (id != consentVM.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var parameters = new Dictionary<string, string>()
                {
                    {"ConsentID", consentVM.ID.ToString()},
                    {"ConsentName", consentVM.ConsentName},
                    {"LastModifiedAt" , DateTime.Now.ToString() },
                    {"LastModifiedBy", consentVM.LastModifiedBy },
                    {"Title", consentVM.Title },
                    {"Content", consentVM.Content },
                    {"LanguageID", "1" }
                };

                    await _sqlService.ExecuteQueryAsync<object>("[dbo].[spConsentUpdate]", System.Data.CommandType.StoredProcedure, parameters);

                    //_context.Update(consent);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsentExists(consentVM.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(consentVM);
        }

        // GET: Consents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consent = await _context.Consent
                .FirstOrDefaultAsync(m => m.ID == id);
            if (consent == null)
            {
                return NotFound();
            }

            return View(consent);
        }

        // POST: Consents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parameters = new Dictionary<string, string>()
                {
                    {"ConsentId", id.ToString()},
                };

            await _sqlService.ExecuteQueryAsync<object>("[dbo].[spConsentDelete]", System.Data.CommandType.StoredProcedure, parameters);

            //var consent = await _context.Consent.FindAsync(id);
            //_context.Consent.Remove(consent);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsentExists(int id)
        {
            return _context.Consent.Any(e => e.ID == id);
        }
    }
}
