using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRMVCSolution.Models;

namespace SignalRMVCSolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly InstrumentDataContext _db;
        private readonly IEXService _IEXService;

        //public HomeController(IEXService IEXService)
        //{

        //}

        public HomeController(InstrumentDataContext db, IEXService IEXService)
        {
            _IEXService = IEXService;
            _db = db;
        }

        public string OnGet()
        {
            try
            {
                var task = Task.Run(async () => await _IEXService.HttpRequest());
                task.Wait();
                string resposne = task.Result;
                return resposne;
            }
            catch (HttpRequestException)
            {
                //GetIssuesError = true;
                //LatestIssues = Array.Empty<GitHubIssue>();
                return null;
            }
        }

        public IActionResult Index()
        {
            string response = OnGet();
            //string result = await task;
            //await Task.WhenAll(task);
            return View((object)response);
        }

        public IActionResult Privacy(string benchmark)
        {
            DummyDataCls dummyDataCls = new DummyDataCls()
            {
                Benchmark = benchmark
            };
            //return new ContentResult { Content = id.ToString() };
            return View(dummyDataCls);
        }

        public IActionResult InstrumentCreate(string ticker, string benchmark, double price)
        {
            DummyDataCls dummyDataCls = new DummyDataCls()
            {
                Ticker = ticker,
                Benchmark = benchmark,
                Price = price
            };

            _db.DummyDataCls.Add(dummyDataCls);
            _db.SaveChanges();

            //return new ContentResult { Content = id.ToString() };
            return View(dummyDataCls);
        }

        public IActionResult MainPortfolioView(string ticker, string benchmark, double price)
        {
            DummyDataCls dummyDataCls = new DummyDataCls()
            {
                Ticker = ticker,
                Benchmark = benchmark,
                Price = price
            };

            _db.DummyDataCls.Add(dummyDataCls);
            _db.SaveChanges();

            //return new ContentResult { Content = id.ToString() };
            return View("~/Views/PortfolioViews/MainPortfolioView.cshtml", dummyDataCls);
        }

        public IActionResult MainPortfolioRead(int id = 0)
        {
            DummyDataCls dummyDataCls;
            dummyDataCls = _db.DummyDataCls.First<DummyDataCls>();
            List<DummyDataCls> list;

            //using (_db)
            //{
            if (id == 0)
            {
                // Query for all blogs with names starting with B
                var blogs = from b in _db.DummyDataCls
                            where b.Benchmark.StartsWith("123")
                            select b;
                list = blogs.ToList();
                return View("~/Views/PortfolioViews/PortfolioReadRecordList.cshtml", list);
            }
            else
            {
                // Query for the Blog named ADO.NET Blog
                DummyDataCls blogs = _db.DummyDataCls
                                .Where(b => b.Ticker == "fcvsa")
                                .FirstOrDefault();
                List<DummyDataCls> lista = new List<DummyDataCls>();
                lista.Add(blogs);
                return View("~/Views/PortfolioViews/PortfolioReadSingleRecord.cshtml", lista);
            }
//            }

            //return new ContentResult { Content = id.ToString() };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
