// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using Microsoft.AspNet.WebHooks;
using Microsoft.AspNet.WebHooks.Config;
using Microsoft.AspNet.WebHooks.Storage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System.Web.Http
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Configures a Microsoft Azure Table Storage implementation of <see cref="IWebHookStore"/>
        /// which provides a persistent store for registered WebHooks used by the custom WebHooks module.
        /// </summary>
        /// <param name="services">The current <see cref="HttpConfiguration"/>config.</param>
        public static void InitializeCustomWebHooksAzureQueueSender(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var serviceProvider = services.BuildServiceProvider();
            ILogger logger = serviceProvider.GetLogger<IWebHookSender>();
            SettingsDictionary settings = SettingsDictionary.ReadFromConfig(serviceProvider);

            IStorageManager storageManager = StorageManager.GetInstance(logger);
            IWebHookSender sender = new AzureWebHookSender(storageManager, settings, logger);
            services.AddSingleton(sender);
        }

        /// <summary>
        /// Configures a Microsoft Azure Table Storage implementation of <see cref="IWebHookStore"/>
        /// which provides a persistent store for registered WebHooks used by the custom WebHooks module.
        /// Using this initializer, the data will be encrypted using <see cref="IDataProtector"/>.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/>config.</param>
        public static void InitializeCustomWebHooksAzureStorage(this IServiceCollection services)
        {
            InitializeCustomWebHooksAzureStorage(services, encryptData: true);
        }

        /// <summary>
        /// Configures a Microsoft Azure Table Storage implementation of <see cref="IWebHookStore"/>
        /// which provides a persistent store for registered WebHooks used by the custom WebHooks module.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/>config.</param>
        /// <param name="encryptData">Indicates whether the data should be encrypted using <see cref="IDataProtector"/> while persisted.</param>
        public static void InitializeCustomWebHooksAzureStorage(this IServiceCollection services, bool encryptData)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var serviceProvider = services.BuildServiceProvider();

            var settingsDictionary = SettingsDictionary.ReadFromConfig(serviceProvider);

            ILogger logger = serviceProvider.GetLogger<IWebHookStore>();

            IStorageManager storageManager = StorageManager.GetInstance(logger);
            IWebHookStore store;
            if (encryptData)
            {
                IDataProtector protector = DataSecurity.GetDataProtector();
                store = new AzureWebHookStore(storageManager, settingsDictionary, protector, logger);
            }
            else
            {
                store = new AzureWebHookStore(storageManager, settingsDictionary, logger);
            }
            services.AddSingleton(store);
        }
    }
}
