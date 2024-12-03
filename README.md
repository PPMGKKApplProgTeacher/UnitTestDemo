# UnitTestDemo

Explanation of Attributes in Use

    [OneTimeSetUp] / [OneTimeTearDown]:
        These methods run once before/after all tests in the test fixture.
        Useful for initializing or releasing shared resources.

    [SetUp] / [TearDown]:
        Runs before/after each test method in the test fixture.
        Initializes or cleans up resources specific to each test.

    [TestCase]:
        Supplies input arguments and expected results for parameterized tests.

    [TestCaseSource]:
        Uses external data for parameterized tests, making tests more flexible and reusable.

    [Category]:
        Tags the test fixture or individual tests, enabling filtering during test execution.

    [Explicit]:
        Marks a test that runs only when explicitly triggered.

    [Ignore]:
        Temporarily disables a test with a reason.
