using System.Collections.Generic;
using System.Web.Mvc;
using Calculator.ArraySort;
using Calculator.OneArgument;
using Calculator.TwoArgument;
using Calculator.Validation;

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
                                         new { value = "Вычитание", calculatorName = "Sub", operationType = "twoArguments" },
                                         new { value = "Умножение", calculatorName = "Mult", operationType = "twoArguments" },
                                         new { value = "Деление", calculatorName = "Div", operationType = "twoArguments" },
                                         new { value = "Степерь", calculatorName = "Pow", operationType = "twoArguments" },
                                         new { value = "Корень по любому основанию", calculatorName = "Root", operationType = "twoArguments" },
                                         new { value = "Модуль", calculatorName = "Abs", operationType = "oneArgument" },
                                         new { value = "Синус", calculatorName = "Sin", operationType = "oneArgument" },
                                         new { value = "Косинус", calculatorName = "Cos", operationType = "oneArgument" },
                                         new { value = "Тангенс", calculatorName = "Tan", operationType = "oneArgument" },
                                         new { value = "Арксинус", calculatorName = "Arcsin", operationType = "oneArgument" },
                                         new { value = "Арккосинус", calculatorName = "Arccos", operationType = "oneArgument" },
                                         new { value = "Арктангенс", calculatorName = "Arctan", operationType = "oneArgument" },
                                         new { value = "Извлечение квадратного корня", calculatorName = "Sqrt", operationType = "oneArgument" },
                                         new { value = "Быстрая", calculatorName = "FastSort", operationType = "order" },
                                         new { value = "Медленная", calculatorName = "SlowSort", operationType = "order" }
                                     };
            return View();
        }

        [HttpPost]
        public ActionResult Index(string operationType, string operation, string FirstArgument, string SecondArgument)
        {
            String result;
            try
            {
                var validator = new Validator();
                switch (operationType)
                {
                    case "oneArgument":
                        var argument = validator.ValidateNumber(FirstArgument);
                        var calculator = OneArgmumentFactory.CreateCalculator(operation);
                        result = Convert.ToString(calculator.Calculate(argument));
                        break;
                    case "twoArguments":
                        var firstArgument = validator.ValidateNumber(FirstArgument);
                        var secondArgument = validator.ValidateNumber(SecondArgument);
                        var calculator2 = TwoArgmumentFactory.CreateCalculator(operation);
                        result = Convert.ToString(calculator2.Calculate(firstArgument, secondArgument));
                        break;
                    case "order":
                        var array = validator.ValidateArray(FirstArgument);
                        var calculator3 = ArraySortFactory.CreateCalculator(operation);
                        calculator3.Calculate(array);
                        result = "";
                        for (Int16 i = 0; i < array.Length; i++)
                        {
                            result += Convert.ToString(array[i]);
                            result += ", ";
                        }
                        break;
                    default:
                        result = "Undefined operation";
                        break;
                }
            }
            catch (InvalidOperationException)
            {
                result = "Error: second argument is empty";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            TempData["Result"] = new Dictionary<string, object>
                                     {
                                         { "result", result }
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