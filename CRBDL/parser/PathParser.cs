using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDL.parser
{
    class PathParser
    {

        public static int SetGamePath(string name, ComboBox comboBox)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (name == comboBox.Items[i].ToString())
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
