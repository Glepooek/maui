using System;
using System.ComponentModel;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform;

namespace Microsoft.Maui.Controls
{
	public abstract class Effect
	{
		internal Effect()
		{
		}

		internal PlatformEffect PlatformEffect { get; set; }

		public Element Element { get; internal set; }

		public bool IsAttached { get; private set; }

		public string ResolveId { get; internal set; }

		#region Statics

		public static Effect Resolve(string name)
		{
			Effect result = null;
			if (Internals.Registrar.Effects.TryGetValue(name, out Type effectType))
			{
				result = (Effect)DependencyResolver.ResolveOrCreate(effectType);
			}

			if (result == null)
				result = new NullEffect();
			result.ResolveId = name;
			return result;
		}

		#endregion

		// Received after Control/Container/Element made valid
		protected abstract void OnAttached();

		// Received after Control/Container made invalid
		protected abstract void OnDetached();

		internal virtual void ClearEffect()
		{
			if (IsAttached)
				SendDetached();
			Element = null;
		}

		internal virtual void SendAttached()
		{
			if (IsAttached)
				return;
			OnAttached();
			IsAttached = true;
			PlatformEffect?.SendAttached();
		}

		internal virtual void SendDetached()
		{
			if (!IsAttached)
				return;
			OnDetached();
			IsAttached = false;
			PlatformEffect?.SendDetached();
		}

		internal virtual void SendOnElementPropertyChanged(PropertyChangedEventArgs args)
		{
		}
	}
}