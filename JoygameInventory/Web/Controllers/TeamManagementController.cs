using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    [Authorize]
    public class TeamManagementController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IJoyStaffService _staffManager;

        public TeamManagementController(ITeamService teamService, IJoyStaffService staffManager)
        {
            _teamService = teamService;
            _staffManager = staffManager;
        }

        [HttpGet]
        public async Task<IActionResult> TeamList(string searchTerm)
        {
            var teams = await _teamService.SearchTeam(searchTerm);

            if (string.IsNullOrEmpty(searchTerm))
            {
                teams = await _teamService.GetAllTeamsAsync();
            }

            ViewBag.SearchTerm = searchTerm;

            return View(teams);
        }

        [HttpGet]
        public async Task<IActionResult> TeamDetails(int id)
        {
            var teams = await _teamService.GetTeamByIdAsync(id);

            if (teams != null)
            {
                var userTeams = await _teamService.GetTeamUserAssignmentsAsync(teams.Id);

                var model = new TeamEditViewModel
                {
                    Id = teams.Id,
                    TeamName = teams.TeamName,
                    Teams = userTeams,
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
            if (!await _teamService.IsTeamUnique(model.TeamName))
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
                var result = await _teamService.AddTeam(teams);
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
        public async Task<IActionResult> TeamDetails(TeamEditViewModel model)
        {
            if (model.TeamName == null)
            {
                TempData["ErrorMessage"] = "Lütfen Gerekli Alanları Doldurunuz";
                return View(model);
            }

            if (!await _teamService.IsTeamUnique(model.TeamName))
            {
                TempData["ErrorMessage"] = "Bu ünvana sahip başka takım var!";
                return View(model);
            }

            var teamToUpdate = await _teamService.GetTeamByIdAsync(model.Id);

            if (teamToUpdate != null)
            {
                teamToUpdate.TeamName = model.TeamName;

                await _teamService.TeamUpdateAsync(teamToUpdate);

                return RedirectToAction("TeamDetails", new { id = model.Id });
            }

            TempData["ErrorMessage"] = "Team not found!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> TeamDelete(int id)
        {
            await _teamService.DeleteTeamAsync(id);
            return RedirectToAction("TeamList", "TeamManagement");
        }
    }
}
