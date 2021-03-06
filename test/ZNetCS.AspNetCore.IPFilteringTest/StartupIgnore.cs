﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupIgnore.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The startup class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.IPFilteringTest
{
    #region Usings

    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using ZNetCS.AspNetCore.IPFiltering.DependencyInjection;

    #endregion

    /// <summary>
    /// The startup class.
    /// </summary>
    public class StartupIgnore
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupIgnore"/> class.
        /// </summary>
        /// <param name="env">
        /// The hosting environment.
        /// </param>
        public StartupIgnore(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettingsIgnore.json", true, true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        /// The application builder.
        /// </param>
        /// <param name="env">
        /// The hosting environment.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            app.UseIPFiltering();
            app.Run(async context => { await context.Response.WriteAsync("Hello World"); });

            app.Map(
                "/ignoreget",
                builder => { builder.Run(async context => { await context.Response.WriteAsync("Hello World Ignore Get"); }); });

            app.Map(
                "/ignorepost",
                builder => { builder.Run(async context => { await context.Response.WriteAsync("Hello World Ignore Post"); }); });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// The service collection.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIPFiltering(this.Configuration.GetSection("IPFiltering"));
        }

        #endregion
    }
}