using System;

namespace Columns
{
    class RotatebleShape : AbstractShape
    {
        Random r = new Random();
        public RotatebleShape(int _x, int _y) : base(_x, _y)
        {
           
        }


        public override void MainAction()
        {
            if (matrix[0, 1] != 0)
            {
                base.MainAction();
            }
            else
            {
                int temp = matrix[1, 2];
                matrix[1, 2] = matrix[1, 1];
                matrix[1, 1] = matrix[1, 0];
                matrix[1, 0] = temp;
            }
        }

        public override void SecondAction()
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
              int offset1 = 6 - (x + sizeMatrix);
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
    }
}
