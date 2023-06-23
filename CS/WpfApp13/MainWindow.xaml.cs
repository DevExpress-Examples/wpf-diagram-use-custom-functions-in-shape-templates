using DevExpress.Data.Filtering;
using DevExpress.Diagram.Core;
using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace WpfApp13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            CreateArcPoint.Register();

            RegisterStencil();
        }

        void RegisterStencil() {
            ResourceDictionary customShapesDictionary = new ResourceDictionary() { Source = new Uri("CustomShapes.xaml", UriKind.Relative) };
            var stencil = DiagramStencil.Create("CustomStencil", "Custom Shapes", customShapesDictionary, shapeName => shapeName);
            DiagramToolboxRegistrator.RegisterStencil(stencil);

            diagramControl1.SelectedStencils = new StencilCollection() { "CustomStencil" };
        }
    }

    public class CreateArcPoint : ICustomFunctionOperator {
        private static readonly CreateArcPoint instance = new CreateArcPoint();

        public static void Register() {
            CriteriaOperator.RegisterCustomFunction(instance);
        }
        public static void Unregister() {
            CriteriaOperator.UnregisterCustomFunction(instance);
        }

        public string Name => nameof(CreateArcPoint);

        public Type ResultType(params Type[] operands) {
            return typeof(double);
        }

        public object Evaluate(params object[] operands) {
            if (operands.Length == 3
                && operands[0] is double X
                && operands[1] is double Y
                && operands[2] is string axe) {

                if (axe is "X")
                    return X > 0.5 ? X : 0.5;
                return X > 0.5 ? Y : 1;
            }
            else
                return null;
        }
    }
}
