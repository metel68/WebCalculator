using System.Collections.Generic;
using System.Web.Mvc;

namespace Example.Controllers
{
    using System;

    /// <summary>
    /// Calculation controller.
    /// </summary>
    public class CalculatorController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.operations = new List<object>
                                     {
                                         new { value = "Сложение", calculatorName = "Add", operationType = "twoArguments" },
                                         new { value = "Извлечение квадратного корня", calculatorName = "Sqrt", operationType = "oneArgument" }
                                     };
            return View();
        }

        [HttpPost]
        public ActionResult Index(string operationType, string operation, double firstArgument, double? secondArgument)
        {

            TempData["Result"] = new Dictionary<string, object>
                                     {
                                         { "result", 0 }
                                     };
            return RedirectToAction("Result");
        }

        /// <summary>
        /// The result.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Result()
        {
            try
            {
                var result = TempData["result"] as Dictionary<string, object>;
                if (result == null)
                {
                    throw new Exception("No data.");
                }

                foreach (var key in result.Keys)
                {
                    ViewData[key] = result[key];
                }

                TempData.Keep();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);

                ViewBag.Error = true;

                ViewBag.ErrorMessage = e.Message;
            }

            return View();
        }
    }
}