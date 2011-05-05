using Ninject;
using Ninject.Extensions.NamedScope;
using Xunit;
using Xunit.Should;

namespace NinjectExamples
{
    // Not part of the main Ninject library. 
    // See https://github.com/ninject/ninject.extensions.namedscope
    public class Chapter_12_Named_Scopes
    {
        [Fact]
        public void NamedScopesDefineTheNamedObjectAsTheScopeObject()
        {
            const string scopeName = "TheATeam";

            var kernel = new StandardKernel();
            kernel.Bind<ATeam>().ToSelf().DefinesNamedScope(scopeName);
            kernel.Bind<Van>().ToSelf().InNamedScope(scopeName);

            var team = kernel.Get<ATeam>();
            var otherTeam = kernel.Get<ATeam>();

            team.TheVan.ShouldBe(team.TheOtherVan);
            otherTeam.TheVan.ShouldNotBe(team.TheVan);
        }

        [Fact]
        public void InCallScopeDefinedOneInstancePerGet()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ATeam>().ToSelf();
            kernel.Bind<Van>().ToSelf().InCallScope();

            var team = kernel.Get<ATeam>();
            var otherTeam = kernel.Get<ATeam>();

            team.TheVan.ShouldBe(team.TheOtherVan);
            otherTeam.TheVan.ShouldNotBe(team.TheVan);
        }
        
        public class ATeam
        {
            [Inject]
            public Van TheVan { get; set; }

            [Inject]
            public Van TheOtherVan { get; set; }
        }

        public class Van {}
    }
}