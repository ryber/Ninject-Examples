using System.Linq;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_5_Injection
    {
        [Test]
        public void ConstructorInjection()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var luke = kernel.Get<Jedi>();

            Assert.That(luke.Weapon, Is.InstanceOfType(typeof(Lightsaber)));
        }

        [Test]
        public void PropertyInjection()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var darth = kernel.Get<SithLord>();

            Assert.That(darth.Weapon, Is.InstanceOfType(typeof(Lightsaber)));
        }

        [Test]
        public void MethodInjection()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Blaster>();

            var larry = kernel.Get<StormTrooper>();

            Assert.That(larry.Weapons, Is.InstanceOfType(typeof(Blaster)));
        }

        [Test]
        public void CanResolveDependenciesOnExistingObject()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var darth = new SithLord();

            Assert.That(darth.Weapon, Is.Null);

            kernel.Inject(darth);

            Assert.That(darth.Weapon, Is.InstanceOfType(typeof(Lightsaber)));
        }

        [Test]
        public void CanGetAnArrayWhenMoreThanOneBindingExists()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Blaster>();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var grievous = kernel.Get<Cyborg>();

            var weaponTypes = grievous.Weapons.Select(x => x.GetType()).ToList();
            Assert.That(weaponTypes, Has.Member(typeof(Lightsaber)));
            Assert.That(weaponTypes, Has.Member(typeof(Blaster)));
        }

        [Test]
        public void WillGiveYouTheFirstBindingIfThereIsMoreThanOne()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Blaster>();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var luke = kernel.Get<Jedi>();

            Assert.That(luke.Weapon, Is.InstanceOfType(typeof(Blaster)));
        }

        public interface IWeapon{}
        public class Lightsaber : IWeapon {}
        public class Blaster : IWeapon { }

        public class Jedi
        {
            public readonly IWeapon Weapon;

            public Jedi(IWeapon weapon)
            {
                Weapon = weapon;
            }
        }

        public class SithLord
        {
            [Inject]
            public IWeapon Weapon { get; set; }
        }

        public class StormTrooper
        {
            public IWeapon Weapons { get; private set; }

            [Inject]
            public void SetWeapon(IWeapon weapon)
            {
                Weapons = weapon;
            }
        }

        public class Cyborg
        {
            public IWeapon[] Weapons;

            public Cyborg(IWeapon[] weapon)
            {
                Weapons = weapon;
            }
        }
    }
}