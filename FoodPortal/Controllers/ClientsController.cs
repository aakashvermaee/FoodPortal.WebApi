using FoodPortal.Data.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace FoodPortal.Controllers {
    //[EnableCors(origins: "*", methods: "*", headers: "*")]
    public class ClientsController : ApiController
    {
        private FoodOrderingDbEntities db;

        public ClientsController()
        {
            this.db = new FoodOrderingDbEntities();
        }

        public ClientsController(FoodOrderingDbEntities _db) {
            db = _db;
        }

        [HttpPost]
        public JsonResult<string> PostClient(Client client) {
            try {
                db.Clients.Add(client);
                db.SaveChanges();
                return Json("success");
            } catch(Exception e) {
                Console.Write(e);
            }
            return Json("error");
        }

        [HttpPost]
        public JsonResult<string> UserExists(Client c) {
            if (c.ClientId == null || c.ClientId.Equals(" "))
                return Json("");

            var clientId = db.Clients
                             .Where(a => a.ClientId.Equals(c.ClientId))
                             .FirstOrDefault();
            if (clientId != null)
                return Json("Username Taken!");
            else
                return Json("Username can be used!");
        }

        [HttpPost]
        public JsonResult<string> EmailExists(Client c) {
            var clientEmail = db.Clients
                                .Where(emailExists => emailExists.Email.Equals(c.Email))
                                .FirstOrDefault();
            if (clientEmail != null)
                return Json("Email ID Exists!");
            else
                return Json("Email ID can be used");
        }

        [HttpPost]
        public JsonResult<string> ClientLogin(Client client) {
            var user = db.Clients.Where(a => a.ClientId.Equals(client.ClientId) && a.Password.Equals(client.Password)).FirstOrDefault();
            return Json(user.ClientId);
        }

        [HttpPost]
        public JsonResult<string> ClientPassword(Client d) {
            Client user = db.Clients.Where(a => a.ClientId.Equals(d.ClientId)).FirstOrDefault();
            user.Password = d.Password;
            db.SaveChanges();
            return Json("Password Changed Successfully !!");
        }

        [HttpPost]
        public JsonResult<string> DeleteProduct(Product d) {
            Product product = db.Products.Where(a => a.ProductId.Equals(d.ProductId)).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return Json("Product deleted successfully !!");
        }
    }
}
