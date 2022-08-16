using _5._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ControllersApp.Util; // пространство имен класса HtmlResult
//using System.IO.dll;
using System.IO; // для работы с СОМ портом
using System.IO.Ports;




namespace _5._1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static bool isPortOpen = false;
        private static SerialPort port;

        const long KEY_TEMP = 0x31;
        const long KEY_LIGHT = 0x32;
        const long KEY_REGIST = 0x33;

        private double tempVolts = 0;
        private string lightPounts = "";

        int flag = 1;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public string GetData()
        {
            tempVolts = 1.0;
            lightPounts = "2.0";
            if (isPortOpen == false)
            {
                port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
                //port.Handshake = Handshake.RequestToSend;
                port.RtsEnable = true;
                port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                port.Open();
            }
            flag = 1;
            port.Write(BitConverter.GetBytes(KEY_TEMP), 0, BitConverter.GetBytes(KEY_TEMP).Length);
            System.Threading.Thread.Sleep(20000);

            flag = 2;
            port.Write(BitConverter.GetBytes(KEY_REGIST), 0, BitConverter.GetBytes(KEY_REGIST).Length);
            System.Threading.Thread.Sleep(20000);
            return "" + tempVolts + " " + lightPounts;
            
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int bytes = port.BytesToRead;
            byte[] buffer = new byte[bytes];
            port.Read(buffer, 0, bytes);
            for (int i = 0; i < buffer.Length; i++)
            {
                string dex = "" + buffer[i];
                string hex = Convert.ToString(Convert.ToInt32(dex, 10), 16);
                if (flag == 1)
                {

                    tempVolts = buffer[i] / 100.0d;

                }
                else if (flag == 2)
                {

                    if (buffer[i] > 100)
                        lightPounts = "100";
                    else
                        lightPounts = dex;


                }


            }

        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
