using Ninject;
using NUnit.Core;
using NUnit.Framework;


namespace NinjectExamples
{
    //"https://github.com/ninject/ninject/wiki/Contextual-Binding"
    [TestFixture]
    public class Chapter_08_Named_Bindings
    {
        [Test]
        public void CanUseNamedAttributeToGetProperType()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Human>().Named("Tall");
            kernel.Bind<IWarrior>().To<Hobbit>().Named("Short");

            Assert.That(kernel.Get<ShireArmy>().Warrior, Is.InstanceOf<Hobbit>());
        }

        [Test]
        public void CanAccessNamedAttributeDirectly()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Human>().Named("Tall");
            kernel.Bind<IWarrior>().To<Hobbit>().Named("Short");

            Assert.That(kernel.Get<IWarrior>("Short"), Is.InstanceOf<Hobbit>());
            Assert.That(kernel.Get<IWarrior>("Tall"), Is.InstanceOf<Human>());
        }

        [Test]
        public void CanUseNamedAttributeToGetProperTypeFromProperty()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Human>().Named("Tall");
            kernel.Bind<IWarrior>().To<Hobbit>().Named("Short");

            Assert.That(kernel.Get<Gondor>().Warrior, Is.InstanceOf<Human>());
        }

        public class ShireArmy
        {
            public readonly IWarrior Warrior;

            public ShireArmy([Named("Short")] IWarrior warrior)
            {
                Warrior = warrior;
            }
        }

        public class Gondor
        {
            [Inject, Named("Tall")]
            public IWarrior Warrior { get; set; }
        }

        public interface IWarrior{}
        public class Human : IWarrior{}
        public class Hobbit : IWarrior{}
    }
}