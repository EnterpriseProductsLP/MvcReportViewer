using System;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using MvcReportViewer.Configuration;
using System.Collections;
using System.IO;

namespace MvcReportViewer
{
    internal class ReportRunner
    {
        private readonly ReportViewerConfiguration _config = new ReportViewerConfiguration();

        private readonly ReportViewerParameters _viewerParameters;

        private readonly string _filename;

        private readonly string _deviceInfo;

        public ReportRunner(IProvideReportConfiguration configuration)
        {
            var eventsHandlerType = configuration.EventsHandlerType?.AssemblyQualifiedName;
            var localReportDataSources = configuration.DataSources?.ToDictionary(pair => pair.Key, pair => pair.Value);
            var reportParameters = ParameterHelpers.GetReportParameters(configuration.ReportParameters);

            _viewerParameters = new ReportViewerParameters
            {
                EmbeddedResourceStream = configuration.EmbeddedResourceStream,
                IsReportRunnerExecution = true,
                Password = _config.Password,
                ProcessingMode = configuration.ProcessingMode,
                ReportIsEmbeddedResource = configuration.ReportIsEmbeddedResource,
                ReportPath = configuration.ReportPath,
                ReportServerUrl = _config.ReportServerUrl,
                SubreportEmbeddedResourceStreams = configuration.SubreportEmbeddedResourceStreams,
                Username = _config.Username
            };

            ReportFormat = configuration.ReportFormat;
            _deviceInfo = configuration.DeviceInfo;
            _filename = configuration.Filename;

            if (configuration.ProcessingMode == ProcessingMode.Local)
            {
                if (localReportDataSources != null)
                {
                    _viewerParameters.LocalReportDataSources = localReportDataSources;
                }

                if (eventsHandlerType != null)
                {
                    _viewerParameters.EventsHandlerType = eventsHandlerType;
                }
            }

            _viewerParameters.ReportServerUrl = configuration.ReportServerUrl ?? _viewerParameters.ReportServerUrl;
            if (configuration.Username != null || configuration.Password != null)
            {
                _viewerParameters.Username = configuration.Username;
                _viewerParameters.Password = configuration.Password;
            }

            ParseParameters(reportParameters);
        }

        public ReportRunner(
            ReportFormat reportFormat,
            string reportPath,
            string deviceInfo = DefaultParameterValues.DeviceInfo,
            ProcessingMode mode = ProcessingMode.Remote,
            IDictionary<string, object> localReportDataSources = null,
            string filename = null)
            : this(reportFormat, reportPath, null, null, null, null, deviceInfo, mode, localReportDataSources, filename)
        {
        }

        public ReportRunner(
            ReportFormat reportFormat,
            string reportPath,
            IDictionary<string, object> reportParameters,
            string deviceInfo = DefaultParameterValues.DeviceInfo,
            ProcessingMode mode = ProcessingMode.Remote,
            IDictionary<string, object> localReportDataSources = null,
            string filename = null)
            : this(
                reportFormat,
                reportPath,
                reportParameters?.ToList(),
                deviceInfo,
                mode,
                localReportDataSources,
                filename)
        {
        }

        public ReportRunner(
            ReportFormat reportFormat,
            string reportPath,
            IEnumerable<KeyValuePair<string, object>> reportParameters,
            string deviceInfo = DefaultParameterValues.DeviceInfo,
            ProcessingMode mode = ProcessingMode.Remote,
            IDictionary<string, object> localReportDataSources = null,
            string filename = null)
            : this(reportFormat, reportPath, null, null, null, reportParameters, deviceInfo, mode, localReportDataSources, filename)
        {
        }

        public ReportRunner(
            ReportFormat reportFormat,
            string reportPath,
            string reportServerUrl,
            string username,
            string password,
            IDictionary<string, object> reportParameters,
            string deviceInfo = DefaultParameterValues.DeviceInfo,
            ProcessingMode mode = ProcessingMode.Remote,
            IDictionary<string, object> localReportDataSources = null,
            string filename = null)
            : this(
                reportFormat,
                reportPath,
                reportServerUrl,
                username,
                password,
                reportParameters?.ToList(),
                deviceInfo,
                mode,
                localReportDataSources,
                filename)
        {
        }

        public ReportRunner(
            ReportFormat reportFormat,
            string reportPath,
            string reportServerUrl,
            string username,
            string password,
            IEnumerable<KeyValuePair<string, object>> reportParameters,
            string deviceInfo = DefaultParameterValues.DeviceInfo,
            ProcessingMode mode = ProcessingMode.Remote,
            IDictionary<string, object> localReportDataSources = null,
            string filename = null)
        {
            _viewerParameters = new ReportViewerParameters
            {
                IsReportRunnerExecution = true,
                Password = _config.Password,
                ProcessingMode = mode,
                ReportPath = reportPath,
                ReportServerUrl = _config.ReportServerUrl,
                Username = _config.Username
            };

            ReportFormat = reportFormat;
            _deviceInfo = deviceInfo;
            _filename = filename;

            if (mode == ProcessingMode.Local && localReportDataSources != null)
            {
                _viewerParameters.LocalReportDataSources = localReportDataSources;
            }

            _viewerParameters.ReportServerUrl = reportServerUrl ?? _viewerParameters.ReportServerUrl;
            if (username != null || password != null)
            {
                _viewerParameters.Username = username;
                _viewerParameters.Password = password;
            }

            ParseParameters(reportParameters);
        }

        // The property is only used for unit-testing
        internal ReportViewerParameters ViewerParameters => _viewerParameters;

        // The property is only used for unit-testing
        internal ReportFormat ReportFormat { get; }

        public byte[] Render(out string mimeType)
        {
            string encoding;
            return Render(out mimeType, out encoding);
        }

        public byte[] Render(out string mimeType, out string encoding)
        {
            string fileNameExtension;
            return Render(out mimeType, out encoding, out fileNameExtension);
        }

        public byte[] Render(out string mimeType, out string encoding, out string fileNameExtension)
        {
            string[] streams;
            return Render(out mimeType, out encoding, out fileNameExtension, out streams);
        }

        public byte[] Render(out string mimeType, out string encoding, out string fileNameExtension, out string[] streams)
        {
            Warning[] warnings;
            return Render(out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }

        public byte[] Render(out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings)
        {
            Validate();

            var reportViewer = new ReportViewer();
            reportViewer.Initialize(_viewerParameters);
            ValidateReportFormat(reportViewer);

            if (_viewerParameters.ProcessingMode == ProcessingMode.Remote)
            {
                var format = ReportFormat2String(ReportFormat);

                var output = reportViewer.ServerReport.Render(
                    format,
                    _deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return output;
            }
            else
            {
                var localReport = reportViewer.LocalReport;
                if (_viewerParameters.LocalReportDataSources != null)
                {
                    foreach (var dataSource in _viewerParameters.LocalReportDataSources)
                    {
                        var reportDataSource = new ReportDataSource(dataSource.Key, dataSource.Value);
                        localReport.DataSources.Add(reportDataSource);
                    }
                }

                var format = ReportFormat2String(ReportFormat);

                var reportDocument = localReport.Render(
                    format,
                    null,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return reportDocument;
            }
        }

        public FileStreamResult Run()
        {
            string mimeType;
            var reportDocument = Render(out mimeType);

            if (!string.IsNullOrEmpty(_filename))
            {
                var response = HttpContext.Current.Response;
                response.ContentType = mimeType;
                response.AddHeader("Content-Disposition", $"attachment; filename={_filename}");
            }

            var reportDocumentStream = reportDocument.ToMemoryStream();

            return new FileStreamResult(reportDocumentStream, mimeType);
        }

        private void ParseParameters(IEnumerable<KeyValuePair<string, object>> reportParameters)
        {
            if (reportParameters == null)
            {
                return;
            }

            foreach (var reportParameter in reportParameters)
            {
                var parameterName = reportParameter.Key;
                var parameterValue = reportParameter.Value;
                var parameterList = parameterValue as IEnumerable;
                if (parameterList != null && !(parameterValue is string))
                {
                    // I can loop through the values. User is using an array or a list.

                    foreach(var value in parameterList)
                    {
                        if (_viewerParameters.ReportParameters.ContainsKey(parameterName))
                        {
                            _viewerParameters.ReportParameters[parameterName].Values.Add(value.ToString());
                        }
                        else
                        {
                            var parameter = new ReportParameter(parameterName);
                            parameter.Values.Add(value.ToString());
                            _viewerParameters.ReportParameters.Add(parameterName, parameter);
                        }
                    }
                }
                else
                {
                    // Parameter is a literal object. Just add it to the list.

                    if (_viewerParameters.ReportParameters.ContainsKey(parameterName))
                    {
                        _viewerParameters.ReportParameters[parameterName].Values.Add(reportParameter.Value?.ToString());
                    }
                    else
                    {
                        var parameter = new ReportParameter(parameterName);
                        parameter.Values.Add(reportParameter.Value?.ToString());
                        _viewerParameters.ReportParameters.Add(parameterName, parameter);
                    }
                }
                
            }
        }

        private string ReportFormat2String(ReportFormat format)
        {
            return format == ReportFormat.Html ? "HTML4.0" : format.ToString();
        }

        private void Validate()
        {
            if (_viewerParameters.ProcessingMode == ProcessingMode.Remote && string.IsNullOrEmpty(_viewerParameters.ReportServerUrl))
            {
                throw new MvcReportViewerException("Report Server is not specified.");
            }

            if (!_viewerParameters.ReportIsEmbeddedResource && string.IsNullOrEmpty(_viewerParameters.ReportPath))
            {
                throw new MvcReportViewerException("Report is not specified.");
            }

            if (_viewerParameters.ReportIsEmbeddedResource && _viewerParameters.EmbeddedResourceStream == null)
            {
                throw new MvcReportViewerException("Report was specified as an embedded resource, but EmbeddedResourceStream was not provided.");
            }
        }

        private void ValidateReportFormat(ReportViewer reportViewer)
        {
            var format = ReportFormat2String(ReportFormat);
            var formats = _viewerParameters.ProcessingMode == ProcessingMode.Remote
                ? reportViewer.ServerReport.ListRenderingExtensions()
                : reportViewer.LocalReport.ListRenderingExtensions();

            if (formats.All(f => string.Compare(f.Name, format, StringComparison.InvariantCultureIgnoreCase) != 0))
            {
                throw new MvcReportViewerException($"{ReportFormat} is not supported");
            }
        }
    }
}
