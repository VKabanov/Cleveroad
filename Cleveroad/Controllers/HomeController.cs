using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cleveroad.Models;
using System.Net;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Data.Entity.Core;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Cleveroad.Auxilary_classes;

namespace Cleveroad.Controllers
{

    public class CustomJsonResult : JsonResult
    {
        private const string _dateFormat = "yyyy-MM-dd HH:mm:ss";

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var isoConvert = new IsoDateTimeConverter();
                isoConvert.DateTimeFormat = _dateFormat;
                response.Write(JsonConvert.SerializeObject(Data, isoConvert));
            }
        }
    }
    public class HomeController : Controller
    {
        OrdersContext db = new OrdersContext();

        [HttpGet]
        public ActionResult Index()
        {
             IEnumerable <Order> model = db.Orders;
            ViewBag.Title = "Simple demonstrator";
             return View("Index",model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Session["Order"] = "New";
            Order model = new Order();

            ViewBag.CustomersSelectList = GetCustomersSelectList(); 
            return View("Edit", model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {      
            ViewBag.CustomersSelectList = GetCustomersSelectList(id.Value);

            if (id == null)
            {
                Session["Order"] = "New";
                Order model = new Order();
                return View("Edit", model);
            }
            else {
                Order order = db.Orders.First(o => o.Id == id);

                if (order != null)
                {
                    ViewBag.Title = "Simple Orders DATABASE";
                    Session["Order"]="Old";
                    return View("Edit", order);
                }
                else
                {
                    ModelState.AddModelError("", "Unable to get order with the following ID#" + id + ". Try again, and if the problem persists, see your system administrator.");
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public ActionResult GetCustomerInfo(int id)
        {
            Customer customer = new Customer();
            var result = db.Customers.First(c => c.Id == id);
            if (result != null) return PartialView("SmallCustomer", result);
            else return PartialView("SmallCustomer", new Customer());
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {
                if (ModelState.IsValid)
                {
                try
                {
                    Order inserted_order = new Order();
                    if ((string)(Session["Order"]) == "New")
                    {
                        inserted_order = order;
                        inserted_order.OrderDate = DateTime.Now;
             
                        db.Orders.Add(inserted_order);
                        db.Entry(inserted_order).State = System.Data.Entity.EntityState.Added;
                    }
                    else db.Orders.AddOrUpdate(order);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                catch (OptimisticConcurrencyException)
                {
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
            }
            else ModelState.AddModelError("", "Unable to save changes. POST Edit method have got malformed model.");
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CustomerDetails(int id)
        {
            Customer phone = await db.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (phone != null)
                return View(phone);
            else return View();
        }
        [HttpPost]
        public JsonResult Remove(int? id)
        {
            if (id == null)
            {
                return Json("Order deletion without ID is not possible!");
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Json("Order with ID=" + id + " was not found");
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            try
            {
                order = db.Orders.First();
            }
            catch { }
   
            ViewBag.DeletionResult = Json("Order #" + id + " deleted successfully!");
            return ViewBag.DeletionResult;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";
            return View();
        }


        public SelectList GetCustomersSelectList(int selectedId = 0)
        {
            var result = db.Customers.OrderBy(customer => customer.Name)
                      .Select(customer =>
                          new SelectListItem
                          {
                              Selected = (customer.Id == selectedId),
                              Text = customer.Name,
                              Value = customer.Id.ToString()
                          });
            return new SelectList(result, "Value", "Text");
        }


        [HttpGet]
        public JsonResult GetOrdersData(jQueryDataTableParamModel param)
        {
            db.Configuration.LazyLoadingEnabled = false;
            IEnumerable<Order> model = db.Orders.Include(o => o.Customer);

            var result = new List<string[]>() { };
            foreach (Order order in model)
            {
                result.Add(new string[] { order.Id.ToString(), String.Format("{0:dd'/'MM'/'yyyy}",order.OrderDate),
                                          order.Customer.Name, order.OrderData, order.OrderRoundedWeight.ToString() });
            }

            var jsondata = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = model.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

            return jsondata;
        }

    }
}
