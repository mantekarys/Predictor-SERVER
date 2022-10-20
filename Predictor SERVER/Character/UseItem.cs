using System;
using System.Collections.Generic;


namespace Predictor_SERVER.Character
{
    internal class UseItem : ICommand
    {
        public void Execute(Class character,int position)
        {
            character.useItem(position);
        }

        public void Undo(Class character)
        {
            character. undoActiveItem();
        }
    }
}
