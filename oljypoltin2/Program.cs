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

            //Sarjaportin määritys
            myport = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            myport.Open();
            
            Stopwatch timer = new Stopwatch();

            //Lokitiedoston polku .txt
            string path = @"C:\SakuJ-Oljylammittimen-seuranta_12-2019-master\Loki.txt";

            while (true)
            {
                Thread.Sleep(1000);

                string in_data = myport.ReadLine();

                if (!int.TryParse(in_data, out int data)) ;

                if (lukitus == 0 && data == 1)
                {
                    string appendText = DateTime.Now.ToString() + " PÄÄLLE " ;
                    File.AppendAllText(path, appendText);
                    timer.Start();

                    lukitus = 1;
                }

                if(lukitus == 1 && data == 0)
                {
                    timer.Stop();
                    TimeSpan ts = timer.Elapsed;
                    string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                    float kulutus = ts.Hours * 4f;
                    
                    string appendText = DateTime.Now.ToString() + "Päälläoloaika: " + elapsedTime + "  Kokonaiskulutus: " + kulutus + " Litraa" + Environment.NewLine;
                    File.AppendAllText(path, appendText);

                    lukitus = 0;
                }
            }
        }
    }
}
