﻿// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Windows.Input;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.View.Controls.Command
{
    /// <summary>
    /// <see cref="ICommand"/> that decrements the <see cref="IIntValueControl.Value"/>
    /// property of the <see cref="IIntValueControl"/> passed in as a parameter. This
    /// class should be bound to the appropriate button in a
    /// <see cref="NumericUpDown"/> control template.
    /// </summary>
    public class DecrementNumericUpDownValue : ICommand
    {
        #pragma warning disable 67
        // ICommand interface member is only used by XAML
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 67

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var control = parameter as IIntValueControl;

            if (control != null &&
                control.Value > control.Minimum)
            {
                // The minimum value should be checked before decrementing
                // Value: otherwise Value is updated by data binding before it
                // can be corrected (if necessary) by the value coercion
                // callback in NumericUpDown

                control.Value--;
            }
        }
    }
}
