using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportStars.Models;
using System.IO;
using PagedList;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SportStars.Controllers
{

    public class HomeController : Controller
    {

        [Authorize(Roles = "Admin")]
        public ActionResult Orders(String msg, int? page)
        {
            ViewBag.msg = msg;
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {

                

                List<orders> orders = bd.orders.ToList();
                int pagesize = 5;
                int pagenumber = page ?? 1; //Nullable type

                return View(orders.ToPagedList(pagenumber, pagesize));

            }

        }

        //LISTAGEM CUSTOMER
        [Authorize(Roles = "Admin")]
        public ActionResult Customer(String msg, int? page)
        {

            ViewBag.msg = msg;
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {

                List<AspNetUsers> aspNetUsers = bd.AspNetUsers.ToList();
                var roles = new List<string>();
                foreach(var user in aspNetUsers)
                {
                    var t = user.AspNetRoles.First().Name;
                    roles.Add(t);
                }
                ViewBag.roles = roles;
                //var roles = new List<string>();
                //foreach (var user in aspNetUsers)
                //{
                //    string str = "";
                //    foreach (var role in UserManager.GetRoles(user.Id))
                //    {
                //        str = (str == "") ? role.ToString() : str + " - " + role.ToString();
                //    }
                //    roles.Add(str);
                //}
                //ViewBag.roles = roles;

                //string userId = User.Identity.GetUserId();

                //// get user roles
                //List<string> roles = UserManager.GetRoles(userId).ToList();

                int pagesize = 5;
                int pagenumber = page ?? 1; //Nullable type
                ViewBag.page = pagenumber;
                ViewBag.pagesize = pagesize;
                return View(aspNetUsers.ToPagedList(pagenumber, pagesize));

            }



        }

        //GET E POST DE "CRIAR CUSTOMER" -> REGISTER 
        [Authorize(Roles = "Admin")]
        public ActionResult CriarCustomer()
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                List<AspNetRoles> aspNetRoles = bd.AspNetRoles.ToList();
                ViewBag.AspNetRoles = new SelectList(aspNetRoles, "Name", "Name");
            }

            RegisterViewModel novo = new RegisterViewModel();
            return View(novo);
        }

        //// POST: /Account/Register
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CriarCustomer (AspNetUsers novo, HttpPostedFileBase mainfich)
        //{
        //    using(finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
        //    {
        //        bd.AspNetUsers.Add(novo);
        //        bd.SaveChanges();

        //        if(mainfich != null && mainfich.ContentLength > 0 && mainfich.ContentType.Contains("image")){

        //            String caminho = "~/Images.customers/" + novo.Id + System.IO.Path.GetExtension(mainfich.FileName);
        //            mainfich.SaveAs(HttpContext.Server.MapPath(caminho));

        //            novo.images_path = caminho;
        //            bd.SaveChanges();

        //            return RedirectToAction("Customers", new { msg = "Customer inserido com Image!" });
        //        }

        //        return RedirectToAction("Customers", new { msg = "Customer inserido sem Image!" });
        //    }

        //}

        
        private ApplicationUserManager _userManager;

        public void AccountController()
        {
        }
        public void AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            
        }
        

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CriarCustomer(RegisterViewModel novo, String Name)
        {
            
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                //Código verifica se Email já existe
                //var isEmailAlreadyExists = bd.AspNetUsers.Any(x => x.Email == novo.Email);
                //if (isEmailAlreadyExists)
                //{
                //    return RedirectToAction("Customer", new { msg = "Email/User already exists" });
                //}

                var user = new ApplicationUser
                {
                    UserName = novo.Email,
                    Email = novo.Email,
                    

                };

                var result = await UserManager.CreateAsync(user, novo.Password);

                if (result.Succeeded)
                {

                    var isRoleexist = bd.AspNetRoles.Any(e => e.Id.ToLower() == Name.ToLower());

                    if (!isRoleexist)
                    {
                        result = UserManager.AddToRole(user.Id, "Guest");
                    }
                    else
                    {
                        result = UserManager.AddToRole(user.Id, Name);//LINHA ADICIONADA COM O OBJEITVO DE ADICIONAR QUALQUER USER À PERMISSAO "GUEST"


                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    }


                    return RedirectToAction("Customer", "Home", new { msg = "User created!" });
                }

                return RedirectToAction("Customer", new { msg = "Erro" });

            }

        }


        [HttpPost]
        public ActionResult DeleteCustomer(String[] id)
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                if (id != null)
                {
                    foreach (string a in id)
                    {
                        AspNetUsers mortos = bd.AspNetUsers.Find(a);
                        bd.AspNetUsers.Remove(mortos);
                    }
                    try
                    {
                        bd.SaveChanges();
                        return Json(new { msg = "Customer(s) apagado(s) com sucesso!" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        return Json(new { msg = "Não foi possível apagar o Customer" }, JsonRequestBehavior.AllowGet);

                    }
                }

                return Json(new { msg = "Selecione algum elemento" }, JsonRequestBehavior.AllowGet);

            }

        }

        //GET
        [Authorize(Roles = "Admin")]
        public ActionResult EditCustomer(String id)
        {


            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                List<AspNetRoles> aspNetRoles = bd.AspNetRoles.ToList();
                ViewBag.AspNetRoles = new SelectList(aspNetRoles, "Name", "Name");

                AspNetUsers este = bd.AspNetUsers.Find(id);
                if (este != null)
                {
                    return View(este);
                }
                else return RedirectToAction("Customer", new { msg = "Erro, Customer não existe" });
            }
        }

        //POST
        [HttpPost]
        public ActionResult EditCustomer(AspNetUsers novo, HttpPostedFileBase fich, String Name)
        {

            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                
                AspNetUsers este = bd.AspNetUsers.Find(novo.Id);
                if (este != null)
                {
                    este.EmailConfirmed = novo.EmailConfirmed;
                    este.UserName = novo.UserName;
                    este.TwoFactorEnabled = novo.TwoFactorEnabled;
                    este.PhoneNumber = novo.PhoneNumber;
                    este.PhoneNumberConfirmed = novo.PhoneNumberConfirmed;
                    este.user_status = novo.user_status;
                    este.LockoutEnabled = novo.LockoutEnabled;
                    este.AccessFailedCount = novo.AccessFailedCount;
                    //Dava null e não conseguia entrar com contas de customers editados => null
                    //este.SecurityStamp = novo.SecurityStamp;


                    if (!novo.Email.Equals(este.Email))
                    {
                        var isEmailAlreadyExists = bd.AspNetUsers.Any(x => x.Email == novo.Email);
                        if (!isEmailAlreadyExists)
                        {
                            este.Email = novo.Email;
                        }
                        else
                        {
                            return RedirectToAction("Customer", new { msg = "Email/User already exists" });

                        }

                    }



                    var isRoleexist = bd.AspNetRoles.Any(e => e.Id.ToLower() == Name.ToLower());

                    if (isRoleexist)
                    {
                        var t =  UserManager.FindById(este.Id);

                        if(t.Roles.Count == 1)
                        {
                            var s = UserManager.GetRoles(este.Id);
                            UserManager.RemoveFromRole(este.Id, s[0]);
                            UserManager.AddToRoles(este.Id, Name);
                        }
                        else
                        {
                            return RedirectToAction("Customer", new { msg = "User has more than 1 role already exists" });
                        }
                    }



                    if (fich != null && fich.ContentLength > 0 && fich.ContentLength < 4096 && fich.ContentType.Contains("image"))
                    {
                        string caminho = "~/Images.customers/" + este.Id + System.IO.Path.GetExtension(fich.FileName);
                        este.images_path = caminho;
                        fich.SaveAs(HttpContext.Server.MapPath(caminho));
                        bd.SaveChanges();
                        return RedirectToAction("Customer", new { msg = "Customer Editado com sucesso com foto" });

                    }
                    bd.SaveChanges();
                    return RedirectToAction("Customer", new { msg = "Customer Editado com sucesso sem passar foto" });
                }
                return RedirectToAction("Customer", new { msg = "Erro na edição do Customer" });

            }

        }

        //GET ONLY
        [Authorize(Roles = "Admin")]
        public ActionResult DetailCustomer(String id)
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                AspNetUsers m = bd.AspNetUsers.Find(id);
                if (m != null)
                {
                    return View(m);
                }
                else
                {
                    return RedirectToAction("Customer", new { msg = "Erro" });
                }
            }
        }



        [Authorize(Roles = "Admin")]
        public ActionResult Produtos(String msg, int? page)
        {
            ViewBag.msg = msg;
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {

                List<product> products = bd.product.ToList();
                int pagesize = 5;
                int pagenumber = page ?? 1; //Nullable type

                return View(products.ToPagedList(pagenumber, pagesize));

            }

        }

        //GET FK de tabelas associadas a produtos
        [Authorize(Roles = "Admin")]
        public ActionResult CriarProduto()
        {

            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                //Vai buscar a tabela category para usar na dropdownlist
                List<category> categories = bd.category.ToList();
                ViewBag.categories = new SelectList(categories, "category_name", "category_name");

                //Vai buscar a tabela subcategory para usar na DROPDOWNLIST INICIAL
                List<subcategory> subcategories = bd.subcategory.ToList();
                ViewBag.subcategories = new SelectList(subcategories, "subcategory_name", "subcategory_name");

                //Vai buscar a tabela competition para usar na dropdownlist
                List<competition> competitions = bd.competition.ToList();
                ViewBag.competitions = new SelectList(competitions, "competition_name", "competition_name");

                //Vai buscar a tabela team para usar na dropdownlist
                List<team> teams = bd.team.ToList();
                ViewBag.teams = new SelectList(teams, "team_name", "team_name");

                List<sport> sports = bd.sport.ToList();
                ViewBag.sports = new SelectList(sports, "sport_id", "sport_name");

                List<continent> continents = bd.continent.ToList();
                ViewBag.continents = new SelectList(continents, "continent_id", "continent_name");

                List<country> countries = bd.country.ToList();
                ViewBag.countries = new SelectList(countries, "country_id", "country_name");

                //Vai buscar a tabela brands para usar na dropdownlist
                List<brands> brands = bd.brands.ToList();
                ViewBag.brands = new SelectList(brands, "brands_name", "brands_name");


            }
            product novo = new product();
            return View(novo);
        }

        [HttpPost]
        public ActionResult CriarProduto(product novo, HttpPostedFileBase mainfich, HttpPostedFileBase[] fich)
        {

            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                bd.product.Add(novo);
                bd.SaveChanges();

                if (mainfich != null && mainfich.ContentLength > 0 && mainfich.ContentType.Contains("image"))
                {
                    String caminho = "~/Images.products/Main_Images/" + novo.product_id + System.IO.Path.GetExtension(mainfich.FileName);
                    mainfich.SaveAs(HttpContext.Server.MapPath(caminho));

                    novo.product_mainimage = caminho;
                    bd.SaveChanges();

                    if (fich != null)
                    {
                        var count = 0;
                        foreach (HttpPostedFileBase fiche in fich)
                        {
                            if (fiche != null && fiche.ContentLength > 0 && fiche.ContentType.Contains("image"))
                            {
                                String caminha = "~/Images.products/Product_Images/" + novo.product_id + "." + count + System.IO.Path.GetExtension(fiche.FileName);
                                fiche.SaveAs(HttpContext.Server.MapPath(caminha));


                                images imag = new images();
                                imag.product_id = novo.product_id;
                                imag.images_path = caminha;
                                novo.images.Add(imag);
                                //bd.images.Add(imag);
                                count += 1;

                            }


                            bd.SaveChanges();
                        }
                        return RedirectToAction("Produtos", new { msg = "Produto inserido com várias fotos" });
                    }
                    return RedirectToAction("Produtos", new { msg = "Produto inserido com Main Image" });
                }
                return RedirectToAction("Produtos", new { msg = "Produto inserido sem nenhuma foto" });

            }

        }

        private IList<team> GetTeams(int? sportId, int? countryId)
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                if (sportId == null && countryId == null)
                {
                    return bd.team.ToList();
                }
                else if (sportId != null && countryId == null)
                {
                    return bd.team.Where(m => m.sport_id == sportId).ToList();

                }
                else if (sportId == null && countryId != null)
                {
                    return bd.team.Where(m => m.country_id == countryId).ToList();

                }
                else
                {
                    return bd.team.Where(m => m.sport_id == sportId).Where(m => m.country_id == countryId).ToList();

                }

            }
        }

        public JsonResult LoadTeamBytwoId(int? sportId, int? countryId)
        {
            var teamsList = GetTeams(sportId, countryId);
            var teamsData = teamsList.Select(m => new SelectListItem()
            {
                Text = m.team_name,
            });
            return Json(teamsData, JsonRequestBehavior.AllowGet);
        }

        private IList<competition> GetCompetitions(int? sportId, int? continentId, int? countryId)
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                if (sportId == null && continentId == null && countryId == null)
                {
                    return bd.competition.ToList();
                }
                else
                {
                    return bd.competition.Where(m => m.sport_id == sportId).Where(m => m.continent_id == continentId).
                        Where(m => m.country_id == countryId).ToList();
                }

            }
        }

        public JsonResult LoadCompBythreeId(int? sportId, int? continentId, int? countryId)
        {
            var competitionsList = GetCompetitions(sportId, continentId, countryId);
            var competitionsData = competitionsList.Select(m => new SelectListItem()
            {
                Text = m.competition_name,
            });
            return Json(competitionsData, JsonRequestBehavior.AllowGet);
        }



        private IList<country> GetCountries(int? continentId)
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                if (continentId == null)
                {
                    return bd.country.ToList();
                }
                else
                {
                    return bd.country.Where(m => m.continent_id == continentId).ToList();
                }

            }
        }

        public JsonResult LoadCountryBycontinentId(int? continentId)
        {
            var countriesList = GetCountries(continentId);
            var countriesData = countriesList.Select(m => new SelectListItem()
            {
                Value = m.country_id.ToString(),
                Text = m.country_name,
            });
            return Json(countriesData, JsonRequestBehavior.AllowGet);
        }

        private IList<subcategory> Getsubcategories(String categoryId)
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                if (categoryId == "")
                {
                    return bd.subcategory.ToList();
                }
                else
                {
                    return bd.subcategory.Where(m => m.category_name == categoryId).ToList();
                }

            }
        }

        public JsonResult LoadSubCatBycategoryId(String categoryId)
        {
            var subcategoriesList = Getsubcategories(categoryId);
            var subcategoriesData = subcategoriesList.Select(m => new SelectListItem()
            {
                Text = m.subcategory_name,
            });
            return Json(subcategoriesData, JsonRequestBehavior.AllowGet);
        }

        public enum USER_STATUS
        {
            Ativo,
            Inativo
        }

        public enum LETTER_SIZE
        {
            XS,
            S,
            M,
            L,
            XL
        }

        public enum NUMBER_SIZE
        {
            D30,
            C31,

        }

        public enum PRODUCT_TYPE
        {
            Vintage,
            Retro
        }

        public enum PRODUCT_STATUS
        {
            Disponível,
            Indisponível
        }




        [HttpPost]
        public ActionResult DeleteProduto(int[] id)
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {


                if (id != null)
                {
                    foreach (int a in id)
                    {
                        product morto = bd.product.Find(a);
                        bd.product.Remove(morto);
                    }

                    try
                    {
                        bd.SaveChanges();
                        return Json(new { msg = "Registo apagado com sucesso" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        return Json(new { msg = "Não foi possível apagar o registo" }, JsonRequestBehavior.AllowGet);

                    }

                }
                return Json(new { msg = "Selecione algum elemento" }, JsonRequestBehavior.AllowGet);

            }

        }

        //GET
        [Authorize(Roles = "Admin")]
        public ActionResult EditProduto(int? id)
        {


            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {

                //Vai buscar a tabela category para usar na dropdownlist
                List<category> categories = bd.category.ToList();
                ViewBag.categories = new SelectList(categories, "category_name", "category_name");

                //Vai buscar a tabela subcategory para usar na DROPDOWNLIST INICIAL
                List<subcategory> subcategories = bd.subcategory.ToList();
                ViewBag.subcategories = new SelectList(subcategories, "subcategory_name", "subcategory_name");

                //Vai buscar a tabela competition para usar na dropdownlist
                List<competition> competitions = bd.competition.ToList();
                ViewBag.competitions = new SelectList(competitions, "competition_name", "competition_name");

                //Vai buscar a tabela team para usar na dropdownlist
                List<team> teams = bd.team.ToList();
                ViewBag.teams = new SelectList(teams, "team_name", "team_name");

                List<sport> sports = bd.sport.ToList();
                ViewBag.sports = new SelectList(sports, "sport_id", "sport_name");

                List<continent> continents = bd.continent.ToList();
                ViewBag.continents = new SelectList(continents, "continent_id", "continent_name");

                List<country> countries = bd.country.ToList();
                ViewBag.countries = new SelectList(countries, "country_id", "country_name");

                //Vai buscar a tabela brands para usar na dropdownlist
                List<brands> brands = bd.brands.ToList();
                ViewBag.brands = new SelectList(brands, "brands_name", "brands_name");

                product este = bd.product.Find(id);
                if (este != null)
                {
                    return View(este);
                }
                else return RedirectToAction("Produtos", new { msg = "Erro registo não existe" });
            }
        }

        //POST
        [HttpPost]
        public ActionResult EditProduto(int id, product novo, HttpPostedFileBase mainfich)
        {
            //ADICIONEI O ID À PARTE NO EDIT PQ ELE RECEBI SEMPRE ID=0 NO POST METHOD, NÃO SEI PORQUÉ.
            //DESTA FORMA ELE IDENTIFICA O ID E EU IGUALO AO "novo.product_id"
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                novo.product_id = id;
                product este = bd.product.Find(novo.product_id);
                if (este != null)
                {
                    este.product_name = novo.product_name;
                    este.product_price = novo.product_price;
                    este.product_weight = novo.product_weight;
                    este.product_letter_size = novo.product_letter_size;
                    este.product_number_size = novo.product_number_size;
                    este.product_qtd = novo.product_qtd;
                    este.product_comment = novo.product_comment;
                    este.product_year = novo.product_year;
                    este.product_type = novo.product_type;
                    este.product_status = novo.product_status;
                    este.subcategory_name = novo.subcategory_name;
                    este.brands_name = novo.brands_name;
                    este.competition_name = novo.competition_name;
                    este.team_name = novo.team_name;
                    este.product_player_name = novo.product_player_name;

                    if (mainfich != null && mainfich.ContentLength > 0 && mainfich.ContentType.Contains("image"))
                    {
                        string caminho = "~/Images.products/Main_Images" + este.product_id + System.IO.Path.GetExtension(mainfich.FileName);
                        este.product_mainimage = caminho;
                        mainfich.SaveAs(HttpContext.Server.MapPath(caminho));
                    }
                    bd.SaveChanges();
                    return RedirectToAction("Produtos", new { msg = "Registo Editado com sucesso" });
                }
                return RedirectToAction("Produtos", new { msg = "Erro na edição do Registo" });

            }

        }

        //GET ONLY
        [Authorize(Roles = "Admin")]
        public ActionResult DetailProduto(int? id)
        {
            using (finalbootstrap_dbEntities bd = new finalbootstrap_dbEntities())
            {
                product m = bd.product.Find(id);
                if (m != null)
                {
                    return View(m);
                }
                else
                {
                    return RedirectToAction("Produtos", new { msg = "Erro" });
                }
            }
        }



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}