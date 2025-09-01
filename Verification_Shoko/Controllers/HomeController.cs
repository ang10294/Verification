using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Verification_Shoko.Controllers
{
    public class HomeController : Controller
    {

        Logger _logger = LogManager.GetCurrentClassLogger();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Result = (TempData != null && TempData["Result"] != null) ? TempData["Result"] : null;

            return View();
        }

        [HttpPost]
        public ActionResult WriteToFile(FormCollection model)
        {
            string _result = string.Empty;

            string _card = model?["card"]?.ToString();
            string _phone = model?["phone"]?.ToString();

            if (!string.IsNullOrEmpty(_card) && !string.IsNullOrEmpty(_phone))
            {
                try
                {

                    string _date = DateTime.Now.ToString("yyyy-MM-dd") + ("  PlasTekShokoVerification");

                    string path = Server.MapPath("/Content/file/" + _date + ".txt");

                    string createForm = string.Format("{0},{1}", _card, _phone) + Environment.NewLine;

                    if (!System.IO.File.Exists(path))
                    {
                        System.IO.File.WriteAllText(path, createForm, Encoding.UTF8);
                    }
                    else
                    {
                        System.IO.File.AppendAllText(path, createForm, Encoding.UTF8);
                    }

                    _result = "Спасибо за предоставленную информацию. <br /> С Вами свяжутся специалисты для подтверждения введенных данных";

                    _logger.Info("[Type][WriteToFile],[Card][" + _card + "],[Phone][" + _phone + "]");
                }
                catch (Exception ex)
                {
                    _result = "Извините произошла ошибка, попробуйте записать данные позже";

                    _logger.Error("[Type][WriteToFile],[Card][" + _card + "],[Phone][" + _phone + "],[Mess][" + ex.Message + "]");
                }
            }

            if (_result == null)
            {
                _result = "Ошибка: некорректная ссылка, повторите операцию!";
            }

            TempData["Result"] = _result;

            return RedirectToAction("Index");

        }
    }
}
