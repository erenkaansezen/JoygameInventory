using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class TeamManagementController : Controller
    {
        public TeamService _teamservice;
        public JoyStaffService _staffmanager;
        public TeamManagementController(TeamService teamservice, JoyStaffService staffmanager)
        {
            _teamservice = teamservice;
            _staffmanager = staffmanager;
        }
        
        [HttpGet]
        public async Task<IActionResult> TeamList(string searchTerm)
        {
            // Arama terimi varsa, arama sonuçlarını alıyoruz
            var teams = await _teamservice.SearchTeam(searchTerm);


            // Eğer arama yapılmamışsa tüm staff'ı alıyoruz
            if (string.IsNullOrEmpty(searchTerm))
            {
                teams = await _teamservice.GetAllTeamsAsync();
            }

            // Arama terimi ViewBag içinde gönderiliyor
            ViewBag.SearchTerm = searchTerm;


            // Arama sonuçlarını view'a gönderiyoruz
            return View(teams);
        }

        [HttpGet]
        public async Task<IActionResult> TeamDetails(int id)
        {
            var teams = await _teamservice.GetTeamByIdAsync(id);





            if (teams != null)
            {
                var userteams = await _teamservice.GetTeamUserAssignmentsAsync(teams.Id);

                var model = new TeamEditViewModel
                {
                    Id = teams.Id,
                    TeamName = teams.TeamName,
                    Teams = userteams,
                };


                return View(model);



            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> TeamCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TeamCreate(TeamEditViewModel model)
        {

            if (!await _teamservice.IsTeamUnique(model.TeamName))
            {
                TempData["ErrorMessage"] = "Bu ünvana sahip başka takım var!";
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.TeamName))
            {

                var teams = new Team
                {
                    TeamName = model.TeamName,
                };
                var result = await _teamservice.AddTeam(teams);
                if (result)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu!";
                    return RedirectToAction("TeamList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulurken bir hata oluştu.");
                    return View(model);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen Belirtilen alanı doldurunuz";
                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> TeamDelete(int id)
        {
            await _teamservice.DeleteTeamAsync(id);
            return RedirectToAction("TeamList", "TeamManagement");
        }

    }
}
