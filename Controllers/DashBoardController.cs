using Expose_Tracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Expose_Tracker.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ApplicetionDbContext _logger;
        public DashBoardController(ApplicetionDbContext context)
        {
            _logger = context;
        }
        public async Task<ActionResult> Index()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;
            var select = await _logger.Transactions
                .Include(x => x.Category).Where(y => y.Date >= StartDate && y.Date <= EndDate).ToListAsync();
            //Total income
            int TotalIncome = select.Where(i => i.Category.Type == "Income").Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");
            //TotalExpense
            int TotalExpense = select.Where(i => i.Category.Type == "Expense").Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            //Balance
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            int Balance = TotalIncome - TotalExpense;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);
            //Doughnut
            ViewBag.DougnutChartData = select
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.CategoryId).Select(k => new
            {
            CategorytitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
            amount = k.Sum(j => j.Amount),
            formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
            }).OrderByDescending(l => l.amount).ToList();

            //Spline Chart 
            //Income
            List<SplineChartData> IncomeSummary = select.Where(i => i.Category.Type == "Income").GroupBy(j => j.Date).Select(k => new SplineChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                income = k.Sum(l => l.Amount)
            }
            ).ToList();
            //Expense
            List<SplineChartData> ExpenseSummary = select.Where(i => i.Category.Type == "Expense").GroupBy(j => j.Date).Select(k => new SplineChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                expense = k.Sum(l => l.Amount)
            }
            ).ToList();
            string[] Last7days = Enumerable.Range(0, 7).Select(i => StartDate.AddDays(i).ToString("dd-MMM")).ToArray();
            ViewBag.SplineChartData = from day in Last7days
                                      join income
                                      in IncomeSummary on day equals income.day into dayincomeJoined
                                      from income in dayincomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                      day =day,
                                      income = income==null? 0 : income.income,
                                      expense = expense==null? 0 : expense.expense,
                                      };


            ViewBag.RecentTransactions=await _logger.Transactions.Include(i=>i.Category).OrderByDescending(j=>j.Date).Take(5).ToListAsync();

            return View();
        }
    }
    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}
