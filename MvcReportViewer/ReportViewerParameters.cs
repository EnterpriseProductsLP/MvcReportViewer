using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System;
using System.IO;

namespace MvcReportViewer
{
    internal class ReportViewerParameters
    {
        public ReportViewerParameters()
        {
            ReportParameters = new Dictionary<string, ReportParameter>();
        }

        public Stream EmbeddedResourceStream { get; set; }

        public string ReportServerUrl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ReportPath { get; set; }

        public Guid? ControlId { get; set; }

        public ProcessingMode ProcessingMode { get; set; }

        public bool IsAzureSsrs { get; set; }

        public IDictionary<string, ReportParameter> ReportParameters { get; set; }

        public IDictionary<string, object> LocalReportDataSources { get; set; }

        public DataSourceCredentials[] DataSourceCredentials { get; set; }

        public bool IsReportRunnerExecution { get; set; }

        public ControlSettings ControlSettings { get; set; }

        public string EventsHandlerType { get; set; }

        public bool ReportIsEmbeddedResource { get; set; }

        public IEnumerable<KeyValuePair<string, Stream>> SubreportEmbeddedResourceStreams { get; set; }
    }
}
