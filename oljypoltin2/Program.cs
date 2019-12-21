/*

Öljypolttimon päälläoloaikaa seuraava järjestelmä, joka tekee taustalla tekstitiedostoon lokia ajasta ja kulutuksesta ( 4L / tunti)
Sarjadata tulee arduinolta, joka lukee öljypolttimon päälläoloa REED -putkella polttimon magneettiventtiilistä.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace oljypoltin2
{
    class Program
    {
        static SerialPort myport;
        
        static void Main(string[] args)
        {
            int lukitus = 0;
            float kulutus = 0;
            //Sarjaportin määritys
            myport = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            myport.Open();
            
            Stopwatch timer = new Stopwatch();
            Stopwatch timer2 = new Stopwatch();
            Stopwatch timer3 = new Stopwatch();

            //Lokitiedoston polku .txt
            string path = @"C:\SakuJ-Oljylammittimen-seuranta_12-2019-master\Loki.txt";

            while (true)
            {
                //Thread.Sleep(1000);

                string in_data = myport.ReadLine();

                if (!int.TryParse(in_data, out int data)) ;

                if (lukitus == 0 && data == 1)
                {
                    timer3.Stop();
                    TimeSpan ts3 = timer3.Elapsed;

                    string appendText = DateTime.Now.ToString() + "  Väli: " + ts3 ;
                    File.AppendAllText(path, appendText);

                    timer.Start();
                    timer2.Start();

                    

                    lukitus = 1;
                }

                if(lukitus == 1 && data == 0)
                {
                    timer.Stop();
                    timer2.Stop();

                    timer3.Reset();
                    timer3.Start();

                    TimeSpan ts = timer.Elapsed;
                    TimeSpan ts2 = timer2.Elapsed;

                    string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                    
                    if(ts.Days > 0)
                    {
                        kulutus = (((ts.Days * 24) + ts.Hours) + (ts.Minutes / 60)) * 4f;
                    }
                    else if(ts.Hours > 0)
                    {
                        kulutus = (ts.Hours + (ts.Minutes / 60)) * 4f;
                    }
                    else
                    {
                        kulutus = (ts.Minutes / 60) * 4f;
                    }
                    
                    string appendText = "  Kesto: " + ts2 + "  Kok.Kesto: " + elapsedTime + "   Kulutus: " + kulutus + Environment.NewLine;
                    File.AppendAllText(path, appendText);

                    timer2.Reset();

                    lukitus = 0;
                }
            }
        }
    }
}
