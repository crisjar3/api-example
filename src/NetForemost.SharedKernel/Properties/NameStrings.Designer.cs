﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetForemost.SharedKernel.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class NameStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal NameStrings() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NetForemost.SharedKernel.Properties.NameStrings", typeof(NameStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a X-Api-Key.
        /// </summary>
        public static string HeaderName_ApiKey {
            get {
                return ResourceManager.GetString("HeaderName_ApiKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a X-Language.
        /// </summary>
        public static string HeaderName_Language {
            get {
                return ResourceManager.GetString("HeaderName_Language", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a TimeZone.
        /// </summary>
        public static string HeaderName_TimeZone {
            get {
                return ResourceManager.GetString("HeaderName_TimeZone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a 404.
        /// </summary>
        public static string HttpError_BadRequest {
            get {
                return ResourceManager.GetString("HttpError_BadRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Admin.
        /// </summary>
        public static string RoleName_Admin {
            get {
                return ResourceManager.GetString("RoleName_Admin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Member.
        /// </summary>
        public static string RoleName_Member {
            get {
                return ResourceManager.GetString("RoleName_Member", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Owner.
        /// </summary>
        public static string RoleName_Owner {
            get {
                return ResourceManager.GetString("RoleName_Owner", resourceCulture);
            }
        }
    }
}