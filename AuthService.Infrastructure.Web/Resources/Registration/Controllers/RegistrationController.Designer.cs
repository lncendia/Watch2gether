﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AuthService.Infrastructure.Web.Resources.Registration.Controllers {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RegistrationController {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RegistrationController() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AuthService.Infrastructure.Web.Resources.Registration.Controllers.RegistrationCon" +
                            "troller", typeof(RegistrationController).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mail format is not valid.
        /// </summary>
        internal static string EmailFormatInvalid {
            get {
                return ResourceManager.GetString("EmailFormatInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password length is invalid.
        /// </summary>
        internal static string InvalidPasswordLength {
            get {
                return ResourceManager.GetString("InvalidPasswordLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password must contain numbers.
        /// </summary>
        internal static string PasswordRequiresDigit {
            get {
                return ResourceManager.GetString("PasswordRequiresDigit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password must contain lowercase letters.
        /// </summary>
        internal static string PasswordRequiresLower {
            get {
                return ResourceManager.GetString("PasswordRequiresLower", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password must contain special characters.
        /// </summary>
        internal static string PasswordRequiresNonAlphanumeric {
            get {
                return ResourceManager.GetString("PasswordRequiresNonAlphanumeric", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password must contain uppercase letters.
        /// </summary>
        internal static string PasswordRequiresUpper {
            get {
                return ResourceManager.GetString("PasswordRequiresUpper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User already registered.
        /// </summary>
        internal static string UserAlreadyExist {
            get {
                return ResourceManager.GetString("UserAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The username has an incorrect format.
        /// </summary>
        internal static string UserNameFormatInvalid {
            get {
                return ResourceManager.GetString("UserNameFormatInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The length of the user name should not be from 3 to 20 characters.
        /// </summary>
        internal static string UserNameLengthInvalid {
            get {
                return ResourceManager.GetString("UserNameLengthInvalid", resourceCulture);
            }
        }
    }
}
