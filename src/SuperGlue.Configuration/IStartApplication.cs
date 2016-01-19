﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperGlue.Configuration
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public interface IStartApplication
    {
        string Chain { get; }
        Task Start(AppFunc chain, IDictionary<string, object> settings, string environment);
        Task ShutDown(IDictionary<string, object> settings);
        AppFunc GetDefaultChain(IBuildAppFunction buildApp, IDictionary<string, object> settings, string environment);
    }
}