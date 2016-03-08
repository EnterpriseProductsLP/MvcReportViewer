using NUnit.Framework;

namespace MvcReportViewer.Tests
{
    [TestFixture]
    public class ReportRunnerTests
    {
        [Test]
        public void ReportNameAsEmbeddedResourceOnlyFromConfiguration()
        {
            IProvideReportConfiguration configuration = new ReportConfigurationProvider
            {
                EmbeddedResourceStream = TestData.EmbeddedResourceStream,
                ReportFormat = ReportFormat.Excel,
                ReportIsEmbeddedResource = true,
            };

            var reportRunner = new ReportRunner(configuration);
            var parameters = reportRunner.ViewerParameters;
            Assert.IsTrue(parameters.ReportIsEmbeddedResource);
            Assert.AreEqual(null, parameters.ReportPath);
            Assert.AreEqual(TestData.DefaultServer, parameters.ReportServerUrl);
            Assert.AreEqual(TestData.DefaultUsername, parameters.Username);
            Assert.AreEqual(TestData.DefaultPassword, parameters.Password);
            Assert.AreEqual(TestData.EmbeddedResourceStream, parameters.EmbeddedResourceStream);
            Assert.AreEqual(ReportFormat.Excel, reportRunner.ReportFormat);
            Assert.AreEqual(0, parameters.ReportParameters.Count);
        }

        [Test]
        public void ReportNameOnlyFromConfiguration()
        {
            IProvideReportConfiguration configuration = new ReportConfigurationProvider
            {
                ReportFormat = ReportFormat.Excel,
                ReportPath = TestData.ReportName
            };

            var reportRunner = new ReportRunner(configuration);
            var parameters = reportRunner.ViewerParameters;
            Assert.IsFalse(parameters.ReportIsEmbeddedResource);
            Assert.AreEqual(TestData.ReportName, parameters.ReportPath);
            Assert.AreEqual(TestData.DefaultServer, parameters.ReportServerUrl);
            Assert.AreEqual(TestData.DefaultUsername, parameters.Username);
            Assert.AreEqual(TestData.DefaultPassword, parameters.Password);
            Assert.AreEqual(ReportFormat.Excel, reportRunner.ReportFormat);
            Assert.AreEqual(0, parameters.ReportParameters.Count);
        }

        [Test]
        public void ReportNameOnlyConstructor()
        {
            var reportRunner = new ReportRunner(ReportFormat.Excel, TestData.ReportName);
            var parameters = reportRunner.ViewerParameters;
            Assert.IsFalse(parameters.ReportIsEmbeddedResource);
            Assert.AreEqual(TestData.ReportName, parameters.ReportPath);
            Assert.AreEqual(TestData.DefaultServer, parameters.ReportServerUrl);
            Assert.AreEqual(TestData.DefaultUsername, parameters.Username);
            Assert.AreEqual(TestData.DefaultPassword, parameters.Password);
            Assert.AreEqual(ReportFormat.Excel, reportRunner.ReportFormat);
            Assert.AreEqual(0, parameters.ReportParameters.Count);
        }

        [Test]
        public void ReportNameAndParametersFromConfiguration()
        {
            IProvideReportConfiguration configuration = new ReportConfigurationProvider
            {
                ReportFormat = ReportFormat.Excel,
                ReportParameters = TestData.ActualParameters,
                ReportPath = TestData.ReportName
            };

            var reportRunner = new ReportRunner(configuration);

            var parameters = reportRunner.ViewerParameters;
            Assert.IsFalse(parameters.ReportIsEmbeddedResource);
            Assert.AreEqual(TestData.ReportName, parameters.ReportPath);
            Assert.AreEqual(TestData.DefaultServer, parameters.ReportServerUrl);
            Assert.AreEqual(TestData.DefaultUsername, parameters.Username);
            Assert.AreEqual(TestData.DefaultPassword, parameters.Password);
            Assert.AreEqual(ReportFormat.Excel, reportRunner.ReportFormat);
            var errors = TestHelpers.ValidateReportParameters(parameters);
            if (!string.IsNullOrEmpty(errors))
            {
                Assert.Fail(errors);
            }
        }

        [Test]
        public void ReportNameAndParametersConstructor()
        {
            var reportRunner = new ReportRunner(
                ReportFormat.Excel, 
                TestData.ReportName,
                TestData.ActualParameters);

            var parameters = reportRunner.ViewerParameters;
            Assert.IsFalse(parameters.ReportIsEmbeddedResource);
            Assert.AreEqual(TestData.ReportName, parameters.ReportPath);
            Assert.AreEqual(TestData.DefaultServer, parameters.ReportServerUrl);
            Assert.AreEqual(TestData.DefaultUsername, parameters.Username);
            Assert.AreEqual(TestData.DefaultPassword, parameters.Password);
            Assert.AreEqual(ReportFormat.Excel, reportRunner.ReportFormat);
            var errors = TestHelpers.ValidateReportParameters(parameters);
            if (!string.IsNullOrEmpty(errors))
            {
                Assert.Fail(errors);
            }
        }

        [Test]
        public void AllParametersSetFromConfiguration()
        {
            IProvideReportConfiguration configuration = new ReportConfigurationProvider
            {
                Password = TestData.Password,
                ReportFormat = ReportFormat.Excel,
                ReportParameters = TestData.ActualParameters,
                ReportPath = TestData.ReportName,
                ReportServerUrl = TestData.Server,
                Username = TestData.Username,
            };

            var reportRunner = new ReportRunner(configuration);

            var parameters = reportRunner.ViewerParameters;
            Assert.IsFalse(parameters.ReportIsEmbeddedResource);
            Assert.AreEqual(TestData.ReportName, parameters.ReportPath);
            Assert.AreEqual(TestData.Server, parameters.ReportServerUrl);
            Assert.AreEqual(TestData.Username, parameters.Username);
            Assert.AreEqual(TestData.Password, parameters.Password);
            Assert.AreEqual(ReportFormat.Excel, reportRunner.ReportFormat);
            var errors = TestHelpers.ValidateReportParameters(parameters);
            if (!string.IsNullOrEmpty(errors))
            {
                Assert.Fail(errors);
            }
        }

        [Test]
        public void AllParamtersSetInConstructor()
        {
            var reportRunner = new ReportRunner(
                ReportFormat.Excel,
                TestData.ReportName,
                TestData.Server,
                TestData.Username,
                TestData.Password,
                TestData.ActualParameters);

            var parameters = reportRunner.ViewerParameters;
            Assert.IsFalse(parameters.ReportIsEmbeddedResource);
            Assert.AreEqual(TestData.ReportName, parameters.ReportPath);
            Assert.AreEqual(TestData.Server, parameters.ReportServerUrl);
            Assert.AreEqual(TestData.Username, parameters.Username);
            Assert.AreEqual(TestData.Password, parameters.Password);
            Assert.AreEqual(ReportFormat.Excel, reportRunner.ReportFormat);
            var errors = TestHelpers.ValidateReportParameters(parameters);
            if (!string.IsNullOrEmpty(errors))
            {
                Assert.Fail(errors);
            }
        }
    }
}
