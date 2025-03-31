using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelMainWindow : INotifyPropertyChanged
    {
        private readonly AbstractModelAPI ModelAPI;

        public ObservableCollection<IModelBall> ModelBalls { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand? StartBallsButton { get; set; }
        public ICommand? StopBallsButton { get; set; }
        public string? BallsNumber { get; set; }
        public bool Start
        {
            set { NotifyPropertyChanged(); }
        }
        public bool Stop
        {
            set { NotifyPropertyChanged(); }
        }

        public ViewModelMainWindow()
        {
            ModelAPI = AbstractModelAPI.CreateNewInstance();
            ModelBalls = new ObservableCollection<IModelBall>();

            StartBallsButton = new RelayCommand(() => StartBalls());
            StopBallsButton = new RelayCommand(() => StopBalls());
        }

        public void StartBalls()
        {
            Start = false; Stop = true;
            int userBallsNum = UserEnteredNoOfBallsToInt();
            ModelAPI.BallsToModelBalls(userBallsNum);
            for (int i = 0; i < userBallsNum; i++)
            {
                ModelBalls.Add(ModelAPI.GetModelBall(i));
            }
            ModelAPI.StartModelBalls();
        }

        public void StopBalls()
        {
            Start = true; Stop = false;
            ModelAPI.ClearModelBoard();
            ModelBalls.Clear();
        }

        public int UserEnteredNoOfBallsToInt()
        {
            if (int.TryParse(BallsNumber, out int number))
            {
                int parsed = int.Parse(BallsNumber);
                if (parsed > 0)
                {
                    return parsed;
                }
            }
            return 0;
        }

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}