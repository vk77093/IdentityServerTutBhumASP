using IdentityTutBhumMVC.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTutBhumMVC.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext dBContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(ApplicationDbContext dBContext, RoleManager<IdentityRole> roleManager)
        {
            this.dBContext = dBContext;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = dBContext.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Upsert(string roleid)
        {
            if (string.IsNullOrEmpty(roleid))
            {
                return View();
            }
            else
            {
                var obj = dBContext.Roles.FirstOrDefault(u => u.Id == roleid);
                return View(obj);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole roles)
        {
            if(await roleManager.RoleExistsAsync(roles.Name))
            {
                //Error will show
                TempData[SD.Error] = "Role is Already Exist Can;t craete";
                return RedirectToAction(nameof(Index));
            }
            if (roles.Name == null)
            {
                TempData[SD.Error] = "Role Can't be Null at Create";
                return RedirectToAction(nameof(Index));
            }
            if (string.IsNullOrEmpty(roles.Id))
            {
                //crate roles
                await roleManager.CreateAsync(new IdentityRole() { Name = roles.Name });
                TempData[SD.Success] = "Roles Got Created Succesfully";
               // return RedirectToAction(nameof(Index));
            }
            else
            {
                //update
                var obj = dBContext.Roles.FirstOrDefault(u => u.Id == roles.Id);
                if (obj == null)
                {
                    TempData[SD.Error] = "Role Can't be Null at update";
                    return RedirectToAction(nameof(Index));
                }
                obj.Name = roles.Name;
                obj.NormalizedName = roles.Name.ToUpper();
                var result = await roleManager.UpdateAsync(obj);
                TempData[SD.Success] = "Roles Got Updated Successfully";
                
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
