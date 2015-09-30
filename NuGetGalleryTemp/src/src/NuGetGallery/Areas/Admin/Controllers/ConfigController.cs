﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using NuGetGallery.Areas.Admin.ViewModels;
using NuGetGallery.Authentication;
using NuGetGallery.Configuration;

namespace NuGetGallery.Areas.Admin.Controllers
{
    public partial class ConfigController : AdminControllerBase
    {
        private readonly ConfigurationService _config;
        private readonly AuthenticationService _auth;

        public ConfigController(ConfigurationService config, AuthenticationService auth)
        {
            _config = config;
            _auth = auth;
        }

        public virtual ActionResult Index()
        {
            var settings = (from p in typeof(IAppConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        where p.CanRead
                        select p)
                       .ToDictionary(p => p.Name, p => Tuple.Create(p.PropertyType, p.GetValue(_config.Current)));
            var features = (from p in typeof(FeatureConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            where p.CanRead
                            select new FeatureConfigViewModel(p, _config.Features))
                            .ToList();


            var configModel = new ConfigViewModel(settings, features, _auth.Authenticators.Values);

            return View(configModel);
        }
    }
}