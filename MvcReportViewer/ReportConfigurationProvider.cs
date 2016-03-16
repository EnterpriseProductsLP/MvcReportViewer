using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

using Microsoft.Reporting.WebForms;

namespace MvcReportViewer
{
    public class ReportConfigurationProvider : IProvideReportConfiguration
    {
        private Type _eventsHandlerType;

        public IEnumerable<KeyValuePair<string, object>> DataSources { get; set; }

        public string DeviceInfo { get; set; }

        public Stream EmbeddedResourceStream { get; set; }

        public Type EventsHandlerType
        {
            get
            {
                return _eventsHandlerType;
            }
            set
            {
                if (value.GetInterfaces().All(interfaceType => interfaceType != typeof(IReportViewerEventsHandler)))
                {
                    throw new MvcReportViewerException(
                        $"{value.FullName} must implement IReportViewerEventsHandler interface.");
                }

                _eventsHandlerType = value;
            }
        }

        public string Filename { get; set; }

        public Guid ControlId { get; set; }

        public ControlSettings ControlSettings { get; set; }

        public FormMethod FormMethod { get; set; }

        public object HtmlAttributes { get; set; }

        public string Password { get; set; }

        public ProcessingMode ProcessingMode { get; set; }

        public ReportFormat ReportFormat { get; set; }

        public bool ReportIsEmbeddedResource { get; set; }

        public object ReportParameters { get; set; }

        public string ReportPath { get; set; }

        public string ReportServerUrl { get; set; }

        public string Username { get; set; }
    }
}