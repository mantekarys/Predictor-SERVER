using System;
using System.Collections.Generic;


namespace Predictor_SERVER.Character
{
    internal class UseSecondItem : ICommand
    {
        public void Execute(Class character)
        {
            character.useItem(1);
        }

        public void Undo(Class character)
        {
            character. undoActiveItem();
        }
    }
}
