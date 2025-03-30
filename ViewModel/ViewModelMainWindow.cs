using Logic;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelMainWindow : INotifyPropertyChanged
    {
        private ModelApi modelApi = ModelApi.ApiInstance;
        private LogicApi logicApi = LogicApi.ApiInstance;

        public ViewModelMainWindow()
        {
            this.SignalStart = new Signals(Start);
            this.SignalStop = new Signals(Stop);
        }

        public string numberOfModelBalls
        {
            get;
            set;
        }

        private ObservableCollection<IModelBall> ModelBallCollection;

        public ObservableCollection<IModelBall> modelBallCollection
        {
            get { return ModelBallCollection; }
            set
            {
                ModelBallCollection = value;
                OnPropertyChanged("ModelBallCollection");
            }
        }

        public ICommand SignalStart
        {
            get;
            set;
        }

        public ICommand SignalStop
        {
            get;
            set;
        } 

        public void Start()
        {
            logicApi.GenerateBalls(Convert.ToInt16(numberOfModelBalls));
            modelApi.ConvertBallsToModelBalls();
            ModelBallCollection = modelApi.GetModelBallCollection();
            foreach (IModelBall modelBall in ModelBallCollection)
            {
                modelBall.PropertyChanged += propertyChanged;
            }
            logicApi.CreateThreads();
        }

        public void Stop()
        {
            logicApi.StopThreads();
        }

        private void propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("ModelBallCollection");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
