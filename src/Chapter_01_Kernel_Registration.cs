using System.Linq;
using System.Reflection;
using Ninject;

using NUnit.Core;
using NUnit.Framework;
using Ninject.Modules;


namespace NinjectExamples
{
    //"https://github.com/ninject/ninject/wiki/Modules-and-the-Kernel"
    [TestFixture]
    public class Chapter_01_Kernel_Registration
    {

        [Test]
        public void SingleBinding()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IVegetable>().To<Carrot>();

            Assert.That(kernel.GetBindings(typeof (IVegetable)).Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanRemoveBinding()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IVegetable>().To<Carrot>();
            kernel.Unbind<IVegetable>();
            Assert.That(kernel.GetBindings(typeof(IVegetable)).Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanReplaceBinding()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IVegetable>().To<Carrot>();
            kernel.Rebind<IVegetable>().To<GreenBean>();
            Assert.That(kernel.Get<IVegetable>(), Is.InstanceOf<GreenBean>());
        }

        [Test]
        public void RebindClearsAllBindingsForAType()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IVegetable>().To<Carrot>();
            kernel.Bind<IVegetable>().To<GreenBean>();

            Assert.That(kernel.GetBindings(typeof(IVegetable)).Count(), Is.EqualTo(2));

            kernel.Rebind<IVegetable>().To<Peas>();

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
        public void CanGetBackRegisteredModules()
        {
            var kernel = new StandardKernel();
            var module = new VeggieModule();
            kernel.Load(module);

            Assert.That(kernel.GetModules(), Has.Member(module));
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

        public interface IVegetable {}

        public class Carrot : IVegetable{}

        public class GreenBean : IVegetable {}

        public class Peas : IVegetable {}
        
        public class VeggieModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IVegetable>().To<Carrot>();
            }
        }
    }
}