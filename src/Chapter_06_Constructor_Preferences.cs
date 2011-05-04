using System;
using Ninject;
using Xunit;
using Xunit.Should;


namespace NinjectExamples
{

    public class Chapter_06_Constructor_Preferences
    {

        [Fact]
        public void WillUseInjectAttributeFirst()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TheOneRing>().ToSelf();
            kernel.Bind<TheDarkCrystal>().ToSelf();

            kernel.Get<InjectAttributeFirst>().InjectAttributeWasUsed.ShouldBeTrue();
        }

        [Fact]
        public void MostComplexThatKernelHasBindingsForIsNext()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TheOneRing>().ToSelf();
            kernel.Bind<TheDarkCrystal>().ToSelf();

            kernel.Get<MostComplexThatCanbeResolvedNext>().MostComplexWasUsed.ShouldBeTrue();
        }

        [Fact(Skip="Currently Broken: Note that if the zero param constructor is physically located first in the class. This will pass.")]
        public void DefaultNoParamsIsNext()
        {
            var kernel = new StandardKernel();
            kernel.Get<NoParamsIsNext>().NoParamCtorUsed.ShouldBeTrue();
        }

        [Fact(Skip="Currently Broken: see issue 35")]
        public void TwoInjectsOnDifferentContructorsWillResultInException()
        {
            var kernel = new StandardKernel();

            Assert.Throws<NotSupportedException>(
                delegate{
                             kernel.Get<TwoInjectedConstructorsThrows>();
                         }
            );
            
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

    public class TwoInjectedConstructorsThrows
    {
        [Inject]
        public TwoInjectedConstructorsThrows(TheOneRing ring)
        {
            Console.WriteLine("ring");
        }

        [Inject]
        public TwoInjectedConstructorsThrows(TheDarkCrystal rock)
        {
            Console.WriteLine("rock");
        }
    }


    public interface IAmNothing {}

}