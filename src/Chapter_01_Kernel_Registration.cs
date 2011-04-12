using System.Linq;
using System.Reflection;
using Ninject;
using Ninject.Modules;
using Xunit;


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

            kernel.GetBindings(typeof (IVegetable)).Count().Equals(1);
        }

        [Fact]
        public void ModuleBinding()
        {
            var kernel = new StandardKernel();
            kernel.Load(new VeggieModule());

            kernel.GetBindings(typeof(IVegetable)).Count().Equals(1);
        }

        [Fact]
        public void AssemblyScanningByFileName()
        {
            var kernel = new StandardKernel();
            kernel.Load("NinjectExamples.dll");

            kernel.GetBindings(typeof(IVegetable)).Count().Equals(1);
        }

        [Fact]
        public void AssemblyScanningByAssembly()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.GetBindings(typeof(IVegetable)).Count().Equals(1);
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