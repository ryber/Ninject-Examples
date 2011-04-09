using Ninject;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture, Documentation("https://github.com/ninject/ninject/wiki/Contextual-Binding")]
    public class Chapter_08_Named_Bindings
    {
        [Test]
        public void CanUseNamedAttributeToGetProperType()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Human>().Named("Tall");
            kernel.Bind<IWarrior>().To<Hobbit>().Named("Short");

            Assert.That(kernel.Get<ShireArmy>().Warrior, Is.InstanceOfType(typeof(Hobbit)));
        }

        [Test]
        public void CanAccessNamedAttributeDirectly()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWarrior>().To<Human>().Named("Tall");
            kernel.Bind<IWarrior>().To<Hobbit>().Named("Short");

            Assert.That(kernel.Get<IWarrior>("Short"), Is.InstanceOfType(typeof(Hobbit)));
            Assert.That(kernel.Get<IWarrior>("Tall"), Is.InstanceOfType(typeof(Human)));
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