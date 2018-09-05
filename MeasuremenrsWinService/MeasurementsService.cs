using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MeasuremenrsWinService.Models;
using MeasurementsLibrary.Models;
using Newtonsoft.Json;
using NLog;

namespace MeasuremenrsWinService
{
    public partial class MeasurementsService : ServiceBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<IDevice> devices = null;
        //Определяем таймер
        private System.Timers.Timer timer;
        public MeasurementsService()
        {
            InitializeComponent();
            devices = new List<IDevice>()
            {
                 new XmlFTPDevice(1),
                 new JsonDevice(2)
            };
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Service is runned");
            //Создаем таймер и выставляем его параметры
            this.timer = new System.Timers.Timer();
            this.timer.Enabled = true;
            //Интервал 
            if (double.TryParse(ConfigurationManager.AppSettings["ServerTimeInterval"], out double interval))
                this.timer.Interval = interval;
            else
                this.timer.Interval = 60000;
            this.timer.Elapsed +=
             new System.Timers.ElapsedEventHandler(this.DoWork);
            this.timer.AutoReset = true;
            this.timer.Start();
        }

        protected override void OnStop()
        {
            this.timer.Stop();
            logger.Info("Service is stopped");
        }

        private void DoWork(object sender, System.Timers.ElapsedEventArgs args)
        {
            foreach (var device in devices)
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    var devData = device.GetData();

                    if (devData?.Count() > 0)
                    {
                        SaveToDB(devData, device.DeviceId);
                        device.SaveLastValueToConfig();
                    }
                });
            }
        }

        private void SaveToDB(IEnumerable<DeviceData> devDataArg, int deviceId)
        {
            try
            {
                logger.Info("Saving to DB ...");
                var readings = new List<Readings>();
                var count = 0;
                using (MeasurementsDBEntities db = new MeasurementsDBEntities())
                {
                    foreach (var data in devDataArg)
                    {
                        readings.Add(new Readings
                        {
                            Id = Guid.NewGuid(),
                            DeviceId = deviceId,
                            Value = data.Value,
                            MeasurementDateTime = data.Date
                        });
                        count++;
                    }
                    db.Readings.AddRange(readings);
                    db.SaveChanges();
                }
                logger.Info($"Data is saved to DB: {count} entry(es).");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

    }
}

