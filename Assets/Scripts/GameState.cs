using EpPathFinding.cs;
namespace PathFinding
{
    public class GameState
    {
        public float TimeUntilSpawnLeftSide = 0;
        public float TimeUntilSpawnRightSide = 0;

        public BaseGrid SearchGrid;
        public JumpPointParam SearchPathPoints;
        
        public void Initialize(Configuration configuration)
        {
            var width = configuration.gridWidth;
            var height = configuration.gridHeight;
            bool [][] movableMatrix = new bool [width][];
            for(int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                movableMatrix[widthTrav]=new bool[height];
                for(int heightTrav = 0; heightTrav < height;  heightTrav++)
                { 
                    movableMatrix[widthTrav][heightTrav]=true; 
                }  
            }
            
            SearchGrid = new StaticGrid(width, height, movableMatrix);
            SearchPathPoints = new JumpPointParam(SearchGrid, EndNodeUnWalkableTreatment.ALLOW);
        }
    }
}