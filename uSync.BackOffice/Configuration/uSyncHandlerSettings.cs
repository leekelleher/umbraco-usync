﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Umbraco.Extensions;

namespace uSync.BackOffice.Configuration
{
    /// <summary>
    /// Settings to control who a handler works
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class HandlerSettings
    {
        /// <summary>
        /// Is handler enabled or disabled
        /// </summary>
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// List of actions the handler is configured for. 
        /// </summary>
        public string[] Actions { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Should use a flat folder structure when exporting items
        /// </summary>
        [DefaultValue(true)]
        public bool UseFlatStructure { get; set; } = true;

        /// <summary>
        /// Items should be saved with their guid/key value as the filename
        /// </summary>
        [DefaultValue(false)]
        public bool GuidNames { get; set; } = false;

        /// <summary>
        /// Imports should fail if the parent item is missing (if false, item be importated go a close as possible to location)
        /// </summary>
        [DefaultValue(false)]
        public bool FailOnMissingParent { get; set; } = false;

        /// <summary>
        /// Override the group the handler belongs too.
        /// </summary>
        [DefaultValue("")]
        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// Additional settings for the handler
        /// </summary>
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    ///  Extensions to the handler settings
    /// </summary>
    public static class HandlerSettingsExtensions
    {
        /// <summary>
        ///  get a setting from the settings dictionary.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="settings"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TResult GetSetting<TResult>(this HandlerSettings settings, string key, TResult defaultValue)
        {
            if (settings.Settings != null && settings.Settings.ContainsKey(key))
            {
                var attempt = settings.Settings[key].TryConvertTo<TResult>();
                if (attempt) return attempt.Result;
            }

            return defaultValue;
        }

        /// <summary>
        ///  Add a setting to the settings Dictionary (creating the dictionary if its missing)
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddSetting<TObject>(this HandlerSettings settings, string key, TObject value)
        {
            if (settings.Settings == null)
                settings.Settings = new Dictionary<string, string>();

            settings.Settings[key] = value.ToString();
        }

        /// <summary>
        ///  create a copy of this handlers settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static HandlerSettings Clone(this HandlerSettings settings)
        {
            return new HandlerSettings
            {
                Actions = settings.Actions,
                Enabled = settings.Enabled,
                FailOnMissingParent = settings.FailOnMissingParent,
                UseFlatStructure = settings.UseFlatStructure,
                Group = settings.Group,
                GuidNames = settings.GuidNames,
                Settings = new Dictionary<string, string>(settings.Settings)
            };
        }

    }

}
