using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
	public interface IFormatDecor
    {
        string MakeCaption(string caption);
		string BeginList();
        string MakeItem(string valueType, string entry);
        string EndList();
    }

	public interface IFormatStatistics
	{
		string Caption { get; }
		object MakeStatistics(IEnumerable<double> _data);
    }

    public class ReportMaker
    {
		public static string MakeReport(IEnumerable<Measurement> measurements, IFormatDecor formatDecor, IFormatStatistics formatStatistics)
		{
			var data = measurements.ToList();
			var result = new StringBuilder();
			result.Append(formatDecor.MakeCaption(formatStatistics.Caption));
			result.Append(formatDecor.BeginList());
			result.Append(formatDecor.MakeItem("Temperature", formatStatistics.MakeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(formatDecor.MakeItem("Humidity", formatStatistics.MakeStatistics(data.Select(z => z.Humidity)).ToString()));
			result.Append(formatDecor.EndList());
			return result.ToString();
		}
	}

    public class MeanAndStdStatistics : IFormatStatistics
    {
        public string Caption => "Mean and Std";

        public object MakeStatistics(IEnumerable<double> _data)
        {
            var data = _data.ToList();
            var mean = data.Average();
            var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

            return new MeanAndStd
            {
                Mean = mean,
                Std = std
            };
        }
    }

    public class MedianStatistics : IFormatStatistics
    {
        public string Caption => "Median";

        public object MakeStatistics(IEnumerable<double> data)
        {
            var list = data.OrderBy(z => z).ToList();
            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

            return list[list.Count / 2];
        }
    }

    public class HtmlFormat : IFormatDecor
    {	
		public string MakeCaption(string caption) => $"<h1>{caption}</h1>";

        public string BeginList() => "<ul>";	

        public string EndList() => "</ul>";		

        public string MakeItem(string valueType, string entry) => $"<li><b>{valueType}</b>: {entry}"; 
	}

	public class MarkdownFormat : IFormatDecor
    {
        public string BeginList() => "";

        public string EndList() => "";

        public string MakeCaption(string caption) => $"## {caption}\n\n";

        public string MakeItem(string valueType, string entry) => $" * **{valueType}**: {entry}\n\n";
   
	}

	public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return ReportMaker.MakeReport(data, new HtmlFormat(), new MeanAndStdStatistics());
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return ReportMaker.MakeReport(data, new MarkdownFormat(), new MedianStatistics()); 
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
			return ReportMaker.MakeReport(measurements, new MarkdownFormat(), new MeanAndStdStatistics());
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
			return ReportMaker.MakeReport(measurements, new HtmlFormat(), new MedianStatistics());
        }
	}
}
