using System.Linq;
using Ninject;
using Xunit;
using Xunit.Should;


namespace NinjectExamples
{

    public class Chapter_05_Injection
    {
        [Fact]
        public void ConstructorInjection()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var luke = kernel.Get<Jedi>();

            luke.Weapon.ShouldBeInstanceOf<Lightsaber>();
        }

        [Fact]
        public void PropertyInjection()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var darth = kernel.Get<SithLord>();

            darth.Weapon.ShouldBeInstanceOf<Lightsaber>();
        }

        [Fact]
        public void MethodInjection()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Blaster>();

            var larry = kernel.Get<StormTrooper>();

            larry.Weapons.ShouldBeInstanceOf<Blaster>();
        }

        [Fact]
        public void CanResolveDependenciesOnExistingObject()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var darth = new SithLord();

            darth.Weapon.ShouldBeNull();

            kernel.Inject(darth);

            darth.Weapon.ShouldBeInstanceOf<Lightsaber>();
        }

        [Fact] //"https://github.com/ninject/ninject/wiki/Multi-injection"
        public void CanGetAnArrayWhenMoreThanOneBindingExists()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Blaster>();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            var grievous = kernel.Get<Cyborg>();

            var weaponTypes = grievous.Weapons.Select(x => x.GetType()).ToList();
            weaponTypes.ShouldContain(typeof(Lightsaber));
            weaponTypes.ShouldContain(typeof(Blaster));
        }

        [Fact]
        public void MoreThanOneBindingWillThrowActivationException()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IWeapon>().To<Blaster>();
            kernel.Bind<IWeapon>().To<Lightsaber>();

            Assert.Throws<ActivationException>(delegate
                              {
                                  var luke = kernel.Get<Jedi>();
                              });
            

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