﻿using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace TestProject.Server.Properties
{
    [Serializable]
    public class Settings
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int LowerBoundary { get; set; }
        public int UpperBoundary { get; set; }

        public static Settings GetSettings()
        {
            var serializer = new XmlSerializer(typeof(Settings));
            var sr = new StreamReader("settings.xml");

            return (Settings)serializer.Deserialize(sr);
        }

        protected Settings() { }
        public static void SaveSettings()
        {
            var ser = new XmlSerializer(typeof(Settings));
            using (TextWriter writer = new StreamWriter(Environment.CurrentDirectory + "/settings.xml"))
                ser.Serialize(writer, new Settings
                {
                    IpAddress = "224.100.0.1",
                    Port = 8088,
                    LowerBoundary = int.MinValue,
                    UpperBoundary = int.MaxValue
                });
        }
    }
}