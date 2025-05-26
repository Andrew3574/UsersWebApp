using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_Tak4.Data;
using WebApp_Tak4.Models;
using WebApp_Tak4.Services.Auth;
using WebApp_Task4.Repositories;

namespace WebApp_Task4.Controllers
{
    [JwtAuth]
    public class UsersController : Controller
    {
        private readonly UserRepository _userRepository; 

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Block(string[] selectedUserIndexes)
        {
            if (selectedUserIndexes.Length != 0)
            {
                var indexes = selectedUserIndexes.Select(int.Parse).ToArray();
                await _userRepository.BlockById(indexes);
                ViewData["Operation"] = "Successfully Blocked";
            }
            return View("Index", await GetUsers());
        }

        [HttpPost]
        public async Task<IActionResult> UnBlock(string[] selectedUserIndexes)
        {
            if (selectedUserIndexes.Length != 0)
            {
                var indexes = selectedUserIndexes.Select(int.Parse).ToArray();
                await _userRepository.UnBlockById(indexes);
                ViewData["Operation"] = "Successfully Unblocked";
            }
            return View("Index", await GetUsers());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] selectedUserIndexes)
        {
            if (selectedUserIndexes.Length != 0)
            {
                List<User> selectedUsers = await _userRepository.GetById(selectedUserIndexes.Select(int.Parse).ToArray());
                await _userRepository.DeleteRange(selectedUsers);
                ViewData["Operation"] = "Successfully Deleted";
            }
            return View("Index", await GetUsers());
        }

        [HttpPost]
        public async Task<IActionResult> Filter(string filterInput)
        {
            if (!string.IsNullOrEmpty(filterInput))
            {
                var filteredUsers = await _userRepository.FilterByEmail(filterInput);
                return View("Index", filteredUsers);
            }
            return View("Index", await GetUsers());
        }

        private async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetAll();
        }
    }
}
