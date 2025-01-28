using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserManagement.Api.Models;
using UserManagement.Api.Services;
using UserManagement.Application.DTOs;
using UserManagement.Application.ViewModel;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Api.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [ServiceFilter<SessionExpiryFilters>]
        public IActionResult Index()
        {
            List<UserResponse> model = new List<UserResponse>();
            model = _userRepository.GetUserList();
            return View(model);
        }
        [ServiceFilter<SessionExpiryFilters>]
        public IActionResult AddUpdateUser(int? id)
        {
            AddUpdateUserViewModel model = new AddUpdateUserViewModel();
            List<RoleResponse> res = _userRepository.GetAllRoles().ToList();
            model.Roles = GetRolesForSelectList(model.RoleId);
            if (id == null)
                return View(model);
            UserResponse? user = _userRepository.GetUserById(id);
            model.Id = user.UserId;
            model.Email = user.Email;
            model.Gender = user.Gender;
            model.Username = user.Username;
            model.RoleId = user.RoleId;
            model.IsActive = user.IsActive;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddUpdateUser(AddUpdateUserViewModel model)
        {
            model.Roles = new List<SelectListItem>();
            if (ModelState.IsValid)
            {
                UserCommon user = new UserCommon
                {
                    Username = model.Username,
                    Email = model.Email,
                    RoleId = model.RoleId,
                    IsActive = model.IsActive,
                    Gender = model.Gender
                };
                if (model.Id == null)
                {

                }
                else
                {
                    user.UserId = model.Id;
                    DbResponse res = _userRepository.UpdateUser(user);
                    if(res.Status_Code != "0")
                    {
                        List<SelectListItem> roleRes = GetRolesForSelectList(model.RoleId);
                        model.Roles = roleRes;

                        TempData["Message"] = res.Msg;
                        return View(model);
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public List<SelectListItem> GetRolesForSelectList(string? roleId)
        {
            var roles = _userRepository.GetAllRoles().ToList();
            var items = roles.Select(role => new SelectListItem
            {
                Value = role.Id.ToString(),
                Text = role.Name,
                Selected = roleId != null && role.Id.ToString() == roleId // Set the selected property based on roleId
            }).ToList();

            return items;
        }

    }

}


