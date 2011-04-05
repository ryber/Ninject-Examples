using System;
using System.IO;
using System.Threading;
using System.Web;
using Ninject;
using Ninject.Activation;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_3_Scopes
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

            var kernel = new StandardKernel();
            kernel.Bind<ITool>().To<Hammer>().InRequestScope();

            StartNewHttpRequest();

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            StartNewHttpRequest();

            var tool3 = kernel.Get<ITool>();

            Assert.That(tool1, Is.EqualTo(tool2));
            Assert.That(tool1, Is.Not.EqualTo(tool3));

        }

        [Test]
        public void WeCanEvenDefineOurOwnScope()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ITool>().To<Hammer>().InScope(CustomScope);

            var tool1 = kernel.Get<ITool>();
            var tool2 = kernel.Get<ITool>();

            cachedContext = null;

            var tool3 = kernel.Get<ITool>();

            Assert.That(tool1, Is.EqualTo(tool2));
            Assert.That(tool1, Is.Not.EqualTo(tool3));
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

        public interface ITool {}
        public class Hammer : ITool {}
    }
}