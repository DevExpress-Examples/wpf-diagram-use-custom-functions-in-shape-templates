<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1174053)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# WPF DiagramControl - Complex Expressions and Custom Functions in Shape Templates

This example demonstrates how to specify complex expressions and use custom functions to calculate the `ShapeTemplate`'s [Parameter](https://docs.devexpress.com/CoreLibraries/DevExpress.Diagram.Core.Shapes.Parameter) value. You can use custom expressions or functions to implement complex logic for `Parameters` (for example, a circular motion).

In this example, the custom **Sector** shape allows users to specify its angle:

![image](https://github.com/DevExpress-Examples/wpf-custom-functions-ShapeTemplates/assets/65009440/21d31189-787e-4141-9d62-6a1f19325498)

## Implementation Details

`ShapeTemplates` accept functions that implement the [ICustomFunctionOperator](https://docs.devexpress.com/CoreLibraries/DevExpress.Data.Filtering.ICustomFunctionOperator) interface. The [Criteria Language Syntax](https://docs.devexpress.com/CoreLibraries/4928/devexpress-data-library/criteria-language-syntax) allows you to calculate shape `Parameters`.

Follow the steps below to create a custom calculation function:
1. Create a function class that implements the [ICustomFunctionOperator](https://docs.devexpress.com/CoreLibraries/DevExpress.Data.Filtering.ICustomFunctionOperator) interface.
2. Use the [CriteriaOperator.RegisterCustomFunction](https://docs.devexpress.com/CoreLibraries/DevExpress.Data.Filtering.CriteriaOperator.RegisterCustomFunction(DevExpress.Data.Filtering.ICustomFunctionOperator)) method to register this function.

```cs
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
```

```xaml
<ShapeTemplate x:Key="{ShapeKey Sector}" DefaultSize="120, 120">
	<Start X="0.5" Y="0" />
	<Line X="0.5" Y="0.5"/>
	<Line X="(COS(P0) + 1)/2" Y="(SIN(P0) + 1)/2" />
	<Arc X="CreateArcPoint((COS(P0) + 1)/2, (SIN(P0) + 1)/2, 'X')"
		 Y="CreateArcPoint((COS(P0) + 1)/2, (SIN(P0) + 1)/2, 'Y')"
		 Direction="Counterclockwise"
		 Size="CreateSize(W/2, H/2)"/>

	<Arc X="0.5" Y="0" Direction="Counterclockwise" Size="CreateSize(W/2, H/2)" />
	<ShapeTemplate.ConnectionPoints>
		<ShapePoint X="0.5" Y="1" />
		<ShapePoint X="1" Y="0.5" />
		<ShapePoint X="0.5" Y="0" />
		<ShapePoint X="0" Y="0.5" />
	</ShapeTemplate.ConnectionPoints>
	<ShapeTemplate.Parameters>
		<Parameter DefaultValue="0"
			   Point="CreatePoint((COS(P) + 1)/2*W, (SIN(P) + 1)/2*H)"
			   Value="atn2(P.Y/H*2-1, P.X/W*2-1)"
			   Min="-3.14" Max="3.14" />
	</ShapeTemplate.Parameters>
</ShapeTemplate>
```

## Files to Review

- [MainWindow.xaml.cs](./CS/WpfApp13/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/WpfApp13/MainWindow.xaml.vb))
- [CustomShapes.xaml](./CS/WpfApp13/CustomShapes.xaml)

## Documentation

- [ICustomFunctionOperator](https://docs.devexpress.com/CoreLibraries/DevExpress.Data.Filtering.ICustomFunctionOperator)
- [Shapes](https://docs.devexpress.com/WPF/116099/controls-and-libraries/diagram-control/diagram-items/shapes)
- [Use Shape Templates to Create Shapes and Containers](https://docs.devexpress.com/WPF/117037/controls-and-libraries/diagram-control/diagram-items/creating-shapes-and-containers-using-shape-templates)

## More Examples

- [WPF DiagramControl - Create Custom Shapes with Connection Points](https://github.com/DevExpress-Examples/wpf-diagramdesigner-create-custom-shapes-with-connection-points)
