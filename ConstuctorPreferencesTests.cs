using Ninject;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class ConstuctorPreferencesTests
    {
        private IKernel _dojo;

        [SetUp]
        public void SetUp()
        {
            _dojo = new StandardKernel();
            _dojo.Bind<ICar>().To<Car>();
            _dojo.Bind<IAmmo>().To<Ammo>();
        }

        [Test]
        public void WillUseInjectAttributeFirst()
        {
            Assert.That(_dojo.Get<InjectAttributeFirst>().InjectAttributeWasUsed, Is.True);
        }

        [Test]
        public void MostComlexIsNext()
        {
            Assert.That(_dojo.Get<MostComplexThatCanbeResolvedNext>().MostComplexWasUser, Is.True);
        }

        [Test]
        [Category("Broken; waiting for fix in Ninject")]
        public void DefaultNoParamsIsNext()
        {
            Assert.That(_dojo.Get<NoParamsIsNext>().NoParamCtorUsed, Is.True);
        }
    }

    public class InjectAttributeFirst
    {
        public bool MostComplexWasUser;
        public bool InjectAttributeWasUsed;

        public InjectAttributeFirst(ICar car, IAmmo ammo)
        {
            MostComplexWasUser = true;
        }

        [Inject]
        public InjectAttributeFirst()
        {
            InjectAttributeWasUsed = true;
        }
    }

    public class MostComplexThatCanbeResolvedNext
    {
        public bool MostComplexWasUser;
        public bool NoParamCtorUsed;
        public bool AmNothingWasUsed;

        public MostComplexThatCanbeResolvedNext()
        {
            NoParamCtorUsed = true;
        }

        public MostComplexThatCanbeResolvedNext(IAmNothing nothing)
        {
            AmNothingWasUsed = true;
        }

        public MostComplexThatCanbeResolvedNext(ICar car, IAmmo ammo)
        {
            MostComplexWasUser = true;
        }
    }


    public class NoParamsIsNext
    {
        public bool NoParamCtorUsed;
        public bool AmNothingWasUsed;

        public NoParamsIsNext(IAmNothing nothing)
        {
            AmNothingWasUsed = true;
        }

        public NoParamsIsNext()
        {
            NoParamCtorUsed = true;
        }
    }


    public interface IAmNothing {}

}