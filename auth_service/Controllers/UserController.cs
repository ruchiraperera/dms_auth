using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Encrypt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using auth_service.Models;
using WebApi.Jwt;
using auth_service.Helpers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http.Cors;

namespace auth_service.Controllers
{
    [Authorize]
    [RoutePrefix("users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private fileManagerEntities db = new fileManagerEntities();

        public object HttpContext { get; private set; }
        public object JsonExtension { get; private set; }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

        //GET:api/users/login
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        //[DisableCors]
        public async Task<IHttpActionResult> login(tbl_user _tbluser)
        {
            try
            {
                var password = EncryptionHelper.Encrypt(_tbluser.password);
                var tbl_user = await db.tbl_user.Where(b => b.password == password && b.username == _tbluser.username).FirstOrDefaultAsync();

                if (tbl_user != null)
                {

                    if (tbl_user.active == false) {

                        throw new Exception("Account is deleted.");
                    }

                    var permmission = await db.tbl_permission.Where(c => c.userId == tbl_user.userId).FirstOrDefaultAsync();
                    var role = await db.tbl_userType.Where(i => i.Id == tbl_user.userType).FirstOrDefaultAsync();

                    var jwt = JwtManager.GenerateToken(tbl_user.username, tbl_user.userId, role.type, permmission, 1440);
                    return Ok(new ResponseHandler() { status = true, data = jwt });
                }
                else
                {
                    throw new Exception("login failed, Please try again later...!!");

                }
            }
            catch (Exception ex)
            {
                return Ok(new ResponseHandler() { status = false, error = ex.Message }); ;
            }
        }
        [Route("getAll")]
        [Authorize(Roles = "admin")]
        // GET: api/User
        //[ResponseType(typeof(tbl_user))]
        public IQueryable<tbl_user> Gettbl_user(string userType = "user")
        {
            //var t = User.Identity.Name;
            //var f = User;
            //var u = User.Identity;


            //ClaimsIdentity user = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = user.Claims;

            //var y = claims.Where(c => c.Type == "permission").FirstOrDefault().Value;
            //var y1 = claims.Where(c => c.Type == "userId").FirstOrDefault().Value;
            //var y2 = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
            //dynamic _obj = JsonExtensions.DeserializeFromJson<tbl_permission>(y);

            tbl_userType tbl_type = db.tbl_userType.Where(x => x.type.ToLower() == userType.ToLower()).FirstOrDefault();
            return db.tbl_user.Where(x=> x.userType == tbl_type.Id);
        }

        // GET: api/User/5

        [ResponseType(typeof(tbl_user))]
        public async Task<IHttpActionResult> Gettbl_user(int id)
        {
            tbl_user tbl_user = await db.tbl_user.FindAsync(id);
            if (tbl_user == null)
            {
                return NotFound();
            }

            return Ok(tbl_user);
        }

        // PUT: api/User/5
        [Route("updateUser")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_user(int id, tbl_user tbl_user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_user.userId)
            {
                return BadRequest();
            }

            db.Entry(tbl_user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_userExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        // POST: api/User
        [Route("registration")]
        [AllowAnonymous]
        //[ResponseType(typeof(tbl_user))]
        public async Task<IHttpActionResult> Posttbl_user(userModel _usermodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                tbl_user _user = db.tbl_user.Where(t => t.username == _usermodel.user.username).FirstOrDefault();
                if (_user != null)
                {
                    throw new Exception("User is existing");
                }

                //validation .....
                if (string.IsNullOrEmpty(_usermodel.user.password) || string.IsNullOrEmpty(_usermodel.user.password))
                    throw new Exception("Please fill all the fields");

                //encrypt password....
                _usermodel.user.password = EncryptionHelper.Encrypt(_usermodel.user.password);
                _usermodel.user.active = true;

                db.tbl_user.Add(_usermodel.user);
                await db.SaveChangesAsync();

                _usermodel.permission.userId = _usermodel.user.userId;
                db.tbl_permission.Add(_usermodel.permission);

                await db.SaveChangesAsync();

                Dictionary<string, string> _dic = new Dictionary<string, string>();
                _dic.Add("userId", _usermodel.user.userId.ToString());


                return Ok(new ResponseHandler() { status = true, data=_dic });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseHandler() { status = false, error = ex.Message });
            }

        }
        // DELETE: api/User/5
        [Route("deleteUser")]
        [ResponseType(typeof(tbl_user))]
        public async Task<IHttpActionResult> Deletetbl_user(int id)
        {
            tbl_user tbl_user = await db.tbl_user.FindAsync(id);
            if (tbl_user == null)
            {
                return NotFound();
            }

            db.tbl_user.Remove(tbl_user);
            await db.SaveChangesAsync();

            return Ok(tbl_user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_userExists(int id)
        {
            return db.tbl_user.Count(e => e.userId == id) > 0;
        }
    }
}