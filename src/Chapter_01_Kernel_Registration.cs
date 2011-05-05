using System.Linq;
using System.Reflection;
using Ninject;
using Ninject.Modules;
using Xunit;
using Xunit.Should;


namespace NinjectExamples
{
    //"https://github.com/ninject/ninject/wiki/Modules-and-the-Kernel"

    public class Chapter_01_Kernel_Registration
    {

        [Fact]
        public void SingleBinding()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IVegetable>().To<Carrot>();

            kernel.GetBindings(typeof (IVegetable)).Count().ShouldBe(1);
        }

        [Fact]
        public void CanRemoveBinding()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IVegetable>().To<Carrot>();
            kernel.Unbind<IVegetable>();
            kernel.GetBindings(typeof(IVegetable)).Count().ShouldBe(0);
        }

        [Fact]
        public void CanReplaceBinding()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IVegetable>().To<Carrot>();
            kernel.Rebind<IVegetable>().To<GreenBean>();
            kernel.Get<IVegetable>().ShouldBeInstanceOf<GreenBean>();
        }

        [Fact]
        public void ModuleBinding()
        {
            var kernel = new StandardKernel();
            kernel.Load(new VeggieModule());

            kernel.GetBindings(typeof(IVegetable)).Count().ShouldBe(1);
        }

        [Fact]
        public void CanGetBackRegisteredModules()
        {
            var kernel = new StandardKernel();
            var module = new VeggieModule();
            kernel.Load(module);

            kernel.GetModules().ShouldContain(module);
        }

        [Fact]
        public void AssemblyScanningByFileName()
        {
            var kernel = new StandardKernel();
            kernel.Load("NinjectExamples.dll");

            kernel.GetBindings(typeof(IVegetable)).Count().ShouldBe(1);
        }

        [Fact]
        public void AssemblyScanningByAssembly()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.GetBindings(typeof(IVegetable)).Count().ShouldBe(1);
        }

        public interface IVegetable
        {
            
        }

        public class Carrot : IVegetable
        {
            
        }

        public class GreenBean : IVegetable
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