using System;
using System.Diagnostics;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Phone.Tasks;

namespace UtilityBelt
{
    public class HackTest
    {
        public static bool IsHacked()
        {
            try
            {
                if (Debugger.IsAttached == true) //then WMAppPRHeader.xml file will be added during AppHub certification only! So this has to be skipped during development.
                    return false;

                //scramble WMAppPRHeader.xml file name to make life a little harder in case of reverse engineering
                string fl = "xxx" + "W" + "xxxx" + "M" + "xxxx" + "A" + "xxxx" + "p" + "xxxpxxx" + "PxR" + "xxxxx" + "Hxxxxxxx" + "exxxxxxa" + "xxxx" + "d" + "xxxx" + "xxxxe" + "rxx" + "xxx";
                fl = fl.Replace("x", string.Empty) + "." + "x" + "m" + "l";
                XDocument doc = XDocument.Load(fl); //is hacked, this file is missing or empty!!!
                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("This app was pirated and is not safe to use, please download the original one from Marketplace.");
                MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
                marketplaceDetailTask.Show();

                return true;
            }
        }
    }
}
