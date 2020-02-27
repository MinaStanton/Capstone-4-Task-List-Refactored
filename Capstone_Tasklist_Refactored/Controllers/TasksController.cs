﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Capstone_Tasklist_Refactored.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Tasklist_Refactored.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {

        private readonly IdentityTaskListDbContext _context;

        public TasksController(IdentityTaskListDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Index()
        {

            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.TaskOwnerId == id).ToList();
            return View(thisUsersTasks);
        }


        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTask(Tasks newTask)
        {
           // newTask.TaskOwnerId = "1";
            newTask.TaskOwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            

            if (ModelState.IsValid)
            {
                _context.Tasks.Add(newTask);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult EditTask(int id)
        {
            
            Tasks found = _context.Tasks.Find(id); 
            if(found != null)
            {
                return View(found);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult EditTask(Tasks editedTask)
        {
           //editedTask.TaskOwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           Tasks dbTask = _context.Tasks.Find(editedTask.Id);
         


            if (ModelState.IsValid)
            {
                dbTask.TaskDescription = editedTask.TaskDescription;
                dbTask.DueDate = editedTask.DueDate;
                dbTask.Completed = editedTask.Completed; 

                //may need to delete if this doesn't 
                _context.Entry(dbTask).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(dbTask);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTask(int id)
        {
            Tasks found = _context.Tasks.Find(id);

            if(found != null)
            {
                _context.Tasks.Remove(found);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

    }
}