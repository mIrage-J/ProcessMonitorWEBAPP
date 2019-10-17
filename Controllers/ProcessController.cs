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
        IConfigurationRoot configuration { get { return BuildConfiguration(); } }

        public ProcessController()
        {
            ViewData.Add(new KeyValuePair<string, object>(uidListName, null));
            ViewData.Add(new KeyValuePair<string, object>(noticeName, null));
        }

        public ActionResult Index()
        {
            GetProcessList();
            ProcessModel[] processes = new ProcessModel[processList.Count];
            for (int i = 0; i < processes.Length; i++)
            {
                processes[i] = new ProcessModel() { ProcessName = processList[i].ProcessName, UID = processList[i].Id };
            }
            ViewData[uidListName] = processes;
            return View("Index");
        }
        private void GetProcessList()
        {

            processList = new List<Process>();
            foreach (var process in Process.GetProcessesByName(configuration["ProcessInfo:ProcessName"]))
            {
                processList.Add(process);
            }

        }

        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.Development.json").Build();
        }

        public ActionResult LaunchProcessResult()
        {
            LaunchProcess();
            return Index();
        }

        private void LaunchProcess()
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = configuration["ProcessInfo:ProcessFilePath"];
                if (p.Start())
                    ViewData[noticeName] = $"Process {p.ProcessName} has been started,uid as {p.Id}";
                else
                    ViewData[noticeName] = $"Fail to launch process";
            }
            catch (Exception ex)
            {
                ViewData[noticeName] = "Launching process failed with message " + ex.Message;
            }
        }

        public ActionResult KillProcessResult(int uid)
        {
            KillProcess(uid);
            return Index();
        }
        
        private void KillProcess(int uid)
        {
            try
            {
                Process tokill = Process.GetProcessById(uid);
                tokill.Kill();
                if (tokill.WaitForExit(3000))
                    ViewData[noticeName] = $"Process {tokill.Id} has been killed";
                else
                    ViewData[noticeName] = $"Fail to kill process {tokill.Id} , time out expired in 3 seconds";
            }
            catch (Exception ex)
            {
                ViewData[noticeName] = "Fail to kill process with message" + ex.Message;
            }
        }
    }
}
