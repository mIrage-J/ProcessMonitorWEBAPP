using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.FileExtensions;
using System.IO;
using NewWebAPP201910.Models;

namespace NewWebAPP201910.Controllers
{
    public class ProcessController : Controller
    {
        List<Process> processList;
        const string noticeName = "Notification";
        const string uidListName = "UIDList";

       
        public  ProcessController()
        {
            ViewData.Add(noticeName,null);
            ViewData.Add(uidListName,null);
        }

        public ActionResult Index()
        {
            GetProcessList();
            ProcessModel[] processes=new ProcessModel[processList.Count];
            for (int i = 0; i < processes.Length; i++)
            {
                processes[i]=new ProcessModel(){ProcessName=processList[i].ProcessName,UID=processList[i].Id};
            }
            ViewData[uidListName]=processes;
            return View();
        }
        private void GetProcessList()
        {
            var configuration=new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json").Build();

            processList=new List<Process>();
            foreach(var process in Process.GetProcessesByName(configuration["ProcessInfo:ProcessName"]))
            {
                processList.Add(process);
            }
            
        }

        public ActionResult LaunchProcess(){
            Process p=new Process();
            p.StartInfo.FileName="iexplore";
            if( p.Start())
                ViewData[noticeName] = $"Process {p.ProcessName} has been started,uid as {p.Id}";
            else
                ViewData[noticeName]=$"Fail to launch process";
            return View("Index");

        }
    }
}