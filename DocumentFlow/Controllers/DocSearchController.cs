using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlow.Controllers
{
	public class DocSearchController : Controller
	{
		// 2023-06-24 Add
		// GET: DocSearchController
		public ActionResult Search()
		{
			return View();
		}
		// GET: DocSearchController
		public ActionResult Index()
		{
			return View();
		}

		// GET: DocSearchController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: DocSearchController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: DocSearchController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
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

		// GET: DocSearchController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: DocSearchController/Edit/5
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

		// GET: DocSearchController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: DocSearchController/Delete/5
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
