using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChartSample
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            ObservableCollection<SolidColorBrush> paletteBrushes1 = new ObservableCollection<SolidColorBrush>()
            {
                new SolidColorBrush(Color.FromArgb(255, 0, 127, 0)),
                new SolidColorBrush(Color.FromArgb(255, 227, 35, 111)),
                new SolidColorBrush(Color.FromArgb(255, 177, 70, 194)),
                new SolidColorBrush(Color.FromArgb(255, 226, 24, 47)),
                new SolidColorBrush(Color.FromArgb(255, 250, 153, 1)),
                new SolidColorBrush(Color.FromArgb(255, 255, 185, 0)),
                new SolidColorBrush(Color.FromArgb(255, 14, 0, 230)),
                new SolidColorBrush(Color.FromArgb(255, 122, 117, 116)),
                new SolidColorBrush(Color.FromArgb(255, 0, 120, 222)),
                new SolidColorBrush(Color.FromArgb(255, 0, 204, 106)),
            };

            PaletteBrushes = paletteBrushes1;
        }

        ObservableCollection<ExpandoObject> items = new ObservableCollection<ExpandoObject>();

        private ObservableCollection<SolidColorBrush> paletteBrushes;
        public ObservableCollection<SolidColorBrush> PaletteBrushes
        {
            get
            {
                return paletteBrushes;
            }
            set
            {
                paletteBrushes = value;
                OnPropertyRaised(nameof(PaletteBrushes));

            }
        }

        private string xBindingPath;
        public string XBindingPath
        {
            get
            {
                return xBindingPath;
            }
            set
            {
                xBindingPath = value;
                OnPropertyRaised(nameof(XBindingPath));

            }
        }

        private string yBindingPath;
        public string YBindingPath
        {
            get
            {
                return yBindingPath;
            }
            set
            {
                yBindingPath = value;
                OnPropertyRaised(nameof(YBindingPath));

            }
        }

      

        private string jsonData;
        public string JsonData
        {
            get
            {
                return jsonData;
            }
            set
            {
                if(value != jsonData)
                {
                    jsonData = value;
                    GetDataAsync(jsonData);
                    OnPropertyRaised(nameof(JsonData));
                }
            }
        }

        private DataTable data;

        public DataTable Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                OnPropertyRaised(nameof(Data));
            }
        }

        private List<string> xcolumns;

        public List<string> XColumns
        {
            get
            {
                return xcolumns;
            }
            set
            {
                xcolumns = value;
                OnPropertyRaised(nameof(XColumns));
            }
        }

        private List<string> ycolumns;

        public List<string> YColumns
        {
            get
            {
                return ycolumns;
            }
            set
            {
                ycolumns = value;
                OnPropertyRaised(nameof(YColumns));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        private bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || 
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) 
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private async void GetDataAsync(string json)
        {
            Data = null;
            XBindingPath = string.Empty;
            YBindingPath = string.Empty;
            string jsonString = json;

            Uri uriResult;
            bool result = Uri.TryCreate(jsonString, UriKind.Absolute, out uriResult) && 
                                        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (result)
            {
                using (HttpClient httpClient = new HttpClient()) {
                    jsonString = await httpClient.GetStringAsync(jsonString);
                }
            }
            
            if (IsValidJson(jsonString))
            {
                items = JsonConvert.DeserializeObject<ObservableCollection<ExpandoObject>>(jsonString);
                Data = ToDataTable(items);
                GenerateXYColumns();
            }
            else
            {
                Data = null;
            }
        }

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }  
   
      
DataTable ToDataTable(IEnumerable<ExpandoObject> items)
{
    if (!items.Any()) return null;

    var table = new DataTable();
    bool isFirst = true;

    items.Cast<IDictionary<string, object>>().ToList().ForEach(x =>
    {
        if (isFirst)
        {
            foreach (var key in x.Keys)
            {
                table.Columns.Add(new DataColumn(key, x[key].GetType()));
            }
                   
            isFirst = false;
        }
        table.Rows.Add(x.Values.ToArray());
    });
            
    return table;
}

        void GenerateXYColumns()
        {
            var xdataCollection = new List<string>();
            var ydataCollection = new List<string>();
            foreach (DataColumn item in Data.Columns)
            {
                xdataCollection.Add(item.ColumnName);
                if (item.DataType.Name != "String" && item.DataType.Name != "DateTime")
                {
                    ydataCollection.Add(item.ColumnName);
                }
            }
            XColumns = xdataCollection;
            YColumns = ydataCollection;
            if (ydataCollection.Count > 0 && xdataCollection.Count > 0)
            {
                XBindingPath = XColumns[0];
                YBindingPath = YColumns[0];
            }
        }
    }
}

