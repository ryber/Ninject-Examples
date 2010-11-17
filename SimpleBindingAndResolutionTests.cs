using Ninject;
using Ninject.Parameters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
	[TestFixture]
	public class SimpleBindingAndResolutionTests
	{
		private IKernel _dojo;

		[TestFixtureSetUp]
		public void StartUp()
		{
			_dojo = new StandardKernel();
			_dojo.Bind<ICar>().To<Car>();
		}

		[Test]
		public void ResolveBasicPropertyInjection()
		{
			var murphy = _dojo.Get<RoboCop>();
			Assert.That(murphy.Automobile, Is.Not.Null);
		}

		[Test]
		public void CanResoveForAlreadyCreatedObject()
		{
			var murphy = new RoboCop();
			_dojo.Inject(murphy);
			Assert.That(murphy.Automobile, Is.Not.Null);
		}

		[Test]
		public void CanResolveChains()
		{
			var murphy = _dojo.Get<RoboCop>();
			Assert.That(murphy.Automobile.Engine.Name, Is.EqualTo("V8"));
		}

		[Test]
		public void CanInjectContructor()
		{
			var copShop = _dojo.Get<CopShop>();
			Assert.That(copShop.HasMurphy, Is.True);
			Assert.That(copShop.HasAuto, Is.True);
		}

		[Test]
		public void CanInjectContructorWithOtherImplimentationsRegistered()
		{
			var doughnutShop = _dojo.Get<DoughnutShop>();
			Assert.That(doughnutShop.HasMurphy, Is.True);
			Assert.That(doughnutShop.HasAuto, Is.True);
		}

		[Test]
		public void InjectMethod()
		{
			var murphy = _dojo.Get<RoboCop>();
			Assert.That(murphy.HasAmmo, Is.True);
		}

		[Test]
		public void CanInjectASpecificInstanceIntoConstructor()
		{
			var truck = new Truck();
			var copShop = _dojo.Get<CopShop>(new ConstructorArgument("auto", truck));

			Assert.That(copShop.GetCar(), Is.EqualTo(truck));
		}

		[Test]
		public void CanInjectASpecificInstanceIntoProperty()
		{
			var truck = new Truck();
			var murphy = _dojo.Get<RoboCop>(new PropertyValue("Automobile", truck));

			Assert.That(murphy.Automobile, Is.EqualTo(truck));
		}

		[Test]
		public void CanBindToAProvider()
		{
			_dojo.Bind<IBigBadRobot>().ToProvider<RobotFactory>();
			var ed = _dojo.Get<IBigBadRobot>();
			Assert.That(ed, Is.InstanceOfType(typeof(Ed209)));
		}

        [Test]
        public void CanBindToAnArray()
        {
            _dojo.Bind<ICar>().To<Truck>();
            Assert.That(_dojo.Get<AutoDealer>().Cars.Length, Is.EqualTo(2));
        }

        [Test]
        public void IfThereAreTwoBindingsThenTheFirstWillbeUsedByDefault()
        {
            _dojo.Bind<ICar>().To<Truck>();
            Assert.That(_dojo.Get<OneCarCarage>().Car, Is.TypeOf(typeof(Car)));
        }
	}
}