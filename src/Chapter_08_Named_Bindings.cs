using Ninject;
using Xunit;
using Xunit.Should;


namespace NinjectExamples
{
    //"https://github.com/ninject/ninject/wiki/Contextual-Binding"
    public class Chapter_08_Named_Bindings
    {
        [Fact]
        public void CanUseNamedAttributeToGetProperType()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Human>().Named("Tall");
            kernel.Bind<IWarrior>().To<Hobbit>().Named("Short");

            kernel.Get<ShireArmy>().Warrior.ShouldBeInstanceOf<Hobbit>();
        }

        [Fact]
        public void CanAccessNamedAttributeDirectly()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Human>().Named("Tall");
            kernel.Bind<IWarrior>().To<Hobbit>().Named("Short");

            kernel.Get<IWarrior>("Short").ShouldBeInstanceOf<Hobbit>();
            kernel.Get<IWarrior>("Tall").ShouldBeInstanceOf<Human>();
        }

        public class ShireArmy
        {
            public readonly IWarrior Warrior;

            public ShireArmy([Named("Short")] IWarrior warrior)
            {
                Warrior = warrior;
            }
        }

        public interface IWarrior{}
        public class Human : IWarrior{}
        public class Hobbit : IWarrior{}
    }
}