using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using FoodPortal.Data.Models;

namespace FoodPortal.Controllers {
    public class VendorsController : ApiController
    {
        private FoodOrderingDbEntities db;
        public VendorsController()
        {
            this.db = new FoodOrderingDbEntities();
        }

        public VendorsController(FoodOrderingDbEntities _db) {
            db = _db;
        }

        [HttpPost]
        public JsonResult<string> VendorLogin(Vendor d) {
            var user = db.Vendors.Where(a => a.VendorId.Equals(d.VendorId) && a.Password.Equals(d.Password)).FirstOrDefault();
            return Json(user.VendorId);
        }

        [HttpPost]
        public JsonResult<string> VendorPassword(Vendor d) {
            Vendor user = db.Vendors.Where(a => a.VendorId.Equals(d.VendorId)).FirstOrDefault();
            user.Password = d.Password;
            db.SaveChanges();
            return Json("Password Changed Successfully !!");
        }
    }
}
