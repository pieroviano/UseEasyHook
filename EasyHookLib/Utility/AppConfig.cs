using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace EasyHookLib.Utility
{
    /// <summary>
    /// This class reads setting from the app.config xml file 
    /// </summary>
    /// <seealso cref="string" />
    public sealed class AppConfig : Dictionary<string, string>
    {
        private static AppConfig instance;
        private static readonly object Padlock = new object();
        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public static string Location { get; }

        /// <summary>
        /// Initializes the <see cref="AppConfig"/> class.
        /// </summary>
        static AppConfig()
        {
            Location = Assembly.GetExecutingAssembly().Location;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="AppConfig"/> class from being created.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private AppConfig()
        {
            //internalDict = new System.Collections.Generic.Dictionary<string, string>();
            var combine = string.IsNullOrEmpty(Location) ?
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                    throw new InvalidOperationException(), @"app.config") : Location+".config";
            var streamReader = File.OpenText(combine);
            var oldAppConfig = streamReader.ReadToEnd();
            var xElement = XElement.Parse(oldAppConfig);
            var xElements = xElement.Element("appSettings")?.Elements("add").ToArray();

            if (xElements != null)
            {
                for (var i = 0; i < xElements.Length; i++)
                {
                    var element = xElements[i];
                    var keyName = element.Attribute("key").Value;
                    var keyValue = element.Attribute("value").Value;
                    if (ContainsKey(keyName))
                    {
                        this[keyName] = keyValue;
                    }
                    else
                    {
                        Add(keyName, keyValue);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static AppConfig Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (instance == null)
                    {
                        instance = new AppConfig();
                    }
                    return instance;
                }
            }
        }
    }
}