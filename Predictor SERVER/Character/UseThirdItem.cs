using System;
using System.Collections.Generic;


namespace Predictor_SERVER.Character
{
    internal class UseThirdItem : ICommand
    {
        public void Execute(Class character)
        {
            character.useItem(2);
        }

        public void Undo(Class character)
        {
            character. undoActiveItem();
        }
    }
}
