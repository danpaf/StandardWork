﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sarf.Resources {
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
    internal class SarfRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SarfRes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sarf.Resources.SarfRes", typeof(SarfRes).Assembly);
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
        ///   Looks up a localized string similar to Co0l_YoU_HacKeD_My_JwT...I_kn0W_WhO_Y_R.
        /// </summary>
        internal static string HackedJwt {
            get {
                return ResourceManager.GetString("HackedJwt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid login or password.
        /// </summary>
        internal static string InvalidAuthCredentials {
            get {
                return ResourceManager.GetString("InvalidAuthCredentials", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid email passed.
        /// </summary>
        internal static string InvalidEmail {
            get {
                return ResourceManager.GetString("InvalidEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid token passed.
        /// </summary>
        internal static string InvalidJwtToken {
            get {
                return ResourceManager.GetString("InvalidJwtToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password must be longer than {0} symbols and shorter than {1}.
        /// </summary>
        internal static string InvalidPasswordLength {
            get {
                return ResourceManager.GetString("InvalidPasswordLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid refresh token.
        /// </summary>
        internal static string InvalidRefreshToken {
            get {
                return ResourceManager.GetString("InvalidRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Username must be longer than {0} symbols and shorter than {1}.
        /// </summary>
        internal static string InvalidUsernameLength {
            get {
                return ResourceManager.GetString("InvalidUsernameLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Something went wrong.
        /// </summary>
        internal static string SomethingWentWrong {
            get {
                return ResourceManager.GetString("SomethingWentWrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Token already used.
        /// </summary>
        internal static string TokenAlreadyUsed {
            get {
                return ResourceManager.GetString("TokenAlreadyUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Token was expired.
        /// </summary>
        internal static string TokenWasExpired {
            get {
                return ResourceManager.GetString("TokenWasExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User already exists.
        /// </summary>
        internal static string UserAlreadyExists {
            get {
                return ResourceManager.GetString("UserAlreadyExists", resourceCulture);
            }
        }
    }
}
