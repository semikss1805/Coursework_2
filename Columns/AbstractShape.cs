using System;

namespace Columns
{
    abstract class AbstractShape
    {
        public int x;
        public int y;
        public int[,] matrix;
        public int[,] nextMatrix;
        public int sizeMatrix;
        public int sizeNextMatrix;
        Random r = new Random();
        public int[,] column = new int[3, 3]
        {
            {0,0,0},
            {0,0,0},
            {0,0,0},
        };

        public AbstractShape(int _x, int _y)
        {
            x = _x;
            y = _y;
            r = new Random(GetHashCode() + (int)DateTime.Now.Ticks);
            matrix = GenerateMatrix();
            sizeMatrix = (int)Math.Sqrt(matrix.Length);
            nextMatrix = GenerateMatrix();
            sizeNextMatrix = (int)Math.Sqrt(nextMatrix.Length);
        }

        public void ResetShape(int _x, int _y)
        {
            x = _x;
            y = _y;
            r = new Random(GetHashCode() + (int)DateTime.Now.Ticks);
            matrix = (int[,])nextMatrix.Clone();
            sizeMatrix = (int)Math.Sqrt(matrix.Length);
            nextMatrix = GenerateMatrix();
            sizeNextMatrix = (int)Math.Sqrt(nextMatrix.Length);
        }
        public int[,] GenerateMatrix()
        {
            int[,] _matrix = (int[,])column.Clone();
            _matrix[0, 1] = r.Next(1, 6);
            _matrix[1, 1] = r.Next(1, 6);
            _matrix[2, 1] = r.Next(1, 6);
            return _matrix;
        }

        public virtual void MainAction()
        {
            int temp = matrix[2, 1];
            matrix[2, 1] = matrix[1, 1];
            matrix[1, 1] = matrix[0, 1];
            matrix[0, 1] = temp;
        }

        public abstract void SecondAction();

        public void MoveDown()
        {
            y++;
        }
        public void MoveRight()
        {

            x++;
        }
        public void MoveLeft()
        {
            x--;
        }
    }
}
