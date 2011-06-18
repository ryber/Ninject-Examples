using System;
using System.IO;
using System.Threading;
using System.Web;
using Ninject;
using Ninject.Activation.Caching;
using Ninject.Infrastructure.Disposal;
using Ninject.Web.Common;
using NUnit.Core;
using NUnit.Framework;


namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_03_Scopes
    {
        [Test]
        public void TransientScopeGivesYouANewInstanceEveryTime()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITool>().To<Hammer>().InTransientScope();

            //Transient is the default!
            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            Assert.That(tool1, Is.Not.EqualTo(tool2));

        }

        [Test]
        public void SingleTonScopeGivesYouTheSameInstranceEveryTime()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITool>().To<Hammer>().InSingletonScope();

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            Assert.That(tool1, Is.EqualTo(tool2));
        }

        [Test]
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

            Assert.That(tool1, Is.EqualTo(tool2));
            Assert.That(tool1, Is.Not.EqualTo(tool3));
        }

        [Test]
        public void RequestScopeReturnsTheSameInstanceForAHttpRequest()
        {
            StartNewHttpRequest();

            var settings = new NinjectSettings { CachePruningInterval = TimeSpan.MaxValue };
            var kernel = new StandardKernel(settings);
            kernel.Bind<ITool>().To<Hammer>().InRequestScope();

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            Assert.That(tool1, Is.EqualTo(tool2));


            StartNewHttpRequest();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            var tool3 = kernel.Get<ITool>();

            Assert.That(tool1, Is.Not.EqualTo(tool3));
        }

        [Test]
        public void InstancesAreDisposedWhenRequestEndsAndCacheIsPruned()
        {
            var settings = new NinjectSettings { CachePruningInterval = TimeSpan.MaxValue };
            var kernel = new StandardKernel(settings);

            kernel.Bind<ITool>().To<Hammer>().InRequestScope();
            var cache = kernel.Components.Get<ICache>();

            StartNewHttpRequest();

            var instance = kernel.Get<ITool>();

            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.InstanceOf<Hammer>());

            StartNewHttpRequest();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            cache.Prune();

            Assert.That(instance.IsDisposed, Is.True);
        }


        [Test]
        public void InstancesAreDisposedViaOnePerRequestModule()
        {
            var settings = new NinjectSettings { CachePruningInterval = TimeSpan.MaxValue };
            var kernel = new StandardKernel(settings);
            kernel.Bind<ITool>().To<Hammer>().InRequestScope();

            StartNewHttpRequest();

            var instance = kernel.Get<ITool>();

            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.InstanceOf<Hammer>());

            var opr = new OnePerRequestModule();
            opr.DeactivateInstancesForCurrentHttpRequest();

            Assert.That(instance.IsDisposed, Is.True);
        
        }

        [Test]
        public void WeCanEvenDefineOurOwnScope()
        {
            var kernel = new StandardKernel();
            var scopeObject = new ScopeObject();


            kernel.Bind<ITool>().To<Hammer>().InScope(ctx=>scopeObject);

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            scopeObject.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            scopeObject = new ScopeObject();

            var tool3 = kernel.Get<ITool>();

            Assert.That(tool1, Is.EqualTo(tool2));
            Assert.That(tool1, Is.Not.EqualTo(tool3));
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


        public class ScopeObject : DisposableObject
        {
            
        }
   
    }
}