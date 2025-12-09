using day02model.Models;
using Day02Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day02Models.Controllers
{
    public class PeopleController : Controller
    {
        // GET: PeopleController
        public ActionResult Index()
        {
            var peoples = Data.GetPeoples();
            return View(peoples);
        }

        // GET: PeopleController/Details/5
        public ActionResult Details(int id)
        {
            var people = Data.GetPeopleById(id);
            return View(people);
        }

        // GET: PeopleController/Create
        public ActionResult Create()
        {
            var people = new people();
            return View(people);
        }

        // POST: PeopleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(people model)
        {
            if (ModelState.IsValid)
            {
                // upload file
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var fileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\avatar", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        model.Avatar = "images/avatar/" + fileName;
                    }
                }

            }
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: PeopleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PeopleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PeopleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PeopleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}