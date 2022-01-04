using ImageUpload.DAL;
using ImageUpload.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageUpload.Controllers
{
    public class StudentController : Controller
    {
        private readonly DatabaseContext db;
        private string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };

        //for image 
        private readonly IWebHostEnvironment hostEnvironment;

        public StudentController(DatabaseContext _db, IWebHostEnvironment _hostEnvorinment)
        {
            db = _db;
            hostEnvironment = _hostEnvorinment;
        }
        // GET: StudentController
        public ActionResult Index()
        {
            var model = db.Student.ToList();
            return View(model);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            var model = db.Student.Find(id);
            return View(model);
        }

        // GET: StudentController/Create
        public ActionResult Create(int id)
        {
            Student model = new Student();
            if (id == 0)
            {
                return View(model);
            }
            model = db.Student.Find(id);
            return View(model);
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, Student data)
        {
            try
            {
                if (data.Id == 0)
                {
                    if (data.StudentProfile != null)
                    {
                       var formFileName = data.StudentProfile.FileName;
                       //Save image to wwwroot/images
                       string wwwRootPath = hostEnvironment.WebRootPath;
                       string fileName = Path.GetFileNameWithoutExtension(formFileName);
                       string extension = Path.GetExtension(formFileName).ToLower();
                       //checking the extension and size of file
                        IFormFile postedFile = data.StudentProfile;
                        long size = postedFile.Length;
                        if(permittedExtensions.Contains(extension)){
                            if (size <= 1000000)
                            {
                                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                                string filePath = Path.Combine("images/",fileName);
                                data.ProfilePicture = filePath;
                                string path = Path.Combine(wwwRootPath, filePath);
                                using(var fileStream =new FileStream(path, FileMode.Create))
                                {
                                    data.StudentProfile.CopyTo(fileStream);
                                }
                                db.Student.Add(data);
                            }
                            else
                            {
                              TempData["Size Message"] = "<script> alert('File Size should not be more than 1mb')</script>";
                              return View(data);
                            }
                        }
                        else
                        {
                            TempData["Extension Message"] = "<script> alert('File Format not Matched')</script>";
                            return View(data);
                        }
                    }
                }
                else
                {
                    var model = db.Student.Find(id);
                    if (data.StudentProfile != null)
                    {
                        string wwwRootPath = hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(data.StudentProfile.FileName);
                        string extension = Path.GetExtension(data.StudentProfile.FileName);
                        //checking the extension and size of file
                        IFormFile postedFile = data.StudentProfile;
                        long size = postedFile.Length;
                        if (permittedExtensions.Contains(extension))
                        {
                            if (size <= 1000000)
                            {
                                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                                string filePath = Path.Combine("images/", fileName);
                                string path = Path.Combine(wwwRootPath, filePath);
                                using (var fileStream = new FileStream(path, FileMode.Create))
                                {
                                    data.StudentProfile.CopyTo(fileStream);
                                }
                                //Getting old value from database and deleting the old image
                                
                                if (model != null)
                                {
                                    var imagePath = Path.Combine(hostEnvironment.WebRootPath, model.ProfilePicture);
                                    if (System.IO.File.Exists(imagePath))
                                    {
                                        System.IO.File.Delete(imagePath);
                                    }
                                }
                                model.Name = data.Name;
                                model.Address = data.Address;
                                model.ProfilePicture = filePath;
                                db.Student.Update(model);
                            }
                            else
                            {
                                TempData["Size Message"] = "<script> alert('File Size should not be more than 1mb')</script>";
                                return View(model);
                            }
                        }
                        else
                        {
                            TempData["Extension Message"] = "<script> alert('File Format not Matched')</script>";
                            return View(model);
                        }
                        
                    }
                }
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Student student = new Student();
                return View(student);
            }
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
           var model = db.Student.Find(id);
            return View(model);
        }

        // POST: StudentController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                var model = db.Student.Find(id);
                if (model.ProfilePicture != null)
                {
                    var imagePath = Path.Combine(hostEnvironment.WebRootPath,model.ProfilePicture);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    db.Student.Remove(model);
                    db.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }


        //public bool ImageSizeValidation(IFormFile File)
        //{
        //    IFormFile file=File;
        //    long size = file.Length;
            
        //    if (size <= 1000000)
        //    {
        //       return true;
        //    }
        //    else
        //    {
        //        TempData["Size Message"] = "<script> alert('File Size should not be more than 1mb')</script>";
        //        return false;
        //    }
        //}

        //public bool ImageFormatValidation(IFormFile File)
        //{
        //    IFormFile file = File;
        //    string extension = Path.GetExtension(file).ToLower();
        //    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        TempData["Extension Message"] = "<script> alert('File Format not Matched')</script>";
        //        return false;
        //    }

        //}

    }
}
