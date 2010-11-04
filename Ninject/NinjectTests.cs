using Ninject;
using Ninject.Parameters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace WindsorTests.Ninject
{
	[TestFixture]
	public class NinjectTests
	{
		private IKernel dojo;

		[TestFixtureSetUp]
		public void StartUp()
		{
			dojo = new StandardKernel();
			dojo.Bind<ICar>().To<Car>();
		}

		[Test]
		public void StartUpTimeIsFast()
		{
			//"No need for universal registration. Only for injected items (not the injectee)s";
		}

		[Test]
		public void ResolveBasicPropertyInjection()
		{
			var robocop = dojo.Get<RoboCop>();
			Assert.That(robocop.Automobile, Is.Not.Null);
		}

		[Test]
		public void CanResoveForAlreadyCreatedObject()
		{
			var robo = new RoboCop();
			dojo.Inject(robo);
			Assert.That(robo.Automobile, Is.Not.Null);
		}

		[Test]
		public void CanResolveChains()
		{
			var murphy = dojo.Get<RoboCop>();
			Assert.That(murphy.Automobile.Engine.Name, Is.EqualTo("V8"));
		}

		[Test]
		public void CanInjectContructor()
		{
			var robocop = dojo.Get<CopShop>();
			Assert.That(robocop.HasMurphy, Is.True);
			Assert.That(robocop.HasAuto, Is.True);
		}

		[Test]
		public void CanInjectContructorWithOtherImplimentationsRegistered()
		{
			var robocop = dojo.Get<DoughnutShop>();
			Assert.That(robocop.HasMurphy, Is.True);
			Assert.That(robocop.HasAuto, Is.True);
		}

		[Test]
		public void InjectMethod()
		{
			var murphy = dojo.Get<RoboCop>();
			Assert.That(murphy.HasAmmo, Is.True);
		}

		[Test]
		public void CanInjectASpecificInstanceIntoConstructor()
		{
			var truck = new Truck();
			var copShop = dojo.Get<CopShop>(new ConstructorArgument("auto", truck));

			Assert.That(copShop.GetCar(), Is.EqualTo(truck));
		}

		[Test]
		public void CanInjectASpecificInstanceIntoProperty()
		{
			var truck = new Truck();
			var murphy = dojo.Get<RoboCop>(new PropertyValue("Automobile", truck));

			Assert.That(murphy.Automobile, Is.EqualTo(truck));
		}

		[Test]
		public void CanBindToAProvider()
		{
			dojo.Bind<IBigBadRobot>().ToProvider<RobotFactory>();
			var ed = dojo.Get<IBigBadRobot>();
			Assert.That(ed, Is.InstanceOfType(typeof(Ed209)));
		}
	}
}