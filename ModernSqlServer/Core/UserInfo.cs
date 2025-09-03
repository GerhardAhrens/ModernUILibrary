//-----------------------------------------------------------------------
// <copyright file="UserInfo.cs" company="Lifeprojects.de">
//     Class: UserInfo
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>19.10.2021</date>
//
// <summary>
// Die Klasse Gibt zum aktuellenBenutzer verschiedene Informationen zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernSqlServer.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// Die Klasse stellt Eigenschaften für einen Timestamp auf einer Tabelle zur Verfügung
    /// </summary>
    public class UserInfo
    {
        private readonly string currentUser;
        private readonly string currentDomain;
        private readonly DateTime currentTime;

        private UserInfo()
        {
            this.currentUser = Environment.UserName;
            this.currentDomain = Environment.UserDomainName;
            this.currentTime = new DateTime(1900, 1, 1, 0, 0, 0);
        }

        private UserInfo(string user)
        {
            if (user.Contains("\\") == true)
            {
                this.currentUser = user.Split('\\')[0];
                this.currentDomain = user.Split('\\')[1];
            }
            else
            {
                this.currentUser = user;
                this.currentDomain = Environment.UserDomainName;
            }
        }

        private UserInfo(string user, DateTime time)
        {
            if (user.Contains("\\") == true)
            {
                this.currentUser = user.Split('\\')[0];
                this.currentDomain = user.Split('\\')[1];
            }
            else
            {
                this.currentUser = user;
                this.currentDomain = Environment.UserDomainName;
            }

            this.currentTime = time;
        }

        private UserInfo(string user, string time, string dateFormat = "")
        {
            if (user.Contains("\\") == true)
            {
                this.currentUser = user.Split('\\')[0];
                this.currentDomain = user.Split('\\')[1];
            }
            else
            {
                this.currentUser = user;
                this.currentDomain = Environment.UserDomainName;
            }

            if (string.IsNullOrEmpty(dateFormat) == true)
            {
                dateFormat = "dd.MM.yyyy";
            }

            DateTime outDateTime;
            CultureInfo deDE = Thread.CurrentThread.CurrentCulture;
            bool isOk = DateTime.TryParseExact(time, dateFormat, deDE, DateTimeStyles.None, out outDateTime);
            if (isOk == true)
            {
                this.currentTime = outDateTime;
            }
        }

        /// <summary>
        /// Die Methode gibt eine Instanz der Klasse UserInfo zurück
        /// </summary>
        /// <returns></returns>
        public static UserInfo TS()
        {
            return new UserInfo();
        }

        public static UserInfo TS(string user)
        {
            return new UserInfo(user);
        }

        public static UserInfo TS(string user, DateTime time)
        {
            return new UserInfo(user, time);
        }

        public static UserInfo TS(string user, string time)
        {
            return new UserInfo(user,time);
        }

        /// <summary>
        /// Die Eigenschaft gibt die aktuelle Zeit zurück
        /// </summary>
        public DateTime CurrentTime
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Die Eigenschaft gibt ein Default Datum '1.1.1900 00:00:00' zurück
        /// </summary>
        public DateTime DefaultDateTime
        {
            get { return this.currentTime; }
            private set { value = this.currentTime; }
        }

        /// <summary>
        /// Die Eigenschaft gibt die Bezeichnung der Domain und User zurück
        /// </summary>
        public string CurrentDomainUser
        {
            get { return $"{this.currentDomain}\\{this.currentUser}"; }
        }

        /// <summary>
        /// Die Eigenschaft gibt die Bezeichnung der aktuellen Domain zurück
        /// </summary>
        public string CurrentDomain
        {
            get { return this.currentDomain; }
            private set { value = this.currentDomain; }
        }

        /// <summary>
        /// Die Eigenschaft gibt die Bezeichnung des aktuellen User zurück
        /// </summary>
        public string CurrentUser
        {
            get { return this.currentUser; }
            private set { value = this.currentUser; }
        }

        /// <summary>
        /// Die Eigenschaft gibt die Bezeichnung des aktuellen PC (MachineName) zurück
        /// </summary>
        public string CurrentMachineName
        {
            get { return Environment.MachineName; }
        }

        /// <summary>
        /// Die Eigenschaft gibt die den Pfad des aktuellen Directory zurück
        /// </summary>
        public string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
        }

        public static string LocalIPv4WLAN
        {
            get { return GetAllLocalIPv4(NetworkInterfaceType.Wireless80211).FirstOrDefault() ; }
        }

        public static string LocalIPv6WLAN
        {
            get { return GetAllLocalIPv4(NetworkInterfaceType.Wireless80211, AddressFamily.InterNetworkV6).FirstOrDefault(); }
        }

        /// <summary>
        /// Die Methode gibt eine Liste der SpecialFolders als Dictionary zurück
        /// </summary>
        public static Dictionary<string,string> SpecialFolders()
        {
            Dictionary<string, string> folders = new Dictionary<string, string>();
            foreach (Environment.SpecialFolder folderType in Enum.GetValues(typeof(Environment.SpecialFolder)))
            {
                string folder = Environment.GetFolderPath(folderType);
                if (folders.ContainsKey(folderType.ToString()) == false)
                {
                    folders.Add(folderType.ToString(), folder);
                }
            }

            return folders;
        }

        private static List<string> GetAllLocalIPv4(NetworkInterfaceType _type, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            List<string> ipAddrList = new List<string>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == addressFamily)
                        {
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }

            return ipAddrList;
        }
    }
}
