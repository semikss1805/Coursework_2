using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Shape
    {
        public int x;
        public int y;
        public int[,] matrix;
        public int[,] nextMatrix;
        public int sizeMatrix;
        public int sizeNextMatrix;
        Random r = new Random();
        public int[,] tetr1 = new int[3, 3]
        {
            {0,0,0},
            {0,0,0},
            {0,0,0},
        };

        public Shape(int _x, int _y)
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
            matrix = nextMatrix;
            sizeMatrix = (int)Math.Sqrt(matrix.Length);
            nextMatrix = GenerateMatrix();
            sizeNextMatrix = (int)Math.Sqrt(nextMatrix.Length);
        }
        public int[,] GenerateMatrix()
        {
            int[,] _matrix = tetr1;
            _matrix[0, 1] = r.Next(1, 6);
            _matrix[1, 1] = r.Next(1, 6);
            _matrix[2, 1] = r.Next(1, 6);
            return _matrix;
        }

        public void RotateShape()
        {
            int[,] tempMatrix = new int[sizeMatrix, sizeMatrix];
            for (int i = 0; i < sizeMatrix; i++)
            {
                for (int j = 0; j < sizeMatrix; j++)
                {
                    tempMatrix[i, j] = matrix[j, (sizeMatrix - 1) - i];
                }
            }
            matrix = tempMatrix;
            int offset1 = (10 - (x - sizeMatrix));
            if (offset1 < 0)
            {
                for (int i = 0; i < Math.Abs(offset1); i++)
                    MoveLeft();
            }

            if (x < 0)
            {
                for (int i = 0; i < Math.Abs(x) + 1; i++)
                    MoveRight();
            }

        }
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
