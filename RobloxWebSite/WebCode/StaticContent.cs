using System;
using System.Collections.Generic;
using System.Web;
using Roblox.Web.StaticContent;

namespace Roblox.Website
{
    /// <summary>
    /// Acts as a wrapper for all things static content.
    /// </summary>
    public class StaticContent
    {
        public static string GetUrl(string fileName)
        {
            return StaticContentV1.GetUrl(fileName);
        }


        // ROBLOX JavaScript //

        public static RobloxScriptBundle CreateScriptBundle(string name, params string[] files)
        {
            return RobloxScripts.CreateBundle(
                name,
                files,
                Roblox.Web.StaticContent.Properties.Settings.Default.MinifyJavascript
            );
        }

        public static RobloxScriptBundle CreateScriptBundle(string name, ICollection<string> files)
        {
            return RobloxScripts.CreateBundle(
                name,
                files,
                Roblox.Web.StaticContent.Properties.Settings.Default.MinifyJavascript
            );
        }

        public static RobloxScriptBundle GetPageScriptBundle()
        {
            if (RobloxScripts.PageScripts.Count <= 0)
                return null;

            RobloxScriptBundle bundle = CreateScriptBundle("page", RobloxScripts.PageScripts);
            return bundle;
        }


        // ROBLOX CSS //

        public static RobloxCSSBundle CreateCSSBundle(string name, params string[] files)
        {
            return (RobloxCSSBundle)RobloxCSS.CreateBundle(
                name,
                files,
                Roblox.Web.StaticContent.Properties.Settings.Default.MinifyCss
            );
        }

        public static RobloxCSSBundle CreateCSSBundle(string name, ICollection<string> files)
        {
            // It must be like this, otherwise stack over blow :(
            // *whooosh*
            return (RobloxCSSBundle)RobloxCSS.CreateBundle(
                name,
                files,
                Roblox.Web.StaticContent.Properties.Settings.Default.MinifyCss
            );
        }

        public static RobloxCSSBundle GetPageCSSBundle()
        {
            if (RobloxCSS.PageCSS.Count <= 0)
                return null;

            RobloxCSSBundle bundle = CreateCSSBundle("page", RobloxCSS.PageCSS);
            return bundle;
        }
    }
}