# Visualize-JSON-Data-Quickly-Using-WPF-Chart

JSON is one of the most widely used data formats, but working with it can be challenging.

This article focuses on extracting data from JSON files and visualizing it using bar charts, line charts, and pie charts from the Syncfusion WPF Charts control.

Four easy steps can be used to quickly display data from a JSON web service using the WPF Charts simplicity:
1.	Retrieve the data from json string or web service using HttpClient.
2.	Deserialize the JSON data to create a list of ExpandoObject.
3.	List of ExpandoObjects to DataTable Conversion
4.	Set the ItemsSource property of the chart to the data table along with the binding path of the X and Y in the Chart Series.

![Create beautiful charts using JSON data in WPF](https://github.com/SyncfusionExamples/Visualize-JSON-Data-Quickly-Using-WPF-Chart/blob/master/WPFChartJSONDataOverview.gif)

For a detailed, step-by-step guide on how to effectively visualize JSON data using WPF charts, refer to the comprehensive blog titled [Visualize JSON Data Quickly Using WPF Charts](https://www.syncfusion.com/blogs/post/visualize-json-data-wpf-charts).

## Troubleshooting
### Path too long exception
If you are facing a path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.
