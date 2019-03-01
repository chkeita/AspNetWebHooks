// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNet.WebHooks
{
    /// <summary>
    /// Extension methods for <see cref="IServiceProvider"/> facilitating getting the services used by custom WebHooks.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Gets an <see cref="IWebHookStore"/> implementation registered with the Dependency Injection engine
        /// or a default implementation if none are registered.
        /// </summary>
        /// <param name="services">The <see cref="IServiceProvider"/> implementation.</param>
        /// <returns>The registered <see cref="IWebHookStore"/> instance or a default implementation if none are registered.</returns>
        public static IWebHookStore GetStore(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHookStore>();
        }

        /// <summary>
        /// Gets an <see cref="IWebHookUser"/> implementation registered with the Dependency Injection engine
        /// or a default implementation if none are registered.
        /// </summary>
        /// <param name="services">The <see cref="IServiceProvider"/> implementation.</param>
        /// <returns>The registered <see cref="IWebHookUser"/> instance or a default implementation if none are registered.</returns>
        public static IWebHookUser GetUser(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHookUser>();
        }

        /// <summary>
        /// Gets the set of <see cref="IWebHookFilterProvider"/> instances registered with the Dependency Injection engine
        /// or an empty collection if none are registered.
        /// </summary>
        /// <param name="services">The <see cref="IDependencyScope"/> implementation.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the registered instances.</returns>
        public static IEnumerable<IWebHookFilterProvider> GetFilterProviders(this IServiceProvider services)
        {
            return services.GetRequiredService<IEnumerable<IWebHookFilterProvider>>();
        }

        /// <summary>
        /// Gets an <see cref="IWebHookFilterManager"/> implementation registered with the Dependency Injection engine
        /// or a default implementation if none are registered.
        /// </summary>
        /// <param name="services">The <see cref="IDependencyScope"/> implementation.</param>
        /// <returns>The registered <see cref="IWebHookFilterManager"/> instance or a default implementation if none are registered.</returns>
        public static IWebHookFilterManager GetFilterManager(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHookFilterManager>();
        }

        /// <summary>
        /// Gets an <see cref="IWebHookSender"/> implementation registered with the Dependency Injection engine
        /// or a default implementation if none are registered.
        /// </summary>
        /// <param name="services">The <see cref="IDependencyScope"/> implementation.</param>
        /// <returns>The registered <see cref="IWebHookSender"/> instance or a default implementation if none are registered.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by caller.")]
        public static IWebHookSender GetSender(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHookSender>();
        }

        /// <summary>
        /// Gets an <see cref="IWebHookManager"/> implementation registered with the Dependency Injection engine
        /// or a default implementation if none are registered.
        /// </summary>
        /// <param name="services">The <see cref="IDependencyScope"/> implementation.</param>
        /// <returns>The registered <see cref="IWebHookManager"/> instance or a default implementation if none are registered.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by caller.")]
        public static IWebHookManager GetManager(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHookManager>();
        }

        /// <summary>
        /// Gets an <see cref="IWebHookRegistrationsManager"/> implementation registered with the Dependency Injection engine
        /// or a default implementation if none are registered.
        /// </summary>
        /// <param name="services">The <see cref="IDependencyScope"/> implementation.</param>
        /// <returns>The registered <see cref="IWebHookRegistrationsManager"/> instance or a default implementation if none are registered.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by caller.")]
        public static IWebHookRegistrationsManager GetRegistrationsManager(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHookRegistrationsManager>();
        }
    }
}
