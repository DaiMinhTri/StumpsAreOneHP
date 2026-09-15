// Minimal stubs for JetBrains.Annotations attributes used by vendored code
// (ItemManager.cs / Plugin.cs). Avoids shipping a JetBrains.Annotations dependency.
namespace JetBrains.Annotations
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
    internal sealed class UsedImplicitlyAttribute : System.Attribute { }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
    internal sealed class PublicAPIAttribute : System.Attribute { }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
    internal sealed class MeansImplicitUseAttribute : System.Attribute { }
}
