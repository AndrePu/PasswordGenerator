using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PasswordGenerator.Interfaces;

namespace PasswordGenerator.Helpers
{
    public class DialogPasswordSaver : IPasswordSaver
    {
        private readonly SaveFileDialog saveFileDialog;

        public DialogPasswordSaver()
        {
            saveFileDialog = new SaveFileDialog()
            {
                Title = "Save password..",
                Filter = "Text files(*.txt)|*.txt" ,
                DefaultExt = "(*.txt)|*.txt",
                AddExtension = true
            };
        }

        public void Save(string password)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            System.IO.File.WriteAllText(saveFileDialog.FileName, password);
        }

    }
}
