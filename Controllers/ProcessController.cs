using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace NewWebAPP201910.Controllers
{
    public class ProcessController : Controller
    {
        List<Process> processLits;
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
            return View();
        }

        private void GetProcessList()
        {
            Process.GetProcessesByName()
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