﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

using Microsoft.Reporting.WebForms;

namespace MvcReportViewer
{
    public class ReportConfigurationProvider : IProvideReportConfiguration
    {
        public IEnumerable<KeyValuePair<string, object>> DataSources { get; set; }

        public Stream EmbeddedResourceStream { get; set; }

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