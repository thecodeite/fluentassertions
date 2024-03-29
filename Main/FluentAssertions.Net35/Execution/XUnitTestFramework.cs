﻿namespace FluentAssertions.Execution
{
    internal class XUnitTestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName
        {
            get { return "xunit"; }
        }

        protected override string ExceptionFullName
        {
            get { return "Xunit.Sdk.AssertException"; }
        }
    }
}