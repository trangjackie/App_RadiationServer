using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Collections.ObjectModel;

namespace App_RadiationServer
{
    public class EmulatedData : ViewModelBase
    {
        private DispatcherTimer timer;
        private Random r;
        
        public EmulatedData()
        {
            r = new Random();
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += Timer_Tick;
            this.LoadData();
        }

        public void StopTimer()
        {
            this.timer.Stop();
        }

        public void StartTimer()
        {
            this.timer.Start();
        }

        private double rs_temperature;

        public double RSTemperature
        {
            get { return rs_temperature; }
            set
            {
                rs_temperature = value;
                this.OnPropertyChanged("RSTemperature");
            }
        }

        private double rs_smoke;

        public double RSSmoke
        {
            get { return rs_smoke; }
            set
            {
                rs_smoke = value;
                this.OnPropertyChanged("RSSmoke");
            }
        }

        private double rs_humidity;
        public double RSHumidity
        {
            get { return rs_humidity; }
            set
            {
                rs_humidity = value;
                this.OnPropertyChanged("RSHumidity");
            }
        }

        private double rs_autonomy;
        public double RSAutonomy
        {
            get { return rs_autonomy; }
            set
            {
                rs_autonomy = value;
                this.OnPropertyChanged("RSAutonomy");
            }
        }
        // Tham số liên quan đến gió
        private WindData rs_wind;
        public WindData RSWind
        {
            get { return rs_wind; }
            set
            {
                rs_wind = value;
                this.OnPropertyChanged("RSWind");
            }
        }
        private List<string> strHuong = new List<string>() { "Dong", "Tay", "Nam", "Bac" };
        public List<string> StrHuong
        {
            get { return strHuong; }
            private set
            {
                strHuong = value;
                this.OnPropertyChanged("StrHuong");
            }
        }
        // store data line 1
        private CallsData currentActiveCalls;

        public CallsData CurrentActiveCalls
        {
            get { return currentActiveCalls; }
            set
            {
                currentActiveCalls = value;
                this.OnPropertyChanged("CurrentActiveCalls");
            }
        }
        private ObservableCollection<CallsData> callHistory;

        public ObservableCollection<CallsData> CallHistory
        {
            get { return callHistory; }
            private set
            {
                callHistory = value;
                this.OnPropertyChanged("CallHistory");
            }
        }

        // store data line 2
        private CallsData currentActiveCalls2;

        public CallsData CurrentActiveCalls2
        {
            get { return currentActiveCalls2; }
            set
            {
                currentActiveCalls2 = value;
                this.OnPropertyChanged("CurrentActiveCalls2");
            }
        }
        private ObservableCollection<CallsData> callHistory2;

        public ObservableCollection<CallsData> CallHistory2
        {
            get { return callHistory2; }
            private set
            {
                callHistory2 = value;
                this.OnPropertyChanged("CallHistory2");
            }
        }

        void Timer_Tick(object sender, object e)
        {
            this.UpdateIndicators();
            var lastRecorderData = this.CallHistory.Last();
            this.CurrentActiveCalls = new CallsData {
                Date = lastRecorderData.Date.AddMilliseconds(1000),
                ActiveCalls = r.Next(10, 30)
            };
            this.CallHistory.Add(CurrentActiveCalls);
            this.CallHistory.RemoveAt(0);

            var lastRecorderData2 = this.CallHistory2.Last();
            this.CurrentActiveCalls2 = new CallsData
            {
                Date = lastRecorderData2.Date.AddMilliseconds(1000),
                ActiveCalls = r.Next(30, 50)
            };
            this.CallHistory2.Add(CurrentActiveCalls2);
            this.CallHistory2.RemoveAt(0);
        }

        private void LoadData()
        {
            UpdateIndicators();
            var now = DateTime.Now;
            var historyData = from c in Enumerable.Range(0, 24)
                select new CallsData
                {
                    ActiveCalls = r.Next(10, 30),
                    Date = now.AddMilliseconds(-1000 * c)
                };
            this.CallHistory = new ObservableCollection<CallsData>(historyData.OrderBy(c => c.Date));
            var historyData2 = from c in Enumerable.Range(0, 24)
                select new CallsData
                {
                    ActiveCalls = r.Next(30, 50),
                    Date = now.AddMilliseconds(-1000 * c)
                };
            this.CallHistory2 = new ObservableCollection<CallsData>(historyData2.OrderBy(c => c.Date));
        }

        private void UpdateIndicators()
        {
            this.RSTemperature = r.Next(0, 70);
            this.RSSmoke = r.Next(0, 30);
            this.RSHumidity = r.Next(0, 100);
            this.RSAutonomy = r.Next(0, 100);
            this.RSWind = new WindData
            {
                TocdoGio = r.Next(0, 200),
                HuongGio = r.Next(0, 360)
            };
        }
    }

    

    public class CallsData : ViewModelBase
    {
        private int activeCalls;
        public int ActiveCalls
        {
            get { return activeCalls; }
            set
            {
                activeCalls = value;
                this.OnPropertyChanged("ActiveCalls");
            }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                this.OnPropertyChanged("Date");
            }
        }
    }

    public class WindData: ViewModelBase
    {
        private int huongGio;
        public int HuongGio
        {
            get { return huongGio; }
            set
            {
                huongGio = value;
                this.OnPropertyChanged("HuongGio");
            }
        }
        private float tocdoGio;
        public float TocdoGio
        {
            get { return tocdoGio; }
            set
            {
                tocdoGio = value;
                this.OnPropertyChanged("TocdoGio");
            }
        }
    }

    public class StringFormatConverter : IValueConverter
    {
        public string StringFormat { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!String.IsNullOrEmpty(StringFormat))
                return String.Format(StringFormat, value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
