using System;
using System.IO;
using System.Threading;
using System.Web;
using Ninject;
using Ninject.Activation;
using Ninject.Activation.Caching;
using Ninject.Infrastructure.Disposal;
using Ninject.Web.Common;
using Xunit;
using Xunit.Should;


namespace NinjectExamples
{
  
    public class Chapter_03_Scopes
    {
        [Fact]
        public void TransientScopeGivesYouANewInstanceEveryTime()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITool>().To<Hammer>().InTransientScope();

            //Transient is the default!
            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            tool1.ShouldNotBe(tool2);

        }

        [Fact]
        public void SingleTonScopeGivesYouTheSameInstranceEveryTime()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITool>().To<Hammer>().InSingletonScope();

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            tool1.ShouldBe(tool2);
        }

        [Fact]
        public void ThreadScopeReturnsTheSameInstanceForAThread()
        {

            var kernel = new StandardKernel();
            kernel.Bind<ITool>().To<Hammer>().InThreadScope();

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();
            ITool tool3 = null;

            ThreadStart callback = () => tool3 = kernel.Get<ITool>();

            var thread = new Thread(callback);
            thread.Start();
            thread.Join();

            tool1.ShouldBe(tool2);
            tool1.ShouldNotBe(tool3);
        }

        [Fact]
        public void RequestScopeReturnsTheSameInstanceForAHttpRequest()
        {
            StartNewHttpRequest();

            var settings = new NinjectSettings { CachePruningInterval = TimeSpan.MaxValue };
            var kernel = new StandardKernel(settings);
            kernel.Bind<ITool>().To<Hammer>().InRequestScope();

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            tool1.ShouldBe(tool2);


            StartNewHttpRequest();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            var tool3 = kernel.Get<ITool>();

            tool1.ShouldNotBe(tool3);
        }

        [Fact]
        public void InstancesAreDisposedWhenRequestEndsAndCacheIsPruned()
        {
            var settings = new NinjectSettings { CachePruningInterval = TimeSpan.MaxValue };
            var kernel = new StandardKernel(settings);

            kernel.Bind<ITool>().To<Hammer>().InRequestScope();
            var cache = kernel.Components.Get<ICache>();

            StartNewHttpRequest();

            var instance = kernel.Get<ITool>();

            instance.ShouldNotBeNull();
            instance.ShouldBeInstanceOf<Hammer>();

            StartNewHttpRequest();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            cache.Prune();

            instance.IsDisposed.ShouldBeTrue();
        }


        [Fact]
        public void InstancesAreDisposedViaOnePerRequestModule()
        {
            var settings = new NinjectSettings { CachePruningInterval = TimeSpan.MaxValue };
            var kernel = new StandardKernel(settings);
            kernel.Bind<ITool>().To<Hammer>().InRequestScope();

            StartNewHttpRequest();

            var instance = kernel.Get<ITool>();

            instance.ShouldNotBeNull();
            instance.ShouldBeInstanceOf<Hammer>();

            var opr = new OnePerRequestModule();
            opr.DeactivateInstancesForCurrentHttpRequest();

            instance.IsDisposed.ShouldBeTrue();
        
        }

        [Fact]
        public void WeCanEvenDefineOurOwnScope()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITool>().To<Hammer>().InScope(CustomScope);

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            cachedContext = null;

            var tool3 = kernel.Get<ITool>();

            tool1.ShouldBe(tool2);
            tool1.ShouldNotBe(tool3);
        }

        private IContext cachedContext;

        private object CustomScope(IContext arg)
        {
            if(cachedContext == null)
            {
                cachedContext = arg;
            }
            
            return cachedContext;
        }

        private static void StartNewHttpRequest()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("i.html","http://foo",string.Empty),new HttpResponse(new StringWriter()));
        }

        public interface ITool : IDisposableObject {}
        public class Hammer : DisposableObject, ITool
        {
            public event EventHandler Disposed;

            public override void Dispose(bool disposing)
            {
                lock (this)
                {
                    if (disposing && !IsDisposed)
                    {
                        var evt = Disposed;
                        if (evt != null) evt(this, EventArgs.Empty);
                        Disposed = null;
                    }

                    base.Dispose(disposing);
                }
            }
        }
    }
}