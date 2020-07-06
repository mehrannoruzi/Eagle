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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Eagle.ProjectSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.ItemsSource = SolutionProjects.GetProjects();
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            VSTFileGenerator.DestinationFulPath = $"{VSTFileGenerator.SolutionFullPath}\\Eagle.Template";
            VSTFileGenerator.DeleteDirectory(System.IO.Path.Combine(VSTFileGenerator.DestinationFulPath, "t"));
            var projs = (dataGrid.ItemsSource as IEnumerable<ProjectModel>).Where(x => x.Selected).ToList();
            foreach (var proj in projs)
            {
                var content = VSTFileGenerator.Fire(proj.Path, System.IO.Path.Combine(VSTFileGenerator.DestinationFulPath, "t", proj.Name));
                VSTFileGenerator.Genarate(content, proj.Name);
            }
            VSTFileGenerator.GenarateMain(projs);
            MessageBoxResult result = MessageBox.Show("Your Custom Template Been Created.", "Successfullt Done", MessageBoxButton.OK);
            if (result == MessageBoxResult.OK) this.Close();
        }
    }
}
