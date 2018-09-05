using MeasuremenrsWinService.Models;
using MeasurementsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeviceWebAPI.Controllers
{
    public class DeviceValueController : ApiController
    {
        static Random ran = new Random();
        static List<DeviceData> myList = new List<DeviceData>()
        {
                new DeviceData{Value = ran.Next(600,900) , Date = new DateTime(2018,07,15,12,0,0)},
                new DeviceData{Value =  ran.Next(600,900), Date = new DateTime(2018,07,28,10,0,0)},
                new DeviceData{Value =  ran.Next(600,900), Date = new DateTime(2018,08,07,16,30,0)},
                new DeviceData{Value =  ran.Next(600,900), Date = new DateTime(2018,08,13,9,0,0)},
                new DeviceData{Value =  ran.Next(600,900), Date = new DateTime(2018,08,25,13,30,0)},
        };



        public IEnumerable<DeviceData> GetAll() => myList;



        public IEnumerable<DeviceData> GetFromDate(string date)
        {
            if (DateTime.TryParse(date, out DateTime _date))
            {
                return myList.Where(i => i.Date >= _date);
            }

            else
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Incorrect date format"),
                    ReasonPhrase = "Critical Exception"
                });
        }


        [HttpPost]
        public IHttpActionResult AddData([FromBody] DeviceData data)
        {
            try
            {
                myList.Add(new DeviceData { Value = data.Value, Date = data.Date });
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
           
        }
    }
}
