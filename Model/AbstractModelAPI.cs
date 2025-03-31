using Logic;

namespace Model
{
    public abstract class AbstractModelAPI : IObserver<int>
    {
        public static AbstractModelAPI CreateNewInstance()
        {
            return new ModelAPI();
        }
        public abstract void BallsToModelBalls(int num);

        public abstract void ClearModelBoard();
        public abstract void StartModelBalls();
        public abstract IModelBall GetModelBall(int i);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int i);

        internal sealed class ModelAPI : AbstractModelAPI
        {
            private readonly AbstractLogicAPI logicApi;
            private readonly List<IModelBall> ModelBallList = new List<IModelBall>();
            private readonly IDisposable? observerManager;

            internal ModelAPI()
            {
                logicApi = AbstractLogicAPI.CreateNewInstance();
                observerManager = logicApi.Subscribe(this);
            }

            public override void ClearModelBoard()
            {
                logicApi.ClearBoard();
                ModelBallList.Clear();
                observerManager?.Dispose();
            }

            public override void BallsToModelBalls(int num)
            {
                logicApi.CreateBoard();
                logicApi.CreateBalls(num);
                List<List<double>> dataBalls = logicApi.GetBallsCoordsAndRadius();
                for (int i = 0; i < dataBalls.Count; i++)
                {
                    List<double> ballCoords = dataBalls[i];
                    ModelBallList.Add(IModelBall.CreateModelBall(ballCoords[0], ballCoords[1], ballCoords[2]));
                }
            }

            public override IModelBall GetModelBall(int i)
            {
                return ModelBallList[i];
            }

            public override void OnCompleted()
            {
                observerManager?.Dispose();
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public override void OnNext(int i)
            {
                if (i < ModelBallList.Count)
                {
                    IModelBall modelBall = ModelBallList[i];
                    List<double> modelBallCoords = logicApi.GetBallsCoordsAndRadius()[i];
                    modelBall.Moving(modelBallCoords[0], modelBallCoords[1]);
                }
            }
            public override void StartModelBalls()
            {
                logicApi.StartBalls();
            }
        }
    }
}
