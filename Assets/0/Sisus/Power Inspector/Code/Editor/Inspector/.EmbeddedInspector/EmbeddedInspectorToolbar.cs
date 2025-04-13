#if !POWER_INSPECTOR_LITE
using Sisus.Attributes;

namespace Sisus
{
	[ToolbarFor(typeof(EmbeddedInspector))]
	public sealed class EmbeddedInspectorToolbar : InspectorToolbar
	{
		public const float DefaultToolbarHeight = PowerInspectorToolbar.DefaultToolbarHeight;
		
		public readonly float ToolbarHeight = DefaultToolbarHeight;
		
		/// <inheritdoc/>
		public override float Height
		{
			get
			{
				return ToolbarHeight;
			}
		}

		public EmbeddedInspectorToolbar() : base()
		{
			ToolbarHeight = DefaultToolbarHeight;
		}

		public EmbeddedInspectorToolbar(float setHeight) : base()
		{
			ToolbarHeight = setHeight;
		}
	}
}
#endif