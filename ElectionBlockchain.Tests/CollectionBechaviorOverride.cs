using Xunit;
//This file disable test parallelization and enforce ro run tests sequentially
[assembly: CollectionBehavior(DisableTestParallelization = true)]

////This file is used to configure test startup
//[assembly: TestFramework("ElectionBlockchain.Tests.Startup", "ElectionBlockchain.Tests")]
