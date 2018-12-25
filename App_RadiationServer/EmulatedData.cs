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
            timer.Tick += timer_Tick;
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


        void timer_Tick(object sender, object e)
        {
            this.UpdateIndicators();
            var lastRecorderData = this.CallHistory.Last();
            this.CurrentActiveCalls = new CallsData { Date = lastRecorderData.Date.AddMilliseconds(500), ActiveCalls = r.Next(10, 30) };
            this.CallHistory.Add(CurrentActiveCalls);
            this.CallHistory.RemoveAt(0);
        }

        private void LoadData()
        {
            UpdateIndicators();
            var now = DateTime.Now;
            var historyData = from c in Enumerable.Range(0, 24)
                              select new CallsData { ActiveCalls = r.Next(10, 30), Date = now.AddMilliseconds(-500 * c) };

            this.CallHistory = new ObservableCollection<CallsData>(historyData.OrderBy(c => c.Date));
        }

        private void UpdateIndicators()
        {
            this.RSTemperature = r.Next(0, 70);
            this.RSSmoke = r.Next(0, 30);
            this.RSHumidity = r.Next(0, 100);
        }
    }

    public class CoerceValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Math.Min((double)value, double.Parse((string)parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
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
}
