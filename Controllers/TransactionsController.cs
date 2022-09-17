using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expose_Tracker.Data;
using Expose_Tracker.Models;

namespace Expose_Tracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicetionDbContext _context;

        public TransactionsController(ApplicetionDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
                DateTime StartDate = DateTime.Today.AddDays(-6);
                DateTime EndDate = DateTime.Today;
            var applicetionDbContext = _context.Transactions.Include(t => t.Category);
            return View(await applicetionDbContext.ToListAsync());
        }

        // GET: Transactions/Create
        public IActionResult AddOrEdit(int id = 0)
        {

            PopulateCategories();
            if (id == 0)
            {

                return base.View(new Models.Transaction());
            }
            else
            {
                return View(_context.Transactions.Find(id));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Models.Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                    _context.Add(transaction);

                else
                    _context.Update(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(transaction);

        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'ApplicetionDbContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public void PopulateCategories()
        {
            var CategoryCollection = _context.Categories.ToList();
            Category defaultCategory = new Category() { CategoryId = 0, Title = "Choose a Category" };
            CategoryCollection.Insert(0, defaultCategory);
            ViewBag.Categories = CategoryCollection;

        }
    }
}
