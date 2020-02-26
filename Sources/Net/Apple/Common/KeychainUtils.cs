using System;
using Foundation;
using Security;

// only in paid version of plugin or local library
#if !__PLUGIN__ || !__EVALUATION__ || __DEV__

namespace Advexp
{
    static class KeyChainUtils
    {
        /// <summary>
        /// Deletes a username/password record.
        /// </summary>
        /// <param name="sUsername">the username to query. May not be NULL.</param>
        /// <param name="sService">the service description to query. May not be NULL.</param>
        /// <returns>SecStatusCode.Success if everything went fine, otherwise some other status</returns>
        //------------------------------------------------------------------------------
        public static SecStatusCode DeletePasswordForUsername(string sUsername, string sService, bool bSynchronizable)
        {
            if (sUsername == null)
            {
                throw new ArgumentNullException(nameof(sUsername));
            }

            if (sService == null)
            {
                throw new ArgumentNullException(nameof(sService));
            }

            // Query and remove.
            SecRecord oQueryRec = new SecRecord(SecKind.GenericPassword)
            { 
                Service = sService, 
                Label = sService, 
                Account = sUsername, 
                Synchronizable = bSynchronizable
            };

            SecStatusCode eCode = SecKeyChain.Remove(oQueryRec);

            return eCode;
        }

        /// <summary>
        /// Sets a password for a specific username.
        /// </summary>
        /// <param name="sUsername">the username to add the password for. May not be NULL.</param>
        /// <param name="sPassword">the password to associate with the record. May not be NULL.</param>
        /// <param name="sService">the service description to use. May not be NULL.</param>
        /// <param name="eSecAccessible">defines how the keychain record is protected</param>
        /// <returns>SecStatusCode.Success if everything went fine, otherwise some other status</returns>
        //------------------------------------------------------------------------------
        public static SecStatusCode SetPasswordForUsername(string sUsername, string sPassword, string sService, SecAccessible eSecAccessible, bool bSynchronizable)
        {
            if (sUsername == null)
            {
                throw new ArgumentNullException(nameof(sUsername));
            }

            if (sService == null)
            {
                throw new ArgumentNullException(nameof(sService));
            }

            if (sPassword == null)
            {
                throw new ArgumentNullException(nameof(sPassword));
            }

            // Don't bother updating. Delete existing record and create a new one.
            DeletePasswordForUsername(sUsername, sService, bSynchronizable);

            // Create a new record.
            // Store password UTF8 encoded.
            SecStatusCode eCode = SecKeyChain.Add(new SecRecord(SecKind.GenericPassword)
            {
                Service = sService,
                Label = sService,
                Account = sUsername,
                Generic = NSData.FromString(sPassword, NSStringEncoding.UTF8),
                Accessible = eSecAccessible,
                Synchronizable = bSynchronizable
            });

            return eCode;
        }

        /// <summary>
        /// Gets a password for a specific username.
        /// </summary>
        /// <param name="sUsername">the username to query. May not be NULL.</param>
        /// <param name="sService">the service description to use. May not be NULL.</param>
        /// <returns>
        /// The password or NULL if no matching record was found.
        /// </returns>
        //------------------------------------------------------------------------------
        public static SecStatusCode GetPasswordForUsername(string sUsername, string sService, out string password, bool bSynchronizable)
        {
            if (sUsername == null)
            {
                throw new ArgumentNullException(nameof(sUsername));
            }

            if (sService == null)
            {
                throw new ArgumentNullException(nameof(sService));
            }

            SecStatusCode eCode;
            // Query the record.
            SecRecord oQueryRec = new SecRecord(SecKind.GenericPassword)
            {
                Service = sService, 
                Label = sService, 
                Account = sUsername,
                Synchronizable = bSynchronizable
            };
            oQueryRec = SecKeyChain.QueryAsRecord(oQueryRec, out eCode);

            // If found, try to get password.
            if (eCode == SecStatusCode.Success && oQueryRec != null)
            {
                if (oQueryRec.Generic != null)
                {
                    // Decode from UTF8.
                    password = NSString.FromData(oQueryRec.Generic, NSStringEncoding.UTF8);
                }
                else
                {
                    password = String.Empty;
                }
            }
            else
            {
                password = null;
            }

            // Something went wrong.
            return eCode;
        }

        //------------------------------------------------------------------------------
        public static bool Contains(string sUsername, string sService, bool bSynchronizable)
        {
            if (sUsername == null)
            {
                throw new ArgumentNullException(nameof(sUsername));
            }

            if (sService == null)
            {
                throw new ArgumentNullException(nameof(sService));
            }

            SecStatusCode eCode;
            // Query the record.
            SecRecord oQueryRec = new SecRecord(SecKind.GenericPassword)
            {
                Service = sService, 
                Label = sService, 
                Account = sUsername,
                Synchronizable = bSynchronizable
            };
            oQueryRec = SecKeyChain.QueryAsRecord(oQueryRec, out eCode);

            // If found, return true.
            if (eCode == SecStatusCode.Success && oQueryRec != null)
            {
                if (oQueryRec.Generic != null)
                {
                    return true;
                }
            }

            // Something went wrong.
            return false;
        }
    }
}

#endif // !__PLUGIN__ || !__EVALUATION__ || __DEV__
