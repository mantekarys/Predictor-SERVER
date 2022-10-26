using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal interface ICommand
    {
        /// <summary>
        /// method for calling specific players character UseItem() method
        /// </summary>
        /// <param name="charecter">player's character</param>
        void Execute(Class charecter);
        /// <summary>
        /// Method for calling undo for recently activated item
        /// </summary>
        /// <param name="charecter">player's character</param>
        void Undo(Class charecter);
    }
}
