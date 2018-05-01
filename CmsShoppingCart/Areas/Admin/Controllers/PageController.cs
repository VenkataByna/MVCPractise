using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PageController : Controller
    {
        // GET: Admin/Page
        public ActionResult Index()
        {
            //Declare the list of PageVM
            List<PageVM> pageList;

            //INIT the 
            using (Db db=new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            return View(pageList);
        }

        //GEt: Admin/Page/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {

            return View();
        }

        //GEt: Admin/Page/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check the model State 
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db=new Db())
            {
                //Declare slug
                string slug;

                //Init pageDTO
                PageDTO dto = new PageDTO();

                //DTO Title 
                dto.Title = model.Title;

                //Check for Slug and set Slug if need be 
                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make Sure Title and Slug are Unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Title == slug))
                {
                    ModelState.AddModelError("", "Title or Slug are already exit");
                    return View(model);

                }

                //DTO The rest 
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //Set the temp Message 
            TempData["sm"] = "You Have Added a new page here";

            //redirect
            return Redirect("AddPage");

        }


    }
}