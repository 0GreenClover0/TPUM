using Logic;
using System.Collections.ObjectModel;

namespace Model
{
    public abstract class ModelApi
    {
        private static ModelApi _apiInstance = new Model();

        public static ModelApi ApiInstance
        {
            get { return _apiInstance; }
        }

        public abstract void ConvertBallsToModelBalls();

        public abstract ObservableCollection<IModelBall> GetModelBallCollection();

        internal sealed class Model : ModelApi
        {
            private LogicApi logicApi = LogicApi.ApiInstance;

            ObservableCollection<IModelBall> ModelBallCollection = new ObservableCollection<IModelBall>();

            public override void ConvertBallsToModelBalls()
            {
                List<IBall> ballList = logicApi.GetBallList();
                ModelBallCollection.Clear();
                foreach (IBall ball in ballList)
                {
                    IModelBall newModelBall = IModelBall.CreateModelBall(ball);
                    ModelBallCollection.Add(newModelBall);
                }
            }

            public override ObservableCollection<IModelBall> GetModelBallCollection()
            {
                return ModelBallCollection;
            }
        }
    }
}
