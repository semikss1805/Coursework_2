using System;

namespace Columns
{
    class ReversbleShape : AbstractShape
    {
        Random r = new Random();
        public ReversbleShape(int _x, int _y) : base(_x, _y)
        {

        }

        public override void SecondAction()
        {
            int temp = matrix[2, 1];
            matrix[2, 1] = matrix[0, 1];
            matrix[0, 1] = temp;
        }
    }
}
