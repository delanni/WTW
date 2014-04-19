using CivSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricepsz.Strategies
{
    public class Order
    {
        public Order(object body, OrderType type)
        {
            this.Type = type;
            this.Body = body;
        }

        /// <summary>
        /// Generic holder for an operation body, like a MovementData or ResearchData instance
        /// </summary>
        public object Body { get; set; }
        public string Name { get; set; }
        public OrderType Type { get; set; }
        public bool AllowSkip { get; set; }

        public bool CanExecute(OrderType t)
        {
            return t == Type;
        }
        public object Execute<T>() where T : class
        {
            if (Body is T)
                return (Body as T);
            else throw new System.DataMisalignedException("Not of the right type of order, casting is going to fail");
        }
    }

    public enum OrderType
    {
        UNITMOVE,
        UPGRADE,
        COLONYDEPLOY,
        UNITBUY
    }
}
