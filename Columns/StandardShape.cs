using System;

namespace Columns
{
    class StandardShape : AbstractShape
    {
        Random r = new Random();
        public StandardShape(int _x, int _y) : base(_x, _y)
        {

        }

        public override void MainAction()
        {
            base.MainAction();
        }

        public override void SecondAction()
        {

        }
    }
}
