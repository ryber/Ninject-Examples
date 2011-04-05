using System;
using System.Linq;
using System.Reflection;
using Ninject;
using Ninject.Modules;
using Ninject.Planning.Bindings;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_1_Kernel_Registration
    {

        [Test]
        public void SingleBinding()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IVegetable>().To<Carrot>();

            Assert.That(kernel.GetBindings(typeof(IVegetable)).Count(), Is.EqualTo(1));
        }

        [Test]
        public void ModuleBinding()
        {
            var kernel = new StandardKernel();
            kernel.Load(new VeggieModule());

            Assert.That(kernel.GetBindings(typeof(IVegetable)).Count(), Is.EqualTo(1));
        }

        [Test]
        public void AssemblyScanningByFileName()
        {
            var kernel = new StandardKernel();
            kernel.Load("NinjectExamples.dll");

            Assert.That(kernel.GetBindings(typeof(IVegetable)).Count(), Is.EqualTo(1));
        }

        [Test]
        public void AssemblyScanningByAssembly()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            Assert.That(kernel.GetBindings(typeof(IVegetable)).Count(), Is.EqualTo(1));
        }

        public interface IVegetable
        {
            
        }

        public class Carrot : IVegetable
        {
            
        }
        
        public class VeggieModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IVegetable>().To<Carrot>();
            }
        }
    }
}