using System;
using System.Collections.Generic;


namespace Predictor_SERVER.Character
{
    internal class UseFirstItem : ICommand
    {
        public void Execute(Class character)
        {
            character.useItem(0);
        }

        public void Undo(Class character)
        {
            character. undoActiveItem();
        }
    }
}
