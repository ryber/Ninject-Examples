using Ninject;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectExamples
{
    [TestFixture]
    public class Chapter_7_Constructor_Preferences
    {

        [Test]
        public void WillUseInjectAttributeFirst()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TheOneRing>().ToSelf();
            kernel.Bind<TheDarkCrystal>().ToSelf();

            Assert.That(kernel.Get<InjectAttributeFirst>().InjectAttributeWasUsed, Is.True);
        }

        [Test]
        public void MostComplexThatKernelHasBindingsForIsNext()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TheOneRing>().ToSelf();
            kernel.Bind<TheDarkCrystal>().ToSelf();

            Assert.That(kernel.Get<MostComplexThatCanbeResolvedNext>().MostComplexWasUsed, Is.True);
        }

        [Test, Ignore, Category("Broken; waiting for fix in Ninject; https://github.com/ninject/ninject/issues/closed#issue/23")]
        public void DefaultNoParamsIsNext()
        {
            var kernel = new StandardKernel();
            Assert.That(kernel.Get<NoParamsIsNext>().NoParamCtorUsed, Is.True);
        }
    }

    public class TheOneRing {}
    public class TheDarkCrystal {}

    public class InjectAttributeFirst
    {
        public bool MostComplexWasUsed;
        public bool InjectAttributeWasUsed;

        public InjectAttributeFirst(TheOneRing ring, TheDarkCrystal rock)
        {
            MostComplexWasUsed = true;
        }

        [Inject]
        public InjectAttributeFirst()
        {
            InjectAttributeWasUsed = true;
        }
    }

    public class MostComplexThatCanbeResolvedNext
    {
        public bool MostComplexWasUsed;
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

        public MostComplexThatCanbeResolvedNext(TheOneRing ring, TheDarkCrystal rock)
        {
            MostComplexWasUsed = true;
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