//-----------------------------------------------------------------------
// <copyright file="ApplicationUserRights.cs" company="Lifeprojects.de">
//     Class: ApplicationUserRights
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>25.07.2025 08:39:59</date>
//
// <summary>
// Die Klasse gibt für einen Benutzer die festgelegten Rechte zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    public class ApplicationUserRights : IDisposable
    {
        private bool classIsDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserRights"/> class.
        /// </summary>
        public ApplicationUserRights()
        {
        }

        public ResultUserRights GetUserData(string userId)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));
            DataRow benutzerInfo = null;

            try
            {
                /*
                using (ApplicationUsernameRepository repository = new ApplicationUsernameRepository(App.DatabasePath))
                {
                    benutzerInfo = repository.SelectByUserId(userId);
                }
                */
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return new ResultUserRights(benutzerInfo);
        }

        public ResultUserRights GetUserData(DataRow userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId),"Die übergebene DataRow darf nicht null sein");
            }

            DataRow benutzerInfo = null;

            try
            {
                benutzerInfo = userId;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return new ResultUserRights(benutzerInfo);
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (classIsDisposed == false)
            {
                if (classDisposing)
                {
                    /*
                    Managed Objekte freigeben. Wenn diese Obbjekte selbst
                    IDisposable implementieren, dann deren Dispose()
                    aufrufen 
                    */
                }

                /*
                 * Hier unmanaged Objekte freigeben (z.B. IntPtr)
                */
            }

            classIsDisposed = true;
        }
        #endregion Dispose
    }

    public class ResultUserRights
    {
        public ResultUserRights(DataRow benutzerInfo)
        {
            if (benutzerInfo != null)
            {
                this.Id = new Guid(benutzerInfo.GetAs<string>("Id"));
                this.UserName = benutzerInfo.GetAs<string>("UserName");
                this.DisplayName = benutzerInfo.GetAs<string>("DisplayName");
                int userRole = benutzerInfo.GetAs<int>("UserRolle");
                this.UserRolle = (DialogPermissionRole)userRole;

                string userRights = benutzerInfo.GetAs<string>("UserRights");
                if (string.IsNullOrEmpty(userRights) == false)
                {
                    this.RegisterUserRights(userRights);
                }
            }
        }

        public Guid Id { get; private set; }

        public string UserName { get; private set; }

        public string DisplayName { get; private set; }

        public DialogPermissionRole UserRolle { get; private set; }

        public string UserRights { get; private set; }

        public string UserRightsText { get; private set; }

        public bool IsRead { get; private set; }

        public bool IsWrite { get; private set; }

        public bool IsPrint { get; private set; }

        public bool IsExcelExport { get; private set; }

        public bool IsAdministrator { get; private set; }

        private Dictionary<UserRightsFlag, bool> UserFlagSource { get; set; } = new Dictionary<UserRightsFlag, bool>();

        private void RegisterUserRights(string userRights)
        {
            this.UserFlagSource.Add(UserRightsFlag.Read, true);
            this.UserFlagSource.Add(UserRightsFlag.Write, false);
            this.UserFlagSource.Add(UserRightsFlag.Export, false);
            this.UserFlagSource.Add(UserRightsFlag.Print, false);
            this.UserFlagSource.Add(UserRightsFlag.Administrator, false);
            using (FlagService<UserRightsFlag> fs = new FlagService<UserRightsFlag>(this.UserFlagSource))
            {
                if (string.IsNullOrEmpty(userRights) == false)
                {
                    fs.SetFlags(userRights);
                }

                if (fs.IsFlag(UserRightsFlag.Read) == true)
                {
                    this.IsRead = true;
                }

                if (fs.IsFlag(UserRightsFlag.Write) == true)
                {
                    this.IsWrite = true;
                }

                if (fs.IsFlag(UserRightsFlag.Print) == true)
                {
                    this.IsPrint = true;
                }

                if (fs.IsFlag(UserRightsFlag.Export) == true)
                {
                    this.IsExcelExport = true;
                }

                if (fs.IsFlag(UserRightsFlag.Administrator) == true)
                {
                    this.IsAdministrator = true;
                }

                this.UserRights = fs.GetFlags();
                this.UserRightsText = fs.ToDescription();
            }
        }
    }
}
