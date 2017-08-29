using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Redis;
using WebApplication1Proba.Models;
using System.IO;

namespace WebApplication1Proba.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Portfolio> portfolios;
            var clientsManager = new BasicRedisClientManager();
            using (IRedisClient redis = clientsManager.GetClient())
            {
                var pRedis = redis.As<Portfolio>();
                portfolios = pRedis.GetAll();
            }
            return View(portfolios);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            Portfolio p = new Portfolio();


            var clientsManager = new BasicRedisClientManager();
            using (IRedisClient redis = clientsManager.GetClient())
            {
                var pRedis = redis.As<Portfolio>();

                p.Id = pRedis.GetNextSequence();
                p.Title = formCollection["Title"];
                p.DateTime = formCollection["DateTime"];
                p.Client = formCollection["Client"];
                p.Description = formCollection["Description"];
                p.Service = formCollection["Service"];

                HttpPostedFileBase file = Request.Files["ImageName"];


                if (file != null && file.ContentLength > 0)
                {
                    string fname = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath(Path.Combine("~/img/portfolio/", fname)));
                    p.ImageName = fname;
                }
                else
                {
                    p.ImageName = "defaultImage.jpg";
                }

                pRedis.Store(p);
            }
            return Redirect("/");
        }

        public ActionResult Edit(long id)
        {
            Portfolio p = new Portfolio();
            var clientsManager = new BasicRedisClientManager();
            using (IRedisClient redis = clientsManager.GetClient())
            {
                var pRedis = redis.As<Portfolio>();
                p = pRedis.GetById(id);
            }
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Client,Service,DateTime,Description,ImageName")] Portfolio portfolio)
        {
            
            var clientsManager = new BasicRedisClientManager();
            using (IRedisClient redis = clientsManager.GetClient())
            {
                Portfolio p = new Portfolio();
                var pRedis = redis.As<Portfolio>();
                p.Id = portfolio.Id;
                p.Client = portfolio.Client;
                p.Title = portfolio.Title;
                p.Service = portfolio.Service;
                p.Description = portfolio.Description;
                p.DateTime = portfolio.DateTime;
                p.ImageName = portfolio.ImageName;
                pRedis.Store(p);
            }

            return RedirectToAction("Index");
        }

        public ActionResult List()
        {
            IEnumerable<Portfolio> portfolios;
            var clientsManager = new BasicRedisClientManager();
            using (IRedisClient redis = clientsManager.GetClient())
            {
                var pRedis = redis.As<Portfolio>();
                portfolios = pRedis.GetAll();
            }
            return View(portfolios);
           
        }

        public ActionResult Delete(long id)
        {
            Portfolio p = new Portfolio();
            var clientsManager = new BasicRedisClientManager();
            using (IRedisClient redis = clientsManager.GetClient())
            { 
                var pRedis = redis.As<Portfolio>();
                p = pRedis.GetById(id);
            }
            return View(p);
        }

        [HttpPost]
        public ActionResult Delete(Portfolio portfolio)
        {
            
            var clientsManager = new BasicRedisClientManager();
            using (IRedisClient redis = clientsManager.GetClient())
            {
                var pRedis = redis.As<Portfolio>();
                pRedis.DeleteById(portfolio.Id);
            }
            return Redirect("/Home/List");
        }
    }
}