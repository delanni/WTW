using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricepsz.Strategies
{
    public class Objective
    {
        public string Name;
        public List<Order> Orders;
        public Func<List<Order>, bool> Operation;

        /// <summary>
        /// Executes the given Lambda. The Lambda function gets an OUT list where it should yield its orders, and it should return true if it doesn't want to be executed again.
        /// </summary>
        /// <returns>True if the Objective is completed, and thus may be removed</returns>
        public virtual bool Execute()
        {
            if (Orders == null)
            {
                Orders = new List<Order>();
            }

            // Invocation should yield true if the objective has finished, and may be disposed;
            return Operation.Invoke(Orders);
        }
        public void Clear()
        {
            if (Orders == null) Orders = new List<Order>();
            else Orders.Clear();
        }
    }
}
