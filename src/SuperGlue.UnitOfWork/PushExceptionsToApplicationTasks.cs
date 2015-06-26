using System;
using System.Collections.Generic;
using SuperGlue.Configuration;
using SuperGlue.ExceptionManagement;

namespace SuperGlue.UnitOfWork
{
    public class PushExceptionsToApplicationTasks : IWrapMiddleware<HandleExceptions>
    {
        public IDisposable Begin(IDictionary<string, object> environment)
        {
            return new Disposable(environment);
        }

        private class Disposable : IDisposable
        {
            private readonly IDictionary<string, object> _environment;

            public Disposable(IDictionary<string, object> environment)
            {
                _environment = environment;
            }

            public void Dispose()
            {
                var exception = _environment.GetException();

                if(exception == null)
                    return;

                var applicationTasks = _environment.ResolveAll<IApplicationTask>();

                foreach (var applicationTask in applicationTasks)
                    applicationTask.Exception(exception).Wait();
            }
        }
    }
}