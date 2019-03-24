using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainProgram.CustomControls
{
    /// <summary>
    /// Interaction logic for MessageBoxCustom.xaml
    /// </summary>
    public partial class MessageBoxCustom : Window
    {
        public static MessageBoxCustom setContent(string content)
        {
            return new MessageBoxCustom(content);
        }
        public MessageBoxCustom()
        {
            InitializeComponent();
        }
        public MessageBoxCustom(string content)
        {
            InitializeComponent();
            this.txbtitle.Text = content;
        }
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
