using Xunit;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Mocks.Async.Data;
using ZenProgramming.Chakra.Core.Mocks.Scenarios.Options;
using ZenProgramming.Chakra.Core.Mocks.Tests.Environment.Scenarios;

namespace ZenProgramming.Chakra.Core.Mocks.Tests
{
    //public class MockDataSessionAsyncTests
    //{
    //    [Fact]
    //    public void ShouldMockDataSessionBeOpenedWithRegisterDefaultUsingImplicitScopedMode()
    //    {
    //        //Register data session using "implicit mode" (scoped)
    //        SessionFactory.RegisterDefaultDataSession<MockDataSessionAsync<SimpleScenario>>();

    //        //Open session
    //        IDataSession session = SessionFactory.OpenSession();

    //        //Check that session is created and is valid
    //        Assert.NotNull(session);
    //        Assert.True(session is IMockDataSessionAsync);
    //    }

    //    [Fact]
    //    public void ShouldMockDataSessionBeOpenedWithRegisterDefaultUsingExplicitScopedMode()
    //    {
    //        //Register data session using "implicit mode" (scoped)
    //        SessionFactory.RegisterDefaultDataSession<MockDataSessionAsync<SimpleScenario, ScopedScenarioOption<SimpleScenario>>>();

    //        //Open session
    //        IDataSession session = SessionFactory.OpenSession();

    //        //Check that session is created and is valid
    //        Assert.NotNull(session);
    //        Assert.True(session is IMockDataSessionAsync);
    //    }

    //    [Fact]
    //    public void ShouldMockDataSessionBeOpenedDirectlyUsingImplicitScopedMode()
    //    {
    //        //Open session directly
    //        IDataSession session = SessionFactory.OpenSession<MockDataSessionAsync<SimpleScenario>>();

    //        //Check that session is created and is valid
    //        Assert.NotNull(session);
    //        Assert.True(session is IMockDataSessionAsync);
    //    }

    //    [Fact]
    //    public void ShouldMockDataSessionBeOpenedDirectlyUsingExplicitScopedMode() 
    //    {
    //        //Open session directly
    //        IDataSession session = SessionFactory.OpenSession<MockDataSessionAsync<SimpleScenario, ScopedScenarioOption<SimpleScenario>>>();

    //        //Check that session is created and is valid
    //        Assert.NotNull(session);
    //        Assert.True(session is IMockDataSessionAsync);
    //    }
    //}
}