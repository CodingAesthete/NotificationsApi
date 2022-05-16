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

        public NotificationController(IConfiguration configuration,NotificationDBContext dbContext)
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
        public JsonResult Post(Notification not)
        {
            string query = @"
                           insert into dbo.Notification
                           values (@Title)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("NotificationAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Title", not.Title);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(Notification not)
        {
            string query = @"
                           update dbo.Notification
                           set Title= @Title
                            where Id=@Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("NotificationAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", not.Id);
                    myCommand.Parameters.AddWithValue("@Title", not.Title);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                           delete from dbo.Notification
                            where Id=@Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("NotificationAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }


    }
}