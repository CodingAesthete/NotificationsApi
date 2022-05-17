using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NotificationWebAPI.Models;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NotificationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly NotificationDBContext dbContext;

        public NotificationController(IConfiguration configuration, NotificationDBContext dbContext)
        {
            _configuration = configuration;
            this.dbContext = dbContext;
        }



        [HttpGet]
        public async Task<IEnumerable> Get()
        {
            var myData = await dbContext.Notification.Select(x => new
            {
                x.Id,
                x.Title
            }).ToListAsync();

            return myData;
        }


        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification not)
        {
            //_context.Notifications.Add(notification);
            //await _context.SaveChangesAsync();
            dbContext.Notification.Add(not);
            await dbContext.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(Get), new { id = not.Id }, not);
        }


        [HttpPut]
        public async Task<IActionResult> Put(int id, Notification not)
        {

            dbContext.Entry(not).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //var notification = await _context.Notifications.FindAsync(id);
            //if (notification == null)
            //{
            //    return NotFound();
            //}

            //_context.Notifications.Remove(notification);
            //await _context.SaveChangesAsync();
            var not = dbContext.Notification.FirstOrDefault(x => x.Id == id);
            if (not == null)
            {
                return NotFound();
            }

            dbContext.Notification.Remove(not);
            dbContext.SaveChanges();

            //var myData = await dbContext.Notification.Select(x => new
            //{
            //    x.Id,
            //    x.Title
            //}).ToListAsync();

            return NoContent();
        }


    }
}