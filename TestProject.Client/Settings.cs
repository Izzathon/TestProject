using System;
using System.IO;
using System.Xml.Serialization;

namespace TestProject.Client
{
    [Serializable]
    public class Settings
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int Delay { get; set; }

        public static Settings GetSettings()
        {
            var serializer = new XmlSerializer(typeof(Settings));
            var sr = new StreamReader("Settings.xml");
            return (Settings)serializer.Deserialize(sr);
        }

        //Used to create settings file first time.
        public static void SaveSettings()
        {
            var ser = new XmlSerializer(typeof(Settings));
            using (TextWriter writer = new StreamWriter(Environment.CurrentDirectory + "/Settings.xml"))
            {
                ser.Serialize(writer, new Settings
                {
                    IpAddress = "224.100.0.1",
                    Port = 8088,
                    Delay = 1000
                });
            }
        }
    }
}