using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCatcher
{
    /// <summary>
    /// Интерфейс для описания графа обхода для Теньки
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INode<T>
    {
        T Next();
        T Value();
    }
}
