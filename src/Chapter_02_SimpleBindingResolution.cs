using Ninject;
using Xunit;
using Xunit.Should;


namespace NinjectExamples
{

    public class Chapter_02_SimpleBindingResolution
    {
        [Fact]
        public void GetInterfaceImplimentation()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Sword>();

            kernel.Get<IWeapon>().ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void GetAbstractImplimentation()
        {
            var kernel = new StandardKernel();
            kernel.Bind<HandWeapons>().To<Sword>();

            kernel.Get<HandWeapons>().ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void SelfBindingTypesDoNotNeedExplicitBindings()
        {
            var kernel = new StandardKernel();
            kernel.Get<Sword>().ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void CanCreateExplicitSelfBinds()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Sword>().ToSelf();
            kernel.Get<Sword>().ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void CanBindValueTypesToConstants()
        {
            var kernel = new StandardKernel();
            kernel.Bind<string>().ToConstant("Hello Dojo");
            kernel.Get<string>().ShouldBe("Hello Dojo");
        }

        public interface IWeapon{}  

        public abstract class HandWeapons {}

        public class Sword : HandWeapons, IWeapon {}

        
    }
}