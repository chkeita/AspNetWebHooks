// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNet.WebHooks;
using Microsoft.AspNet.WebHooks.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System.Web.Http
{
    /// <summary>
    /// Extension methods for <see cref="HttpConfiguration"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        ///  Initializes support for adding custom WebHook support to your ASP.NET project. The functionality 
        ///  enables users to manage WebHook subscribers, and to send WebHooks to subscribers with matching 
        ///  registrations.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/>config.</param>
        public static void InitializeCustomWebHooks(this IServiceCollection services)
        {
            services.AddSingleton(serviceProvider => serviceProvider.GetService<IWebHookStore>() ?? new MemoryWebHookStore());
            services.AddSingleton(serviceProvider => serviceProvider.GetService<IWebHookUser>() ?? new WebHookUser());
            services.AddSingleton(serviceProvider =>
            {
                switch (serviceProvider.GetService<IWebHookSender>())
                {
                    case null:
                        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                        return new DataflowWebHookSender(loggerFactory.CreateLogger<IWebHookSender>());
                    case IWebHookSender sender:
                        return sender;
                }
            });
            services.AddSingleton(serviceProvider => 
            {
                switch (serviceProvider.GetService<IWebHookManager>())
                {
                    case null:
                        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger<IWebHookManager>();
                        var sender = serviceProvider.GetService<IWebHookSender>();
                        var store = serviceProvider.GetService<IWebHookStore>();

                        return new WebHookManager(store, sender, logger);
                    case IWebHookManager manager:
                        return manager;
                }
            });

            services.AddSingleton(serviceProvider =>
                serviceProvider.GetService<IEnumerable<IWebHookFilterProvider>>() ??
                    (IEnumerable<IWebHookFilterProvider>) TypeUtilities.GetInstancesFromReferencedAssemblies<IWebHookFilterProvider>(t => TypeUtilities.IsType<IWebHookFilterProvider>(t)));

            services.AddSingleton(serviceProvider =>
            {
                switch (serviceProvider.GetService<IWebHookFilterManager>())
                {
                    case null:
                        var filterProviders = serviceProvider.GetService<IEnumerable<IWebHookFilterProvider>>();

                        return new WebHookFilterManager(filterProviders);
                    case IWebHookFilterManager filterManager:
                        return filterManager;
                }
            });

            services.AddSingleton(serviceProvider =>
            {
                switch (serviceProvider.GetService<IWebHookRegistrationsManager>())
                {
                    case null:
                        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                        var filterManager = serviceProvider.GetService<IWebHookFilterManager>();
                        var userManager = serviceProvider.GetService<IWebHookUser>();
                        var store = serviceProvider.GetService<IWebHookStore>();
                        var manager = serviceProvider.GetService<IWebHookManager>();

                        return new WebHookRegistrationsManager(manager, store, filterManager, userManager);
                    case IWebHookRegistrationsManager registrationsManager:
                        return registrationsManager;
                }
            });
        }
    }
}
